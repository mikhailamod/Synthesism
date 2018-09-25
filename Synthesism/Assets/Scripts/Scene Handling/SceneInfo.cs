using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo: MonoBehaviour  {

	public bool isMultiplayer;
	public GameObject[] cars;
	public GameObject divider;

	private GameObject playerOne;
	private GameObject playerTwo;

	void Start() {
		
		playerOne = Instantiate(cars[0]);
		
		if(isMultiplayer) {
			playerTwo = Instantiate(cars[1]);
			Instantiate(divider);
		}
		else {
			//camera instance
			Camera camera = playerOne.transform.transform.Find("Main Camera").gameObject.GetComponent<Camera>();
			camera.rect = new Rect(0, 0, 1, 1);
		}

	}

}


/*

======= Delete if this doesnt help ==========

[System.Serializable]
public class PlayerInfo {
    public GameObject cars;
	public double[] viewportPositions;
	public double[] cameraPositions;
}

[System.Serializable]
public class MultiplayerInfo : PlayerInfo
{
    public int cars = 2;
	public double[] viewportPositions = new double[] {
		0, 0.5, 1, 0.5, //view port 1
		0, 0, 1, 0.5, //view port 2
	};
	public double[] cameraPositions = new double[] {
		0, 0.5, 1, 0.5, //camera 1
		0, 0, 1, 0.5, //camera 2
	};
}

[System.Serializable]
public class SingleplayerInfo : PlayerInfo
{
    public int cars = 1;
	public double[] viewportPositions = new double[] {
		0, 0.5, 1, 0.5, //view port 1
	};
	public double[] cameraPositions = new double[] {
		0, 0.5, 1, 0.5, //camera 1
	};
}

*/