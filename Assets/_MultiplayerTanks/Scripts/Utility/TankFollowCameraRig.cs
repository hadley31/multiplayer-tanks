using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TankFollowCameraRig : MonoBehaviour
{
	public static TankFollowCameraRig Instance
	{
		get;
		private set;
	}

	public Texture2D crosshair;
	public float scrollSpeed = 5;

	[Header ("Follow Tank")]
	public float followSpeed = 15.0f;

	private readonly List<Tank> m_Tanks = new List<Tank> ();

	private Vector3 m_TargetPosition;
	private bool m_TankWasNull = true;

	#region Properties

	public Camera Camera
	{
		get;
		private set;
	}

	public Camera MinimapCamera
	{
		get;
		private set;
	}

	public Light Light
	{
		get;
		set;
	}

	public Tank MainTarget
	{
		get { return m_Tanks.Count > 0 ? m_Tanks[0] : null; }
	}

	#endregion

	#region Monobehaviours

	private void Awake ()
	{
		Camera = transform.Find ("Camera").GetComponent<Camera> ();
		MinimapCamera = transform.Find ("Minimap").GetComponent<Camera> ();
		Light = GetComponentInChildren<Light> ();
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

	private void Update ()
	{
		UpdateCursor ();
		Scroll ();
	}

	private void LateUpdate ()
	{
		FollowTank ();
	}

	#endregion

	private void UpdateCursor ()
	{
		if ( m_Tanks.Count > 0 && m_TankWasNull )
		{
			Cursor.SetCursor (crosshair, new Vector2 (crosshair.width / 2, crosshair.height / 2), CursorMode.Auto);
		}
		else if ( m_Tanks.Count == 0 && !m_TankWasNull )
		{
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		}
		m_TankWasNull = m_Tanks.Count == 0;
	}

	private void Scroll ()
	{
		if ( m_Tanks.Count == 0 || m_TargetPosition == Vector3.zero )
		{
			Vector3 direction = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;

			if ( direction != Vector3.zero )
			{
				transform.Translate (direction * Time.deltaTime * scrollSpeed, Space.World);
			}
		}
	}

	private void FollowTank ()
	{
		UpdateTargetPosition ();

		transform.position = Vector3.Lerp (transform.position, m_TargetPosition, Time.deltaTime * followSpeed);
	}

	public void OnlyFollow (params Tank[] tank)
	{
		m_Tanks.Clear ();
		m_Tanks.AddRange (tank);
	}

	public void Follow (Tank tank)
	{
		if ( m_Tanks.Contains (tank) )
		{
			return;
		}

		m_Tanks.Add (tank);
	}

	public void StopFollowing (Tank tank)
	{
		m_Tanks.Remove (tank);
	}

	private void UpdateTargetPosition ()
	{
		List<Tank> aliveTanks = m_Tanks.FindAll (x => x.IsAlive);

		if (aliveTanks.Count == 0)
		{
			return;
		}

		Vector3 min = Vector3.positiveInfinity;
		Vector3 max = Vector3.negativeInfinity;

		foreach (Tank tank in aliveTanks)
		{
			min = Vector3.Min (min, tank.transform.position);
			max = Vector3.Max (max, tank.transform.position);
		}

		m_TargetPosition = Vector3.Lerp (min, max, 0.5f);
	}
}
