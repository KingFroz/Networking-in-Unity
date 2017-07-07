using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour {
    //Object to follow
    public Transform follow;

    private Camera actionCamera;

    private const float EVASION_HEIGHT = 12.5f;

    void Start() {
        actionCamera = Camera.main;
    }

	void LateUpdate () {
        if (follow != null) {
            actionCamera.transform.position = new Vector3(follow.transform.position.x, EVASION_HEIGHT, follow.transform.position.z);
        }
    }
}
