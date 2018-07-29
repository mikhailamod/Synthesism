using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echo : ICommand
{

    private static string help_string = "echo:\nPrints a message to the console.\nUsage: 'echo <message to echo>' || 'echo -help'";

    public override object executeCommand(string[] args)
    {

        if(args.Length == 1)
        {
            return "";
        }
        else if(args.Length == 2 && args[1].ToLower() == "-help")
        {
            return help_string;
        }
        else
        {
            string toReturn = "";
            for(int i = 1; i < args.Length; i++)
            {
                toReturn += " " + args[i];
            }
            return toReturn;
        }
    }

    public override string help()
    {
        return help_string;
    }
}
