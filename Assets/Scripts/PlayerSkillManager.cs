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
        int currentLevel = GetSkillLevel(skill);
        int maxLevel = GetMaxLevel(skill);

        if (currentLevel >= maxLevel)
        {
            Debug.Log("La Habilidad ya esta al nivel maximo");
            return false;
        }

        int cost = GetSkillCost(skill);
        
        if (playerResources != null && playerResources.SpendMaterials(cost))
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
            skillLevels.Add(skill, 0);
        }
    }

    #region Skill Levels

    public int GetSkillLevel(SkillType skill)
    {
        return skillLevels[skill];
    }

    public void SetSkillLevel(SkillType skill, int level)
    {
        skillLevels[skill] = Mathf.Clamp(level, 0, GetMaxLevel(skill));
    }

    public bool UpgradeSkill(SkillType skill)
    {
        if (skillLevels[skill] >= GetMaxLevel(skill))
            return false;

        skillLevels[skill]++;

        return true;
    }

    public int GetMaxLevel(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.Salvador:
            case SkillType.SinRemordimiento:
            case SkillType.ConLosAngeles:
            case SkillType.AlQueMadruga:
                return 5;

            case SkillType.GraciaDeDios:
                return 10;

            case SkillType.ElixirDeLosAngeles:
            case SkillType.QueSiganViniendo:
                return 3;

            default:
                return 1;
        }
    }

    #endregion

    #region Damage

    public float GetDamageMultiplier()
    {
        int level = GetSkillLevel(SkillType.SinRemordimiento);

        return 1f + (level * 0.05f);


    }

    public float GetDamageReduction()
    {
        int level = GetSkillLevel(SkillType.Salvador);

        return level * 0.05f;
    }

    #endregion

    #region Movement

    public float GetMovementMultiplier()
    {
        int level = GetSkillLevel(SkillType.ConLosAngeles);

        return 1f + (level * 0.025f);
    }

    #endregion

    #region Materials

    public float GetMaterialMultiplier()
    {
        int level = GetSkillLevel(SkillType.AlQueMadruga);

        return 1f + (level * 0.06f);
    }

    #endregion

    #region Regeneration

    public float GetHealthRegenPerSecond()
    {
        int level = GetSkillLevel(SkillType.GraciaDeDios);

        return level * 0.1f;
    }

    #endregion

    #region Potions

    public int GetPotionHealingBonus()
    {
        int level = GetSkillLevel(SkillType.ElixirDeLosAngeles);

        return level * 5;
    }

    #endregion

    #region QueSiganViniendo

    public int GetKillsRequiredForHeal()
    {
        int level = GetSkillLevel(SkillType.QueSiganViniendo);

        switch (level)
        {
            case 1:
                return 15;

            case 2:
                return 13;

            case 3:
                return 10;

            default:
                return int.MaxValue;
        }
    }

    public bool HasQueSiganViniendo()
    {
        return GetSkillLevel(SkillType.QueSiganViniendo) > 0;
    }

    #endregion


    private void Start()
    {
       
       
    }
}
