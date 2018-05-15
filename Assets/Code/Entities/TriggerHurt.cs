using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHurt : Trigger
{
	public int damage = 0;
	public float hurtInterval = 1;

	protected float hurtTimer = 0;
	protected List<EntityHealth> hurtList;

	protected override void OnTriggerEnterEnt (Entity ent)
	{
		base.OnTriggerEnterEnt (ent);
		Add (ent.GetComponent<EntityHealth> ());
	}

	protected override void OnTriggerStayEnt (Entity ent)
	{
		if (hurtList != null && hurtList.Count > 0)
		{
			hurtTimer += Time.deltaTime;
			if (hurtTimer >= hurtInterval)
			{
				foreach (EntityHealth h in hurtList)
				{
					if (h != null)
						h.Decrease (damage);
				}
			}
		}
	}

	protected override void OnTriggerExitEnt (Entity ent)
	{
		base.OnTriggerEnterEnt (ent);
		Remove (ent.GetComponent<EntityHealth> ());
	}

	protected virtual void Add (EntityHealth h)
	{
		if (h != null) {
			if (hurtList == null)
				hurtList = new List<EntityHealth> ();

			hurtList.Add (h);
		}
	}

	protected virtual void Remove (EntityHealth h)
	{
		hurtList?.Remove (h);
	}
}
