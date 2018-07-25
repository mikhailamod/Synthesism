using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandList : MonoBehaviour
{
    public Dictionary<string, ICommand> custom_commands = new Dictionary<string, ICommand>();
}
