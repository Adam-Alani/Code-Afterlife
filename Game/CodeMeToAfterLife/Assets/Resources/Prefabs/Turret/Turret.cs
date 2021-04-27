using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Turret : MonoBehaviour
{
    public Transform target; // the current target (will be private but I keep it public to debug)

    public string Tag; // the enemy the turret is allowed to shoot
    public float UpdateTime; // how long between each UpdateTarget calls
    public float maxRange; // range of the turret ("in meters")
    public float rotationSpeed; // general speed when someone is in range
    public float freeRotationSpeed; //of how much it turns when no one's here (depends on rotationSpeed) => + : clockwise  AND - : trigonometric
    public Transform partToRotate; // head of the turret
    public float deltaAngleMax; // the max delta between the turret and the enemy (180 for a circle)
    public GameObject limitLeft; // left border
    public GameObject limitRight; // right border
    public GameObject limitMid;
    public bool off; // disables the turret
    public bool limits; // disables the limits view
    public bool ForceMidLaser; // forces or not the mid laser
    public float turretPrecision;
    public GameObject codeEditor;

    public GameObject redWire;
    public GameObject greenWire;
    private bool forceDisable;

    
    // Start is called before the first frame update
    /// <summary>
    /// Calls UpdateTarget every UpdateTime secondes
    /// </summary>    
    void Start()
    {
        limitLeft.transform.localScale = new Vector3(0.03f, 0.03f, maxRange);
        limitRight.transform.localScale = new Vector3(0.03f, 0.03f, maxRange);
        limitMid.transform.localScale = new Vector3(0.01f, 0.01f, maxRange);

        limitLeft.SetActive(limits);
        limitRight.SetActive(limits);


        limitMid.SetActive(false);
        off = false;
        InvokeRepeating("UpdateTarget", 0f, UpdateTime);
    }


    /// <summary>
    /// Finds the closest enemy in range to shoot
    /// </summary>
    void UpdateTarget () 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tag);
        float shortestEnemyDistance = maxRange;
        GameObject nearestEnemy = null;
        //Debug.Log("Shortest distance to enemy : " + shortestEnemyDistance); //************************************************
        foreach (GameObject enemy in enemies)
        {
            CharacterController collider = enemy.GetComponent<CharacterController>();

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position) - collider.radius;
            //Debug.Log("Distance to enemy : " + distanceToEnemy); //*********************************************
            if (distanceToEnemy < shortestEnemyDistance && InRangeEnemy(enemy))
            {
                shortestEnemyDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        //Debug.Log("End Foreach");//*****************************************************
        
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        } else
        { 
            target = null;
        }
    }


    /// <summary>
    /// check with the angles if the target is in range (no behind the turret for example)
    /// </summary>
    /// <param name="enemy"> the current enemy check </param>

    bool InRangeEnemy (GameObject enemy)
    {
        float yRotationTurret = partToRotate.transform.rotation.eulerAngles.y; // takes the rotation of the turret
        float yEnemyDirection = Quaternion.LookRotation(enemy.transform.position - transform.position).eulerAngles.y; // takes the direction of the current chosen enemy
        float deltaAngle = yEnemyDirection - yRotationTurret; // gives the delta between the two
        return (-deltaAngleMax <= deltaAngle && deltaAngle <= deltaAngleMax);
    }

    [PunRPC]
    void Disable()
    {
        redWire.SetActive(false);
        greenWire.SetActive(true);
        forceDisable = true;
        off = true;
    }


    [PunRPC]
    void UpdateRpc()
    {
        off = codeEditor.GetComponent<CodeEditor>().Solved;

        if (off)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Disable", RpcTarget.All);
        }



        limitLeft.SetActive(limits);
        limitRight.SetActive(limits);

        
        //Debug.Log(Shoot() || ForceMidLaser);


        //Shoot();
        if (target == null)
            TurnAlone();
        else
            TurnToEnemy();


        limitRight.transform.rotation = Quaternion.Euler(0f, partToRotate.transform.rotation.eulerAngles.y + deltaAngleMax/2, 0f);
        limitLeft.transform.rotation = Quaternion.Euler(0f, partToRotate.transform.rotation.eulerAngles.y - deltaAngleMax/2, 0f);
        limitMid.transform.rotation = Quaternion.Euler(0f, partToRotate.transform.rotation.eulerAngles.y, 0f);
    }


    // Update is called once per frame
    /// <summary>
    /// Calls either TurnAlone or TurnToEnemy if the current enemy is null or not
    /// </summary>
    void Update()
    {
        limitMid.SetActive(!forceDisable && (Shoot() || ForceMidLaser));

        if (off)
            return;

        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("UpdateRpc", RpcTarget.All);

    }


    /// <summary>
    /// Turns alone
    /// </summary>
    void TurnAlone()
    {
        Quaternion whereToRotate = Quaternion.Euler(0f, partToRotate.rotation.eulerAngles.y + freeRotationSpeed, 0f);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, whereToRotate, Time.deltaTime * rotationSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }


    float Abs(float n)
    {
        if (n<0)
            return -n;
        return n;

    }

    bool Shoot()
    {

        return target != null && Abs(Quaternion.LookRotation(target.position - transform.position).eulerAngles.y - partToRotate.transform.rotation.eulerAngles.y) < turretPrecision;
    }


    /// <summary>
    /// Turns to saved enemy
    /// </summary>
    void TurnToEnemy() 
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }


    /// <summary>
    /// Draws the range of the turret when selected
    /// </summary>
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRange);
    }
}
