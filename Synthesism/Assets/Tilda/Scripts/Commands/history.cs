using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class history : ICommand
{

    private string help_string = "history:\nHistory of previous commands used in this session.\nUsage: 'history' || 'history -help'";

    private List<string> command_history;

    public history(List<string> commandHistory)
    {
        command_history = commandHistory;
    }

    public object executeCommand(string[] args)
    {
        if(args.Length == 1)
        {
            if (command_history.Count == 0)
                return "";

            string to_return = "";
            foreach(string command in command_history)
            {
                to_return += command + "\n";
            }
            return to_return.Substring(0, to_return.Length - 1);
        }
        else if(args.Length == 2 && args[1] == "-help")
        {
            return help_string;
        }
        return "Error: history takes no arguements. Usage: 'history'";
    }

    public string help()
    {
        return help_string;
    }
}
