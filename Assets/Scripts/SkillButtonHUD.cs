using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButtonUI : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private SkillType skillType;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button actionButton;
    [SerializeField] private bool hideIfNonUpgradeable = true;

    private PlayerSkillsManager skillsManager;

    private void Start()
    {
        skillsManager = FindFirstObjectByType<PlayerSkillsManager>();

        // Vinculamos de forma limpia el clic con la habilidad exacta de este botón
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnSkillButtonClicked);
        }

        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnSkillButtonClicked()
    {
        if (skillsManager != null)
        {
            // Intenta comprar la habilidad específica asignada en este botón
            skillsManager.TryPurchaseSkill(skillType);
            UpdateUI(); // Refresca los textos inmediatamente
        }
    }

    public void UpdateUI()
    {
        if (skillsManager == null)
        {
            skillsManager = FindFirstObjectByType<PlayerSkillsManager>();
        }

        if (skillsManager != null)
        {
            int maxLevel = skillsManager.GetMaxLevel(skillType);

            // Si no es pasiva / no se puede mejorar (ej. maxLevel 0)
            if (maxLevel <= 0)
            {
                if (hideIfNonUpgradeable)
                {
                    gameObject.SetActive(false);
                    return;
                }

                if (levelText != null) levelText.text = "Activa";
                if (costText != null) costText.text = "-";
                if (actionButton != null) actionButton.interactable = false;
                return;
            }

            if (actionButton != null)
            {
                actionButton.interactable = true;
            }

            int currentLevel = skillsManager.GetSkillLevel(skillType);
            int cost = skillsManager.GetSkillCost(skillType);

            if (levelText != null)
            {
                levelText.text = $"{currentLevel}/{maxLevel}";
            }

            if (costText != null)
            {
                costText.text = $"{cost} Mat.";
            }
        }
    }
}