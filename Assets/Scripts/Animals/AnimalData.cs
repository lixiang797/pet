using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Pet Games/Animal Data")]
public sealed class AnimalData : ScriptableObject
{
    [SerializeField] private string animalName = "Rabbit";
    [SerializeField, Min(0.1f)] private float runSpeed = 7f;
    [SerializeField, Min(0.1f)] private float jumpForce = 6f;
    [SerializeField, Min(1f)] private float stamina = 100f;
    [SerializeField, Min(0.1f)] private float power = 2f;
    [SerializeField] private SkillBase skill;

    public string AnimalName => animalName;
    public float RunSpeed => runSpeed;
    public float JumpForce => jumpForce;
    public float Stamina => stamina;
    public float Power => power;
    public SkillBase Skill => skill;

    public void Configure(string name, float speed, float jump, float staminaValue, float powerValue, SkillBase animalSkill)
    {
        animalName = name;
        runSpeed = speed;
        jumpForce = jump;
        stamina = staminaValue;
        power = powerValue;
        skill = animalSkill;
    }
}
