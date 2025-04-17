using UnityEngine;
using System;

public class PaperCurrencyManager : MonoBehaviour
{
    #region Singleton

    public static PaperCurrencyManager Instance { get; private set; }

    #endregion

    #region Properties

    public float PaperLength { get; private set; }

    public event Action<float> OnPaperChanged;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadCurrency();
    }

    #endregion

    #region Currency Management

    public void AddPaper(float amount)
    {
        PaperLength += amount;
        SaveCurrency();
        OnPaperChanged?.Invoke(PaperLength);
    }

    public void SetPaper(float amount)
    {
        PaperLength = amount;
        SaveCurrency();
        OnPaperChanged?.Invoke(PaperLength);
    }

    public void ForceUpdateUI()
    {
        OnPaperChanged?.Invoke(PaperLength);
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetFloat("PaperLength", PaperLength);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        PaperLength = PlayerPrefs.GetFloat("PaperLength", 0);
    }

    #endregion
}
