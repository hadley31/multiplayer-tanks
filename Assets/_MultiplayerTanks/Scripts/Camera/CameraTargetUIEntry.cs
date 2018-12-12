using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraTargetUIEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public Entity Target
    {
        get;
        private set;
    }

    public void Prime(Entity target)
    {
        this.Target = target;

        nameText.text = target?.name;
    }


    public void Remove()
    {
        EntityFollowCamera camera = CameraController.CurrentVCam as EntityFollowCamera;

        if (camera == null)
        {
            return;
        }

        camera.StopFollowing(Target);
    }
}
