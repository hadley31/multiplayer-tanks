using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	private static List<Key> registered_keys;
	private static List<Axis> registered_axis;



	public static void RegisterKey (Key key)
	{
		if (registered_keys == null)
		{
			init_registered_keys ();
		}

		if (!Registered (key.Name))
		{
			registered_keys.Add (key);
		}
	}

	public static void UnregisterKey (Key key)
	{
		if ( registered_keys == null )
		{
			return;
		}

		// Remove the key from the list
		registered_keys.Remove (key);

		// If the list is empty, set it equal to null
		if (registered_keys.Count == 0)
		{
			registered_keys = null;
		}			
	}

	private static void init_registered_keys ()
	{
		registered_keys = new List<Key> ();
	}

	public static bool Registered (string name)
	{
		return registered_keys != null && registered_keys.Exists (x => x.Name == name);
	}

	#region Monobehaviours

	private void Update ()
	{
		if (registered_keys != null)
		{

		}

		if (registered_axis != null)
		{
			for (int i = 0; i < registered_axis.Count; i++)
			{
				registered_axis[i].Update_Magnitude ();
			}
		}
	}

	#endregion
}