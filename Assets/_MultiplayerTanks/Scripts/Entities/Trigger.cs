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

	private Collider m_Collider;
	private Rigidbody m_Rigidbody;

	#region Monobehaviours

	private void Awake ()
	{
		m_Collider = GetComponent<Collider> ();
		m_Rigidbody = GetComponent<Rigidbody> ();

		m_Collider.isTrigger = true;
		m_Rigidbody.isKinematic = true;
		m_Rigidbody.useGravity = false;
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
