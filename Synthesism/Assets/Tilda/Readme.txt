Tilda Command Line:

Instructions:

To setup:

	1. Create a C# script that inherits from CommandList
	2. Add the newly created script that inherits from CommandList and the CommandLine script
		as components to a gameobject.
	3. Drag and Drop TildaUI prefab into the scene
	4. Specify the CommandLine component to TildaUI
	5. Specify both the TildaUI and the custom CommandList to the CommandLine Component
	
	You should the be all setup

To add command:
	1. Create a new C# script that implements the ICommand interface.
	2. List that command in your custom CommandList Script's command_list property.

[See Example scripts and scence included]