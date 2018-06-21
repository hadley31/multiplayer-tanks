using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpawner : MonoBehaviour
{
	public static TankSpawner Instance
	{
		get;
		private set;
	}

	public List<Transform> spawnPositions;

	private int m_Index = 0;

	private void OnEnable ()
	{
		if ( Instance == null )
		{
			Instance = this;
		}
		else if ( Instance != this )
		{
			Debug.Log ("A tank spawner already exists. Destroying new instance.");
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

	public void Spawn (Tank tank)
	{
		Transform point = GetNextSpawnPoint ();

		if ( point == null )
		{

			return;
		}

		tank.Movement.Rigidbody.MovePosition (point.position);
		tank.Movement.Rigidbody.MoveRotation (point.rotation);

		tank.SpawnRPC ();
	}
	
	public Transform GetNextSpawnPoint ()
	{
		if (spawnPositions.Count == 0)
		{
			return null;
		}

		if (m_Index >= spawnPositions.Count)
		{
			m_Index = 0;
		}

		return spawnPositions[m_Index++];
	}
}
