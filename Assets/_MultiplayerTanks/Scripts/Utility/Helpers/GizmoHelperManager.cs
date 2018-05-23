using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoHelperManager : MonoBehaviour
{
	private static List<GizmoHelper> helpers = new List<GizmoHelper> ();

	public new bool enabled = true;

	private void OnDrawGizmos ()
	{
		foreach (GizmoHelper helper in helpers)
		{
			if (helper.enabled)
			{
				helper.Draw ();
			}
		}
	}

	public static void AddHelper (GizmoHelper helper)
	{
		if (!helpers.Contains (helper))
		{
			helpers.Add (helper);
		}
	}

	public static void RemoveHelper (GizmoHelper helper)
	{
		helpers.Remove (helper);
	}

	public static T GetHelper<T> () where T : GizmoHelper
	{
		return (T) helpers.Find (x => x is T);
	}
}
