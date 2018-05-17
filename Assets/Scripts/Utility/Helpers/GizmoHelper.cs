using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GizmoHelper : MonoBehaviour
{
	protected void Awake ()
	{
		if ( FindObjectOfType<GizmoHelperManager> () == null )
		{
			Debug.LogWarning ("A GizmoHelperManager object is required for this object to work correctly!");
		}
	}

	protected virtual void OnEnable ()
	{
		GizmoHelperManager.AddHelper (this);
	}

	protected virtual void OnDisable ()
	{
		GizmoHelperManager.RemoveHelper (this);
	}

	public abstract void Draw ();
}
