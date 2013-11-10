using System;
using UnityEngine;

public class OtherPlayer
{
	public string Identity { get; private set; }
	public GameObject Actor { get; set; }
	public Vector3? Position { get; set; }
	
	public OtherPlayer (string identity)
	{
		Identity = identity;
	}
}

