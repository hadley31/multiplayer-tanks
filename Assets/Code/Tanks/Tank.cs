using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EntityHealth))]
public class Tank : Entity, IProjectileInteractive, IDestroyable
{
	private static Plane groundPlane;
	
	public Transform projectileSpawnPoint;
	public Transform top;

	public float fireRate = 0.018f;

	public float moveSpeed = 1;
	public float turnSpeed = 10;
	public float topRotateSpeed = 10;

	public float boost = 0;
	public float boostSpeedMultiplier = 5;
	public float maxBoost = 1;
	public float boostUseSpeed = 3;
	public float boostRegainSpeed = 0.2f;

	public int landmines = 2;
	public int maxLandmines = 2;
	public float landmineUseCooldown = 5;
	public float landmineRechargeCooldown = 20;

	public ProjectileInfo projectileInfo;
	public LandmineInfo landmineInfo;

	public MeshRenderer[] renderers;

	protected new Camera camera;

	protected Rigidbody rb;
	protected BoxCollider col;
	protected Vector3 input;
	protected Vector3 velocity;
	protected float fireTimer = 0;
	protected float landmineUseTimer = 0;
	protected float landmineRechargeTimer = 0;
	protected List<Vector3> wallNormals = new List<Vector3> ();

	public PhotonPlayer owner
	{
		get { return photonView.owner; }
	}

	public int Team
	{
		get;
		protected set;
	}

	#region Monobehaviours

	protected virtual void Awake ()
	{
		if (PhotonNetwork.inRoom && photonView.isMine)
		{
			rb = GetComponent<Rigidbody> ();
			col = GetComponent<BoxCollider> ();

			landmines = maxLandmines;
			camera = Camera.main;

			if ( groundPlane.normal == Vector3.zero )
			{
				groundPlane = new Plane (Vector3.up, Vector3.zero);
			}
		}
	}

	protected virtual void Update ()
	{
		if (photonView.isMine)
		{
			GetInput ();
			Look ();

			LayLandmine ();
			Shoot ();
		}
		else
		{
			RemoteUpdate ();
		}
	}

	protected virtual void FixedUpdate ()
	{
		if (photonView.isMine)
		{
			Move ();
			Rotate ();
		}
	}

	#endregion

	#region Movement

	protected virtual void GetInput ()
	{
		float vert = Input.GetAxisRaw ("Vertical");
		float horiz = Input.GetAxisRaw ("Horizontal");

		input = new Vector3 (horiz, 0, vert).normalized;

		float speed = moveSpeed;
		if ( Input.GetKey (KeyCode.Space) )
		{
			speed = moveSpeed + moveSpeed * boostSpeedMultiplier * boost;
			boost -= Time.deltaTime * boostUseSpeed;
		}
		else if (boost < maxBoost)
		{
			boost += Time.deltaTime * boostRegainSpeed;
		}
		boost = Mathf.Clamp (boost, 0, maxBoost);

		velocity = input * speed;
	}

	protected virtual void Move ()
	{
		rb.AddForce (velocity - rb.velocity, ForceMode.VelocityChange);
	}

	protected virtual void Rotate ()
	{
		if ( velocity.sqrMagnitude > Mathf.Epsilon )
		{
			Quaternion target = Quaternion.LookRotation (velocity, Vector3.up);
			rb.rotation = Quaternion.Slerp (transform.rotation, target, Time.fixedDeltaTime * turnSpeed);
		}
	}

	#endregion

	#region Top Look

	protected virtual void Look ()
	{
		Vector3 target = GetTargetPoint ();
		if ( target == Vector3.zero )
			return;

		Vector3 targetDirection = target - transform.position;
		targetDirection.y = 0;

		Quaternion targetRotation = Quaternion.LookRotation (targetDirection);

		top.rotation = Quaternion.Slerp (top.rotation, targetRotation, Time.deltaTime * topRotateSpeed);
	}

	protected virtual Vector3 GetTargetPoint ()
	{
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);

