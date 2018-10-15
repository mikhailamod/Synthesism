using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {


    public GameObject[] spawnables;

    public Transform[] spawnLocations;

    public float spawnTime = 1.0f;

	void Start ()
    {
	    foreach(Transform t in spawnLocations)
        {
            Instantiate(spawnables[Random.Range(0, spawnables.Length)], t.position, Quaternion.identity);
        }
	}
	
    public void SpawnNew(Vector3 pos)
    {
        StartCoroutine(ISpawn(pos));
    }

    IEnumerator ISpawn(Vector3 position)
    {
        yield return new WaitForSeconds(spawnTime);
        Instantiate(spawnables[Random.Range(0, spawnables.Length)], position, Quaternion.identity);
    }

}
