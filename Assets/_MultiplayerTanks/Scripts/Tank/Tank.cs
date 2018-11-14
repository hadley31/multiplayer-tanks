using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[RequireComponent(typeof(Health))]
public class Tank : TankBase
{
    public static Color Default_Color
    {
        get { return new Color(0.85f, 0.85f, 0.85f); }
    }

    public static Tank Local
    {
        get;
        private set;
    }

    public static readonly List<Tank> All = new List<Tank>();
    public static readonly List<Tank> AllAlive = new List<Tank>();
    public static readonly List<Tank> AllDead = new List<Tank>();

    public UnityEvent onSpawn;
    public UnityEvent onDestroy;

    #region Properties

    public bool IsAlive
    {
        get;
        private set;
    }

    public bool IsPlayer
    {
        get { return TankInput != null; }
    }

    public bool IsLocal
    {
        get { return photonView.isMine; }
    }

    public Player Owner
    {
        get { return photonView.owner; }
    }

    public int ID
    {
        get { return photonView.viewID; }
    }

    public string OwnerAlias
    {
        get { return Owner?.Name ?? string.Empty; }
    }

    public int Score
    {
        get { return GetProperty(TankProperty.Score, 0); }
        set { SetProperty(TankProperty.Score, value); }
    }

    public int Kills
    {
        get { return GetProperty(TankProperty.Kills, 0); }
        set { SetProperty(TankProperty.Kills, value); }
    }

    public int Deaths
    {
        get { return GetProperty(TankProperty.Deaths, 0); }
        set { SetProperty(TankProperty.Deaths, value); }
    }

    public Team Team
    {
        get { return Server.Current.GetTeam(GetProperty(TankProperty.Team, 0)); }
        private set { SetProperty(TankProperty.Team, (int)value); }
    }

    public void SetProperty(string key, object value)
    {
        Server.Current.SetProperty(key + this.ID, value);
    }

    public T GetProperty<T>(string key, T defaultValue = default(T))
    {
        return Server.Current.GetProperty(key + this.ID, defaultValue);
    }

    public void ClearProperty(string key)
    {
        Server.Current.ClearProperty(key + this.ID);
    }

    #endregion

    #region Monobehaviours

    private void Awake()
    {
        if (IsLocal && IsPlayer)
        {
            Local = this;
        }

        if (All.Contains(this) == false)
        {
            All.Add(this);
        }
    }

    private void Start()
    {
        if (photonView.isMine && this.IsPlayer)
        {
            SpectatorCamera.Instance?.Follow(this);
        }

        SpawnRPC();
    }

    private void OnDestroy()
    {
        if (this == Local)
        {
            Local = null;
        }

        All.Remove(this);
        AllAlive.Remove(this);
        AllDead.Remove(this);

        SpectatorCamera.Instance?.StopFollowing(this);
    }

    #endregion

    public virtual void OnProjectileInteraction(Projectile p)
    {
        if (p.Sender.ID == this.ID && p.HasBounced == false)
        {
            return;
        }

        if (NetworkManager.IsMasterClient)
        {
            Health.Decrease(p.Damage);
			// Set last person/thing to damage us. Move to health class?
        }

        p.Destroy();
    }

    #region Spawn / Destroy



    public void Spawn()
    {
        if (photonView.isMine == false && NetworkManager.IsMasterClient == false)
        {
            return;
        }

        photonView.RPC("SpawnRPC", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    private void SpawnRPC()
    {
        IsAlive = true;

        UpdateList();

        onSpawn.Invoke();
    }

    public void Respawn(int delay = 0)
    {
        if (photonView.isMine == false || IsAlive == true)
        {
            return;
        }

        Invoke("Spawn", delay);
    }


    public void Destroy()
    {
        if (photonView.isMine == false)
        {
            return;
        }

        photonView.RPC("DestroyRPC", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    private void DestroyRPC()
    {
        IsAlive = false;

        UpdateList();

        onDestroy.Invoke();
        Deaths++;
    }

    public void SetTeam(int team)
    {
        if (IsLocal == false && NetworkManager.IsMasterClient == false)
        {
            return;
        }

        this.Team = team;
        Visuals.RevertToTeamColor();
    }

    #endregion

    private void UpdateList()
    {
        if (IsAlive)
        {
            AllDead.Remove(this);

            if (AllAlive.Contains(this) == false)
            {
                AllAlive.Add(this);
            }
        }
        else
        {
            AllAlive.Remove(this);

            if (AllDead.Contains(this) == false)
            {
                AllDead.Add(this);
            }
        }
    }
}