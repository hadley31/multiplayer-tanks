using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TankFollowCamera : MonoBehaviour
{
	public Tank tank;
	public Texture2D spectateReticle;
	public CinemachineVirtualCamera virtualCamera;
	public float scrollSpeed = 5;

	private bool tankWasNull = true;

	private void Awake ()
	{
		Prime (tank);
	}

	public void Update ()
	{
		if ( tank && tankWasNull )
		{
			Cursor.SetCursor (spectateReticle, new Vector2 (spectateReticle.width / 2, spectateReticle.height / 2), CursorMode.Auto);
		}
		else if (!tank && !tankWasNull)
		{
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		}
		tankWasNull = !tank;

		if (!tank)
		{
			Vector3 direction = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;

			if ( direction != Vector3.zero )
			{
				transform.Translate (direction * Time.deltaTime * scrollSpeed, Space.World);
			}
		}
	}

	public void Prime (Tank tank)
	{
		this.tank = tank;

		if ( tank )
		{
			virtualCamera.Follow = tank.transform;
			virtualCamera.LookAt = tank.transform;
		}
	}
}
