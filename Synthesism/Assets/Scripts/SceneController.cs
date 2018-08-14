using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour {

    public Text speedText;
    public Text rpmText;
    public AICarController car;

    public float delta = 0.5f;
    float timePassed = 0f;

	// Use this for initialization
	void Start () {
        speedText.text = "Speed: 0";
        rpmText.text = "RPM: 0";
	}
	
	// Update is called once per frame
	void Update () {
        if(timePassed > delta)
        {
            speedText.text = "Speed: " + car.getCurrentSpeed().ToString("0.##");
            rpmText.text = "RPM: " + car.getRpm().ToString("0.##");
        }
        else
        {
            timePassed += Time.deltaTime;
        }
        
	}
}
