using UnityEngine;
using TMPro;

public class PaperCurrencyUI : MonoBehaviour
{
    #region UI References

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI _paperText;

    #endregion

    #region Unity Methods

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

    #endregion

    #region UI Update Logic

    private void UpdateText(float value)
    {
        _paperText.text = $"{value:F1}";
    }

    #endregion
}
