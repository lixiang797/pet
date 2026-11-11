using UnityEngine;

public sealed class Obstacle : MonoBehaviour
{
    [SerializeField] private float penalty = 2.5f;
    private void OnTriggerEnter(Collider other)
    {
        AnimalController animal = other.GetComponentInParent<AnimalController>();
        if (animal == null || animal.transform.position.y > transform.position.y + 0.6f) return;
        animal.ApplySpeedPenalty(penalty);
    }
}
