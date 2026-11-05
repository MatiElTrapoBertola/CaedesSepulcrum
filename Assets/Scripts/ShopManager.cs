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
            }
        }
    }
}
