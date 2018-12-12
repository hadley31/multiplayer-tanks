using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetUI : MonoBehaviour
{
    public CameraTargetUIEntry entryPrefab;

    public void Prime()
    {
        print("CameraTargetUI::Prime()");

        transform.DestroyChildren();

        // foreach (Entity ent in asdfa)
        // {
        //     CameraTargetUIEntry entry = Instantiate(entryPrefab, transform, false);
        //     entry.Prime(ent);
        // }
    }
}
