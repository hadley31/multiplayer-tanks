using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHurt : Trigger
{
	public int damage = 0;
	public float hurtInterval = 1;

	protected float hurtTimer = 0;
	protected List<Health> hurtList;

	protected override void OnTriggerEnterEnt (Entity ent)
	{
		base.OnTriggerEnterEnt (ent);
		Add (ent.GetComponent<Health> ());
	}

	protected override void OnTriggerStayEnt (Entity ent)
	{
		if (hurtList != null && hurtList.Count > 0)
		{
			hurtTimer += Time.deltaTime;
			if (hurtTimer >= hurtInterval)
			{
				foreach (Health h in hurtList)
				{
					if (h != null)
						h.DecreaseHealth (damage);
				}
			}
		}
	}

	protected override void OnTriggerExitEnt (Entity ent)
	{
		base.OnTriggerEnterEnt (ent);
		Remove (ent.GetComponent<Health> ());
	}

	protected virtual void Add (Health h)
	{
		if (h != null) {
			if (hurtList == null)
				hurtList = new List<Health> ();

			hurtList.Add (h);
		}
	}

	protected virtual void Remove (Health h)
	{
		hurtList?.Remove (h);
	}
}
