using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour {

    public List<AudioSource> engineLoop;
    private int currentSound;
    private CarController carController;

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
        engineLoop[currentSound].pitch = (carController.carMovementProperties.GetSpeed() / carController.carMovementProperties.maxSpeed) + 0.5f;
    }
}
