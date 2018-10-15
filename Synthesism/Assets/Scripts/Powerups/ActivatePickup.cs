using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePickup : MonoBehaviour {

	public float heightOffset;

    //0 for none, 1 for Torpedo and 2 for Oil Spill
    private int powerUpID;

	public Transform spikePosition;
	public Transform spillPosition;
	
	public GameObject spike;
	public GameObject oil;

    private void Start()
    {
        powerUpID = 0;
    }

    public int getPickupID() { return powerUpID; }

    public void UsePowerUp()
    {      
        if(powerUpID == 1)
        {
            FireSpike(transform.forward);
            powerUpID = 0;
        }  
        else if(powerUpID == 2)
        {
            SpillOil(transform.forward);
            powerUpID = 0;
        }
    }

    public void FireSpike(Vector3 dir) {
		GameObject moveSpike = Instantiate(spike);
        moveSpike.GetComponent<SpikeController>().Owner = gameObject;
        moveSpike.transform.position = (spikePosition.position);
		moveSpike.transform.rotation = spikePosition.rotation;
	}

	public void SpillOil(Vector3 dir) {
		GameObject moveSpill = Instantiate(oil);
        moveSpill.GetComponent<OilSpillController>().Owner = gameObject;
		moveSpill.transform.position = (spillPosition.position);
	}

	//initiates slip procedure
    public void StartSlip() {
        StartCoroutine(GetComponent<CarController>().carMovementProperties.slip());
    }

    public void SetPowerUp(int id)
    {
        if (powerUpID == 0) powerUpID = id;
    }
}
