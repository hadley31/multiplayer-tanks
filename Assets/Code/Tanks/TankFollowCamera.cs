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
			if ( Input.GetKey (KeyCode.W) )
			{
				transform.Translate (Vector3.forward * Time.deltaTime * scrollSpeed, Space.World);
			}
			if ( Input.GetKey (KeyCode.S) )
			{
				transform.Translate (Vector3.back * Time.deltaTime * scrollSpeed, Space.World);
			}
			if ( Input.GetKey (KeyCode.A) )
			{
				transform.Translate (Vector3.left * Time.deltaTime * scrollSpeed, Space.World);
			}
			if ( Input.GetKey (KeyCode.D) )
			{
				transform.Translate (Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
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
