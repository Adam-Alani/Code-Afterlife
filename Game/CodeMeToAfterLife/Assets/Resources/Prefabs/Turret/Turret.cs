using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    
    // Start is called before the first frame update
    /// <summary>
    /// Calls UpdateTarget every UpdateTime secondes
    /// </summary>    
    void Start()
    {
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

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestEnemyDistance && InRangeEnemy(enemy))
            {
                shortestEnemyDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
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
        float yRotationTurret = partToRotate.transform.rotation.y; // takes the rotation of the turret
        float yEnemyDirection = Quaternion.LookRotation(enemy.transform.position - transform.position).eulerAngles.y; // takes the direction of the current chosen enemy
        float deltaAngle = yEnemyDirection - yRotationTurret; // gives the delta between the two
        return (-deltaAngleMax <= deltaAngle && deltaAngle <= deltaAngleMax);
    }

    // Update is called once per frame
    /// <summary>
    /// Calls either TurnAlone or TurnToEnemy if the current enemy is null or not
    /// </summary>
    void Update()
    {
        if (target == null)
            TurnAlone();
        else
            TurnToEnemy();
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