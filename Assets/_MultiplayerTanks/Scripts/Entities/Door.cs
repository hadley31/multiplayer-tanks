using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : Photon.MonoBehaviour
{
	public bool isOpen = false;
	public float moveSpeed = 10;
	public Vector3 openDirection = Vector3.up;

	public UnityEvent onOpen;
	public UnityEvent onClose;
	public UnityEvent onFullyOpen;
	public UnityEvent onFullyClosed;

	private Vector3 closedPosition;

	public Vector3 openPosition
	{
		get
		{
			return closedPosition + Vector3.Scale (transform.lossyScale, openDirection);
		}
	}

	private void Awake ()
	{
		closedPosition = transform.position;
	}

	private void Update ()
	{
		if (isOpen && Vector3.SqrMagnitude (transform.position - openPosition) > 0.01f)
		{
			transform.position = Vector3.MoveTowards (transform.position, openPosition, Time.deltaTime * moveSpeed);
		}
		else if ( !isOpen && Vector3.SqrMagnitude (transform.position - closedPosition) > 0.01f )
		{
			transform.position = Vector3.MoveTowards (transform.position, closedPosition, Time.deltaTime * moveSpeed);
		}
	}

	public void Open ()
	{
		if ( !isOpen )
			onOpen.Invoke ();
		isOpen = true;
	}

	public void Close ()
	{
		if ( isOpen )
			onClose.Invoke ();
		isOpen = false;
	}

	public void Toggle ()
	{
		if ( isOpen )
			Close ();
		else
			Open ();
	}
}
