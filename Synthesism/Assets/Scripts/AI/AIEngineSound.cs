using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngineSound : MonoBehaviour {

    public List<AudioSource> engineLoop;
    public float maxPitch = 1.1f;
    public float minPitch = 0.5f;

    private int currentSound;
    private CarController carController;

    private float interval = 0.1f;
    private float delta;
    private float averageSpeed;
    private int counter;

    private void Start()
    {
        delta = interval;
        currentSound = 0;
        foreach (AudioSource sound in engineLoop)
        {
            sound.loop = true;
        }
        carController = GetComponent<CarController>();
        counter = 0;
        averageSpeed = 0f;
    }

    private void Update()
    {
        delta -= Time.deltaTime;

        averageSpeed += carController.carMovementProperties.GetSpeed();
        counter++;

        if (delta < 0)
        {
            averageSpeed = averageSpeed / counter;
            if(averageSpeed > carController.carMovementProperties.maxSpeed)
            {
                averageSpeed = carController.carMovementProperties.maxSpeed;
            }
            counter = 0;
            float tempPitch = averageSpeed / carController.carMovementProperties.maxSpeed + 0.25f;
            if (tempPitch > maxPitch) { tempPitch = maxPitch; }
            else if(tempPitch < minPitch) { tempPitch = minPitch; }
            engineLoop[currentSound].pitch = tempPitch;
        }
    }
}
