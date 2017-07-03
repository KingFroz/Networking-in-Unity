using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Transform[] muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float velocity = 35;

    private float nextShotTime;

    public bool Shoot() {
        if (Time.time > nextShotTime)
        {
            for (int i = 0; i < muzzle.Length; i++)
            {
                //Convert seconds to MS
                nextShotTime = Time.time + msBetweenShots / 1000;
                PoolManager.instance.ReuseObject(projectile.gameObject, muzzle[i].position, muzzle[i].rotation, projectile.gameObject.transform.localScale);
                //tempProjectile.SetSpeed(velocity);
            }

            return true;
        } else {
            return false;
        }
    }
}
