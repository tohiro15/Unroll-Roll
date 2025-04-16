using UnityEngine;
using TMPro;

public class PaperCurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _paperText;

    private void OnEnable()
    {
        if (PaperCurrencyManager.Instance != null)
        {
            PaperCurrencyManager.Instance.OnPaperChanged += UpdateText;
            UpdateText(PaperCurrencyManager.Instance.PaperLength);
        }
    }


    private void OnDisable()
    {
        if (PaperCurrencyManager.Instance != null)
        {
            PaperCurrencyManager.Instance.OnPaperChanged -= UpdateText;
            UpdateText(PaperCurrencyManager.Instance.PaperLength);
        }
    }

    private void Start()
    {
        UpdateText(PaperCurrencyManager.Instance.PaperLength);
    }

    private void UpdateText(float value)
    {
        _paperText.text = $"{value:F1}";
    }
}
