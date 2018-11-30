using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : TankBase
{
    private static readonly Vector3 XZ = new Vector3(1, 0, 1);

    [Header("Move & Look Info")]
    public float moveSpeed = 4;
    public float rotateSpeed = 10;
    public float lookSpeed = 50;

    [Header("Boost Info")]
    public float boostSpeedMultiplier = 3;
    public float maxBoost = 1;
    public float boostUseSpeed = 3;
    public float boostRegainSpeed = 0.2f;

    [Header("Other Info")]
    public float gravityMultiplier = 1.0f;

    private float m_speed;

    public float Boost
    {
        get;
        private set;
    }

    public bool IsMoving
    {
        get { return Velocity != Vector3.zero; }
    }

    public bool IsGrounded
    {
        get { return Physics.Raycast(Collider.bounds.center, Physics.gravity.normalized, Collider.bounds.extents.y); }
    }

    public bool IsBoosting
    {
        get;
        private set;
    }

    public Rigidbody Rigidbody
    {
        get;
        private set;
    }

    public Collider Collider
    {
        get;
        private set;
    }

    public Transform Top
    {
        get;
        private set;
    }

    public Vector3 TargetDirection
    {
        get;
        private set;
    }

    public bool BoostHeld
    {
        get;
        private set;
    }

    public Vector3 TargetVelocity
    {
        get { return TargetDirection * m_speed; }
    }

    public Quaternion TargetLook
    {
        get;
        private set;
    }

    public Vector3 Velocity
    {
        get { return Vector3.Scale(Rigidbody.velocity, XZ); }
        private set { Rigidbody.velocity = value; }
    }

    public Vector3 ActualVelocity
    {
        get { return Rigidbody.velocity; }
    }

    public Vector3 Position
    {
        get { return Rigidbody.position; }
    }

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Top = transform.Find("Top");
    }

    protected virtual void Update()
    {
        if (Tank.IsAlive == false)
        {
            return;
        }

        UpdateSpeed();
        Look();
    }

    protected virtual void FixedUpdate()
    {
        if (Tank.IsAlive == false)
        {
            return;
        }

        Rotate();
        Move();
    }

    protected virtual void UpdateSpeed()
    {
        m_speed = moveSpeed;

        if (IsMoving && BoostHeld)
        {
            IsBoosting = true;
            m_speed = moveSpeed + moveSpeed * boostSpeedMultiplier * Boost;
            Boost -= Time.deltaTime * boostUseSpeed;
        }
        else
        {
            IsBoosting = false;
            Boost += Time.deltaTime * boostRegainSpeed;
        }

        Boost = Mathf.Clamp(Boost, 0, maxBoost);
    }

    #region Movement

    protected virtual void Move()
    {
        Rigidbody.AddForce(TargetVelocity - Velocity, ForceMode.VelocityChange);
        if (IsGrounded == false)
        {
            Rigidbody.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    protected virtual void Rotate()
    {
        if (TargetDirection.sqrMagnitude > Mathf.Epsilon)
        {
            Quaternion target = Quaternion.LookRotation(TargetDirection, Vector3.up);
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.rotation = Quaternion.Slerp(Rigidbody.rotation, target, Time.fixedDeltaTime * rotateSpeed);
        }
    }

    protected virtual void Look()
    {
        if (TargetLook == Quaternion.identity)
        {
            return;
        }

        Top.rotation = Quaternion.Slerp(Top.rotation, TargetLook, Time.deltaTime * lookSpeed);
    }

    #endregion

    #region Target Setters

    public void SetTargetDirection(Vector3 direction)
    {
        TargetDirection = direction.Limit();
    }

    public void SetBoostHeld(bool value)
    {
        BoostHeld = value;
    }

    public void SetLookTarget(float targetAngle)
    {
        TargetLook = Quaternion.Euler(0, targetAngle, 0);
    }

    public void SetLookTarget(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        targetDirection.y = 0;

        TargetLook = Quaternion.LookRotation(targetDirection);
    }

    #endregion

    #region Boost Setters

    public void SetBoostToMin()
    {
        Boost = 0;
    }

    public void SetBoostToMax()
    {
        Boost = maxBoost;
    }

    public void SetBoost(float amount)
    {
        Boost = Mathf.Clamp(amount, 0, maxBoost);
    }

    #endregion
}
