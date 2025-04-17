using TMPro;
using UnityEngine;

public class MummyPaperLength : MonoBehaviour
{
    #region UI References

    [Header("UI Components")]
    [SerializeField] private float _needPaper = 100f;
    [SerializeField] private TextMeshProUGUI _paperCount;

    #endregion

    #region Unity Methods

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
            UpdateUI(PaperCurrencyManager.Instance.PaperLength);
        }
    }

    private void Start()
    {
        UpdateUI(PaperCurrencyManager.Instance.PaperLength);
    }

    #endregion

    #region UI Update Logic

    private void UpdateUI(float currentPaper)
    {
        _paperCount.text = $"{currentPaper:F1}/{_needPaper}";
    }

    #endregion
}
