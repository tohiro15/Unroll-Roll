using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public List<Canvas> menus = new List<Canvas>();
    public int defaultMenuIndex = 0;
    public AudioSource uiAudioSource;
    public AudioClip buttonClickSound;

    private void Start()
    {
        if (uiAudioSource == null)
        {
            uiAudioSource = gameObject.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
        }

        SetActiveMenu(defaultMenuIndex);
    }

    public void SetActiveMenu(int menuIndex)
    {
        if (menuIndex < 0 || menuIndex >= menus.Count)
        {
            Debug.LogError("Invalid menu index!");
            return;
        }

        PlayButtonSound();

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

        PlayButtonSound();
        menus[menuIndex].gameObject.SetActive(!menus[menuIndex].gameObject.activeSelf);
    }

    private void PlayButtonSound()
    {
        if (uiAudioSource != null && buttonClickSound != null)
        {
            uiAudioSource.PlayOneShot(buttonClickSound);
        }
    }
}