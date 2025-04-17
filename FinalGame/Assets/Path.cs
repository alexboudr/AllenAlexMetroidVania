using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathType
    {
        Loop,
        ReverseWhenComplete
    }

    public Transform[] waypoints;
    public PathType pathType = PathType.Loop;

    private int direction = 1;
    int index;


    public Vector3 GetCurrentWaypoint()
    {
        return waypoints[index].position;
    }

    public Vector3 GetNextWaypoint()
    {
        if(waypoints.Length == 0) return transform.position;

        index = GetNextWaypointIndex();
        Vector3 nextWaypoint = waypoints[index].position;
        
        return nextWaypoint;
    }

    private int GetNextWaypointIndex()
    {
        // move to the next index
        index += direction;

        if(pathType == PathType.Loop)
        {
            index %= waypoints.Length;
        }
        else if(pathType == PathType.ReverseWhenComplete)
        {
            if(index >= waypoints.Length || index < 0)
            {
                direction *= -1;
                index += direction * 2;
            }
        }

        return index;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
