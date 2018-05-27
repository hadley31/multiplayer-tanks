using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GizmoHelper : MonoBehaviour
{

	protected virtual void OnEnable ()
	{
		if ( FindObjectOfType<GizmoHelperManager> () == null )
		{
			Debug.LogWarning ("A GizmoHelperManager object is required for this object to work correctly!");
			return;
		}

		GizmoHelperManager.AddHelper (this);
	}

	protected virtual void OnDisable ()
	{
		GizmoHelperManager.RemoveHelper (this);
	}

	public abstract void Draw ();
}
