using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ShopPanelController : MonoBehaviour
{
    [SerializeField] private Button elephantButton;
    [SerializeField] private Button foxButton;
    [SerializeField] private Button penguinButton;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text shopStatusText;

    private const int ElephantPrice = 250;
    private const int FoxPrice = 300;
    private const int PenguinPrice = 280;

    private void Start()
    {
        if (elephantButton != null)
            elephantButton.onClick.AddListener(() => BuyAnimal("Elephant", ElephantPrice));

        if (foxButton != null)
            foxButton.onClick.AddListener(() => BuyAnimal("Fox", FoxPrice));

        if (penguinButton != null)
            penguinButton.onClick.AddListener(() => BuyAnimal("Penguin", PenguinPrice));

        RefreshUI();
    }

    private void BuyAnimal(string animalName, int price)
    {
        if (PlayerProgress.Instance == null)
            return;

        if (PlayerProgress.Instance.IsUnlocked(animalName))
        {
            if (shopStatusText != null)
                shopStatusText.text = $"{animalName} 已解锁。";
            return;
        }

        if (PlayerProgress.Instance.Coins < price)
        {
            if (shopStatusText != null)
                shopStatusText.text = $"金币不足，{animalName} 还需要 {price}。";
            return;
        }

        PlayerProgress.Instance.AddCoins(-price);
        PlayerProgress.Instance.UnlockAnimal(animalName);
        if (shopStatusText != null)
            shopStatusText.text = $"{animalName} 解锁成功！";

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (PlayerProgress.Instance == null)
            return;

        if (coinText != null)
            coinText.text = $"金币：{PlayerProgress.Instance.Coins}";

        if (elephantButton != null)
            elephantButton.interactable = !PlayerProgress.Instance.IsUnlocked("Elephant");

        if (foxButton != null)
            foxButton.interactable = !PlayerProgress.Instance.IsUnlocked("Fox");

        if (penguinButton != null)
            penguinButton.interactable = !PlayerProgress.Instance.IsUnlocked("Penguin");
    }
}
