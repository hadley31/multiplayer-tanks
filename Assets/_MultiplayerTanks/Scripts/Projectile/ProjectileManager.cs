using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool), typeof(PhotonView))]
public class ProjectileManager : Photon.MonoBehaviour
{
    public static ProjectileManager Instance
    {
        get;
        private set;
    }

    public static int GetNextID()
    {
        return Instance.m_ProjectileID++;
    }

    #region Private Fields

    private int m_ProjectileID = 0;
    private ObjectPool m_Pool;
    private List<Projectile> m_Projectiles = new List<Projectile>();

    #endregion

    #region Properties

    public int ProjectileCount
    {
        get { return m_Projectiles.Count; }
    }

    public int ReserveCount
    {
        get { return m_Pool.ReserveCount; }
    }

    #endregion

    #region Monobehaviours

    private void Awake()
    {
        m_Pool = GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #endregion

    #region Spawn / Destroy

    public void SpawnNew(Vector3 position, Vector3 direction, int bounces, float radius, int damage, int health, float speed, int viewID, double createTime)
    {
        photonView.RPC("SpawnNewRPC", PhotonTargets.All, position, direction, bounces, radius, damage, health, speed, viewID, GetNextID(), createTime);
    }

    [PunRPC]
    private void SpawnNewRPC(Vector3 position, Vector3 direction, int bounces, float radius, int damage, int health, float speed, int viewID, int id, double createTime)
    {
        Projectile p = m_Pool.Spawn<Projectile>();

        float dt = (float)(PhotonNetwork.time - createTime);
        position += direction * dt;

        p.SetPosition(position);
        p.SetSpeed(speed);
        p.SetDirection(direction);
        p.SetBounces(bounces);
        p.SetDamage(damage);
        p.SetLifeTime(20);
        p.SetSenderID(viewID);
        p.SetID(id);
        p.Health.SetMaxValue(health, true);
        p.transform.SetParent(transform);
        //p.transform.localScale = Vector3.one * radius;

        m_Projectiles.Add(p);

        m_ProjectileID = id + 1;
    }

    public void Destroy(int id)
    {
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("DestroyRPC", PhotonTargets.All, id);
        }
        else
        {
            DestroyRPC(id);
        }
    }

    [PunRPC]
    public void DestroyRPC(int id)
    {
        if (Instance == null)
        {
            return;
        }

        m_Projectiles.RemoveAll(x => x == null);

        Projectile projectile = m_Projectiles.Find(x => x.ID == id);

        if (projectile != null)
        {
            m_Projectiles.Remove(projectile);
            m_Pool.Reserve(projectile);
        }
    }

    #endregion
}
