
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

    public void TakeHit(float dmg, RaycastHit hit) {
        m_Health -= dmg;

        if (m_Health <= 0 && !isDead) {
            DeathEvent();
        }
    }

    public void TakeDamage(float dmg)
    {
        m_Health -= dmg;

        if (m_Health <= 0 && !isDead) {
            DeathEvent();
        }
    }

    protected void DeathEvent() {
        isDead = true;
        if (OnDeath != null) {
            OnDeath();
        }
        Destroy(gameObject);
    }
}
