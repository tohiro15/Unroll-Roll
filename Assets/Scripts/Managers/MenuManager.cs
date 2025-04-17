using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    #region UI References

    [Header("UI Components")]
    [SerializeField] private List<Canvas> _menus = new List<Canvas>();
    [SerializeField] private int _defaultIndex = 0;

    #endregion

    #region Unity Methods

    private void Start()
    {
        SetActiveMenu(_defaultIndex);
    }

    #endregion

    #region Menu Management

    public void SetActiveMenu(int menuIndex)
    {
        if (menuIndex < 0 || menuIndex >= _menus.Count)
        {
            Debug.LogError("Invalid menu index!");
            return;
        }

        AudioManager.Instance.PlayButtonClick();

        for (int i = 0; i < _menus.Count; i++)
        {
            _menus[i].gameObject.SetActive(i == menuIndex);
        }
    }

    public void ToggleMenu(int menuIndex)
    {
        if (menuIndex < 0 || menuIndex >= _menus.Count)
        {
            Debug.LogError("Invalid menu index!");
            return;
        }

        AudioManager.Instance.PlayButtonClick();
        _menus[menuIndex].gameObject.SetActive(!_menus[menuIndex].gameObject.activeSelf);
    }

    #endregion
}
