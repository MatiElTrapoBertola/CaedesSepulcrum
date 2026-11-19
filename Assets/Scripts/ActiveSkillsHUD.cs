using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillsHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerActiveSkills activeSkills;

    [Header("Cooldown Masks")]
    [SerializeField] private Image plegariaMask;
    [SerializeField] private Image mensajeroMask;
    [SerializeField] private Image blessingMask;
    [SerializeField] private Image sinPiedadMask;

    private void Awake()
    {
        activeSkills = FindFirstObjectByType<PlayerActiveSkills>();

        Image[] images = GetComponentsInChildren<Image>(true);

        foreach (Image img in images)
        {
            if (img.transform.parent != null)
            {
                string parentName = img.transform.parent.name;

                if (parentName == "Plegaria")
                    plegariaMask = img;

                else if (parentName == "MensajeroConPrisa")
                    mensajeroMask = img;

                else if (parentName == "Bendición")
                    blessingMask = img;

                else if (parentName == "SinPiedad")
                    sinPiedadMask = img;
            }
        }

      
    }



    private void Update()
    {
        if (activeSkills == null)
            return;

        

        UpdateSkillMask(plegariaMask,activeSkills.PlegariaCooldownRemaining,activeSkills.PlegariaCooldown);

        UpdateSkillMask(mensajeroMask,activeSkills.MensajeroCooldownRemaining,activeSkills.MensajeroCooldown);

        UpdateSkillMask(blessingMask,activeSkills.BlessingCooldownRemaining,activeSkills.BlessingCooldown);

        UpdateSkillMask(sinPiedadMask,activeSkills.SinPiedadCooldownRemaining,activeSkills.SinPiedadCooldown);
    }

    private void UpdateSkillMask(
        Image mask,
        float remainingCooldown,
        float maxCooldown)
    {
        if (mask == null)
            return;

        if (remainingCooldown <= 0f)
        {
            mask.fillAmount = 1f;
            return;
        }

        mask.fillAmount = 1f - (remainingCooldown / maxCooldown);
    }
}