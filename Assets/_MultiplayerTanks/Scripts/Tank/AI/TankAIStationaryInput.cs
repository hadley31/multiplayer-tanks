using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIStationaryInput : TankBase
{
	[Header ("Targeting")]
	public float updateTargetTime;
	public float updateTargetPointTime;

	[Header ("Rotate")]
	public float rotateSpeed = 1;
	public float centerAngle = 0;
	public float angleRadius = 90f;

	[Header ("Shooting")]
	public float shootAngleThreshold = 0.1f;

	private Vector3 m_TargetPoint = Vector3.zero;

	private float m_UpdateTargetTimer;
	private float m_UpdateTargetPointTimer;

	public Tank Target
	{
		get;
		private set;
	}

	private void Update ()
	{
		if (photonView.isMine == false)
		{
			return;
		}

		m_UpdateTargetTimer += Time.deltaTime;
		m_UpdateTargetPointTimer += Time.deltaTime;

		if ( m_UpdateTargetTimer >= updateTargetTime)
		{
			SelectNewTarget ();
			m_UpdateTargetTimer = 0;
		}

		if ( m_UpdateTargetPointTimer >= updateTargetPointTime )
		{
			UpdateTargetPoint ();
			m_UpdateTargetPointTimer = 0;
		}

		float lookTarget = centerAngle + angleRadius * Mathf.Sin (Time.time * rotateSpeed);

		Movement.SetLookTarget (lookTarget);

		if ( Target != null && m_TargetPoint != Vector3.zero )
		{
			float ang = GetAngle (m_TargetPoint);
			if ( ang < shootAngleThreshold )
			{
				Shooting.Shoot ();
			}
		}
	}

	private void SelectNewTarget ()
	{
		List<Tank> players = Tank.AllAlive.FindAll (x => x.IsPlayer);

		if (players.Count == 0)
		{
			Target = null;
			return;
		}

		int closest = 0;
		float closestDist = Mathf.Infinity;

		for (int i = 0; i < players.Count; i++)
		{
			float dist = Vector3.SqrMagnitude (transform.position - players[i].transform.position);

			if (dist < closestDist)
			{
				closest = i;
				closestDist = dist;
			}
		}

		Target = players[closest];
	}

	private void UpdateTargetPoint ()
	{
		if (Target == null)
		{
			return;
		}

		List<RaycastWallHit> wallhits = RaycastWallHit.GetWallHits (Movement.Position, 20, Shooting.radius);

		Vector3 point = Vector3.zero;
		float minAngle = Mathf.Infinity;

		foreach (var hit in wallhits)
		{
			Vector3 reflectPoint = hit.GetReflectionPoint (Movement.Top.position, Target.Movement.Top.position);
			float angle = GetAngle (reflectPoint);

			if (reflectPoint != Vector3.zero && angle < minAngle)
			{
				point = reflectPoint;
				minAngle = angle;
			}
		}

		m_TargetPoint = point;
	}


	private float GetAngle (Vector3 point)
	{
		return Quaternion.Angle (Quaternion.LookRotation (point - Movement.Position), Movement.Top.rotation);
	}
}
