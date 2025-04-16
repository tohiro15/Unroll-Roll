using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MummyWrap : MonoBehaviour
{
    [SerializeField] private Image _mummyImage;
    [SerializeField] private TextMeshProUGUI _mummyWrapButtonText;
    [SerializeField] private float _wrapDuration = 1f;
    [SerializeField] private float _requiredPaper = 100f;

    private void OnEnable()
    {
        if (PaperCurrencyManager.Instance != null)
        {
            PaperCurrencyManager.Instance.OnPaperChanged += UpdateButtonUI;
            UpdateButtonUI(PaperCurrencyManager.Instance.PaperLength);
        }
    }


    private void OnDisable()
    {
        if (PaperCurrencyManager.Instance != null)
        {
            PaperCurrencyManager.Instance.OnPaperChanged -= UpdateButtonUI;
            UpdateButtonUI(PaperCurrencyManager.Instance.PaperLength);
        }
    }
    private void Start()
    {
        UpdateButtonUI(PaperCurrencyManager.Instance.PaperLength);
    }
    public void WrapMummy()
    {
        float currentPaper = PaperCurrencyManager.Instance.PaperLength;

        if (currentPaper >= _requiredPaper)
        {
            _mummyImage.DOFillAmount(1f, _wrapDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    PaperCurrencyManager.Instance.AddPaper(-_requiredPaper);
                    _mummyImage.fillAmount = 0f;
                });
        }
        else
        {
            float progress = Mathf.Clamp01(currentPaper / _requiredPaper);
            _mummyImage.DOFillAmount(progress, _wrapDuration).SetEase(Ease.OutSine);
        }
    }

    private void UpdateButtonUI(float currentPaper)
    {
        _mummyWrapButtonText.text = $"Wrap\r\n{PaperCurrencyManager.Instance.PaperLength:F1} meters";
    }
}
