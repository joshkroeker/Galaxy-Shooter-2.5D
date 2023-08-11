using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingMissile : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private List<Transform> path;
    private int _pathIndex = 1;

    private void Start()
    {
        path = new List<Transform>();
    }

    private void Update()
    {
        if(path.Count == 0 && path != null)
        {
            MoveAlongPath();
        }
    }

    public void ReceivePathPoints(Transform[] pathToReceive)
    {
        foreach(Transform pathPoint in pathToReceive)
        {
            path.Add(pathPoint);
        }
    }

    private void MoveAlongPath()
    {
        while (DistanceCheck(0.5f))
        {
            transform.position = Vector2.MoveTowards(transform.position, path[_pathIndex].position, _speed * Time.deltaTime);

            if (transform.position == path[_pathIndex].position)
            {
                if (_pathIndex > path.Count)
                    break;

                if (_pathIndex < path.Count)
                {
                    _pathIndex++;
                }
            }
        }
    }

    private bool DistanceCheck(float distanceCheckAgainst)
    {
        return Vector2.Distance(transform.position, path[_pathIndex].position) > distanceCheckAgainst;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            player.Damage();
        }
    }
}
