using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static readonly Color[] Colors = { Color.red, Color.blue, Color.green, Color.grey, Color.yellow, Color.magenta };

    [Header("Cursor")]
    public Image cursorPrefab;

    [SerializeField]
    private List<Tank> m_Tanks = new List<Tank>();
    private List<Image> m_TankCursors = new List<Image>();

    private void LateUpdate()
    {
        UpdateCursors();
    }

    private void UpdateCursors()
    {
        NormalizeCursorCount();

        for (int i = 0; i < m_Tanks.Count; i++)
        {
            if (m_Tanks[i].TankInput == null)
            {
                return;
            }

            m_TankCursors[i].color = Colors[i % Colors.Length];

            Vector3 position = m_Tanks[i].photonView.isMine ?
                m_Tanks[i].TankInput.CursorPosition : TankFollowCameraRig.Instance?.Camera.WorldToScreenPoint(m_Tanks[i].NetworkTank.CursorWorldPosition) ?? Vector3.zero;

            m_TankCursors[i].rectTransform.position = position;
        }
    }

    private void NormalizeCursorCount()
    {
        while (m_TankCursors.Count < m_Tanks.Count)
        {
            Image cursor = Instantiate(cursorPrefab, CanvasPanel.Instance.transform);

            m_TankCursors.Add(cursor);
        }

        while (m_TankCursors.Count > m_Tanks.Count)
        {
            Destroy(m_TankCursors[0].gameObject);
            m_TankCursors.RemoveAt(0);
        }
    }
}
