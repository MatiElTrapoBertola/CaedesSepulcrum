using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    [SerializeField] private float attackCooldown = 2.5f;

    [SerializeField] private float attackDuration = 1.2f;

    private float nextAttackTime;

    private bool isAttacking;


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
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
                playerHealth =
                    playerObject.GetComponent<Health>();
            }
        }

        if (agent != null)
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                transform.position,
                out hit,
                10f,
                NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }
    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        if (zombieHealth != null && zombieHealth.IsDead)
        {
            if (agent.hasPath)
                agent.ResetPath();

            return;
        }

        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position);

        if (!isAttacking)
        {
            if (distance <= chaseRange)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                if (agent.hasPath)
                    agent.ResetPath();
            }
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
        //Debug.Log("Attack() llamado");

        if (isAttacking)
        {
            //Debug.Log("Im attacking");
            return;
        }
           

        if (Time.time < nextAttackTime || isAttacking)
        {
            //Debug.Log("Enemy Attack in cooldown");
            return;
        }

        StartCoroutine(AttackRoutine());
    }

    private void TriggerHitAnimation()
    {
        if (zombieHealth != null && zombieHealth.IsDead)
            return;

        if (animator != null)
        {
            animator.SetTrigger("ZombieHit");
        }
    }



    private void TriggerDeathAnimation()
    {
        Debug.Log("Trigger de muerte activado");

        if (animator != null)
        {
            animator.ResetTrigger("ZombieHit");
            animator.ResetTrigger("ZombieAttack");
            animator.SetTrigger("ZombieDeath");
        }
    }

    private int attackNumber = 0;

    private System.Collections.IEnumerator AttackRoutine()
    {
        attackNumber++;

        Debug.Log($"Ataque #{attackNumber}");

        isAttacking = true;

        nextAttackTime = Time.time + attackCooldown;

        agent.isStopped = true;

        if (animator != null)
        {
            animator.SetTrigger("ZombieAttack");
            //Debug.Log("Trigger ZombieAttack");
        }

        yield return new WaitForSeconds(attackDuration);

        if(agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }

        isAttacking = false;

        //Debug.Log("Attack ended");
    }

    public void DealZombieDamage()
    {
        if (player == null || playerHealth == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position);

        if (distance <= attackRange)
        {
            playerHealth.TakeDamage(attackDamage);

            KnockbackReceiver knockback =
    playerHealth.GetComponent<KnockbackReceiver>();

            if (knockback != null)
            {
                knockback.ApplyKnockback(
                    transform.position,
                    4f);
            }
        }
    }
}