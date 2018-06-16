using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileInteractive
{
	void OnProjectileInteraction (Projectile p);
}

public interface IDestroyable
{
	void DestroyObject ();
}

public interface IDestroyableRPC : IDestroyable
{
	void DestroyObjectRPC ();
}

public interface IPhotonSerializable
{
	string PhotonSerialize ();
}