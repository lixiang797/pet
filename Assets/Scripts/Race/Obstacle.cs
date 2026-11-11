using UnityEngine;

public sealed class Obstacle : MonoBehaviour
{
    [SerializeField] private float requiredJumpHeight = 1.5f;
    [SerializeField] private float penaltySpeed = 2.5f;

    private void OnTriggerEnter(Collider other)
    {
        AnimalController animal = other.GetComponent<AnimalController>();
        if (animal == null)
            return;

        float tolerance = GetTolerance(animal);
        float jumpHeight = animal.transform.position.y;

        if (jumpHeight < requiredJumpHeight)
        {
            float reducedPenalty = penaltySpeed / tolerance;
            animal.ApplySpeedPenalty(reducedPenalty);
        }
    }

    private float GetTolerance(AnimalController animal)
    {
        if (animal == null)
            return 1f;

        switch (animal.AnimalName)
        {
            case "Monkey":
                return 1.5f;
            case "Fox":
                return 1.2f;
            case "Rabbit":
                return 1f;
            case "Penguin":
                return 1.1f;
            case "Elephant":
                return 0.8f;
            case "Turtle":
                return 0.9f;
            default:
                return 1f;
        }
    }
}
