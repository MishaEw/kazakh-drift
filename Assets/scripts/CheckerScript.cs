using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerScript : MonoBehaviour
{
    public List<StopScript> stopScripts;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        carcontroller controller = c.GetComponent<carcontroller>();

        if (controller)
        {
            for (int i = 0; i < stopScripts.Count; i++)
                stopScripts[i].stop = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        carcontroller controller = c.GetComponent<carcontroller>();

        if (controller)
        {
            for (int i = 0; i < stopScripts.Count; i++)
                stopScripts[i].stop = false;
        }
    }
}
