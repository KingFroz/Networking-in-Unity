using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject {

    private float speed = 35f;
    public void SetSpeed(float _speed) { speed = _speed; }

	void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}
}
