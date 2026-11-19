using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActiveSkills : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float plegariaDuration = 3f;
    [SerializeField] private float plegariaCooldown = 20f;
    [SerializeField] private PlayerMovement3D movement;
    [SerializeField] private PlayerStamina stamina;
    [SerializeField] private float mensajeroDuration = 5f;
    [SerializeField] private float mensajeroCooldown = 20f;
    [SerializeField] private PlayerDamageManager damageManager;
    [SerializeField] private float blessingCooldown = 25f;
    [SerializeField] private float blessingFXDuration = 3f;

    [SerializeField] private float sinPiedadDuration = 8f;
    [SerializeField] private float sinPiedadCooldown = 30f;

    [Header("Efectos de Partículas (Habilidades)")]
    [SerializeField] private GameObject plegariaFXPrefab;
    [SerializeField] private GameObject mensajeroFXPrefab;
    [SerializeField] private GameObject blessingFXPrefab;
    [SerializeField] private GameObject sinPiedadFXPrefab;

    private bool canUsePlegaria = true;
    public bool canUseMensajero = true;
    public bool IsInvulnerable { get; private set; }
    public bool InfiniteResources { get; private set; }
    public bool NoKnockback { get; private set; }
    public bool DivineAttacks { get; private set; }
    public bool isMensajeroActive;
    public bool canUseBlessing = true;
    private bool canUseSinPiedad = true;
    public bool SinPiedadActive { get; private set; }
    public float PlegariaCooldownRemaining { get; private set; }
    public float MensajeroCooldownRemaining { get; private set; }
    public float BlessingCooldownRemaining { get; private set; }
    public float SinPiedadCooldownRemaining { get; private set; }
    public float PlegariaCooldown => plegariaCooldown;
    public float MensajeroCooldown => mensajeroCooldown;
    public float BlessingCooldown => blessingCooldown;
    public float SinPiedadCooldown => sinPiedadCooldown;
    


    public void OnPlegaria(InputAction.CallbackContext context)
    {
        if (context.performed) ActivatePlegaria();
    }

    public void OnMensajero(InputAction.CallbackContext context)
    {
        if (context.performed) ActivateMensajeroConPrisa();
    }

    public void OnBlessing(InputAction.CallbackContext context)
    {
        if (context.performed) ActivateBlessing();
    }

    public void OnSinPiedad(InputAction.CallbackContext context)
    {
        if (context.performed) ActivateSinPiedad();
    }

    public void ActivatePlegaria()
    {
        if (!canUsePlegaria) return;
        StartCoroutine(PlegariaCoroutine());
    }

    private IEnumerator PlegariaCoroutine()
    {
        canUsePlegaria = false;

        IsInvulnerable = true;

        SpawnSkillFX(plegariaFXPrefab, plegariaDuration);

        health.SetInvulnerable(true);

        yield return new WaitForSeconds(plegariaDuration);

        health.SetInvulnerable(false);


        IsInvulnerable = false;

        yield return StartCoroutine(CooldownRoutine(plegariaCooldown, value => PlegariaCooldownRemaining = value));

        canUsePlegaria = true;
    }

    public void ActivateMensajeroConPrisa()
    {
        if (!canUseMensajero || isMensajeroActive) return;
        StartCoroutine(MensajeroConPrisaCoroutine());
    }

    private IEnumerator MensajeroConPrisaCoroutine()
    {
        canUseMensajero = false;

        isMensajeroActive = true;

        stamina.FillStamina();

        InfiniteResources = true;

        NoKnockback = true;

        movement.SpeedBoostActive = true;


        SpawnSkillFX(mensajeroFXPrefab, mensajeroDuration);

        yield return new WaitForSeconds(mensajeroDuration);

        movement.SpeedBoostActive = false;

        NoKnockback = false;

        InfiniteResources = false;

        isMensajeroActive = false;


        yield return StartCoroutine(CooldownRoutine(mensajeroCooldown, value => MensajeroCooldownRemaining = value));

        canUseMensajero = true;
    }

    private void Start()
    {
        Debug.Log("Health: " + health);
        Debug.Log("Stamina: " + stamina);
        Debug.Log("Movement: " + movement);
    }

    public void ActivateBlessing()
    {
        if (!canUseBlessing) return;
        StartCoroutine(BlessingCoroutine());
    }

    private IEnumerator BlessingCoroutine()
    {
        canUseBlessing = false;

        damageManager.ActivateBlessing();


        SpawnSkillFX(blessingFXPrefab, blessingFXDuration);

        yield return StartCoroutine(CooldownRoutine(blessingCooldown, value => BlessingCooldownRemaining = value));

        canUseBlessing = true;
    }

    public void ActivateSinPiedad()
    {
        if (!canUseSinPiedad) return;
        StartCoroutine(SinPiedadCoroutine());
    }

    private IEnumerator SinPiedadCoroutine()
    {
        canUseSinPiedad = false;

        SinPiedadActive = true;

        Debug.Log("Sin Piedad activada");


        SpawnSkillFX(sinPiedadFXPrefab, sinPiedadDuration);

        yield return new WaitForSeconds(sinPiedadDuration);

        SinPiedadActive = false;

        Debug.Log("Sin Piedad terminada");

        yield return StartCoroutine(CooldownRoutine(sinPiedadCooldown, value => SinPiedadCooldownRemaining = value));

        canUseSinPiedad = true;
    }

    private IEnumerator CooldownRoutine(float duration, System.Action<float> setValue)
    {
        float timer = duration;

        while(timer > 0)
        {
            setValue(timer);

            timer -= Time.deltaTime;

            yield return null;
        }

        setValue(0);
    }

    private GameObject SpawnSkillFX(GameObject prefab, float duration)
    {
        if (prefab == null)
            return null;

        GameObject fx = Instantiate(
            prefab,
            transform
        );

        Destroy(fx, duration);

        return fx;
    }

}