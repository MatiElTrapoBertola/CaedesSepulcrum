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

    [Header("Invulnerability Frames")]
    [SerializeField] private bool enableDamageInvulnerability = true;

    [SerializeField] private bool enableBlinkEffect = false;

    [SerializeField] private float damageInvulnerabilityTime = 0.5f;

    [SerializeField] private float blinkInterval = 0.1f;

    [Header("Eventos")]
    public HealthChangedEvent onHealthChanged;
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    [Header("Damage Popup")]
    [SerializeField] private DamagePopup damagePopupPrefab;

    [Header("Hit Audio")]
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField][Range(0f, 2f)] private float hitVolume = 1f;
    [SerializeField] private float hitMinPitch = 0.95f;
    [SerializeField] private float hitMaxPitch = 1.05f;

    private Renderer[] renderers;
    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        renderers = GetComponentsInChildren<Renderer>();

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
        if (!isDead &&
            healthRegenPerSecond > 0 &&
            currentHealth < maxHealth)
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

        if (isDead || isInvulnerable)
            return;

        float finalDamage =
            amount * (1f - Mathf.Clamp01(damageReduction));

        currentHealth =
            Mathf.Max(currentHealth - finalDamage, 0f);

        ShowDamagePopup(finalDamage);

        PlayHitSound();

        if (enableDamageInvulnerability)
        {
            if (enableBlinkEffect)
            {
                StartCoroutine(DamageInvulnerabilityRoutine());
            }
            else
            {
                StartCoroutine(BasicInvulnerabilityRoutine());
            }
        }

        Debug.Log(
            gameObject.name +
            " recibio dano: " +
            finalDamage +
            " | Vida restante: " +
            currentHealth);

        onTakeDamage?.Invoke();

        onHealthChanged?.Invoke(
            currentHealth,
            maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }

    }

    public void Heal(float amount)
    {
        if (isDead)
            return;

        currentHealth =
            Mathf.Min(currentHealth + amount, maxHealth);

        onHealthChanged?.Invoke(
            currentHealth,
            maxHealth);
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
        if (isDead)
            return;

        isDead = true;

        onDeath?.Invoke();
    }

    private System.Collections.IEnumerator BasicInvulnerabilityRoutine()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(
            damageInvulnerabilityTime);

        isInvulnerable = false;
    }

    private System.Collections.IEnumerator DamageInvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsedTime = 0f;
        bool visible = true;

        while (elapsedTime < damageInvulnerabilityTime)
        {
            visible = !visible;

            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = visible;
                }
            }

            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval;
        }

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }

        isInvulnerable = false;
    }

    private void ShowDamagePopup(float damage)
    {
        if (damagePopupPrefab == null)
            return;

        Vector3 spawnPosition = transform.position + Vector3.up * 2f + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));

        DamagePopup popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);

        popup.Setup(damage);
    }

    private void PlayHitSound()
    {
        if (hitClips == null || hitClips.Length == 0)
            return;

        AudioClip selectedClip =
            hitClips[Random.Range(0, hitClips.Length)];

        float randomPitch =
            Random.Range(hitMinPitch, hitMaxPitch);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                selectedClip,
                hitVolume,
                randomPitch);
        }
    }

}