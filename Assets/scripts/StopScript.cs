using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopScript : MonoBehaviour
{

    public bool stop = true;
    public int priority = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        priority = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        carcontroller carController = other.gameObject.GetComponent<carcontroller>();

        if (carController != null)
        {
            priority++;
            if (stop && !carController.objectDetected)
            {
                carController.SetSpeed(0);
                carController.CheckPointSearch = false;
            }

            if (!stop && !carController.objectDetected)
            {
                carController.SetSpeed(carController.speedLimit);
                carController.CheckPointSearch = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        carcontroller carController = other.gameObject.GetComponent<carcontroller>();

        if (carController != null)
        {
            carController.SetSpeed(carController.speedLimit);
            carController.CheckPointSearch = true;
        }
    }
}
