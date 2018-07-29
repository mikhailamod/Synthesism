Tilda Command Line:

Instructions:

To setup:

	1. Create a new custom command list scriptable object (can be done through the  menu)
	2. Drag and Drop TildaUI prefab into the scene
	3. Specify the CommandLine component to TildaUI
	4 Specify both the TildaUI and the custom CommandList scriptable object to the CommandLine Component
	
	You should the be all setup

To add command:
	1. Create a new C# script that implements the ICommand interface.
	2. List that command in your custom CommandList scriptable object by specifying the alias of your command as well as the name of custom command

[See Example scripts and scence included]