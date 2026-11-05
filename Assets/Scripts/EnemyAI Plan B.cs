using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private Health playerHealth;
    private Animator animator;

    [Header("Detection")]
    [SerializeField] private float chaseRange = 15f;
    [SerializeField] private float runThreshold = 3.5f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    private float nextAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if(player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if(playerObject != null)
            {
                player = playerObject.transform;
                playerHealth = playerObject.GetComponent<Health>();
            }
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }

        if (distance <= attackRange)
        {
            Attack();
        }

        UpdateAnimatorParameters();
        
    }

    private void UpdateAnimatorParameters()
    {
        if (animator == null)
            return;

        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        animator.SetFloat("MoveX", localVelocity.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", localVelocity.z, 0.1f, Time.deltaTime);

        bool isRunning = agent.velocity.magnitude > runThreshold;
        animator.SetBool("isRunning", isRunning);
    }

    private void Attack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        if(animator != null)
        {
            animator.SetInteger("AttackIndex", Random.Range(0,2));
            animator.SetTrigger("Attack");
        }

        if(playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }


   
}
