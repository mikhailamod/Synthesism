using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CarController))]
public class RaceEntity : MonoBehaviour {


    public bool isAi = false;
    public int carType;//0 is slow car, 1 is fast car

    public TextMeshProUGUI lapCountLabel;
    public TextMeshProUGUI lapTimeLabel;
    public TextMeshProUGUI bestTimeLabel;
    public TextMeshProUGUI positionLabel;

    private void Start()
    {
        RaceManager.instance.registerCar(this);
    }

    public float lapTime;
    public float bestLapTime;
    private float startLapTime;
    public int position;
    public float posUpdateLimit = 0.1f;
    private float delta = 0f;

    public void StartRace()
    {
        startLapTime = Time.time;
        if(!isAi) lapCountLabel.text = RaceManager.instance.getLap(this) + "/" + RaceManager.instance.numLaps;
        bestLapTime = float.MaxValue;
        position = -1;
    }

    private void Update()
    {
        delta += Time.deltaTime;
        if(RaceManager.instance.raceStarted && !isAi)
        {
            lapTime = Time.time - startLapTime;
            lapTimeLabel.text = milliTimeToString(lapTime);
            position = getPosition();
            if (delta > posUpdateLimit)
            {
                delta = 0f;
                positionLabel.text = (position + 1) + "/" + (RaceManager.instance.raceEntityPositions.Count);
            }
        }      
    }

    int getPosition()
    {
        int pos = -1;
        for(int i = 0; i < RaceManager.instance.raceEntityPositions.Count; i++)
        {
            if(RaceManager.instance.raceEntityPositions[i].Equals(this))
            {
                pos = i;
                return pos;
            }
        }
        return pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!RaceManager.instance.raceStarted)
        {
            return;
        }

        if(other.CompareTag("Checkpoint"))
        {
            int checkPointNum = other.GetComponent<Checkpoint>().checkPointNum;
            if(RaceManager.instance.checkpoint(this, checkPointNum))
            {
                Debug.Log("Lap Completed");
                if(!isAi) lapCountLabel.text = RaceManager.instance.getLap(this) + "/" + RaceManager.instance.numLaps;
                if (lapTime < bestLapTime)
                {
                    bestLapTime = lapTime;
                    if(!isAi) bestTimeLabel.text = milliTimeToString(bestLapTime);
                }
                startLapTime = Time.time;
            }
        }
    }

    private string milliTimeToString(float time)
    {
        float seconds = (int)( time % 60);
        float minutes = (int) ((time / 60) % 60);

        string minuteStr = (minutes < 10) ? "0" + minutes : minutes + "";
        string secondStr = (seconds < 10) ? "0" + seconds : seconds + "";

        return minuteStr + ":" + secondStr;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }
}
