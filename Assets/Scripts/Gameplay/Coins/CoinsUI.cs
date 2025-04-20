using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private void OnEnable()
    {
        if (CoinsManager.Instance != null)
        {
            CoinsManager.Instance.OnCoinsChanged += UpdateCoinsUI;
            UpdateCoinsUI(CoinsManager.Instance.CoinCount);
        }
    }

    private void OnDisable()
    {
        if (CoinsManager.Instance != null)
        {
            CoinsManager.Instance.OnCoinsChanged -= UpdateCoinsUI;
        }
    }

    private void UpdateCoinsUI(int newCoinCount)
    {
        _coinsText.text = newCoinCount.ToString();
    }
}
