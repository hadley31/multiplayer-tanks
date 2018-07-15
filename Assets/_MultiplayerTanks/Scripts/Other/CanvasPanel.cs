using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CanvasGroup))]
public class CanvasPanel : MonoBehaviour
{
	public static CanvasPanel Instance
	{
		get;
		private set;
	}

	public CanvasGroup Group
	{
		get;
		private set;
	}

	private void Awake ()
	{
		Group = GetComponent<CanvasGroup> ();
	}


	private void OnEnable ()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy (gameObject);
		}
	}

	private void OnDisable ()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}
}
