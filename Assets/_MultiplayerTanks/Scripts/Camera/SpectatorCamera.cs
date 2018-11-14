using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectatorCamera : MonoBehaviour
{
    public static SpectatorCamera Instance
    {
        get;
        private set;
    }

    public float scrollSpeed = 5;

    [Header("Follow Tank")]
    public float followSpeed = 15.0f;
    public float cameraZoomSpeed = 8.0f;
    public float defaultCameraDistance = 15.0f;

    private Vector3 m_TargetPosition;
    private float m_TargetCameraDistance;

    private readonly List<Tank> m_Tanks = new List<Tank>();

    public List<Tank> Tanks
    {
        get { return m_Tanks; }
    }

    #region Properties

    public Camera Camera
    {
        get;
        private set;
    }

    public Camera MinimapCamera
    {
        get;
        private set;
    }

    public Light Light
    {
        get;
        set;
    }

    public Tank MainTarget
    {
        get { return m_Tanks.Count > 0 ? m_Tanks[0] : null; }
    }

    #endregion

    #region Monobehaviours

    private void Awake()
    {
        Camera = transform.Find("Main Camera Parent/Camera").GetComponent<Camera>();
        MinimapCamera = transform.Find("Minimap").GetComponent<Camera>();
        Light = GetComponentInChildren<Light>();

        m_TargetCameraDistance = defaultCameraDistance;
    }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("A camera rig is already active?!");
            Destroy(this.gameObject);
        }
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        Scroll();
    }

    private void LateUpdate()
    {
        FollowTank();
    }

    #endregion

    private void Scroll()
    {
        if (m_Tanks.Count == 0 || (m_Tanks.Count == 1 && MainTarget.IsAlive == false))
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (direction != Vector3.zero)
            {
                transform.Translate(direction * Time.deltaTime * scrollSpeed, Space.World);
            }
        }
    }

    private void FollowTank()
    {
        if (m_Tanks.Count == 0 || (m_Tanks.Count == 1 && MainTarget.IsAlive == false))
        {
            return;
        }

        UpdateTargetPosition();

        Vector3 camPos = Camera.transform.localPosition;

        camPos.z = Mathf.Lerp(camPos.z, -m_TargetCameraDistance - defaultCameraDistance, Time.deltaTime * cameraZoomSpeed);

        Camera.transform.localPosition = camPos;

        transform.position = Vector3.Lerp(transform.position, m_TargetPosition, Time.deltaTime * followSpeed);
    }

    public void OnlyFollow(params Tank[] tanks)
    {
        m_Tanks.Clear();

        Follow(tanks);
    }

    public void Follow(params Tank[] tanks)
    {
        foreach (Tank t in tanks)
        {
            if (m_Tanks.Contains(t) == false)
            {
                m_Tanks.Add(t);
            }
        }
    }

    public void StopFollowing(Tank tank)
    {
        m_Tanks.Remove(tank);
    }

    public void ToggleFollow(Tank tank)
    {
        if (m_Tanks.Contains(tank))
        {
            StopFollowing(tank);
        }
        else
        {
            Follow(tank);
        }
    }

    private void UpdateTargetPosition()
    {
        List<Tank> aliveTanks = m_Tanks.FindAll(x => x.IsAlive);

        if (aliveTanks.Count == 0)
        {
            m_TargetCameraDistance = 0;
            return;
        }

        if (aliveTanks.Count == 1)
        {
            m_TargetCameraDistance = 0;
            m_TargetPosition = aliveTanks[0].transform.position;
            return;
        }

        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;

        foreach (Tank tank in aliveTanks)
        {
            min = Vector3.Min(min, tank.transform.position);
            max = Vector3.Max(max, tank.transform.position);
        }

        float maxDistance = Vector3.Magnitude(min - max);

        m_TargetCameraDistance = (maxDistance / 2 / Camera.aspect) / Mathf.Tan(Camera.fieldOfView * Mathf.Deg2Rad / 2);
        m_TargetPosition = Vector3.Lerp(min, max, 0.5f);
    }
}
