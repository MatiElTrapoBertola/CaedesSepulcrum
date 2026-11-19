using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        fillImage.fillAmount = currentHealth / maxHealth;
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}