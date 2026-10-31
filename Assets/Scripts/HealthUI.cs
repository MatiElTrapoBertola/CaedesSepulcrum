using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Slider healthSlider;

    private void Update()
    {
        healthSlider.value =
            playerHealth.CurrentHealth /
            playerHealth.MaxHealth;
    }
}