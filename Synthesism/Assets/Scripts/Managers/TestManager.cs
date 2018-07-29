using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoSingleton<TestManager>
{

    public string usefulInformation;

    private TestSingletonClass testClass = new TestSingletonClass();

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.C))
        {
            Instantiate(gameObject); //Create copy to show how duplicate managers are destroyed
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            testClass.printUsefulInformation(); //Prints the managers useful information
        }
    }
}

//Just to demostrate how it is used
public class TestSingletonClass
{
    //This class wants to get the useful information to print.
    public void printUsefulInformation()
    {
        /*
         * To avoid Game.FindObject && GetComponent etc...
         * We can just do this.
        */

        Debug.Log(TestManager.instance.usefulInformation); //This ensures we can always access the managers at any time. It will also ensure a manager exists and if it doesn't one will be created.

    }
}
