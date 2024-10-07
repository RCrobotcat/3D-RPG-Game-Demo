using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    playerController player;
    NavMeshAgent agent;
    Animator animator;

    [HideInInspector] public bool followPlayer = false;
    bool isWalking;
    bool isAttacking;

    GameObject attackTarget;

    [Header("NPC Attack Settings")]
    public float NpcAttackRange;
    public int NpcAttackDamage;

    void Awake()
    {
        player = FindObjectOfType<playerController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null)
            player = FindObjectOfType<playerController>();

        SwitchAnimation();

        if (followPlayer)
        {
            if (!player.isAboutToAttack)
                FollowPlayer();
            NpcAttack(player.attackTarget);
        }
    }

    public void SwitchAnimation()
    {
        animator.SetBool("IsWalk", isWalking);
        animator.SetBool("IsAttack", isAttacking);
    }

    public void FollowPlayer()
    {
        isWalking = true;
        agent.destination = player.transform.position;
        agent.stoppingDistance = 2f;

        if (Vector3.SqrMagnitude(player.transform.position - transform.position) <= 4f)
        {
            isWalking = false;
        }
    }

    public void NpcAttack(GameObject target)
    {
        if (target != null)
        {
            attackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
        else
        {
            StopAllCoroutines();
            isAttacking = false;
            attackTarget = null;
            agent.isStopped = false;
        }
    }

    private IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = NpcAttackRange;
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > 4f)
        {
            agent.destination = attackTarget.transform.position;
            isWalking = true;
            yield return null;
        }
        isAttacking = true;
        agent.isStopped = true;
        isWalking = false;
    }

    public void Hit()
    {
        if (attackTarget != null)
        {
            CharacterStatus enemy = attackTarget.GetComponent<CharacterStatus>();
            enemy.TakeDamageByNpc(NpcAttackDamage);
        }
    }
}
