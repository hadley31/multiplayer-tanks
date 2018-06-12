using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode_CaptureTheFlag : Gamemode
{
	public override void OnRoundStart ()
	{
		throw new System.NotImplementedException ();
	}

	public override void OnRoundEnd ()
	{
		throw new System.NotImplementedException ();
	}

	public override string GetShortName ()
	{
		return "CTF";
	}

	public override string ToString ()
	{
		return "Capture the Flag";
	}
}
