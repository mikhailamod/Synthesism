using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour {

    public List<AudioSource> engineLoop;
    private int currentSound;
    private CarController carController;

    public float timeLimit = 3f;
    float delta = 0f;
    float pitchFactor;
    float originalFactor;

    private void Start()
    {
        currentSound = 0;
        foreach(AudioSource sound in engineLoop)
        {
            sound.loop = true;
        }
        carController = GetComponent<CarController>();
        pitchFactor = carController.carMovementProperties.maxSpeed * 2;
        originalFactor = pitchFactor;
    }

    private void Update()
    {
        float tempPitch = (carController.carMovementProperties.GetSpeed() / pitchFactor) + 0.5f;
        if (tempPitch >= 1f) {
            tempPitch = 0.5f;
            pitchFactor*=2;
        }//limit pitch just in case it goes over
        engineLoop[currentSound].pitch = tempPitch;

    }

    public void resetFactor()
    {
        pitchFactor = originalFactor;
    }
}
