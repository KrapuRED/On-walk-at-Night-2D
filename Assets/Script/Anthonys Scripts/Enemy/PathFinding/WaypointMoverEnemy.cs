using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMoverEnemy : MonoBehaviour
{
    [SerializeField] private Transform waypointParent;
    [SerializeField] private float checkChangeDirection = 0.5f;
    [SerializeField] private bool isLooping;

    [Header("Setting")]
    [SerializeField] private float _delayToNextCheckPoint = 0.1f;

    private Transform[] waypoints;
    private int currentIndex = 0;
    private Rigidbody2D rb;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (waypointParent != null)
        {
            waypoints = new Transform[waypointParent.childCount];
            for (int i = 0; i < waypointParent.childCount; i++)
            {
                waypoints[i] = waypointParent.GetChild(i);
            }
        }
    }

    public Vector2 MoveTowardWayPoint()
    {
        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _delayToNextCheckPoint)
            {
                _isWaiting = false;
                _waitTimer = 0f;
                GoToNextWaypoint();
            }
            return Vector2.zero;
        }

        Transform target = waypoints[currentIndex];
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;

        if (Vector2.Distance(rb.position, target.position) <= checkChangeDirection)
        {
            if (_delayToNextCheckPoint > 0f)
            {
                _isWaiting = true;
                _waitTimer = 0f;
                return Vector2.zero;
            }
            else
            {
                GoToNextWaypoint();
            }
        }

        return dir;
    }

    private void GoToNextWaypoint()
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
                currentIndex = waypoints.Length > 1 ? 1 : 0;
            }
        }
    }
}
