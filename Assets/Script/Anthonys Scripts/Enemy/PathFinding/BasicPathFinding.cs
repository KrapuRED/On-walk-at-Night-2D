using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicPathFinding : MonoBehaviour
{
    [SerializeField] private Transform waypointParent;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float checkChangeDirction = 0.5f;
    [SerializeField] private bool isLooping;

    private Transform[] waypoints;
    private int currentIndex = 0;
    private Rigidbody2D rb;


    private void Start()
    {
        waypoints = new Transform[waypointParent.childCount];
        for(int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
        rb = GetComponent<Rigidbody2D>();
    }

    public void EnemySpeedSetUp(float speed)
    {
        movementSpeed = speed;
    }

    public void MoveTowardWayPoint()
    {
        Transform target = waypoints[currentIndex];

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = dir * movementSpeed;

        if (Vector2.Distance(rb.position, target.position) <= checkChangeDirction)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                if (isLooping)
                {
                    currentIndex = 0;
                }
                else
                {
                    rb.position = waypoints[0].position;
                    currentIndex = 1;
                }
            }


        }
    }
}
