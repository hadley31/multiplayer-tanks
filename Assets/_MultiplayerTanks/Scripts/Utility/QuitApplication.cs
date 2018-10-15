using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void Quit(int code = 0)
    {
        Application.Quit(code);
    }
}
