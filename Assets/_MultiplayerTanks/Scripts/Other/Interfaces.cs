using UnityEngine;

public interface IProjectileInteractive
{
	GameObject GameObject
	{
		get;
	}

	void OnProjectileInteraction (Projectile p);
}

public interface IPhotonSerializable
{
	string PhotonSerialize ();
}