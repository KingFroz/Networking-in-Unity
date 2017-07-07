
using UnityEngine;

public class Projectile : PoolObject {
    public LayerMask collisionMask;

    private float dmg = 4;
    private float speed = 35f;
    private float skinWeight = .1f;
    private float lifeTime = 3.5f;

    
    void Start() {
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0) {
            OnHitObject(initialCollisions[0], transform.position);
        }

        lifeTime += Time.time;
    }

	void Update () {
        float distanceThisFrame = speed * Time.deltaTime;

        CheckCollisions(distanceThisFrame);
        transform.Translate(Vector3.forward * distanceThisFrame);

        if (Time.time > lifeTime) {
            Destroy();
        }
	}

    public void SetSpeed(float _speed) { speed = _speed; }

    public override void OnObjectReuse() {
        lifeTime += Time.time;
    }

    void CheckCollisions(float _distane) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _distane + skinWeight, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }

    void OnHitObject(Collider col, Vector3 hitPoint)
    {
        IDamagable damageableObject = col.GetComponent<IDamagable>();
        if (damageableObject != null) {
            damageableObject.TakeHit(dmg, hitPoint, transform.forward);
        }

        Destroy();
    }
}
