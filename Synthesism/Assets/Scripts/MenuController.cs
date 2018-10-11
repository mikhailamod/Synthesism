using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class MenuController : MonoBehaviour
{

    public CarSelector carSelector;
    public GameObject[] children;

    bool isMultiplayer = false;

    private void Start()
    {
        MusicManager.instance.PlaySong();
    }

    public void SinglePlayer()
    {
        isMultiplayer = false;
        disableMenu();
        carSelector.setToActive();
    }

    public void TwoPlayer()
    {
        isMultiplayer = true;
        disableMenu();
        carSelector.setToActive();
    }

    public void Quit()
    {
        Application.Quit();
    }

    void disableMenu()
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetActive(false);
        }
    }
}