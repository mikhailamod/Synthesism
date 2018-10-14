using UnityEngine;
using UnityEngine.UI;

//Facilitates the use of the main menu
//A main menu scene should have a MusicManager instance and RaceManager instance
public class MenuController : MonoBehaviour
{

    public CarSelector carSelector;
    public GameObject[] children;

    private void Start()
    {
        MusicManager.instance.PlaySong();
    }

    public void SinglePlayer()
    {
        disableMenu();
        carSelector.setToActive(false);
    }

    public void TwoPlayer()
    {
        disableMenu();
        carSelector.setToActive(true);
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