using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

//This class gets the player(s) car choice and spawns them in the scene at some predetermined
//starting position, then loads all fields required by various components
public class SceneController : MonoBehaviour {

    public List<GameObject> carTypes;
    public List<RubberBand> aiCars;//List of AI cars that require info about the players

    public Transform p1StartPos;
    public Transform p2StartPos;

    [Header("Fields required by other components")]
    public CameraFollowController cameraFollowController;
    public CameraFollowController cameraFollowController2;

    public TextMeshProUGUI winnerText;

    [Header("Player 1")]
    public TextMeshProUGUI lapCountLabel;
    public TextMeshProUGUI lapTimeLabel;
    public TextMeshProUGUI bestTimeLabel;
    public TextMeshProUGUI positionLabel1;
    public Slider speedSlider1;
    public Slider rpmSlider1;

    [Header("Player 2")]
    public TextMeshProUGUI lapCountLabel2;
    public TextMeshProUGUI lapTimeLabel2;
    public TextMeshProUGUI bestTimeLabel2;
    public TextMeshProUGUI positionLabel2;
    public Slider speedSlider2;
    public Slider rpmSlider2;

    public Path path;


    private void Start()
    {
        int p1Choice = PlayerPrefs.GetInt("P1_choice", -1);
        int p2Choice = PlayerPrefs.GetInt("P2_choice", -1);
        PlayerPrefs.DeleteAll();

        if (p1Choice == -1)
        {
            Debug.Log("Serious error, no player 1 choice");
            Application.Quit();
        }

        GameObject go1 = Instantiate(carTypes[p1Choice], p1StartPos.position, p1StartPos.rotation);
        initializeCar(go1, 0);
        cameraFollowController.targetObject = go1.transform;
        PlayerCarController pcc = go1.GetComponent<PlayerCarController>();
        pcc.speedSlider = speedSlider1;
        pcc.rpmSlider = rpmSlider1;
        go1.GetComponent<RaceEntity>().positionLabel = positionLabel1;

        if (p2Choice != -1)
        {
            GameObject go2 = Instantiate(carTypes[p2Choice], p2StartPos.position, p2StartPos.rotation);
            initializeCar(go2, 1);
            cameraFollowController2.targetObject = go2.transform;
            pcc = go2.GetComponent<PlayerCarController>();
            pcc.speedSlider = speedSlider2;
            pcc.rpmSlider = rpmSlider2;
            go2.GetComponent<RaceEntity>().positionLabel = positionLabel2;
        }
  
        RaceManager.instance.setWinnerText(winnerText);

        RaceManager.instance.StartRace();
    }

    void initializeCar(GameObject go, int playerNum)
    {
        PlayerCarController pcc = go.GetComponent<PlayerCarController>();
        pcc.SetPath(path);
        pcc.LoadNodes();
        pcc.setPlayerNum(playerNum);
        go.GetComponent<RaceEntity>().lapCountLabel = lapCountLabel;
        go.GetComponent<RaceEntity>().bestTimeLabel = bestTimeLabel;
        go.GetComponent<RaceEntity>().lapTimeLabel = lapTimeLabel;

        foreach(RubberBand a in aiCars)
        {
            a.addPlayer(pcc);
        }
    }

    public void StartRace()
    {
        RaceManager.instance.StartRace();
    }
}
