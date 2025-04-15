using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<Canvas> _menus = new List<Canvas>();

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
}