using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager Current
    {
        get;
        private set;
    }

    public static readonly Color[] Colors = { Color.red, Color.blue, Color.green, Color.grey, Color.yellow, Color.magenta };

    [Header("Cursor")]
    public Image cursorPrefab;
    private List<Image> m_TankCursors = new List<Image>();

    private List<Tank> Tanks
    {
        get { return TankFollowCameraRig.Instance.Tanks.FindAll(x => x.IsPlayer); }
    }


    private void OnEnable()
    {
        Current = this;
    }

    private void OnDisable()
    {
        if (Current == this)
        {
            Current = null;
        }
    }


    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        UpdateCursors();
    }

    private void UpdateCursors()
    {
        UpdateCursorCount();

        for (int i = 0; i < Tanks.Count; i++)
        {
            if (Tanks[i].IsPlayer == false)
            {
                continue;
            }

            m_TankCursors[i].color = Colors[i % Colors.Length];

            if (Tanks[i].IsLocal)
            {
                if (Cursor.visible == true)
                {
                    m_TankCursors[i].rectTransform.position = Tanks[i].TankInput.CursorPosition;
                }
            }
            else
            {
                m_TankCursors[i].rectTransform.position = TankFollowCameraRig.Instance.Camera.WorldToScreenPoint(Tanks[i].NetworkTank.CursorWorldPosition);
            }
        }
    }

    private void UpdateCursorCount()
    {
        while (m_TankCursors.Count < Tanks.Count)
        {
            Image cursor = Instantiate(cursorPrefab, CanvasPanel.Instance.transform);

            m_TankCursors.Add(cursor);
        }

        while (m_TankCursors.Count > Tanks.Count)
        {
            Destroy(m_TankCursors[0].gameObject);
            m_TankCursors.RemoveAt(0);
        }
    }
}
