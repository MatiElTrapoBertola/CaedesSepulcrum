using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image fill;
    public Image trail;

    public Gradient colorHealth;
    public float trailDelay = 0.3f;
    public float trailSpeed = 0.4f;
    private Coroutine trailCoroutine;

    [SerializeField] private TextMeshProUGUI enemyCount;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private Health playerHealth;

    private int enemyCountIndex = 0;

    private void Start()
    {
       
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        if (playerHealth != null)
        {
            playerHealth.onHealthChanged.AddListener(OnHealthChanged);

         
            OnHealthChanged(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
        else
        {
            Debug.LogWarning("HUD no pudo encontrar el componente Health del jugador.");
        }
    }

    private void OnDestroy()
    {
      
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged.RemoveListener(OnHealthChanged);
        }
    }

    void OnHealthChanged(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0) return;

        float percent = currentHealth / maxHealth;
        fill.fillAmount = percent;
        fill.color = colorHealth.Evaluate(percent);

        if (trailCoroutine != null)
        {
            StopCoroutine(trailCoroutine);
        }

        if (percent >= trail.fillAmount)
        {
            trail.fillAmount = percent;
        }
        else
        {
            trailCoroutine = StartCoroutine(AnimeTrail(percent));
        }
    }

    IEnumerator AnimeTrail(float targetAmount)
    {
        yield return new WaitForSeconds(trailDelay);
        while (trail.fillAmount > targetAmount)
        {
            trail.fillAmount -= Mathf.MoveTowards(trail.fillAmount, targetAmount, trailSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void EnemyDefeated(GameObject enemy)
    {
        enemyCountIndex++;
        if (enemyCount != null)
        {
            enemyCount.text = "Bestias Derrotadas: " + enemyCountIndex.ToString();
        }
        Destroy(enemy);
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        if (ammoCountText != null)
        {
            ammoCountText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}