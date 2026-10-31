using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float regenDelay = 2f;
    private float lastStaminaUseTime;



    [Header("Regeneration")]
    [SerializeField] private float regenRate = 15f;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        RegenerateStamina();

       
    }

    private void RegenerateStamina()
    {
        if (Time.time < lastStaminaUseTime + regenDelay) return;

        if (currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    public bool HasEnoughStamina(float amount)
    {
        return currentStamina >= amount;
    }

    public bool ConsumeStamina(float amount)
    {
        if (currentStamina < amount)
            return false;

        currentStamina -= amount;

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        
        lastStaminaUseTime = Time.time;
        return true;
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    public void FillStamina()
    {
        currentStamina = maxStamina;
    }
}