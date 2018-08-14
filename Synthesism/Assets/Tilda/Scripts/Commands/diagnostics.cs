using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System;

public class diagnostics : ICommand
{

    private static string help_string = "diagnose:\nA command that lists useful system diagnostics\nUsage: 'diagnose <flag1> <flag2> ... <flagN>' || 'diagnose -help'\n" + 
        "Flags:\n-FPS The current FPS\n-count Get total number of gameobjects in the current scene\n-tag <tag_name> returns the total number of GameObjects in the scene with the specified tag_name\n" +
        "-stats The stats of the current machine.";


    public override object executeCommand(string[] args)
    {
        //Diagnose all case
        if(args.Length == 1)
        {
            return diagnoseAll();
        }

        if(args.Length == 2 && args[1].ToLower() == "-help")
        {
            return help_string;
        }

        string to_return = "";
        for(int i = 1; i < args.Length; i++)
        {
            switch(args[i])
            {
                case "-FPS":
                    to_return += getFPS();
                    break;
                case "-count":
                case "-c":
                    to_return += getGameObjectCount();
                    break;
                case "-tag":
                case "-t":
                    if (i + 1 == args.Length)
                        return "Error: No Tag name specified! Format is -tag <tag_name>";
                    to_return += getTagCount(args[i + 1]);
                    i++;
                    break;
                case "-stats":
                case "-s":
                    to_return += getMachineStats();
                    break;
                default:
                    return args[i] + " is not a valid flag. Use 'diagnose -help' for a list of flags.";
            }
            to_return += "\n";
        }
        return to_return.Substring(0,to_return.Length-1);
    }

    private string getMachineStats()
    {
        string to_return = "Stats:\n";
        //Device stats
        to_return += "Device name: " + SystemInfo.deviceName + '\n';
        to_return += "Device type: " + SystemInfo.deviceType + '\n';
        //CPU stats
        to_return += "Processor: " + SystemInfo.processorType + '\n';
        to_return += "Processors: " + SystemInfo.processorCount + '\n';
        to_return += "Processor Frequency: " + SystemInfo.processorFrequency + " Hz\n";
        //Other stuff
        to_return += "Memory: " + SystemInfo.systemMemorySize + "MB\n";
        to_return += "OS: " + SystemInfo.operatingSystem + '\n';
        //GPU
        to_return += "GPU: " + SystemInfo.graphicsDeviceName + '\n';
        to_return += "GPU Memory: " + SystemInfo.graphicsMemorySize + "MB";
        

        return to_return;
    }

    private string getTagCount(string tag_name)
    {
        try
        {
            return tag_name + ": " + GameObject.FindGameObjectsWithTag(tag_name).Length;
        }
        catch(UnityException)
        {
            return tag_name + ": 0";
        }
    }

    private string getGameObjectCount()
    {
        return "GameObjects : " + UnityEngine.Object.FindObjectsOfType<Transform>().Length;
    }

    private string getFPS()
    {
        return "FPS: " + (Mathf.Round(1.0f / Time.deltaTime)).ToString();
    }

    
    private string diagnoseAll()
    {
        string to_return = "";
        to_return += getMachineStats() + '\n';
        to_return += getFPS() + '\n';
        to_return += getGameObjectCount() + '\n';

        return to_return;
    }

    public override string help()
    {
        return help_string;
    }
}
