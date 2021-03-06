﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public int playerNumber;
    public Vector3[] spawnPoints;

    private Vector3 forward; // vertical axis in isometric system
    private Vector3 right; // horizontal axis in isometric system
    public PhotonView PV;

    public GameObject EscapeMenu;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        // if it's my screen and the code ediotr isn't opened
        if (PV.IsMine && !(IsTerminalOpen() || EscapeMenu.active)) // Main Scene and Don't Destroy on Load Photon MOno
            Move();
    }

    /// <summary>
    /// checks if the terminal is open or not
    /// </summary>
    bool IsTerminalOpen()
    {
        GameObject terminal = GameObject.Find("Terminal");
        return terminal != null;
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
        //Debug.Log($"EscapeMenu status : {EscapeMenu.active}");
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

    public void SetPlayerNumber(int num)
    {
        playerNumber = num;
    }

    public void SetPlayerSpawnPoints(Vector3[] spawnpoints)
    {
        spawnPoints = spawnpoints;
    }

    /// <summary>
    /// detects when hit by a laser and kill the player lol
    /// </summary>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Laser")
        {
            FindObjectOfType<AudioManager>().Play("LaserShot");
            
			controller.enabled = false;
            transform.localPosition = spawnPoints[playerNumber];
            Debug.Log("turret pew pew player");
			controller.enabled = true;
        }
    }
  public void SpeedUpPlayer()
	{
		speed = 130;
		Debug.Log("Player goes zoom");
	}

	public void ResetPlayerSpeed()
	{
		speed = 35;
		Debug.Log("Player goes rompiche");
	}
}
