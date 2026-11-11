using UnityEngine;
using UnityEngine.UI;

public sealed class ShopPanelController : MonoBehaviour
{
    [SerializeField] private Button elephantButton;
    [SerializeField] private Button foxButton;
    [SerializeField] private Button penguinButton;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text statusText;
    private void Start()
    {
        if (elephantButton != null) elephantButton.onClick.AddListener(() => Buy("Elephant", 250));
        if (foxButton != null) foxButton.onClick.AddListener(() => Buy("Fox", 300));
        if (penguinButton != null) penguinButton.onClick.AddListener(() => Buy("Penguin", 280));
        Refresh();
    }
    private void Buy(string animal, int price)
    {
        if (PlayerProgress.Instance == null) return;
        if (PlayerProgress.Instance.IsUnlocked(animal)) { if (statusText != null) statusText.text = animal + " already unlocked."; return; }
        if (!PlayerProgress.Instance.SpendCoins(price)) { if (statusText != null) statusText.text = "Not enough coins."; return; }
        PlayerProgress.Instance.UnlockAnimal(animal);
        if (statusText != null) statusText.text = animal + " unlocked!";
        Refresh();
    }
    private void Refresh()
    {
        if (PlayerProgress.Instance == null) return;
        if (coinsText != null) coinsText.text = "Coins: " + PlayerProgress.Instance.Coins;
        if (elephantButton != null) elephantButton.interactable = !PlayerProgress.Instance.IsUnlocked("Elephant");
        if (foxButton != null) foxButton.interactable = !PlayerProgress.Instance.IsUnlocked("Fox");
        if (penguinButton != null) penguinButton.interactable = !PlayerProgress.Instance.IsUnlocked("Penguin");
    }
}
