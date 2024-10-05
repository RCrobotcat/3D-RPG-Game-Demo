using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    playerController player;
    NavMeshAgent agent;
    Animator animator;

    public bool followPlayer = false;
    bool isWalking;

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
            FollowPlayer();
    }

    public void SwitchAnimation()
    {
        animator.SetBool("IsWalk", isWalking);
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
}
