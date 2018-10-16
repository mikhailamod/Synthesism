using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RaceManager : MonoSingleton<RaceManager>
{

    public int numLaps = 1;
    public int numCheckpoints = 0;
    public Dictionary<RaceEntity, List<int>> racers = new Dictionary<RaceEntity, List<int>>();
    public List<RaceEntity> raceEntityPositions = new List<RaceEntity>();

    public bool raceStarted = false;
    public bool raceFinished = false;
    public TextMeshProUGUI winnerText;

    private void Update()
    {
        if(raceStarted)
        {
            raceEntityPositions.Sort(CompareRaceEntities);
        }
    }
    public void setWinnerText(TextMeshProUGUI WT)
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
        {
            racers[car] = new List<int> { 0, 0 };
            raceEntityPositions.Add(car);
        }
    }

    public bool checkpoint(RaceEntity car, int checkpointID)
    {
        Debug.Log("Checkpoint: " + checkpointID);
        if (racers.ContainsKey(car))
        {
            Debug.Log("Contains");
            if ((racers[car][1] + 1) % numCheckpoints == checkpointID)
            {
                Debug.Log("if state");
                racers[car][1] = checkpointID;
                bool lapCompleted =  (racers[car][1] % numCheckpoints == 0) ? true : false;
                racers[car][0] += (lapCompleted) ? 1 : 0;
                if (lapCompleted && !raceFinished && isFinished(car))
                {
                    raceFinished = true;
                    winnerText.text = car.racer_name + " Wins!";
                    winnerText.gameObject.SetActive(true);
                    StartCoroutine(EndRace());
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

    private IEnumerator EndRace()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }

    int CompareRaceEntities(RaceEntity a, RaceEntity b)
    {
        CarController acc = a.GetComponent<CarController>();
        CarController bcc = b.GetComponent<CarController>();
        if(acc.getCurrentNodeCount() == bcc.getCurrentNodeCount())
        {
            float distance = Vector3.Distance(acc.gameObject.transform.position, bcc.gameObject.transform.position);
            if(distance > 0){ return 1; }
            else { return -1; }
        }
        else if(acc.getCurrentNodeCount() > bcc.getCurrentNodeCount())
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
