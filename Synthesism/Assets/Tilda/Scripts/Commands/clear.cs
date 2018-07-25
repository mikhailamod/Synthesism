using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clear : ICommand {

    private TildaUI ui;

    private static string help_string = "clear:\nClears the console log\nUsage: 'clear' || 'clear -help'";

    public clear(ref TildaUI ui)
    {
        this.ui = ui;
    }

    public object executeCommand(string[] args)
    {
        if(args.Length > 2)
        {
            return "'clear' doesn't take any arguments";
        }
        else if(args.Length == 2)
        {
            if (args[1].ToLower() != "-help")
                return "'clear' doesn't take any arguments";
            else
                return help_string;
        }
        else
        {
            ui.clear();
            return "";
        }
    }

    public string help()
    {
        return help_string;
    }
}
