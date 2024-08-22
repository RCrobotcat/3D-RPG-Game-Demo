using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 20f;
    public GameObject rockPrefab;
    public Transform handPos;

    // Animation Event
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            CharacterStatus playerStatus = attackTarget.GetComponent<CharacterStatus>();

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            // direction.Normalize();

            playerStatus.GetComponent<NavMeshAgent>().isStopped = true;
            playerStatus.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            playerStatus.GetComponent<Animator>().SetTrigger("Dizzy");

            playerStatus.TakeDamage(characterStatus, playerStatus);
        }
    }

    // Animation Event
    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
