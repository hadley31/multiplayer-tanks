using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerResetBall : MonoBehaviour
{
	public void ResetBall (Entity ent)
	{
		if (ent.Is<SoccerBall> ())
		{
			ent.GetComponent<SoccerBall> ().ResetPosition ();
		}
	}
}
