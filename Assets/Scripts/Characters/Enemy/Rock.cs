using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockState
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }

    public RockState rockState;

    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;
    public int damage;
    public GameObject target;
    private Vector3 direction;
    public GameObject breakEffect; // break effect of the rock
    public GameObject burstEffect; // burst effect of the rock

    [Header("Camera Shake Settings")]
    public playerCamsController playerCamsController;

    public float DestroyTime;
    private float DestroyTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockState = RockState.HitPlayer;
        DestroyTimer = DestroyTime;
        FlyToTarget();
    }

    void Update()
    {
        if (DestroyTimer > 0)
        {
            DestroyTimer -= Time.deltaTime;
        }
        else
        {
            DestroyOnGround();
        }
    }

    // if you want to use physics, you should use FixedUpdate
    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
            rockState = RockState.HitNothing;
    }

    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<playerController>().gameObject;
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void DestroyOnGround()
    {
        Instantiate(burstEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (rockState)
        {
            case RockState.HitPlayer:
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    collision.gameObject.GetComponent<CharacterStatus>().TakeDamage(damage, collision.gameObject.GetComponent<CharacterStatus>());

                    rockState = RockState.HitNothing;
                }

                break;
            case RockState.HitEnemy:
                if (collision.gameObject.GetComponent<Golem>())
                {
                    var collisionStatus = collision.gameObject.GetComponent<CharacterStatus>();
                    collisionStatus.TakeDamage(damage, collisionStatus);
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }

                break;
        }
    }
}
