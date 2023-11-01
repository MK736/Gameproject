using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationController : MonoBehaviour
{
    // èâä˙à íu
    private Vector3 startPosition;
    // ñ⁄ìIín
    [SerializeField] private Vector3 destination;
    [SerializeField] private Transform[] targets;

    public enum Route { random}
    public Route route;

    void Start()
    {
        startPosition = transform.position;
        SetDestination(transform.position);
    }

    public void CreateDetination()
    {
            CreateRandomDestination();
    }

    private void CreateRandomDestination()
    {
        int num = Random.Range(0, targets.Length);
        SetDestination(new Vector3(targets[num].transform.position.x, 0, targets[num].transform.position.z));
    }

    public void SetDestination(Vector3 position)
    {
        destination = position;
    }
    public Vector3 GetDestination()
    {
        return destination;
    }
}
