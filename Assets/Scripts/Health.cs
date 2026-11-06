using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthChangedEvent : UnityEvent<float, float> { }

public class Health : MonoBehaviour, IDamageable
{
    [Header("Configuración de Vida")]
    [SerializeField] private PlayerSkillsManager skillsManager;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Animator Reference")]
    [SerializeField] private Animator animator;

    [Header("Regeneración (Habilidades Pasivas)")]
    public float healthRegenPerSecond = 0f;

    [Header("Estados")]
    public bool isInvulnerable = false;
    [Range(0f, 1f)]
    public float damageReduction = 0f;

    [Header("Eventos")]
    public HealthChangedEvent onHealthChanged;
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        // Búsqueda segura del Animator en el objeto o en sus hijos (modelo 3D)[cite: 6]
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
    }

    private void Start()
    {
        RefreshStats();
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (!isDead && healthRegenPerSecond > 0 && currentHealth < maxHealth)
        {
            Heal(healthRegenPerSecond * Time.deltaTime);
        }
    }

    private void RefreshStats()
    {
        if (skillsManager == null)
            return;

        damageReduction = skillsManager.GetDamageReduction();
        healthRegenPerSecond = skillsManager.GetHealthRegenPerSecond();
    }

    public void TakeDamage(float amount)
    {
        if (isDead || isInvulnerable) return;

        float finalDamage = amount * (1f - Mathf.Clamp01(damageReduction));
        currentHealth = Mathf.Max(currentHealth - finalDamage, 0f);

        // Mensaje de prueba para verificar en la consola si el script recibe el daño
        Debug.Log("¡Daño recibido! Cantidad: " + finalDamage + " | Vida restante: " + currentHealth);

        // Dispara el trigger "HitReaction" en el Animator para la animación de daño[cite: 6]
        if (animator != null)
        {
            animator.SetTrigger("HitReaction");
        }

        onTakeDamage?.Invoke();
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Dispara el trigger "Dying" en el Animator para la animación de muerte[cite: 6]
        if (animator != null)
        {
            animator.SetTrigger("Dying");
        }

        onDeath?.Invoke();
    }
}