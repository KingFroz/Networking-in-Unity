using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : MonoBehaviour {

    Transform nearestTarget;
    List<Transform> avaibleTargets = new List<Transform>();

    void Update()
    {
        if (avaibleTargets.Count > 0) {
            SelectTarget();

            transform.LookAt(nearestTarget);
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }
    }
 
    void SelectTarget()
    {
        float baseLength = -1f;
        foreach (Transform enemy in avaibleTargets)
        {
            if (enemy == null) {
                avaibleTargets.Remove(enemy);
                return;
            }

            Vector3 vectorToTarget = enemy.transform.position - transform.position;
            vectorToTarget.Normalize();

            float lengthOfAngle = Vector3.Dot(transform.forward, vectorToTarget);
            if (lengthOfAngle > baseLength)
            {
                nearestTarget = enemy.transform;
                baseLength = lengthOfAngle;
            }
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Enemy")
        {
            Transform targetInfo = col.gameObject.transform;
            avaibleTargets.Add(targetInfo);
        }
    }

    void OnTriggerExit(Collider col) {
        avaibleTargets.Remove(col.transform);
    }
}
