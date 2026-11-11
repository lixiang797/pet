using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Pet Games/Animal Data")]
public sealed class AnimalData : ScriptableObject
{
    [SerializeField] private string animalName = "Rabbit";
    [SerializeField, Min(0.1f)] private float runSpeed = 7f;
    [SerializeField, Min(0.1f)] private float jumpForce = 6f;

    public string AnimalName => animalName;
    public float RunSpeed => runSpeed;
    public float JumpForce => jumpForce;
}
