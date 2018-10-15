using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoSingleton<RaceManager>
{

    public int numLaps = 1;
    public int numCheckpoints = 0;
    public Dictionary<RaceEntity, List<int>> racers = new Dictionary<RaceEntity, List<int>>();

    public bool raceStarted = false;
    public bool raceFinished = false;

    public GameObject winnerPanel;
    public Text winnerText;

    public void setWinnerPanel(GameObject WP)
    {
        winnerPanel = WP;
    }

    public void setWinnerText(Text WT)
    {
        winnerText = WT;
    }

    public void registerCheckpoint()
    {
        numCheckpoints++;
    }

    public void registerCar(RaceEntity car)
    {
        if(!racers.ContainsKey(car))
            racers[car] = new List<int> { 0, 0 };
    }

    public bool checkpoint(RaceEntity car, int checkpointID)
    {
        if(racers.ContainsKey(car))
        {
            
            if((racers[car][1] + 1) % numCheckpoints == checkpointID)
            {
                racers[car][1] = checkpointID;
                bool lapCompleted =  (racers[car][1] % numCheckpoints == 0) ? true : false;
                racers[car][0] += (lapCompleted) ? 1 : 0;
                if (lapCompleted && !raceFinished && isFinished(car))
                {
                    raceFinished = true;
                    winnerPanel.SetActive(true);
                    winnerText.text = car.name + " Wins!!!";
                }         
                return lapCompleted;
            }
            return false;
        }
        return false;
    }

    public bool isFinished(RaceEntity car)
    {
        if(racers.ContainsKey(car))
        {
            if (racers[car][0] >= numLaps)
                return true;
            else
                return false;
        }
        return false;
    }

    public int getLap(RaceEntity car)
    {
        if (racers.ContainsKey(car))
            return racers[car][0];
        else
            return -1;
    }

    public float getCheckpointPercentage(RaceEntity car)
    {
        if (racers.ContainsKey(car))
            return (racers[car][1]*100)/numCheckpoints;
        else
            return -1;
    }

    public void StartRace()
    {

        StartCoroutine(StartCountdown());
        
    }

    public IEnumerator StartCountdown(float length = 3f)
    {
        while(length > 0)
        {
            MusicManager.instance.PlaySoundEffect(MusicManagerInfo.COUNTDOWN_BEEP);
            Debug.Log("Countdown: " + length);
            yield return new WaitForSeconds(1.0f);
            length--;
        }
        MusicManager.instance.PlaySoundEffect(MusicManagerInfo.COUNTDOWN_BEEP, 1.5f);
        raceStarted = true;
        foreach (RaceEntity e in racers.Keys)
        {
            e.StartRace();
        }
    }
}
