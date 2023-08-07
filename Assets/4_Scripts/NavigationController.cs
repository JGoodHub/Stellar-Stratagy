using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationController : Singleton<NavigationController>
{
    private List<NavigationWaypoint> allWaypoints;

    private void Awake()
    {
        allWaypoints = FindObjectsOfType<NavigationWaypoint>().ToList();
    }

    public NavigationPath GetPathBetweenWaypoints(NavigationWaypoint start, NavigationWaypoint end)
    {
        HashSet<NavigationWaypoint> exploredWaypoints = new HashSet<NavigationWaypoint>();
        Queue<NavigationWaypoint> waypointsQueue = new Queue<NavigationWaypoint>();
        Dictionary<NavigationWaypoint, NavigationWaypoint> previousWaypoint = new Dictionary<NavigationWaypoint, NavigationWaypoint>();

        // Add the start to the queue and mark it as explored
        exploredWaypoints.Add(start);
        waypointsQueue.Enqueue(start);

        bool pathFound = false;

        while (waypointsQueue.Count > 0 && pathFound == false)
        {
            // Remove current waypoint from queue
            NavigationWaypoint waypoint = waypointsQueue.Dequeue();

            // For each unexplored neighbour
            foreach (NavigationWaypoint connectedWaypoint in waypoint.connectedWaypoints)
            {
                if (exploredWaypoints.Contains(connectedWaypoint))
                    continue;

                // Set it to explored
                exploredWaypoints.Add(connectedWaypoint);

                // Check if its the goal
                if (connectedWaypoint == end)
                {
                    pathFound = true;
                    previousWaypoint.Add(end, waypoint);
                    break;
                }
                else // Else add it to the queue with it's origin waypoint
                {
                    waypointsQueue.Enqueue(connectedWaypoint);
                    previousWaypoint.Add(connectedWaypoint, waypoint);
                }
            }
        }

        // Retrace our steps from the end waypoint to the start then reverse and return the path
        if (pathFound)
        {
            NavigationPath path = new NavigationPath();

            path.Waypoints.Add(end);

            NavigationWaypoint waypoint = previousWaypoint[end];
            while (waypoint != start)
            {
                path.Waypoints.Add(waypoint);
                waypoint = previousWaypoint[waypoint];
            }

            path.Waypoints.Add(start);

            path.Waypoints.Reverse();

            return path;
        }

        return null;
    }

    public NavigationWaypoint GetWaypointByName(string id)
    {
        return allWaypoints.Find(waypoint => waypoint.id == id);
    }
}

public class NavigationPath
{
    public List<NavigationWaypoint> Waypoints = new List<NavigationWaypoint>();
}

public static class PathUtils
{
    public static Vector3 GetPointOnBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        Vector3 ab_bc = Vector3.Lerp(ab, bc, t);
        return ab_bc;
    }

    public static Vector3 GetDirectionOnBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 frontVector = GetPointOnBezierCurve(a, b, c, Mathf.Clamp01(t + 0.01f));
        Vector3 rearVector = GetPointOnBezierCurve(a, b, c, Mathf.Clamp01(t - 0.01f));

        return frontVector - rearVector;
    }

    public static BezierCurve3 GetBezierCurve(Vector3 a, Vector3 b, Vector3 c, int resolution)
    {
        List<Vector3> curve = new List<Vector3>();

        for (float t = 0; t <= 1f; t += 1f / (resolution - 1))
            curve.Add(GetPointOnBezierCurve(a, b, c, t));

        return new BezierCurve3(a, b, c, curve.ToArray());
    }

    public static List<Vector3> GetBezierCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int resolution)
    {
        List<Vector3> curve = new List<Vector3>();

        for (float t = 0; t <= 1f; t += 1f / (resolution - 1))
        {
            Vector3 ab = Vector3.Lerp(a, b, t);
            Vector3 bc = Vector3.Lerp(b, c, t);
            Vector3 cd = Vector3.Lerp(c, d, t);
            Vector3 ab_bc = Vector3.Lerp(ab, bc, t);
            Vector3 bc_cd = Vector3.Lerp(bc, cd, t);
            Vector3 abbc_bccd = Vector3.Lerp(ab_bc, bc_cd, t);
            curve.Add(abbc_bccd);
        }

        return curve;
    }

    /// <summary>
    /// A bezier curve represented by 3 Vector3 components.
    /// </summary>
    public class BezierCurve3
    {
        public readonly Vector3 Start, Mid, End;
        public readonly Vector3[] Curve;

        public int Resolution => Curve.Length;

        public Vector3 this[int index] => Curve[index];

        public BezierCurve3(Vector3 start, Vector3 mid, Vector3 end, Vector3[] curve)
        {
            Start = start;
            Mid = mid;
            End = end;
            this.Curve = curve;
        }

        public float Length()
        {
            float sum = 0;

            for (int i = 0; i < Curve.Length - 1; i++)
                sum += (Curve[i + 1] - Curve[i]).magnitude;

            return sum;
        }
        
        public float DirectLength()
        {
            return (End - Start).magnitude;
        }

        public Vector3 GetTangent(float t)
        {
            return GetDirectionOnBezierCurve(Start, Mid, End, t).normalized;
        }
    }
}