		float enterPoint;
		if ( groundPlane.Raycast (ray, out enterPoint) )
		{
			return ray.GetPoint (enterPoint);
		}

		return Vector3.zero;
	}

	#endregion

	#region Networking

	protected Vector3 networkPosition;
	protected Vector3 networkVelocity;
	protected float networkRotation;
	protected float networkTopRotation;
	protected double lastNetworkDataReceivedTime = 0;

	protected void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if ( stream.isWriting )
		{
			stream.SendNext (transform.position);
			stream.SendNext (rb.velocity);

			stream.SendNext (transform.eulerAngles.y);
			stream.SendNext (top.eulerAngles.y);
		}
		else
		{
			networkPosition = (Vector3) stream.ReceiveNext ();
			networkVelocity = (Vector3) stream.ReceiveNext ();

			networkRotation = (float) stream.ReceiveNext ();
			networkTopRotation = (float) stream.ReceiveNext ();

			lastNetworkDataReceivedTime = info.timestamp;
		}
	}

	protected void RemoteUpdate ()
	{
		float pingInSeconds = PhotonNetwork.GetPing () * 0.001f;
		float timeSinceLastUpdate = (float) ( PhotonNetwork.time - lastNetworkDataReceivedTime );

		float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

		Vector3 estimatedPosition = networkPosition + ( networkVelocity * totalTimePassed );

		Vector3 newPosition = Vector3.Lerp (transform.position, estimatedPosition, Time.deltaTime * moveSpeed);

		if ( Vector3.SqrMagnitude (estimatedPosition - transform.position) > 25f )
		{
			newPosition = estimatedPosition;
		}

		transform.position = newPosition;

		Quaternion newRotation = Quaternion.Euler (0, networkRotation, 0);
		Quaternion newTopRotation = Quaternion.Euler (0, networkTopRotation, 0);

		transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, Time.deltaTime * turnSpeed);
		top.rotation = Quaternion.Lerp (top.rotation, newTopRotation, Time.deltaTime * topRotateSpeed);
	}

	#endregion

	#region Shoot / Landmine

	public virtual void Shoot ()
	{
		fireTimer -= Time.deltaTime;

		if ( CanShoot () )
		{
			Projectile.Spawn (projectileSpawnPoint.position, projectileSpawnPoint.forward, projectileInfo.moveSpeed, projectileInfo.bounces, PhotonNetwork.player.ID);
			fireTimer = fireRate;
		}
	}

	protected virtual bool CanShoot ()
	{
		return ( fireTimer <= 0 && Input.GetMouseButton (0) && Input.GetKey (KeyCode.LeftShift) ) || Input.GetMouseButtonDown (0);
	}

	public virtual void LayLandmine ()
	{
		landmineUseTimer -= Time.deltaTime;

		if ( landmines < maxLandmines )
		{
			landmineRechargeTimer -= Time.deltaTime;

			if ( landmineRechargeTimer <= 0 )
			{
				landmines++;
				landmineRechargeTimer = landmineRechargeCooldown;
			}
		}
		else
		{
			landmineRechargeTimer = landmineRechargeCooldown;
		}

		if ( CanLayLandmine () )
		{
			print ("Placed Landmine");
			Landmine.SpawnOnNetwork (transform.position, landmineInfo, PhotonNetwork.player.ID);
			landmineUseTimer = landmineUseCooldown;
			landmines--;
		}
	}

	protected virtual bool CanLayLandmine ()
	{
		return landmines > 0 && landmineUseTimer <= 0 && Input.GetKeyDown (KeyCode.X);
	}

	#endregion

	public virtual void OnProjectileInteraction (Projectile p)
	{
		if (PhotonNetwork.isMasterClient)
		{
			p.DestroyObject ();

			photonView.RPC ("DestroyObject", photonView.owner);
		}
	}

	[PunRPC]
	public void DestroyObject ()
	{
		if (photonView.isMine)
		{
			PhotonNetwork.Destroy (photonView);
			FindObjectOfType<GameManager> ().SpawnPlayer ();
		}
	}
}