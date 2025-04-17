using TMPro;
using UnityEngine;

public class MummyPaperLength : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _paperCount;

    private void OnEnable()
    {
        if (PaperCurrencyManager.Instance != null)
        {
            PaperCurrencyManager.Instance.OnPaperChanged += UpdateUI;
            UpdateUI(PaperCurrencyManager.Instance.PaperLength);
        }
    }

    private void OnDisable()
    {
        if (PaperCurrencyManager.Instance != null)
        {
            PaperCurrencyManager.Instance.OnPaperChanged -= UpdateUI;
        }
    }

    private void Start()
    {
        UpdateUI(PaperCurrencyManager.Instance.PaperLength);
    }

    private void UpdateUI(float currentPaper)
    {
        _paperCount.text = $"{currentPaper:F1}/{MummyWrap.RequiredPaper:F1}";
    }
}
