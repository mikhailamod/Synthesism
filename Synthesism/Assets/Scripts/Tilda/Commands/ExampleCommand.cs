using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCommand : ICommand
{
    //Actual Functionality called.
    public object executeCommand(string[] args)
    {
        return "You called the " + args[0] + " command";
    }

    //String returned with help <command_name> is called
    public string help()
    {
        return "You asked for help but I am not giving it to you";
    }
}
