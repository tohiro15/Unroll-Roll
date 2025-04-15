using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public List<Canvas> menus = new List<Canvas>();
    public int defaultMenuIndex = 0;

    public void SetActiveMenu(int menuIndex)
    {
        if (menuIndex < 0 || menuIndex >= menus.Count)
        {
            Debug.LogError("Invalid menu index!");
            return;
        }

        AudioManager.Instance.PlayButtonClick();

        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].gameObject.SetActive(i == menuIndex);
        }
    }

    public void ToggleMenu(int menuIndex)
    {
        if (menuIndex < 0 || menuIndex >= menus.Count)
        {
            Debug.LogError("Invalid menu index!");
            return;
        }

        AudioManager.Instance.PlayButtonClick();
        menus[menuIndex].gameObject.SetActive(!menus[menuIndex].gameObject.activeSelf);
    }
}