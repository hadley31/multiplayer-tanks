using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : Photon.MonoBehaviour
{
	public InfoPath startingPath;
	public float moveSpeed = 1f;
	public float rotateSpeed = 1000f;
	public float acceleration = 100;
	public bool moving = false;
	public bool faceTargetPath = false;

	protected InfoPath currentTarget;
	protected float speed = 0;

	protected virtual void Awake ()
	{
		currentTarget = startingPath;

		if ( currentTarget != null )
		{
			transform.position = currentTarget.transform.position;
		}
	}

	protected virtual void Update ()
	{
		if ( currentTarget != null )
		{
			if ( moving )
			{
				if ( Vector3.SqrMagnitude (transform.position - currentTarget.transform.position) > 0.01f )
				{
					speed = Mathf.MoveTowards (speed, moveSpeed, Time.deltaTime * acceleration);
					transform.position = Vector3.MoveTowards (transform.position, currentTarget.transform.position, Time.deltaTime * speed);
				}
				else
				{
					currentTarget = currentTarget.GetNextPath ();
				}
			}
			else
			{
				speed = 0;
			}
		}
		if ( faceTargetPath )
		{
			Quaternion targetRotation = Quaternion.LookRotation (currentTarget.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
		}
	}

	public void SetSpeed (float speed)
	{
		this.moveSpeed = speed;
	}

	public void SetRotateSpeed (float speed)
	{
		this.rotateSpeed = speed;
	}

	public void IncreaseSpeed (float amount)
	{
		this.moveSpeed += amount;
	}
}
