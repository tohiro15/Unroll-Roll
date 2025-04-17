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
    [SerializeField] private Button _mummyWrapButton;
    [SerializeField] private Image _mummyImage;
    [SerializeField] private TextMeshProUGUI _mummyWrapButtonText;

    [SerializeField] private GameObject _enterNamePanel;
    [SerializeField] private TMP_InputField _enterNameInputField;

    #endregion

    #region Settings

    [Header("Wrap Settings")]
    [SerializeField] private List<Color> _availableColors = new List<Color>();
    private static Color _lastMummyColor = Color.white;

    [SerializeField] private float _wrapDuration = 1f;
    [SerializeField] private float _defaultRequiredPaper = 100f;
    [SerializeField] private float _additionPaper = 50f;

    private const string RequiredPaperKey = "RequiredPaperLength";
    public static float RequiredPaper { get; private set; }

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
        _enterNamePanel.SetActive(false);
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

                    _mummyWrapButton.gameObject.SetActive(false);
                    _enterNamePanel.SetActive(true);
                });
        }
        else
        {
            float progress = Mathf.Clamp01(currentPaper / RequiredPaper);
            _mummyImage.DOFillAmount(progress, _wrapDuration).SetEase(Ease.OutSine);
        }
    }

    private void UpdateButtonUI(float currentPaper)
    {
        _mummyWrapButtonText.text = $"Wrap\n{currentPaper:F1} meters";
    }

    public void ConfirmNameInput()
    {
        _mummyName = _enterNameInputField.text;

        EventSystem.current.SetSelectedGameObject(null);

        _enterNameInputField.text = "";
        _mummyWrapButton.gameObject.SetActive(true);
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
            ColorHex = ColorUtility.ToHtmlStringRGBA(currentColor)
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
