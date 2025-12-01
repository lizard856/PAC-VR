using UnityEngine;

public class PathFindingBasic : MonoBehaviour
{
    [SerializeField] Transform[] path;
    [SerializeField] private float moveSpeed = 3f;
    private int pathIndex;

    void Start()
    {
        if (path.Length > 0)
        {
            pathIndex = 0;
            transform.position = path[0].position;
        }
    }

    void Update()
    {
        if (path.Length == 0) return;

        Transform target = path[pathIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if ((transform.position - target.position).sqrMagnitude < 0.0001f)
        {
            pathIndex++;
            if (pathIndex >= path.Length)
                pathIndex = 0;
        }
    }

    // called from PacGameManager.ResetLevel
    public void ResetPath()
    {
        if (path.Length == 0) return;

        pathIndex = 0;
        transform.position = path[0].position;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PathFindingBasic : MonoBehaviour
// {
//     [SerializeField] private Transform[] path;
//     [SerializeField] private float moveSpeed = 3f;

//     private int pathIndex = 0;

//     void Start()
//     {
//         if (path.Length > 0)
//         {
//             transform.position = path[0].position;
//         }
//     }

//     void Update()
//     {
//         if (path.Length == 0) return;

//         Transform target = path[pathIndex];

//         // 1: use Vector3, not Vector2
//         transform.position = Vector3.MoveTowards(
//             transform.position,
//             target.position,
//             moveSpeed * Time.deltaTime
//         );

//         // 2: use a small distance threshold, not ==
//         if ((transform.position - target.position).sqrMagnitude < 0.0001f)
//         {
//             pathIndex++;

//             if (pathIndex >= path.Length)
//             {
//                 pathIndex = 0;      // loop
//             }
//         }
//     }
// }
