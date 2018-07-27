using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TildaUI : MonoBehaviour {


    public Canvas canvas;

    public Text log;
    public InputField inputField;

    public CommandLine commandLine;    

    public bool showUI = false;

    public KeyCode consoleKey = KeyCode.BackQuote;

    public KeyCode enterKey = KeyCode.Return;

    public KeyCode upHistory = KeyCode.UpArrow;
    public KeyCode downHistory = KeyCode.DownArrow;

	// Use this for initialization
	void Start ()
    {
        canvas.enabled = showUI;
        log.text = "";
        inputField.text = "";
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(consoleKey))
        {
            showUI = !showUI;
            canvas.enabled = showUI;
            inputField.enabled = showUI;
            if(inputField.enabled)
                inputField.ActivateInputField();

        }


        if(showUI)
        {
            if (Input.GetKeyDown(downHistory))
            {
                inputField.text = CommandLine.instance.toggleCommandHistory(1);
            }
            else if (Input.GetKeyDown(upHistory))
            {
                inputField.text = CommandLine.instance.toggleCommandHistory(-1);
            }

            else if (Input.GetKeyDown(enterKey))
            {
                log.text += inputField.text + "\n";
                string commandReturnVal = commandLine.parseCommand(inputField.text).ToString();
                //Manage Capacity Here
                if(commandReturnVal != "")
                {
                    log.text += commandReturnVal +"\n";
                }
                inputField.text = "";
                inputField.ActivateInputField();
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C))
            {
                clear();
            }
        }

	}

    public void clear()
    {
        log.text = "";
    }

    public override string ToString()
    {
        return "Console Key is: " + consoleKey.ToString() + " && Enter Key is: " + enterKey.ToString() +
            "\nConsole History is controlled by the " +upHistory.ToString()+" && "+ downHistory.ToString() + " keys";
    }

}
