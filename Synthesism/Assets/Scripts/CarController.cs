using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	// Use this for initialization
	private CarMovement car; 

	void Start () {
		car = GetComponent<CarMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetButtonDown("Horizontal")){
			car.MoveHorizontal();
		}
		else if(Input.GetButtonDown("Vertical")){
			car.MoveVertical();
		}
	}

}
