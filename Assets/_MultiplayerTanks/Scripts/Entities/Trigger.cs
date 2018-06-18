using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Rigidbody)), RequireComponent (typeof (BoxCollider))]
public class Trigger : Photon.MonoBehaviour
{
	public bool fireOnce = false;

	public UnityEvent onTriggerEnter;
	public UnityEvent onTriggerStay;
	public UnityEvent onTriggerExit;

	protected bool m_Fired = false;

	protected Collider m_Collider;
	protected Rigidbody m_Rigidbody;

	#region Monobehaviours

	protected void Awake ()
	{
		m_Collider = GetComponent<Collider> ();
		m_Rigidbody = GetComponent<Rigidbody> ();

		m_Collider.isTrigger = true;
		m_Rigidbody.isKinematic = true;
		m_Rigidbody.useGravity = false;
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
		onTriggerEnter.Invoke ();
	}

	protected virtual void InvokeOnTriggerStay ()
	{
		onTriggerStay.Invoke ();
	}

	protected virtual void InvokeOnTriggerExit ()
	{
		onTriggerExit.Invoke ();
	}
}
