using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Rigidbody)), RequireComponent (typeof (BoxCollider))]
public sealed class Trigger : Photon.MonoBehaviour
{
	public EntUnityEvent onTriggerEnter;
	public EntUnityEvent onTriggerStay;
	public EntUnityEvent onTriggerExit;

	public Collider Collider
	{
		get;
		private set;
	}
	public Rigidbody Rigidbody
	{
		get;
		private set;
	}

	#region Monobehaviours

	private void Awake ()
	{
		Collider = GetComponent<Collider> ();
		Rigidbody = GetComponent<Rigidbody> ();

		Collider.isTrigger = true;
		Rigidbody.isKinematic = true;
		Rigidbody.useGravity = false;
	}

	private void OnTriggerEnter (Collider other)
	{
		Entity ent = other.GetComponent<Entity> ();
		if (ent != null)
		{
			onTriggerEnter.Invoke (ent);
		}
	}

	private void OnTriggerStay (Collider other)
	{
		Entity ent = other.GetComponent<Entity> ();
		if (ent != null)
		{
			onTriggerStay.Invoke (ent);
		}
	}

	private void OnTriggerExit (Collider other)
	{
		Entity ent = other.GetComponent<Entity> ();
		if (ent != null)
		{
			onTriggerExit.Invoke (ent);
		}
	}

	#endregion
}
