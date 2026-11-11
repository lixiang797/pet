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
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public bool IsUnlocked(string name) => UnlockedAnimals.Contains(name);
    public void UnlockAnimal(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        if (!UnlockedAnimals.Contains(name)) UnlockedAnimals.Add(name);
        Save();
    }
    public void SelectAnimal(string name)
    {
        if (IsUnlocked(name)) { SelectedAnimalName = name; Save(); }
    }
    public bool SpendCoins(int amount)
    {
        if (amount < 0 || Coins < amount) return false;
        Coins -= amount; Save(); return true;
    }
    public void AddCoins(int amount) { Coins = Mathf.Max(0, Coins + amount); Save(); }
    public void AddExperience(int amount) { Experience = Mathf.Max(0, Experience + amount); Save(); }
    public int GetSpeedTraining(string name) => PlayerPrefs.GetInt("pet_speed_" + name, 0);
    public int GetJumpTraining(string name) => PlayerPrefs.GetInt("pet_jump_" + name, 0);
    public bool Train(string name, bool speed)
    {
        if (!SpendCoins(50)) return false;
        string key = speed ? "pet_speed_" : "pet_jump_";
        PlayerPrefs.SetInt(key + name, PlayerPrefs.GetInt(key + name, 0) + 1);
        PlayerPrefs.Save(); return true;
    }
    public void ApplyPlayerReward(int rank)
    {
        int coins = rank == 1 ? 120 : rank == 2 ? 80 : 40;
        int xp = rank == 1 ? 50 : rank == 2 ? 30 : 15;
        AddCoins(coins); AddExperience(xp);
    }

    private void Load()
    {
        Coins = PlayerPrefs.GetInt("pet_coins", 300);
        Experience = PlayerPrefs.GetInt("pet_xp", 0);
        SelectedAnimalName = PlayerPrefs.GetString("pet_selectedAnimal", "Rabbit");
        string saved = PlayerPrefs.GetString("pet_unlockedAnimals", "Rabbit,Turtle,Monkey");
        UnlockedAnimals.Clear();
        foreach (string part in saved.Split(','))
            if (!string.IsNullOrWhiteSpace(part) && !UnlockedAnimals.Contains(part.Trim())) UnlockedAnimals.Add(part.Trim());
        if (UnlockedAnimals.Count == 0) { UnlockedAnimals.Add("Rabbit"); UnlockedAnimals.Add("Turtle"); UnlockedAnimals.Add("Monkey"); }
        if (!IsUnlocked(SelectedAnimalName)) SelectedAnimalName = "Rabbit";
    }
    private void Save()
    {
        PlayerPrefs.SetInt("pet_coins", Coins);
        PlayerPrefs.SetInt("pet_xp", Experience);
        PlayerPrefs.SetString("pet_selectedAnimal", SelectedAnimalName);
        PlayerPrefs.SetString("pet_unlockedAnimals", string.Join(",", UnlockedAnimals));
        PlayerPrefs.Save();
    }
}
