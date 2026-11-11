using UnityEngine;

public sealed class ObstacleRaceManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    public void Configure(GameObject[] items) => obstacles = items;
    public void EnableObstacles() => SetActive(true);
    public void DisableObstacles() => SetActive(false);
    private void SetActive(bool active)
    {
        if (obstacles == null) return;
        foreach (GameObject item in obstacles) if (item != null) item.SetActive(active);
    }
}
