using UnityEngine;
using System;

public class PaperCurrencyManager : MonoBehaviour
{
    public static PaperCurrencyManager Instance { get; private set; }

    public float PaperLength { get; private set; }

    public event Action<float> OnPaperChanged;

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

    private void SaveCurrency()
    {
        PlayerPrefs.SetFloat("PaperLength", PaperLength);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        PaperLength = PlayerPrefs.GetFloat("PaperLength", 0);
    }
}
