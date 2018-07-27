using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCommandList : CommandList {

	//Add all the functions you want in your command line terminal here [Working on a better way]. This is excluding the builtin commands they are included automatically.
	void Start ()
    {
        //format is custom_commands[<unique command name>] = <command object>
        //custom_commands is dictionary<string,ICommmand> created in the CommandList
        custom_commands["example"] =  new ExampleCommand();
	}
	
}
