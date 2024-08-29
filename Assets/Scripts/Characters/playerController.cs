using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator playerAnimator;

    private GameObject attackTarget;
    private float lastAttackTime;

    private Rigidbody rb;
    public float speed;

    private float stopDistance;

    bool isDead;

    private CharacterStatus characterStatus;

    private float horizontal;
    private float vertical;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();

        stopDistance = agent.stoppingDistance;
    }

    void OnEnable()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += AttackEvent;
        GameManager.Instance.RegisterPlayer(characterStatus);
    }

    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
    }

    void OnDisable()
    {
        if (!MouseManager.Instance.IsInitialized) return;
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= AttackEvent;
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);
        if (inputDirection != Vector3.zero)
        {
            Vector3 CamRelativeMove = ConvertToCameraSpace(inputDirection);
            MovePlayer(CamRelativeMove);
        }

        isDead = characterStatus.currentHealth == 0;

        if (isDead)
            GameManager.Instance.NotifyObserver();

        switchAnimation();

        lastAttackTime -= Time.deltaTime;
    }

    private void MovePlayer(Vector3 inputDirection)
    {
        Vector3 targetPosition = transform.position + inputDirection;

        agent.SetDestination(targetPosition);
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        Vector3 CamForwardZProduct = vectorToRotate.z * camForward;
        Vector3 CamRightXProduct = vectorToRotate.x * camRight;

        Vector3 vectorRotateToCameraSpace = CamForwardZProduct + CamRightXProduct;
        return vectorRotateToCameraSpace;
    }

    private void switchAnimation()
    {
        float playerSpeed = agent.velocity.sqrMagnitude;
        playerAnimator.SetFloat("Speed", playerSpeed);
        playerAnimator.SetBool("Death", isDead);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }

    public void AttackEvent(GameObject target)
    {
        if (isDead) return;
        if (target != null)
        {
            attackTarget = target;
            characterStatus.isCritical = Random.value < characterStatus.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    // Coroutine to move to attack target
    private IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStatus.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStatus.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;

        // Implement attack logic
        if (lastAttackTime < 0)
        {
            playerAnimator.SetBool("Critical", characterStatus.isCritical);
            playerAnimator.SetTrigger("Attack");
            lastAttackTime = 0.5f; // reset attack cooldown time
        }
    }

    // Animation Event
    public void Hit()
    {
        if (attackTarget.gameObject.CompareTag("Attackable")
            && attackTarget.GetComponent<Rock>().rockState == Rock.RockState.HitNothing)
        {
            if (attackTarget.GetComponent<Rock>())
            {
                attackTarget.GetComponent<Rock>().rockState = Rock.RockState.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStatus = attackTarget.GetComponent<CharacterStatus>();
            targetStatus.TakeDamage(characterStatus, targetStatus);
        }

    }
}
