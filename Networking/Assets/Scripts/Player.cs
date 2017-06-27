using System;
using UnityEngine;

public class Player : Entity {
    private Vector3 destination;
    private bool flag;

    private Camera viewCamera;

    public enum State { Moving, Firing }
    private State _state;

    public override void InitMovement()
    {
        speed = 2.0f;
        _state = State.Moving;
    }

    // Use this for initialization
    void Start () {
        InitMovement();
        viewCamera = Camera.main;
    }

    void FixedUpdate () {
        DFSM();
	}

    private void DFSM()
    {
        //If Double Tap: Change State
        if (Input.touchCount > 0)
        {

        }

        switch (_state)
        {
            case State.Moving:
                Move();
                break;
            case State.Firing:
                break;
        }
    }
    public override void TakeActionU()
    {
        throw new NotImplementedException();
    }

    public override void TakeActionD()
    {
        throw new NotImplementedException();
    }

    public override void TakeActionL()
    {
        throw new NotImplementedException();
    }

    public override void TakeActionR()
    {
        throw new NotImplementedException();
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
