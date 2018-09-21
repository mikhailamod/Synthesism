using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Controller/Create Controller Map")]
public class KeyMapping : ScriptableObject 
{
	public string vertAxis;
	public string horizontalAxis;
	public string handbrake;
}
