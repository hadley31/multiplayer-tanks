using UnityEngine;

public interface IProjectileInteractive
{
	void OnProjectileInteraction (Projectile p);
}

public interface IPhotonSerializable
{
	string PhotonSerialize ();
}