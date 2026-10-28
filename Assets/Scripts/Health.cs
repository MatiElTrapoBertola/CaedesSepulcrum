using System;
using UnityEngine;
using UnityEngine.Events;

// Este script es para manejar la vida de cualquier entidad que tenga este script, permitiendo recibir daño, regeneración y eventos relacionados con la salud
[System.Serializable]
public class HealthChangedEvent : UnityEvent<float, float> { }

public class Health : MonoBehaviour, IDamageable
{
    [Header("Configuración de Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    // El valor actual de vida se inicializa en el valor máximo al inicio del juego
    [Header("Regeneración (Habilidades Pasivas)")]
    public float healthRegenPerSecond = 0f;

    [Header("Estados")]
    public bool isInvulnerable = false;
    [Range(0f, 1f)]
    public float damageReduction = 0f;
    // El valor de reducción de daño se aplica como un porcentaje
    [Header("Eventos")]
    public HealthChangedEvent onHealthChanged;
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (!isDead && healthRegenPerSecond > 0 && currentHealth < maxHealth)
        {
            Heal(healthRegenPerSecond * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead || isInvulnerable) return;

        float finalDamage = amount * (1f - Mathf.Clamp01(damageReduction));
        currentHealth = Mathf.Max(currentHealth - finalDamage, 0f);

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

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        onDeath?.Invoke();
    }
}