using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

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
    [SerializeField] private ParticleSystem plegariaFX;
    [SerializeField] private float sinPiedadDuration = 8f;
    [SerializeField] private float sinPiedadCooldown = 30f;



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





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivatePlegaria();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateMensajeroConPrisa();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ActivateBlessing();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateSinPiedad();
        }
    }

    public void ActivatePlegaria()
    {
        if (!canUsePlegaria)
            return;

        StartCoroutine(PlegariaCoroutine());
    }

    private IEnumerator PlegariaCoroutine()
    {
        canUsePlegaria = false;


        IsInvulnerable = true;

        if (plegariaFX != null)
        {
            plegariaFX.Play();
        }

        health.SetInvulnerable(true);

        yield return new WaitForSeconds(plegariaDuration);

        health.SetInvulnerable(false);

        if (plegariaFX != null)
        {
            plegariaFX.Stop();
        }

        IsInvulnerable = false;

        yield return new WaitForSeconds(plegariaCooldown);

        canUsePlegaria = true;
    }

    public void ActivateMensajeroConPrisa()
    {
        if (!canUseMensajero)
            return;
        if (!isMensajeroActive)
        {
            StartCoroutine(MensajeroConPrisaCoroutine());
        }
        
    }


    private IEnumerator MensajeroConPrisaCoroutine()
    {
        canUseMensajero = false;

        isMensajeroActive = true;

        stamina.FillStamina();

        InfiniteResources = true;

        NoKnockback = true;

        movement.SpeedBoostActive = true;

        yield return new WaitForSeconds(mensajeroDuration);

        movement.SpeedBoostActive = false;

        NoKnockback = false;

        InfiniteResources = false;

        isMensajeroActive = false;

        yield return new WaitForSeconds(mensajeroCooldown);

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
        if (!canUseBlessing)
            return;

        StartCoroutine(BlessingCoroutine());
    }


    private IEnumerator BlessingCoroutine()
    {
        canUseBlessing = false;

        damageManager.ActivateBlessing();

        yield return new WaitForSeconds(blessingCooldown);

        canUseBlessing = true;
    }

    public void ActivateSinPiedad()
    {
        if (!canUseSinPiedad)
            return;

        StartCoroutine(SinPiedadCoroutine());
        
    }

    private IEnumerator SinPiedadCoroutine()
    {
        canUseSinPiedad = false;

        SinPiedadActive = true;

        Debug.Log("Sin Piedad activada");

        yield return new WaitForSeconds(sinPiedadDuration);

        SinPiedadActive = false;

        Debug.Log("Sin Piedad terminada");

        yield return new WaitForSeconds(sinPiedadCooldown);

        canUseSinPiedad = true;
    }
}



