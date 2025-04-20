using UnityEngine;
using System;

public class CoinsManager : MonoBehaviour
{
    #region Singleton

    public static CoinsManager Instance { get; private set; }

    #endregion

    #region Properties

    public int CoinCount { get; private set; }

    public event Action<int> OnCoinsChanged;

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
        LoadCoins();
    }

    #endregion

    #region Coins Management

    public void AddCoins(int amount)
    {
        CoinCount += amount;
        SaveCoins();
        OnCoinsChanged?.Invoke(CoinCount);
    }

    public void SubtractCoins(int amount)
    {
        CoinCount = Mathf.Max(0, CoinCount - amount);
        SaveCoins();
        OnCoinsChanged?.Invoke(CoinCount);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", CoinCount);
        PlayerPrefs.Save();
    }

    private void LoadCoins()
    {
        CoinCount = PlayerPrefs.GetInt("Coins", 0);
    }

    #endregion
}
