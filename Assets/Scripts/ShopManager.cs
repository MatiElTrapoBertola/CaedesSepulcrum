using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerSkillsManager skillsManager;

    public void BuySkillbyIndex(int skillTypeIndex)
    {
        if (skillsManager != null)
        {
            SkillType skillToBuy = (SkillType)skillTypeIndex;

            bool success = skillsManager.TryPurchaseSkill(skillToBuy);

            if (success)
            {
                Debug.Log("Compra realizada desde el HUD.");

                // Actualizamos todos los botones de la tienda en pantalla para que reflejen el nuevo nivel y costo
                SkillButtonUI[] skillButtons = FindObjectsByType<SkillButtonUI>(FindObjectsSortMode.None);
                foreach (SkillButtonUI button in skillButtons)
                {
                    button.UpdateUI();
                }
            }
        }
    }
}