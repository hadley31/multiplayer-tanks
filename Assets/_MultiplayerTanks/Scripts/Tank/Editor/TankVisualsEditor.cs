using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TankVisuals))]
public class TankVisualsEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		if (Application.isPlaying)
		{
			TankVisuals visuals = target as TankVisuals;

			if ( visuals == null )
				return;

			visuals.SetColor (EditorGUILayout.ColorField ("Color", visuals.Color));
		}
		else
		{
			base.OnInspectorGUI ();
		}
		
	}
}
