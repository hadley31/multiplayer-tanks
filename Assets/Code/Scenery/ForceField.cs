using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
	public Vector3 moveDirection;
	public float moveSpeed = 1;

	private void Update ()
	{
		transform.Translate (moveDirection * Time.deltaTime * moveSpeed);
	}
}
