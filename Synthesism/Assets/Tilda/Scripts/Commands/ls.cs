using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ls : ICommand
{
    private static string help_string = "ls:\nListing command\nUsage:\nUsage:'ls' || 'ls <flag>'\n" + 
        "Flags:\n -help\n-command Lists all the commands available in this command line. Default function and can also be called by typing 'ls'\n"+
        "-tag <tag_name> lists all the games tags\n"+
        "-objects <optional: tag_name> lists all the gameobjects";

    public override object executeCommand(string[] args)
    {
        
        if (args.Length == 1)
        {
            return listCommands();
        }
        
        if(args[1].ToLower() == "-help")
        {
            if (args.Length == 2)
                return help_string;
            else
                return "Error: -help takes no parameters. Usage: ls -help";
        }
        else if(args[1] == "-tag" || args[1] == "-t")
        {
            if (args.Length == 2)
                return listTags();
            else
                return "Error: -tag takes 1 parameter. Usage: ls -tag <tag_name>";
        }
        else if(args[1] == "-objects" || args[1] == "-o")
        {
            if (args.Length == 2)
            {
                return listAllGameObjects();
            }
            else if (args.Length == 3)
            {
                try
                {
                    return listAllGameObjectsWithTag(args[2]);
                }
                catch(UnityException)
                {
                    return "Error: Tag '" + args[2] + "' does not exist";
                }
            }
            else
            {
                return "Error: Incorrect Format for -objects. Usage: -objects <optional:tag_name>";
            }
        }
        else if(args[1] == "-command" || args[1] == "-c")
        {
            if (args.Length == 2)
                return listCommands();
            else
                return "Error: -command takes no parameters. Usage: ls -command";
        }
        else
        {
            string to_search = "";
            int tag_index = -1;
            for(int i = 1; i < args.Length; i++)
            {
                if (args[i] == "-component" || args[i] == "-c")
                {
                    tag_index = i;
                    break;
                }   
                to_search += args[i] + " ";
            }

            if(to_search == "")
            {
                return "Error: No GameObject specified. Usage: ls <obj_name> <optional:<-component> <component_name>>";
            }
            if(tag_index < 0)
                return listGameObjectComponents(to_search.Substring(0,to_search.Length-1));

            if (tag_index + 1 == args.Length)
                return "Error: No component specified. Usage: ls <obj_name> <optional:<-component> <component_name>>";

            return listGameObjectComponent(to_search.Substring(0, to_search.Length - 1), args[tag_index + 1]);
        }
    }

    private object listGameObjectComponent(string obj_string, string component_name)
    {
        GameObject obj = GameObject.Find(obj_string);
        if (obj)
        {
            Component comp = obj.GetComponent(component_name);
            if(comp)
            {
                return comp.ToString();
            }
            else
            {
                return "Error: " + obj_string + " does not have a component of type " + component_name;
            }
        }
        return "Error: " + obj_string + " does not exist.";
    }

    private string listGameObjectComponents(string obj_string)
    {
        GameObject obj = GameObject.Find(obj_string);
        if(obj)
        {
            string to_return = obj_string + ":\n";
            foreach(Component component in obj.GetComponents<Component>())
            {
                to_return += component.GetType().Name + "\n";
            }
            return to_return.Substring(0, to_return.Length - 1);
        }
        return "Error: " + obj_string + " does not exist.";
    }

    private object listAllGameObjectsWithTag(string v)
    {
        string to_return = "";

        foreach (UnityEngine.Object obj in GameObject.FindGameObjectsWithTag(v))
        {
            to_return += obj.name + "\n";
        }

        return to_return.Substring(0, to_return.Length - 1);
    }

    private object listAllGameObjects()
    {
        string to_return = "";

        foreach(UnityEngine.Object obj in UnityEngine.Object.FindObjectsOfType(typeof(Transform)))
        {
            to_return += obj.name + "\n";
        }

        return to_return.Substring(0, to_return.Length - 1);
    }

    private object listCommands()
    {
        string toReturn = "";
        foreach (string s in CommandLine.instance.getCommandIdentifiers())
        {
            toReturn += " '" + s + "'";
        }
        return toReturn;
    }

    public string listTags()
    {
        string toReturn = "";

        /*foreach(string tag in UnityEditorInternal.InternalEditorUtility.tags)
        {
            toReturn += tag + '\n';
        }*/
        return toReturn.Substring(0, toReturn.Length - 1);
    }

    public override string help()
    {
        return help_string;
    }
}
