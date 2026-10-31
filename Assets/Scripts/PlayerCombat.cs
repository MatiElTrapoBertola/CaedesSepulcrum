using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Weapon")]
    [SerializeField] private WeaponType currentWeapon = WeaponType.Shovel;

    [Header("WeaponModels")]
    [SerializeField] private GameObject shovelObject;
    [SerializeField] private GameObject pistolObject;

    [Header("Shovel")]
    [SerializeField] private float attackStaminaCost = 10f;

    [Header("References")]
    [SerializeField] private PlayerDamageManager damageManager;

    [Header("Pistol")]
    [SerializeField] private int maxAmmo = 12;
    [SerializeField] private float rangedAttackDistance = 30f;
    [SerializeField] private Transform weaponCannon;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private Bullet bullet;
    private int currentAmmo;
    private float nextFireTime;

    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float explosionDamage = 15f;
    [SerializeField] private ParticleSystem divineExplosionFX;
    [SerializeField] private PlayerActiveSkills activeSkills;
    [SerializeField] private PlayerStamina stamina;
    
    
    
    private void Start()
    {
        currentAmmo = maxAmmo;

        UpdateWeaponVisuals();
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
    if (!activeSkills.SinPiedadActive)
    {
        if (!stamina.ConsumeStamina(attackStaminaCost))
            return;
    }

    Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward, attackRange,enemyLayer);

    float damage = damageManager.GetFinalDamage();


    foreach (Collider enemy in enemies)
    {
        Health enemyHealth = enemy.GetComponent<Health>();

        if (enemyHealth != null)
        {
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

        Bullet newBullet = Instantiate(bullet, weaponCannon.position, weaponCannon.rotation);

        float damage = damageManager.GetFinalDamage();

        bool divineAttack = damageManager.ConsumeDivineHit();

        newBullet.Initialize(damage, divineAttack, explosionRadius, explosionDamage, enemyLayer);

       
    }

        

       

    public void Reload()
    {
        currentAmmo = maxAmmo;

        Debug.Log("Recargando");
    }

    private void UpdateWeaponVisuals()
    {
        shovelObject.SetActive(currentWeapon == WeaponType.Shovel);

        pistolObject.SetActive(currentWeapon == WeaponType.Pistol);
    }
    
       
    
}
