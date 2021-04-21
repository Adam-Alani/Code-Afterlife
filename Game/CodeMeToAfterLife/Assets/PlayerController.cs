using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    
    public Vector3 forward; // vertical axis in isometric system
    public Vector3 right; // horizontal axis in isometric system
    public PhotonView PV;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }
        
    // Update is called once per frame
    void Update()
    {
        // if it's my screen and the code ediotr isn't opened
        if (PV.IsMine && SceneManager.sceneCount <= 2) // Main Scene and Don't Destroy on Load Photon MOno
            Move();
    }
    
    /// <summary>
    /// Setup the forward and right axis in isometric system
    /// </summary>
    void Setup()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0; 
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }
    
    /// <summary>
    ///  Perform the Movement
    /// </summary>
    void Move()
    {
        Vector3 direction = GetDirection();
        if (direction.magnitude >= 0.1f)
        {
            PlayerRotate(direction);
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Use the key inputs to compute the direction of the player
    /// </summary>
    Vector3 GetDirection()
    {
        Vector3 direction = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");
        direction.y = 0f;
        direction = Vector3.Normalize(direction);
    
        return direction;
    }

    /// <summary>
    /// Rotation the player based on his direction
    /// </summary>
    void PlayerRotate(Vector3 direction)
    {
        // create the angle
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // smooth the rotation
        float angle =
            Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        // rotate around the y axis
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
