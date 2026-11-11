using UnityEngine;

public sealed class ObstacleRaceManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;

    public void EnableObstacles()
    {
        if (obstacles == null)
            return;

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
                obstacle.SetActive(true);
        }
    }

    public void DisableObstacles()
    {
        if (obstacles == null)
            return;

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
                obstacle.SetActive(false);
        }
    }
}
