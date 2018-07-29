using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ICommand : Component
{
    /// <summary>
    /// Functioanlity that is called when the command is called.
    /// </summary>
    /// <param name="args">The arguements of the command. args[0] is the name of the command.</param>
    /// <returns>object. To string is called on that object an the result is printed to the console</returns>
    public abstract object executeCommand(string[] args);
    /// <summary>
    /// Returns a string that highlights the usage of this command.
    /// </summary>
    /// <returns>help string</returns>
    public abstract string help();

}
