using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Gamemode
{
	public abstract void OnRoundStart ();
	public abstract void OnRoundEnd ();
	public abstract string GetShortName ();
	public abstract override string ToString ();
}
