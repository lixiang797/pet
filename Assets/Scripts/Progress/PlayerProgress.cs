using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance { get; private set; }

    public int Coins { get; private set; }
    public int Experience { get; private set; }
    public string SelectedAnimalName { get; private set; } = "Rabbit";
    public List<string> UnlockedAnimals { get; } = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void UnlockAnimal(string animalName)
    {
        if (string.IsNullOrWhiteSpace(animalName))
            return;

        if (!UnlockedAnimals.Contains(animalName))
            UnlockedAnimals.Add(animalName);

        Save();
    }

    public bool IsUnlocked(string animalName)
    {
        return UnlockedAnimals.Contains(animalName);
    }

    public void SelectAnimal(string animalName)
    {
        if (IsUnlocked(animalName))
        {
            SelectedAnimalName = animalName;
            Save();
        }
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        Save();
    }

    public void AddExperience(int amount)
    {
        Experience += amount;
        Save();
    }

    public void ApplyRaceRewards(List<AnimalController> finishOrder)
    {
        if (finishOrder == null)
            return;

        for (int i = 0; i < finishOrder.Count; i++)
        {
            AnimalController racer = finishOrder[i];
            if (racer == null)
                continue;

            int rewardCoins = i == 0 ? 120 : i == 1 ? 80 : 40;
            int rewardXp = i == 0 ? 50 : i == 1 ? 30 : 15;

            AddCoins(rewardCoins);
            AddExperience(rewardXp);
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt("pet_coins", Coins);
        PlayerPrefs.SetInt("pet_xp", Experience);
        PlayerPrefs.SetString("pet_selectedAnimal", SelectedAnimalName);
        PlayerPrefs.SetString("pet_unlockedAnimals", string.Join(",", UnlockedAnimals));
        PlayerPrefs.Save();
    }

    private void Load()
    {
        Coins = PlayerPrefs.GetInt("pet_coins", 300);
        Experience = PlayerPrefs.GetInt("pet_xp", 0);
        SelectedAnimalName = PlayerPrefs.GetString("pet_selectedAnimal", "Rabbit");

        string unlocked = PlayerPrefs.GetString("pet_unlockedAnimals", "Rabbit,Turtle,Monkey");
        UnlockedAnimals.Clear();

        string[] parts = unlocked.Split(',');
        foreach (string part in parts)
        {
            if (!string.IsNullOrWhiteSpace(part))
                UnlockedAnimals.Add(part.Trim());
        }

        if (UnlockedAnimals.Count == 0)
        {
            UnlockedAnimals.Add("Rabbit");
            UnlockedAnimals.Add("Turtle");
            UnlockedAnimals.Add("Monkey");
        }
    }
}
