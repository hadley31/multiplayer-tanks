using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLandmine : MonoBehaviour
{
	public const string resourceName = "Landmine";

	protected Landmine landmine;

	protected void Awake ()
	{
		this.landmine = GetComponent<Landmine> ();
	}

	[PunRPC]
	public void NetworkPrime (Vector3 position, float fuse, int sender, double time)
	{
	//	float dt = (float) ( PhotonNetwork.time - time );
		this.transform.position = position;
		landmine.fuseTime = fuse;
	//	landmine.fuseTimer -= dt;
	}
}
