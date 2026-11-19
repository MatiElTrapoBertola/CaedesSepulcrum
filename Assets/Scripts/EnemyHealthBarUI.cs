using System.Collections;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Health health;

    [Header("Health Bar")]
    [SerializeField] private EnemyHealthBar healthBarPrefab;

    [SerializeField]
    private Vector3 barOffset = new Vector3(0f, 2.2f, 0f);

    [SerializeField] private float hideDelay = 3f;

    private EnemyHealthBar healthBarInstance;
    private Coroutine hideCoroutine;

    private void Start()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
        }

        if (healthBarPrefab == null)
        {
            Debug.LogError(gameObject.name + " no tiene asignado Health Bar Prefab");
            return;
        }

        healthBarInstance = Instantiate(
            healthBarPrefab,
            transform);

        Debug.Log("Barra creada para: " + gameObject.name);

        // Ahora usa el valor configurado en el Inspector
        healthBarInstance.transform.localPosition = barOffset;

        healthBarInstance.gameObject.SetActive(false);

        health.onHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.onHealthChanged.RemoveListener(OnHealthChanged);
        }
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        if (healthBarInstance == null)
            return;

        healthBarInstance.SetHealth(currentHealth, maxHealth);

        healthBarInstance.gameObject.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(hideDelay);

        if (healthBarInstance != null &&
            health != null &&
            !health.IsDead)
        {
            healthBarInstance.gameObject.SetActive(false);
        }
    }
}