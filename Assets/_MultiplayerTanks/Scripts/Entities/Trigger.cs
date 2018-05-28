using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Rigidbody)), RequireComponent (typeof (BoxCollider))]
public class Trigger : Photon.MonoBehaviour
{
	public UnityEvent onTriggerEnter;
	public UnityEvent onTriggerStay;
	public UnityEvent onTriggerExit;

	protected new Collider collider;
	protected new Rigidbody rigidbody;

	#region Monobehaviours

	protected void Awake ()
	{
		collider = GetComponent<Collider> ();
		rigidbody = GetComponent<Rigidbody> ();

		collider.isTrigger = true;
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
	}

	protected void OnTriggerEnter (Collider other)
	{
		Entity ent = other.GetComponent<Entity> ();
		if (ent != null)
		{
			OnTriggerEnterEnt (ent);
		}
	}

	protected void OnTriggerStay (Collider other)
	{
		Entity ent = other.GetComponent<Entity> ();
		if (ent != null)
		{
			OnTriggerStayEnt (ent);
		}
	}

	protected void OnTriggerExit (Collider other)
	{
		Entity ent = other.GetComponent<Entity> ();
		if (ent != null)
		{
			OnTriggerExitEnt (ent);
		}
	}

	#endregion

	protected virtual void OnTriggerEnterEnt (Entity ent)
	{
		InvokeOnTriggerEnter ();
	}

	protected virtual void OnTriggerStayEnt (Entity ent)
	{
		InvokeOnTriggerStay ();
	}

	protected virtual void OnTriggerExitEnt (Entity ent)
	{
		InvokeOnTriggerExit ();
	}

	protected virtual void InvokeOnTriggerEnter ()
	{
		if (onTriggerEnter != null)
		{
			onTriggerEnter.Invoke ();
		}
	}

	protected virtual void InvokeOnTriggerStay ()
	{
		if (onTriggerStay != null)
		{
			onTriggerStay.Invoke ();
		}
	}

	protected virtual void InvokeOnTriggerExit ()
	{
		if (onTriggerExit != null)
		{
			onTriggerExit.Invoke ();
		}
	}
}
