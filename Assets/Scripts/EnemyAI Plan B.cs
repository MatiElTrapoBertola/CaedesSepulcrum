using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private Health playerHealth;
    private Health zombieHealth;
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

        // Búsqueda segura del Animator en el modelo hijo del zombi
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Obtenemos la vida propia del zombi para escuchar eventos de daño y muerte
        zombieHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (zombieHealth != null)
        {
            zombieHealth.onTakeDamage.AddListener(TriggerHitAnimation);
            zombieHealth.onDeath.AddListener(TriggerDeathAnimation);
        }
    }

    private void OnDisable()
    {
        if (zombieHealth != null)
        {
            zombieHealth.onTakeDamage.RemoveListener(TriggerHitAnimation);
            zombieHealth.onDeath.RemoveListener(TriggerDeathAnimation);
        }
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
                playerHealth = playerObject.GetComponent<Health>();
            }
        }
    }

    private void Update()
    {
        // Si el zombi está muerto, detenemos su movimiento y lógica
        if (zombieHealth != null && zombieHealth.IsDead)
        {
            agent.ResetPath();
            return;
        }

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

        if (animator != null)
        {
            animator.SetTrigger("ZombieAttack");
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    private void TriggerHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("ZombieHit");
        }
    }

    private void TriggerDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("ZombieDeath");
        }
    }
}