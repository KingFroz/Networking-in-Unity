
using UnityEngine;

public class Player : Entity {
    private Vector3 joystickInput;
        
    //private Camera viewCamera;

    public Joystick mJoystick;

    public float movementSpeed;
    public float rotationSpeed;

    public InteractionButton interact;

    WeaponController weaponControl;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        //viewCamera = Camera.main;
        weaponControl = GetComponent<WeaponController>();
    }

    void Update()
    {
        JoystickMovement();

        if (interact.GetPress())
            Shoot();
    }

    private void JoystickMovement()
    {
        joystickInput = mJoystick.GetDirection();

        float moveX = joystickInput.x;
        float moveZ = joystickInput.y;

        if (joystickInput != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveZ, moveX);

            moveX *= Mathf.Abs(Mathf.Cos(angle));
            moveZ *= Mathf.Abs(Mathf.Sin(angle));

            joystickInput = new Vector3(moveX, 0, moveZ);
            joystickInput = transform.TransformDirection(joystickInput);

            Vector3 tempPosition = transform.position;
            tempPosition.x += moveX;
            tempPosition.z += moveZ;

            Vector3 lookDirection = tempPosition - transform.position;
            if (lookDirection != Vector3.zero)
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);

            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
    }

    private void Shoot() {
        weaponControl.Shoot();
    }

    //private void Controller()
    //{
    //    Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
    //    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    //    float rayDistance;
    //    if (groundPlane.Raycast(ray, out rayDistance)) {
    //        Vector3 point = ray.GetPoint(rayDistance);
    //        Vector3 sendPoint = new Vector3(point.x, transform.position.y, point.z);
    //        transform.LookAt(sendPoint);
    //        //weaponControl.Shoot(rigidBody, new Vector3(transform.forward.x, 0, transform.forward.z), speed);
    //    }
    //}
}
