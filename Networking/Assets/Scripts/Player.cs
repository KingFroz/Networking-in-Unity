using System;
using UnityEngine;

public class Player : Entity {
    private Vector3 destination;
    private bool flag;

    private Rigidbody rigidBody;
    
    private Camera viewCamera;

    WeaponController weaponControl;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        weaponControl = GetComponent<WeaponController>();
        speed = 50f;
    }

    void Update()
    {
        ShootToMove();
    }

    private void ShootToMove()
    {
        if (Input.touchCount > 0)
            Controller();
    }

    private void Controller()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 sendPoint = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(sendPoint);
            weaponControl.Shoot(rigidBody, new Vector3(-transform.forward.x, 0, -transform.forward.z), speed);
        }
    }

    public override void Move()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved
            || (Input.GetMouseButton(0)))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray();

            //Touch 
#if UNITY_EDITOR
            ray = viewCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
#elif UNITY_ANDROID
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#else 
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
            //Check if ray hits any collider
            if (Physics.Raycast(ray, out hit))
            {
                if (true)//hit.collider.gameObject.layer == 12)
                {
                    flag = true;
                    destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }
            }

            //check if the flag for movement is true and the current gameobject position is not same as the clicked / tapped position
            if (flag && !Mathf.Approximately(gameObject.transform.position.magnitude, destination.magnitude))
            {
                //move the gameobject to the desired position
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, destination, Time.deltaTime * speed);
            }
            //set the movement indicator flag to false if the endPoint and current gameobject position are equal
            else if (flag && Mathf.Approximately(gameObject.transform.position.magnitude, destination.magnitude))
            {
                flag = false;
            }

            transform.LookAt(destination + Vector3.up * transform.position.y);
        }
    }
}
