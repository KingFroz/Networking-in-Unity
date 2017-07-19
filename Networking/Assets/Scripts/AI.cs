
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class AI : Entity {
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    public ParticleSystem ParticleEffect;

    NavMeshAgent pathfinder;
    Transform target;
    bool hasTarget;

    Material skinMaterial;
    Color originalColour;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float damage = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    Entity playerEntity;

    protected override void Start()
    {
        base.Start();

        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponentInChildren<Renderer>().material;
        originalColour = skinMaterial.color;

        

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;

            myCollisionRadius = 0.5f;
            targetCollisionRadius = target.GetComponent<SphereCollider>().radius;

            playerEntity = target.GetComponent<Entity>();
            playerEntity.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }
    }

    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    AudioManager.instance.PlaySound("Enemy Attack", transform.position);
                    StartCoroutine(Attack());
                }

            }
        }
    }

    void OnTargetDeath() {
        hasTarget = false;
        currentState = State.Idle;
    }

    public override void TakeHit(float _damage, Vector3 _hitPoint, Vector3 _hitDirection)
    {

        //AudioManager.instance.PlaySound("Impact", transform.position);

        if (damage >= m_Health)
        {
            AudioManager.instance.PlaySound("Enemy Death", transform.position);
            Destroy(Instantiate(ParticleEffect.gameObject, _hitPoint, Quaternion.FromToRotation(Vector3.forward, _hitDirection)) as GameObject, 2);
        }
       
        base.TakeHit(_damage, _hitPoint, _hitDirection);
    }

    IEnumerator Attack()
    {

        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDmg = false;
        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDmg)
            {
                hasAppliedDmg = true;
                playerEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColour;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!isDead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
