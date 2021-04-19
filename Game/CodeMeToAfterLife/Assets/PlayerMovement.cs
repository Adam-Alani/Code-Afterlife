using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    
    // those variables are used to perform a smooth rotation of the player
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public Vector3 forward; // used to compute the vertical axis in isometric system
    public Vector3 right; // used to compute the horizontal axis in isometric system

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        // set forward to the forward axis (blue) of the main camera
        // usefull to not compute isometric coordinates
        forward = Camera.main.transform.forward;
        forward.y = 0; 
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        PV = GetComponent<PhotonView>();
    }
    
    // Update is called once per frame
    // an optimized way is to store the previous vector in a variable, and only change motin when input changes
    void Update()
    {
        // if the window is not the one with our characater, do nothing (differentiate the input)
        if (!PV.IsMine)
            return;

        // computation of the normalized direction vector
        Vector3 direction = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");
        direction.y = 0f;
        direction = Vector3.Normalize(direction);

        if (direction.magnitude >= 0.1f)
        {
            // rotation of the character
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation =  Quaternion.Euler(0f, angle, 0f); // rotation around the y axis

            // movement
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
}
