using UnityEngine;
using UnityEngine.UI;

//Facilitates the use of the main menu
//A main menu scene should have a MusicManager instance and RaceManager instance
public class MenuController : MonoBehaviour
{

    public CarSelector selectController;
    public GameObject[] children;

    private void Start()
    {
        MusicManager.instance.StartMusic();
    }

    public void SinglePlayer()
    {
        MusicManager.instance.PlaySoundEffect(MusicManagerInfo.MAIN_MENU_BEEP);
        disableMenu();
        selectController.setToActive(false);
    }

    public void TwoPlayer()
    {
        MusicManager.instance.PlaySoundEffect(MusicManagerInfo.MAIN_MENU_BEEP);
        disableMenu();
        selectController.setToActive(true);
    }

    public void Quit()
    {
        MusicManager.instance.PlaySoundEffect(MusicManagerInfo.MAIN_MENU_BEEP);
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