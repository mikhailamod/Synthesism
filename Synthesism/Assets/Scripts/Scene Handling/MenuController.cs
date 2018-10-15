using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Facilitates the use of the main menu
//A main menu scene should have a MusicManager instance and RaceManager instance
public class MenuController : MonoBehaviour
{

    public TrackSelector trackSelector;

    private void Start()
    {
        MusicManager.instance.StartMusic();
    }

    public void SinglePlayer()
    {
        MusicManager.instance.PlaySoundEffect(MusicManagerInfo.MAIN_MENU_BEEP);
        TrackSelector.instance.setMultiplayer(false);
        TrackSelector.instance.setCanChoose(true);
        TrackSelector.instance.setTrackChoice(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TwoPlayer()
    {
        MusicManager.instance.PlaySoundEffect(MusicManagerInfo.MAIN_MENU_BEEP);
        TrackSelector.instance.setMultiplayer(true);
        TrackSelector.instance.setCanChoose(true);
        TrackSelector.instance.setTrackChoice(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        MusicManager.instance.PlaySoundEffect(MusicManagerInfo.MAIN_MENU_BEEP);
        Application.Quit();
    }
}