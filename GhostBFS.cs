using System.Collections.Generic;
using UnityEngine;

public class GhostBFS : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform targetPlayer;      // PacMover root or head

    public MazePoint startNode;         // assign in Inspector if you want
    public MazePoint currentNode;
    MazePoint nextNode;

    void Start()
    {
        // Decide starting node
        if (startNode != null)
        {
            currentNode = startNode;
        }
        else
        {
            if (currentNode == null)
                currentNode = MazeClose.Instance.GetClosestNode(transform.position);

            startNode = currentNode;
        }

        transform.position = currentNode.transform.position;
        ChooseNextNode();
    }

    void Update()
    {
        if (nextNode == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            nextNode.transform.position,
            moveSpeed * Time.deltaTime
        );

        if ((transform.position - nextNode.transform.position).sqrMagnitude < 0.0001f)
        {
            currentNode = nextNode;
            ChooseNextNode();
        }
    }

    public void ResetGhost()
    {
        // back to assigned start node
        if (startNode == null)
            startNode = MazeClose.Instance.GetClosestNode(transform.position);

        currentNode = startNode;
        nextNode = null;

        transform.position = currentNode.transform.position;

        ChooseNextNode();
    }

    void ChooseNextNode()
    {
        if (currentNode == null)
        {
            nextNode = null;
            return;
        }

        if (targetPlayer == null)
        {
            nextNode = currentNode.neighbors.Count > 0 ? currentNode.neighbors[0] : null;
            return;
        }

        MazePoint playerNode = MazeClose.Instance.GetClosestNode(targetPlayer.position);
        if (playerNode == null || playerNode == currentNode)
        {
            nextNode = currentNode.neighbors.Count > 0 ? currentNode.neighbors[0] : null;
            return;
        }

        nextNode = GetNextNodeBFS(currentNode, playerNode);
    }

    MazePoint GetNextNodeBFS(MazePoint start, MazePoint goal)
    {
        var q = new Queue<MazePoint>();
        var visited = new HashSet<MazePoint>();
        var parent = new Dictionary<MazePoint, MazePoint>();

        visited.Add(start);
        q.Enqueue(start);

        while (q.Count > 0)
        {
            MazePoint node = q.Dequeue();
            if (node == goal)
                break;

            foreach (MazePoint n in node.neighbors)
            {
                if (n == null || visited.Contains(n)) continue;
                visited.Add(n);
                parent[n] = node;
                q.Enqueue(n);
            }
        }

        if (!visited.Contains(goal))
        {
            return start.neighbors.Count > 0 ? start.neighbors[0] : null;
        }

        MazePoint cur = goal;
        while (parent.ContainsKey(cur) && parent[cur] != start)
        {
            cur = parent[cur];
        }
        return cur;
    }
}
