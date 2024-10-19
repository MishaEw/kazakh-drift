using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public int speedLimit;

    public List<Transform> nextCheckpoints = new List<Transform> ();

    void Awake()
    {
        for(int i = 0; i < nextCheckpoints.Count; i++)
        {
            if(nextCheckpoints[i] == null)
            {
                nextCheckpoints.RemoveAt(i);
            }
        }
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        carcontroller controller = other.GetComponent<carcontroller>();

        if(controller && controller.nextCheckpoint.gameObject == transform.gameObject)
        {
            if(speedLimit != -1)
                controller.speedLimit = speedLimit;
            {
                int index = Random.Range(0, nextCheckpoints.Count);

                if (nextCheckpoints.Count > 0)
                {
                    controller.nextCheckpoint = nextCheckpoints[index];
                }
            }

        }
    }
}
