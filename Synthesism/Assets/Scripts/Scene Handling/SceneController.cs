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

    public GameObject winnerPanel;
    public Text winnerText;

    public TextMeshProUGUI lapCountLabel;
    public TextMeshProUGUI lapTimeLabel;
    public TextMeshProUGUI bestTimeLabel;

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

        if(p2Choice != -1)
        {
            GameObject go2 = Instantiate(carTypes[p2Choice], p2StartPos.position, p2StartPos.rotation);
            initializeCar(go2, 1);
            cameraFollowController2.targetObject = go2.transform;
        }
  
        RaceManager.instance.setWinnerPanel(winnerPanel);
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
