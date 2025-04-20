using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class MummyWrap : MonoBehaviour
{
    #region UI References

    [Header("UI Components")]
    [SerializeField] private GameObject _meter—ounter;
    [SerializeField] private Button _mummyWrapButton;
    [SerializeField] private Image _mummyImage;
    [SerializeField] private TextMeshProUGUI _mummyWrapButtonText;
    [SerializeField] private GameObject _decorationsPanel;

    [SerializeField] private GameObject _coinsShowAdded;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private Image _coinsIcon;

    [SerializeField] private GameObject _enterNamePanel;
    [SerializeField] private TMP_InputField _enterNameInputField;

    #endregion

    #region Settings

    [Header("Wrap Settings")]
    [SerializeField] private List<Color> _availableColors = new List<Color>();
    private List<string> _appliedDecorations = new List<string>();
    private static Color _lastMummyColor = Color.white;

    [SerializeField] private int _amountEarnings = 100;
    [SerializeField] private float _wrapDuration = 1f;
    [SerializeField] private float _defaultRequiredPaper = 100f;
    [SerializeField] private float _additionPaper = 50f;

    private const string RequiredPaperKey = "RequiredPaperLength";
    public static float RequiredPaper { get; private set; }

    [Header("Animation Settings")]
    [SerializeField] private float _scaleInDuration = 0.3f;
    [SerializeField] private float _displayDuration = 1.0f;
    [SerializeField] private float _scaleOutDuration = 0.3f;
    [SerializeField] private Vector3 _startScale = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 _endScale = new Vector3(1f, 1f, 1f);

    #endregion

    #region Mummy Data

    [Header("Mummy Info")]
    [SerializeField] private string _mummyName;
    public static event Action<string> OnMummyCreated;

    #endregion

    private void Awake()
    {
        LoadRequiredPaper();
    }

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
        }
    }

    private void Start()
    {
        _mummyWrapButton.gameObject.SetActive(true);
        _meter—ounter.gameObject.SetActive(true);
        _decorationsPanel.SetActive(false);
        _enterNamePanel.SetActive(false);
        _coinsShowAdded.SetActive(false);
        UpdateButtonUI(PaperCurrencyManager.Instance.PaperLength);
    }

    public void WrapMummy()
    {
        float currentPaper = PaperCurrencyManager.Instance.PaperLength;

        if (currentPaper >= RequiredPaper)
        {
            _mummyImage.DOFillAmount(1f, _wrapDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    PaperCurrencyManager.Instance.AddPaper(-RequiredPaper);

                    AudioManager.Instance.PlayMummyWrappedSound();

                    RequiredPaper += _additionPaper;
                    SaveRequiredPaper();

                    PaperCurrencyManager.Instance.ForceUpdateUI();

                    ShowCoinsAdded(_amountEarnings);

                    _mummyWrapButton.gameObject.SetActive(false);
                    _meter—ounter.gameObject.SetActive(false);
                    _decorationsPanel.SetActive(true);
                    _enterNamePanel.SetActive(false);
                });
        }
        else
        {
            float progress = Mathf.Clamp01(currentPaper / RequiredPaper);
            _mummyImage.DOFillAmount(progress, _wrapDuration).SetEase(Ease.OutSine);
        }
    }

    private void ShowCoinsAdded(int amount)
    {
        CoinsManager.Instance.AddCoins(amount);

        _coinsText.text = $"+{amount}";
        _coinsShowAdded.transform.localScale = _startScale;
        _coinsShowAdded.SetActive(true);


        _coinsShowAdded.transform.DOScale(_endScale, _scaleInDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            DOVirtual.DelayedCall(_displayDuration, () =>
            {
                _coinsShowAdded.transform.DOScale(_startScale, _scaleOutDuration).SetEase(Ease.InBack).OnComplete(() =>
                {
                    _coinsShowAdded.SetActive(false);
                });
            });
        });
    }
    private void UpdateButtonUI(float currentPaper)
    {
        _mummyWrapButtonText.text = $"Wrap\n{currentPaper:F1} meters";
    }

    public void ApplyDecorations()
    {
        _mummyWrapButton.gameObject.SetActive(false);
        _meter—ounter.gameObject.SetActive(false);
        _decorationsPanel.SetActive(false);
        _enterNamePanel.SetActive(true);
    }

    public void AddDecoration(Decoration decoration)
    {
        _appliedDecorations.Add(decoration.gameObject.name);
    }



    public void ConfirmNameInput()
    {
        if (string.IsNullOrWhiteSpace(_enterNameInputField.text))
        {
            Debug.LogWarning("Name is empty!");
            return;
        }

        _mummyName = _enterNameInputField.text;

        EventSystem.current.SetSelectedGameObject(null);
        _appliedDecorations.Clear();

        _enterNameInputField.text = "";
        _mummyWrapButton.gameObject.SetActive(true);
        _meter—ounter.gameObject.SetActive(true);
        _decorationsPanel.SetActive(false);
        _enterNamePanel.SetActive(false);
        _mummyImage.fillAmount = 0f;

        Color currentColor = _lastMummyColor;

        if (_availableColors.Count > 0)
        {
            _lastMummyColor = _availableColors[UnityEngine.Random.Range(0, _availableColors.Count)];
        }

        _mummyImage.color = _lastMummyColor;

        MummyData data = new MummyData()
        {
            Name = _mummyName,
            ColorHex = ColorUtility.ToHtmlStringRGBA(currentColor),
            AppliedDecorations = new List<string>(_appliedDecorations)
        };

        OnMummyCreated?.Invoke(JsonUtility.ToJson(data));
    }

    private void SaveRequiredPaper()
    {
        PlayerPrefs.SetFloat(RequiredPaperKey, RequiredPaper);
        PlayerPrefs.Save();
    }

    private void LoadRequiredPaper()
    {
        if (PlayerPrefs.HasKey(RequiredPaperKey))
        {
            RequiredPaper = PlayerPrefs.GetFloat(RequiredPaperKey);
        }
        else
        {
            RequiredPaper = _defaultRequiredPaper;
        }
    }
}
