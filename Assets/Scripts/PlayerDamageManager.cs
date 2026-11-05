using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    [Header("Base Damage")]
    [SerializeField] private float baseDamage = 20f;

    [Header("Blessing")]
    [SerializeField] private int maxDivineHits = 5;

    private int remainingDivineHits;

    [Header("References")]
    [SerializeField] private PlayerSkillsManager skillsManager;

    [Header("Divine Attacks")]
    public bool DivineAttacksActive { get; set; }

    public float GetFinalDamage()
    {
        float damage = baseDamage;

        if (skillsManager != null)
        {
            damage *= skillsManager.GetDamageMultiplier();
        }

        Debug.Log("Dano Final: " + damage);

        return damage;
    }

    public float GetBaseDamage()
    {
        return baseDamage;
    }

    public void SetBaseDamage (float newDamage)
    {
        baseDamage = newDamage;
    }

    public void ActivateBlessing()
    {
        DivineAttacksActive = true;
        remainingDivineHits = maxDivineHits;

        Debug.Log("Bendicion activada. Ataques divinos: " + remainingDivineHits);
    }

    public bool ConsumeDivineHit()
    {
        if (!DivineAttacksActive)
            return false;

        remainingDivineHits--;

        Debug.Log("Ataque divino consumido. Ataques restantes: " + remainingDivineHits);

        if (remainingDivineHits < 0)
        {
            DivineAttacksActive=false;

            Debug.Log("Bendicion terminada.");
        }

        return true;
    }
}
