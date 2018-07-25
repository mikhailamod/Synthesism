using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLine : MonoBehaviour {


    private static CommandLine _instance;

    public static CommandLine instance
    {
        get
        {
            if (_instance == null)
                _instance = new CommandLine();
            return _instance;
        }
        set { }
    }


    public int commandHistoryCapacity;
    public CommandList customCommands;
    public List<string> command_history;
    private int history_marker = -1;

    public void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }


        if (commandHistoryCapacity < 1)
            command_history = new List<string>();
        else
            command_history = new List<string>(commandHistoryCapacity);

        registerCommand("ls", new ls());
        registerCommand("echo", new echo());
        registerCommand("clear", new clear(ref UIController));
        registerCommand("diagnose", new diagnostics());
        registerCommand("exists", new exists());
        registerCommand("history", new history(command_history));

        foreach(string commandKey in customCommands.custom_commands.Keys)
        {
            registerCommand(commandKey,customCommands.custom_commands[commandKey]);
        }

    }

    public TildaUI UIController;

    private Dictionary<string, ICommand> commands;

	private CommandLine()
    {
        commands = new Dictionary<string, ICommand>();
    }

    public object parseCommand(string s)
    {

        logCommandHistory(s);

        string[] args = s.Split(' ');

        if(commands.ContainsKey(args[0]))
        {
            return commands[args[0]].executeCommand(args);
        }
        else if(args.Length == 2)
        {
            if (args[0].ToLower() == "help" && commands.ContainsKey(args[1]))
                return commands[args[1]].help();
            else
                return "'" + args[1] + "' is not a command. Type 'ls' for a list of available commands";
        }
        else
        {
            return "'" + args[0] + "' is not a command. Type 'ls' for a list of available commands";
        }

    }

    private void logCommandHistory(string text)
    {
        if (commandHistoryCapacity > 0)
        {
            if (commandHistoryCapacity == command_history.Count)
                command_history.RemoveAt(0);
        }
        command_history.Add(text);
        history_marker = command_history.Count;
    }

    public List<string> getCommandIdentifiers()
    {
        List<string> commandIds = new List<string>();

        foreach(string s in commands.Keys)
        {
            commandIds.Add(s);
        }

        return commandIds;
    }

    public string toggleCommandHistory(int change)
    {
        if (command_history.Count == 0)
            return "";

        if(history_marker + change < 0)
        {
            history_marker = 0;
            return command_history[history_marker];
        }
        else if( history_marker + change >= command_history.Count)
        {
            history_marker = command_history.Count;
            return "";
        }
        else
        {
            history_marker += change;
            return command_history[history_marker];
        }
    }

    public void registerCommand(string name, ICommand command)
    {
        if(commands.ContainsKey(name))
        {
            return;
        }

        commands[name] = command;
    }

}
