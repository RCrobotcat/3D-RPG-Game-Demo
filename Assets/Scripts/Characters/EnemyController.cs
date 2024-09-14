using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    GUARD,
    PATROL,
    CHASE,
    ATTACK,
    DEAD
}
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStatus))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private Collider collider1;

    protected CharacterStatus characterStatus;

    [Header("Basic Settings")]
    public float sightRadius;
    protected GameObject attackTarget;
    public bool isGuard;
    private float speed;
    public float lookAtTime;
    private float lookAtTimer;
    private float lastAttackTime; // to calculate the time between attacks

    private Quaternion guardRotation;

    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint; // the point where the enemy will go to
    private Vector3 guardPosition;

    [Header("Cameras")]
    public GameObject playerCam;
    public GameObject playerFreeLookCam;

    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool playerDead;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
        collider1 = GetComponent<Collider>();

        speed = agent.speed;
        guardPosition = transform.position;
        guardRotation = transform.rotation;
        lookAtTimer = lookAtTime;
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }

        GameManager.Instance.AddObserver(this);
    }

    /*void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
    }*/

    void OnDisable()
    {
        if (!GameManager.Instance.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);

        if (GetComponent<LootSpawner>() && isDead)
            GetComponent<LootSpawner>().SpawnLoot();
    }

    private void Update()
    {
        if (characterStatus.currentHealth <= 0)
            isDead = true;

        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        enemyAnimator.SetBool("Walk", isWalk);
        enemyAnimator.SetBool("Chase", isChase);
        enemyAnimator.SetBool("Follow", isFollow);
        enemyAnimator.SetBool("Critical", characterStatus.isCritical);
        enemyAnimator.SetBool("Death", isDead);
    }

    void SwitchStates()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            /*Debug.Log("Found Player");*/
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;
                // if the enemy is not at the guard position, go back to the guard position
                if (transform.position != guardPosition)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPosition;

                    if (Vector3.SqrMagnitude(transform.position - guardPosition) <= 1f)
                    {
                        isWalk = false;
                        // rotate the enemy to the guard position slowly
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                    }
                }

                break;
            case EnemyStates.PATROL:

                isChase = false;
                agent.speed = speed * 0.5f; // mutiplication(*) is faster than division(/) in C#

                if (Vector3.Distance(transform.position, wayPoint) <= 1f)
                {
                    isWalk = false;
                    if (lookAtTimer > 0)
                        lookAtTimer -= Time.deltaTime;
                    else
                        GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }

                break;
            case EnemyStates.CHASE:

                isWalk = false;
                isChase = true;
                agent.speed = speed;
                if (!FoundPlayer())
                {
                    isFollow = false;
                    if (lookAtTimer > 0)
                    {
                        agent.destination = transform.position;
                        lookAtTimer -= Time.deltaTime;
                    }
                    else if (isGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                        enemyStates = EnemyStates.PATROL;
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }

                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStatus.attackData.coolDown;

                        // critical hit judge
                        characterStatus.isCritical = Random.value < characterStatus.attackData.criticalChance;
                        // implement critical hit
                        Attack();
                    }
                }

                break;
            case EnemyStates.DEAD:
                collider1.enabled = false;
                /*agent.enabled = false;*/
                agent.radius = 0;
                Destroy(gameObject, 2f); // destroy the enemy after 2 seconds
                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
            enemyAnimator.SetTrigger("Attack");
        if (TargetInSkillRange())
            enemyAnimator.SetTrigger("Skill");
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return (Vector3.Distance(transform.position, attackTarget.transform.position) <= characterStatus.attackData.attackRange);
        else return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return (Vector3.Distance(transform.position, attackTarget.transform.position) <= characterStatus.attackData.skillRange);
        else return false;
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    // Get a new random point within the patrol range
    void GetNewWayPoint()
    {
        lookAtTimer = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPosition.x + randomX, transform.position.y, guardPosition.z + randomZ);

        /**
         * Check if the randomPoint is on the NavMesh,
         * if it is, set it as the new wayPoint; if not, set the current position as the new wayPoint
         * 1 is the NavMesh layer(walkable layer)
         */
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ?
            hit.position : transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    // Animation Events
    public void shakeCamera()
    {
        if (playerCam.activeSelf)
            cinemachineShake.instance.shakingCamera(3f, 0.1f);
        else if (playerFreeLookCam.activeSelf)
            cinemachineShakeForFreelook.instance.shakingCamera(3f, 0.1f);
    }

    public void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            CharacterStatus playerStatus = attackTarget.GetComponent<CharacterStatus>();
            playerStatus.TakeDamage(characterStatus, playerStatus);
        }
    }

    public void EndNotify()
    {
        enemyAnimator.SetBool("Win", true);
        playerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
