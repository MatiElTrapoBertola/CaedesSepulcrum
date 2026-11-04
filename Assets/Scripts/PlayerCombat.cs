
using UnityEngine;

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
    [SerializeField] [Range(0f,2f)] private float shovelSwingVolume = 1f;
    [SerializeField] [Range(0f,2f)] private float shovelImpactVolume = 1f;
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
        animator = GetComponent<Animator>();

        currentAmmo = maxAmmo;

        UpdateWeaponVisuals();

        if (animator != null)
        {
            animator.SetInteger("WeaponType", (int)currentWeapon);
        }
    }



    public enum WeaponType
    {
        Shovel, Pistol
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeapon = currentWeapon == WeaponType.Shovel ? WeaponType.Pistol : WeaponType.Shovel;

            UpdateWeaponVisuals();
            
            if(animator != null)
            {
                animator.SetInteger("WeaponType", (int)currentWeapon);
            }

            PlayWeaponSwapSound();

            Debug.Log("Arma actual: " + currentWeapon);
        }

        if (Input.GetMouseButtonDown(0))
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            Reload();
        }
    }

    private void PerformShovelAttack()
    {

        if(animator != null)
        {
            animator.SetInteger("AttackIndex", Random.Range(0, 3));
            animator.SetTrigger("Attack");
        }

        

        if (!activeSkills.SinPiedadActive)
        {
            if (!stamina.ConsumeStamina(attackStaminaCost))
            return;
        }

        PlayShovelSound();

        Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward, attackRange,enemyLayer);

        float damage = damageManager.GetFinalDamage();


        foreach (Collider enemy in enemies)
        {

        

          Health enemyHealth = enemy.GetComponent<Health>();

          if (enemyHealth != null)
          {
            PlayShovelImpactSound(enemy.transform.position);

            bool divineAttack = damageManager.ConsumeDivineHit();

            enemyHealth.TakeDamage(damage);

            if (divineAttack)
            {
                CreateDivineExplosion(enemy.transform.position);
            }

            Debug.Log( enemy.name + " recibió " + damage + " puntos de daño.");
          }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position + transform.forward,
            attackRange);
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

    private void PerformPistolAttack()
    {
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;

        Debug.Log("Pistola disparada");

        if (!activeSkills.SinPiedadActive)
        {
            if (currentAmmo <= 0)
            {
                Debug.Log("Sin municion");
                return;
            }

            currentAmmo--;

            Debug.Log("Municion restante : " + currentAmmo + "/" + maxAmmo);
        }

        Quaternion bulletRotation = Quaternion.LookRotation(playerCamera.transform.forward);

        Bullet newBullet = Instantiate(bullet, weaponCannon.position, bulletRotation);

        PlayPistolShotSound();

        float damage = damageManager.GetFinalDamage();

        bool divineAttack = damageManager.ConsumeDivineHit();

        newBullet.Initialize(damage, divineAttack, explosionRadius, explosionDamage, enemyLayer);

       
    }

        

       

    public void Reload()
    {
        currentAmmo = maxAmmo;

        Debug.Log("Recargando");
    }

    private void PlayPistolShotSound()
    {
        if (pistolAudioSource == null) return;

        if (pistolShotClips == null || pistolShotClips.Length == 0) return;

        AudioClip selectedClip = pistolShotClips[Random.Range(0, pistolShotClips.Length)];

        if (randomPitch)
        {
            pistolAudioSource.pitch = Random.Range(pistolMinPitch, pistolMaxPitch);
        }

        pistolAudioSource.PlayOneShot(selectedClip, pistolVolume);

        pistolAudioSource.pitch = 1f;
    }

    private void PlayShovelSound()
    {
        if (shovelAudioSource == null) return;

        if (shovelSwingClips == null || shovelSwingClips.Length == 0) return;

        AudioClip selectedClip = shovelSwingClips[Random.Range(0, shovelSwingClips.Length)];

        shovelAudioSource.pitch = Random.Range(shovelMinPitch, shovelMaxPitch);

        shovelAudioSource.PlayOneShot(selectedClip, shovelSwingVolume);

        shovelAudioSource.pitch = 1f;
    }

    private void PlayShovelImpactSound(Vector3 hitPosition)
    {
        if (shovelHitClips == null || shovelHitClips.Length == 0)
            return;

        AudioClip selectedClip = shovelHitClips[Random.Range(0, shovelHitClips.Length)];

        float randomPitch = Random.Range(shovelHitMinPitch, shovelHitMaxPitch);

        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(selectedClip, shovelImpactVolume, randomPitch);
        }
           
      
    }

    private void PlayWeaponSwapSound()
    {
        if (weaponSwapAudioSource == null)
            return;

        if (weaponSwapClips == null || weaponSwapClips.Length == 0)
            return;

        AudioClip selectedClip = weaponSwapClips[Random.Range(0, weaponSwapClips.Length)];

        weaponSwapAudioSource.pitch = Random.Range(weaponSwapMinPitch, weaponSwapMaxPitch);

        weaponSwapAudioSource.PlayOneShot(selectedClip, weaponSwapVolume);

        weaponSwapAudioSource.pitch = 1f;
    }

    private void UpdateWeaponVisuals()
    {
        shovelObject.SetActive(currentWeapon == WeaponType.Shovel);

        pistolObject.SetActive(currentWeapon == WeaponType.Pistol);
    }
    
    private Vector3 GetAimPoint()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, aimLayers))
        {
            return hit.point;
        }

        return ray.origin + ray.direction * 100f;
    }
       

    
}
