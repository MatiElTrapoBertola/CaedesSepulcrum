using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsManager : MonoBehaviour
{
    private Dictionary<SkillType, int> skillLevels =
        new Dictionary<SkillType, int>();

    private void Awake()
    {
        InitializeSkills();
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
        SetSkillLevel(SkillType.SinRemordimiento, 5);
        SetSkillLevel(SkillType.Salvador, 5);
        SetSkillLevel(SkillType.GraciaDeDios, 10);
        SetSkillLevel(SkillType.ConLosAngeles, 5);
        SetSkillLevel(SkillType.QueSiganViniendo, 3);
        SetSkillLevel(SkillType.AlQueMadruga, 5);
        SetSkillLevel(SkillType.ElixirDeLosAngeles, 3);
       
    }
}
