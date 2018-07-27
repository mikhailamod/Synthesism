using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exists : ICommand
{

    private string help_string = "exists:\nDetermines if a GameObject exists in the game world\nUsage: exists <obj_name> || exists -help";

    public object executeCommand(string[] args)
    {
        if(args.Length > 1)
        {
            if (args[1].ToLower() == "-help")
                return help_string;

            string compound = "";
            for(int i = 1; i < args.Length; i++)
            {
                compound += args[i] + " ";
            }
            return objectExists(compound.Substring(0,compound.Length-1));
        }
        return "Error: exists takes one argument. Usage: exists <obj_name>";
    }

    private bool objectExists(string v)
    {
        foreach(UnityEngine.Object obj in UnityEngine.Object.FindObjectsOfType(typeof(Transform)))
        {
            if(obj.name == v)
            {
                return true;
            }
        }
        return false;
    }

    public string help()
    {
        return help_string;
    }
}
