using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ProjectileHealth))]
public class Projectile : EntityBase
{
    public const float RADIUS = 0.075f;
    private const float Interaction_Cooldown = 0.03f;

    #region Private Fields

    private float m_InteractTimer;
    private float m_LifeTimer;

    #endregion

    #region Properties

    public float LifeTime
    {
        get;
        private set;
    }

    public float Speed
    {
        get;
        private set;
    }

    public int Damage
    {
        get;
        private set;
    }

    public int MaxBounces
    {
        get;
        private set;
    }

    public int Bounces
    {
        get;
        private set;
    }

    public bool HasBounced
    {
        get { return Bounces < MaxBounces; }
    }

    public float Radius
    {
        get { return RADIUS; }
    }

    public int ID
    {
        get;
        private set;
    }

    public Vector3 Direction
    {
        get
        {
            return Rigidbody.velocity.normalized;
        }
        private set
        {
            Rigidbody.velocity = value.normalized * Speed;
        }
    }

    private int senderID;

    public Tank Sender
    {
        get { return Tank.All?.Find(x => x.ID == senderID); }
    }

    public Player Owner
    {
        get { return Sender?.Owner; }
    }

    public Rigidbody Rigidbody
    {
        get;
        private set;
    }

    #endregion

    #region Monobehaviors

    protected void Awake()
    {
        this.Rigidbody = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        m_InteractTimer -= Time.deltaTime;
        m_LifeTimer -= Time.deltaTime;

        if (m_LifeTimer <= 0)
        {
            Destroy();
        }
    }

    protected void OnTriggerEnter(Collider col)
    {
        if (m_InteractTimer > 0)
            return;

        col.BroadcastMessage("OnProjectileInteraction", this, SendMessageOptions.DontRequireReceiver);

        m_InteractTimer = Interaction_Cooldown;
    }

    #endregion

    public void OnProjectileInteraction(Projectile p)
    {
        Destroy();
    }

    public void Bounce(Vector3 normal)
    {
        if (normal == Vector3.zero || Vector3.Dot(normal, Direction) > 0 || Bounces <= 0)
        {
            Destroy();
            return;
        }

        Direction = Vector3.Reflect(Direction, normal);
        Bounces--;
    }

    #region Property Setters

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetDirection(Vector3 direction)
    {
        this.Direction = direction;
    }

    public void SetLifeTime(float lifetime)
    {
        this.LifeTime = lifetime;
        this.m_LifeTimer = LifeTime;
    }

    public void SetSpeed(float speed)
    {
        this.Speed = speed;
    }

    public void SetDamage(int damage)
    {
        this.Damage = damage;
    }

    public void SetBounces(int maxBounces)
    {
        this.MaxBounces = maxBounces;

        this.Bounces = MaxBounces;
    }

    public void SetID(int id)
    {
        this.ID = id;
    }

    public void SetSenderID(int viewID)
    {
        this.senderID = viewID;

        if (this.Sender?.Team != null)
        {
            SetColor(this.Sender.Team.Color);
        }
    }

    public void SetColor(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }

    #endregion

    #region Destroy

    public void Destroy()
    {
        ProjectileManager.Instance.Destroy(this.ID);
    }

    #endregion
}
