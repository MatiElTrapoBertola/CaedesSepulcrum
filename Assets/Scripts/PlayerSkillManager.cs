using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsManager : MonoBehaviour
{
    [Header("Economy settings")]
    [SerializeField] private PlayerResources playerResources;
    [SerializeField] private int baseCost = 10;

    private Dictionary<SkillType, int> skillLevels = new Dictionary<SkillType, int>();
    private static PlayerSkillsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSkills();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetSkillCost(SkillType skill)
    {
        int currentLevel = GetSkillLevel(skill);
        return baseCost * (currentLevel + 1);
    }

    public bool TryPurchaseSkill(SkillType skill)
    {
        int maxLevel = GetMaxLevel(skill);
        if (maxLevel <= 0)
        {
            Debug.LogWarning($"La habilidad {skill} es activa o no se puede mejorar.");
            return false;
        }

        int currentLevel = GetSkillLevel(skill);

        if (currentLevel >= maxLevel)
        {
            Debug.Log("La Habilidad ya esta al nivel maximo");
            return false;
        }

        int cost = GetSkillCost(skill);

        if (playerResources == null)
        {
            Debug.LogError("¡ERROR: PlayerResources no está asignado en el PlayerSkillsManager!");
            return false;
        }

        if (playerResources.SpendMaterials(cost))
        {
            UpgradeSkill(skill);
            Debug.Log("Mejora Comprada con exito " + skill + "(nivel " + GetSkillLevel(skill) + ")");
            return true;
        }

        Debug.Log("No hay suficientes materiales para mejorar " + skill);
        return false;
    }

    private void InitializeSkills()
    {
        foreach (SkillType skill in Enum.GetValues(typeof(SkillType)))
        {
            // Solo inicializamos aquellas que sean pasivas (maxLevel > 0)
            if (GetMaxLevel(skill) > 0)
            {
                skillLevels[skill] = 0;
            }
        }
    }

    #region Skill Levels

    public int GetSkillLevel(SkillType skill)
    {
        if (!skillLevels.ContainsKey(skill))
        {
            skillLevels[skill] = 0;
        }
        return skillLevels[skill];
    }

    public void SetSkillLevel(SkillType skill, int level)
    {
        int max = GetMaxLevel(skill);
        if (max > 0)
        {
            skillLevels[skill] = Mathf.Clamp(level, 0, max);
        }
    }

    public bool UpgradeSkill(SkillType skill)
    {
        if (GetMaxLevel(skill) <= 0 || skillLevels[skill] >= GetMaxLevel(skill))
            return false;

        skillLevels[skill]++;
        return true;
    }

    /// <summary>
    /// Retorna el nivel máximo de la habilidad. 
    /// Si retorna 0, significa que la habilidad es ACTIVA o NO MEJORABLE y se ignorará en la tienda.
    /// </summary>
    public int GetMaxLevel(SkillType skill)
    {
        switch (skill)
        {
            // --- HABILIDADES PASIVAS MEJORABLES ---
            case SkillType.Salvador:
            case SkillType.SinPiedad:
            case SkillType.SinRemordimiento: // Soporte para ambas nomenclaturas
            case SkillType.ConLosAngeles:
            case SkillType.AlQueMadruga:
                return 5;

            case SkillType.GraciaDeDios:
                return 10;

            case SkillType.ElixirDeLosAngeles:
            case SkillType.QueSiganViniendo:
                return 3;

            // --- HABILIDADES ACTIVAS / NO MEJORABLES ---
            case SkillType.Plegaria:
            default:
                return 0;
        }
    }

    #endregion

    #region Bonificadores Pasivos

    public float GetDamageMultiplier()
    {
        int level = GetSkillLevel(SkillType.SinPiedad) > 0 ? GetSkillLevel(SkillType.SinPiedad) : GetSkillLevel(SkillType.SinRemordimiento);
        return 1f + (level * 0.05f);
    }

    public float GetDamageReduction() => GetSkillLevel(SkillType.Salvador) * 0.05f;
    public float GetMovementMultiplier() => 1f + (GetSkillLevel(SkillType.ConLosAngeles) * 0.025f);
    public float GetMaterialMultiplier() => 1f + (GetSkillLevel(SkillType.AlQueMadruga) * 0.06f);
    public float GetHealthRegenPerSecond() => GetSkillLevel(SkillType.GraciaDeDios) * 0.1f;

    // Única declaración correcta del bono de pociones
    public int GetPotionHealingBonus()
    {
        int level = GetSkillLevel(SkillType.ElixirDeLosAngeles);
        return level * 5;
    }

    public int GetKillsRequiredForHeal()
    {
        int level = GetSkillLevel(SkillType.QueSiganViniendo);
        switch (level)
        {
            case 1: return 15;
            case 2: return 13;
            case 3: return 10;
            default: return int.MaxValue;
        }
    }

    public bool HasQueSiganViniendo() => GetSkillLevel(SkillType.QueSiganViniendo) > 0;

    #endregion
}