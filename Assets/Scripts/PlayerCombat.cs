using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Weapon")]
    [SerializeField] private WeaponType currentWeapon = WeaponType.Shovel;

    [Header("Weapon Swap Audio")]
    [SerializeField][Range(0f, 2f)] private float weaponSwapVolume = 1f;
    [SerializeField] private AudioSource weaponSwapAudioSource;
    [SerializeField] private AudioClip[] weaponSwapClips;
    [SerializeField] private float weaponSwapMinPitch = 0.95f;
    [SerializeField] private float weaponSwapMaxPitch = 1.05f;

    [Header("WeaponModels")]
    [SerializeField] private GameObject shovelObject;
    [SerializeField] private GameObject pistolObject;

    [Header("Shovel")]
    [SerializeField] private float attackStaminaCost = 10f;
    [Header("Shovel Audio")]
    [SerializeField][Range(0f, 2f)] private float shovelSwingVolume = 1f;
    [SerializeField][Range(0f, 2f)] private float shovelImpactVolume = 1f;
    [SerializeField] private AudioSource shovelAudioSource;
    [SerializeField] private AudioClip[] shovelSwingClips;
    [SerializeField] private float shovelMinPitch = 0.9f;
    [SerializeField] private float shovelMaxPitch = 1.1f;
    [SerializeField] private AudioClip[] shovelHitClips;
    [SerializeField] private float shovelHitMinPitch = 0.95f;
    [SerializeField] private float shovelHitMaxPitch = 0.95f;

    [Header("References")]
    [SerializeField] private PlayerDamageManager damageManager;

    [Header("Pistol")]
    [SerializeField] private int maxAmmo = 12;
    [SerializeField] private Transform weaponCannon;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private Bullet bullet;
    private int currentAmmo;
    private float nextFireTime;

    [Header("Pistol Audio")]
    [SerializeField][Range(0f, 2f)] private float pistolVolume = 1f;
    [SerializeField] private AudioSource pistolAudioSource;
    [SerializeField] private AudioClip[] pistolShotClips;
    [SerializeField] private bool randomPitch = true;
    [SerializeField] private float pistolMinPitch = 0.95f;
    [SerializeField] private float pistolMaxPitch = 1.05f;

    [Header("Pistol Reload Audio")]
    [SerializeField][Range(0f, 2f)] private float reloadVolume = 1f;
    [SerializeField] private AudioSource reloadAudioSource;
    [SerializeField] private AudioClip[] reloadClips;
    [SerializeField] private float reloadMinPitch = 0.95f;
    [SerializeField] private float reloadMaxPitch = 1.05f;

    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float explosionDamage = 15f;
    [SerializeField] private ParticleSystem divineExplosionFX;
    [SerializeField] private PlayerActiveSkills activeSkills;
    [SerializeField] private PlayerStamina stamina;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask aimLayers;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("PlayerCombat no encontró ningún Animator en el jugador ni en sus hijos.");
        }

        currentAmmo = maxAmmo;
        UpdateWeaponVisuals();

        if (animator != null)
        {
            animator.SetInteger("WeaponType", (int)currentWeapon);
        }
    }

    public enum WeaponType
    {
        Shovel = 0, Pistol = 1
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentWeapon = currentWeapon == WeaponType.Shovel ? WeaponType.Pistol : WeaponType.Shovel;
            UpdateWeaponVisuals();

            if (animator != null)
            {
                animator.SetInteger("WeaponType", (int)currentWeapon);
            }

            PlayWeaponSwapSound();
            Debug.Log("Arma actual: " + currentWeapon);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (currentWeapon)
            {
                case WeaponType.Shovel:
                    PerformShovelAttack();
                    break;
                case WeaponType.Pistol:
                    PerformPistolAttack();
                    break;
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Reload();
        }
    }

    private void PerformShovelAttack()
    {
        if (animator != null)
        {
            int randomCombo = Random.Range(0, 3);

            if (randomCombo == 0)
            {
                animator.SetTrigger("Melee1");
            }
            else if (randomCombo == 1)
            {
                animator.SetTrigger("Melee2");
            }
            else
            {
                animator.SetTrigger("Melee3");
            }
        }

        if (activeSkills == null || !activeSkills.SinPiedadActive)
        {
            if (stamina != null && !stamina.ConsumeStamina(attackStaminaCost))
                return;
        }

        PlayShovelSound();

        Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward, attackRange, enemyLayer);
        float damage = damageManager != null ? damageManager.GetFinalDamage() : 10f;

        foreach (Collider enemy in enemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();

            if (enemyHealth != null)
            {
                PlayShovelImpactSound(enemy.transform.position);
                bool divineAttack = damageManager != null && damageManager.ConsumeDivineHit();

                enemyHealth.TakeDamage(damage);

                if (divineAttack)
                {
                    CreateDivineExplosion(enemy.transform.position);
                }
            }
        }
    }

    private void PerformPistolAttack()
    {
        if (Time.time < nextFireTime)
            return;

        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        nextFireTime = Time.time + fireRate;

        if (activeSkills == null || !activeSkills.SinPiedadActive)
        {
            if (currentAmmo <= 0)
            {
                Debug.Log("Sin municion");
                return;
            }

            currentAmmo--;
        }

        if (bullet != null && weaponCannon != null && playerCamera != null)
        {
            Quaternion bulletRotation = Quaternion.LookRotation(playerCamera.transform.forward);
            Bullet newBullet = Instantiate(bullet, weaponCannon.position, bulletRotation);

            float damage = damageManager != null ? damageManager.GetFinalDamage() : 10f;
            bool divineAttack = damageManager != null && damageManager.ConsumeDivineHit();

            newBullet.Initialize(damage, divineAttack, explosionRadius, explosionDamage, enemyLayer);
        }

        PlayPistolShotSound();
    }

    private void CreateDivineExplosion(Vector3 position)
    {
        if (divineExplosionFX != null)
        {
            Instantiate(divineExplosionFX, position, Quaternion.identity);
        }

        Collider[] enemies = Physics.OverlapSphere(position, explosionRadius, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(explosionDamage);
            }
        }

        Debug.Log("Explosion divina creada");
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        PlayReloadSound();
        Debug.Log("Recargando");
    }

    private void PlayPistolShotSound()
    {
        if (pistolAudioSource == null || pistolShotClips == null || pistolShotClips.Length == 0) return;
        AudioClip selectedClip = pistolShotClips[Random.Range(0, pistolShotClips.Length)];
        if (randomPitch) pistolAudioSource.pitch = Random.Range(pistolMinPitch, pistolMaxPitch);
        pistolAudioSource.PlayOneShot(selectedClip, pistolVolume);
        pistolAudioSource.pitch = 1f;
    }

    private void PlayReloadSound()
    {
        if (reloadAudioSource == null || reloadClips == null || reloadClips.Length == 0) return;
        AudioClip selectedClip = reloadClips[Random.Range(0, reloadClips.Length)];
        reloadAudioSource.pitch = Random.Range(reloadMinPitch, reloadMaxPitch);
        reloadAudioSource.PlayOneShot(selectedClip, reloadVolume);
        reloadAudioSource.pitch = 1f;
    }

    private void PlayShovelSound()
    {
        if (shovelAudioSource == null || shovelSwingClips == null || shovelSwingClips.Length == 0) return;
        AudioClip selectedClip = shovelSwingClips[Random.Range(0, shovelSwingClips.Length)];
        shovelAudioSource.pitch = Random.Range(shovelMinPitch, shovelMaxPitch);
        shovelAudioSource.PlayOneShot(selectedClip, shovelSwingVolume);
        shovelAudioSource.pitch = 1f;
    }

    private void PlayShovelImpactSound(Vector3 hitPosition)
    {
        if (shovelHitClips == null || shovelHitClips.Length == 0) return;
        AudioClip selectedClip = shovelHitClips[Random.Range(0, shovelHitClips.Length)];
        float randomPitchVal = Random.Range(shovelHitMinPitch, shovelHitMaxPitch);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(selectedClip, shovelImpactVolume, randomPitchVal);
        }
    }

    private void PlayWeaponSwapSound()
    {
        if (weaponSwapAudioSource == null || weaponSwapClips == null || weaponSwapClips.Length == 0) return;
        AudioClip selectedClip = weaponSwapClips[Random.Range(0, weaponSwapClips.Length)];
        weaponSwapAudioSource.pitch = Random.Range(weaponSwapMinPitch, weaponSwapMaxPitch);
        weaponSwapAudioSource.PlayOneShot(selectedClip, weaponSwapVolume);
        weaponSwapAudioSource.pitch = 1f;
    }

    private void UpdateWeaponVisuals()
    {
        if (shovelObject != null) shovelObject.SetActive(currentWeapon == WeaponType.Shovel);
        if (pistolObject != null) pistolObject.SetActive(currentWeapon == WeaponType.Pistol);
    }
}