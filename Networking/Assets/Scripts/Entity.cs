
using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IMoveable, IDamagable
{
    //Movement
    public float speed { get; set;}
    public abstract void Move();

    public float baseHealth;
    protected float m_Health;
    protected bool isDead;

    public event Action OnDeath;

    //Allow Start function to be overridden
    protected virtual void Start() {
        m_Health = baseHealth;
        isDead = false;
    }

    public virtual void TakeHit(float _damage, Vector3 _hitPoint, Vector3 _hitDirection) {
        m_Health -= _damage;

        if (m_Health <= 0 && !isDead) {
            DeathEvent();
        }
    }

    public virtual void TakeDamage(float dmg)
    {
        m_Health -= dmg;

        if (m_Health <= 0 && !isDead) {
            DeathEvent();
        }
    }

    [ContextMenu("Self Destruct")]
    protected void DeathEvent() {
        isDead = true;
        if (OnDeath != null) {
            OnDeath();
        }
        Destroy(gameObject);
    }
}
