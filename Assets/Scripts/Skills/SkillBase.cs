using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    [SerializeField] private string skillName = "Animal Skill";
    [SerializeField, Min(0f)] private float cooldown = 7f;
    public string SkillName => skillName;
    public float Cooldown => cooldown;
    public abstract void Activate(AnimalController animal);
}

public enum AnimalSkillType { Sprint, Jump, Recover, Power }

[CreateAssetMenu(fileName = "AnimalSkill", menuName = "Pet Games/Animal Skill")]
public sealed class AnimalSkill : SkillBase
{
    [SerializeField] private AnimalSkillType type;
    [SerializeField, Min(1f)] private float multiplier = 1.6f;
    [SerializeField, Min(0.1f)] private float duration = 2f;

    public void Configure(AnimalSkillType skillType, float skillMultiplier, float skillDuration)
    {
        type = skillType;
        multiplier = skillMultiplier;
        duration = skillDuration;
    }

    public override void Activate(AnimalController animal)
    {
        if (animal == null) return;
        switch (type)
        {
            case AnimalSkillType.Sprint: animal.ApplySpeedBoost(multiplier, duration); break;
            case AnimalSkillType.Jump: animal.ApplyJumpBoost(multiplier, duration); break;
            case AnimalSkillType.Recover: animal.RecoverStamina(35f); break;
            case AnimalSkillType.Power: animal.ApplyPowerBoost(multiplier, duration); break;
        }
    }
}
