using System.Collections.Generic;
using UnityEngine;

public sealed class TugOfWarManager : MonoBehaviour
{
    [SerializeField] private List<AnimalController> teamA = new List<AnimalController>();
    [SerializeField] private List<AnimalController> teamB = new List<AnimalController>();
    [SerializeField] private float maxPullDistance = 10f;
    [SerializeField] private KeyCode playerPullKey = KeyCode.Space;

    private float ropePosition;
    private bool tugStarted;

    public void StartTugOfWar()
    {
        tugStarted = true;
        ropePosition = 0f;
    }

    private void Update()
    {
        if (!tugStarted)
            return;

        float aForce = GetTeamForce(teamA);
        float bForce = GetTeamForce(teamB);

        if (Input.GetKey(playerPullKey))
            aForce += GetTeamBonus(teamA, 0.6f);

        bForce += GetTeamBonus(teamB, 0.5f);

        ropePosition += (aForce - bForce) * Time.deltaTime;
        ropePosition = Mathf.Clamp(ropePosition, -maxPullDistance, maxPullDistance);

        if (ropePosition >= maxPullDistance)
            EndTugOfWar(true);
        else if (ropePosition <= -maxPullDistance)
            EndTugOfWar(false);
    }

    private float GetTeamForce(List<AnimalController> team)
    {
        float total = 0f;
        foreach (AnimalController animal in team)
        {
            if (animal == null)
                continue;
            total += animal.GetPullForce();
        }
        return total;
    }

    private float GetTeamBonus(List<AnimalController> team, float bonusFactor)
    {
        float total = 0f;
        foreach (AnimalController animal in team)
        {
            if (animal == null)
                continue;
            total += animal.GetPullForce() * bonusFactor;
        }
        return total;
    }

    private void EndTugOfWar(bool teamAWin)
    {
        tugStarted = false;
        Debug.Log(teamAWin ? "Team A 胜！" : "Team B 胜！");
    }
}
