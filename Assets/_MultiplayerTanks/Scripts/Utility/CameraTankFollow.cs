using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTankFollow : MonoBehaviour
{
	public static CameraTankFollow Instance
	{
		get;
		private set;
	}

	public Tank tank;
	public Texture2D spectateReticle;
	public CinemachineVirtualCamera virtualCamera;
	public float scrollSpeed = 5;

	private bool tankWasNull = true;

	private void Awake ()
	{
		Prime (tank);
	}

	private void OnEnable ()
	{
		if ( Instance == null )
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning ("A camera rig is already active?!");
			Destroy (this.gameObject);
		}
	}

	private void OnDisable ()
	{
		if (Instance == this)
		{
			Instance = null;
		}
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

		if (!tank || !tank.IsAlive)
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
		}
	}
}
