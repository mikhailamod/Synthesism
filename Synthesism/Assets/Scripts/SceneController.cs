using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour {

    public List<GameObject> carTypes;
    public Transform p1StartPos;
    public Transform p2StartPos;

    public CameraFollowController cameraFollowController;

    public GameObject winnerPanel;
    public Text winnerText;

    public Text lapCountLabel;
    public Text lapTimeLabel;
    public Text bestTimeLabel;

    public Path path;


    private void Start()
    {
        int p1Choice = PlayerPrefs.GetInt("P1_choice", -1);
        int p2Choice = PlayerPrefs.GetInt("P2_choice", -1);

        if(p1Choice == -1)
        {
            Debug.Log("Serious error, no player 1 choice");
            Application.Quit();
        }

        GameObject go1 = Instantiate(carTypes[p1Choice], p1StartPos.position, p1StartPos.rotation);
        initializeCar(go1);
        cameraFollowController.targetObject = go1.transform;

        if(p2Choice != -1)
        {
            GameObject go2 = Instantiate(carTypes[p2Choice], p2StartPos.position, p2StartPos.rotation);
            initializeCar(go2);
        }
  
        RaceManager.instance.setWinnerPanel(winnerPanel);
        RaceManager.instance.setWinnerText(winnerText);
    }

    void initializeCar(GameObject go)
    {
        go.GetComponent<PlayerCarController>().SetPath(path);
        go.GetComponent<PlayerCarController>().LoadNodes();
        go.GetComponent<RaceEntity>().lapCountLabel = lapCountLabel;
        go.GetComponent<RaceEntity>().bestTimeLabel = bestTimeLabel;
        go.GetComponent<RaceEntity>().lapTimeLabel = lapTimeLabel;
    }

    public void StartRace()
    {
        RaceManager.instance.StartRace();
    }
}
