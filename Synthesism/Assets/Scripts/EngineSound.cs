using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour {

    public List<AudioSource> engineLoop;
    private int currentSound;
    private CarController carController;

    public float timeLimit = 3f;
    float delta = 0f;

    private void Start()
    {
        currentSound = 0;
        foreach(AudioSource sound in engineLoop)
        {
            sound.loop = true;
        }
        carController = GetComponent<CarController>();
    }

    private void Update()
    {
        float tempPitch = (carController.carMovementProperties.GetSpeed() /
        (carController.carMovementProperties.maxSpeed * 2)) + 0.5f;
        if (tempPitch > 1f) { tempPitch = 1f; }//limit pitch just in case it goes over
        engineLoop[currentSound].pitch = tempPitch;

    }

    /*
    IEnumerator playGearShift()
    {
        gearChange.Play();
        yield return new WaitForSeconds(gearChange.clip.length);
        engineLoop[currentSound].Play();
    }
    */
}
