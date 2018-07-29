using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomCommandList", menuName = "Tilda/Create Custom Command List")]
public class CommandList : ScriptableObject
{
    public List<CustomCommand> commandList;
}

[System.Serializable]
public class CustomCommand
{
    public string alias;
    public string commandClass;
}
