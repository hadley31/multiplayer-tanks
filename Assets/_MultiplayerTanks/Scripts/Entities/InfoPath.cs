using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InfoPath : EntityBase
{
	public InfoPath nextPath;
	public InfoPath branchPath;
	public bool useBranchPath = false;
	public UnityEvent onPass;

	public InfoPath GetNextPath ()
	{
		InfoPath returned = useBranchPath && branchPath != null ? branchPath : nextPath;
		onPass.Invoke ();
		return returned;
	}

	public void OnDrawGizmos ()
	{
		if (nextPath != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, nextPath.transform.position);
		}
		if (branchPath != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (transform.position, branchPath.transform.position);
		}
	}

	public void SetNextPath (InfoPath path)
	{
		nextPath = path;
	}

	public void SetBranchPath (InfoPath path)
	{
		branchPath = path;
	}

	public void SetUseBranchPath (bool use)
	{
		useBranchPath = use;
	}

	public void ToggleUseBranchPath ()
	{
		useBranchPath = !useBranchPath;
	}
}
