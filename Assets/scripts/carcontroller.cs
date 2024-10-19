using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



public class carcontroller : MonoBehaviour
{
    [Header("Wheels")]
    public Transform frontRight;
    public Transform frontLeft;
    public Transform rearRight;
    public Transform rearLeft;

    public WheelCollider frontRightCollider;
    public WheelCollider frontLeftCollider;
    public WheelCollider rearRightCollider;
    public WheelCollider rearLeftCollider;

    [Header("Checkpoints And Detections")]
    public Transform nextCheckpoint;
    public List<Transform> checks = new List<Transform> { null };
    public bool CheckPointSearch = true;
    public bool objectDetected = false;
    public bool isCarControlledByAI = true;
    public LayerMask seenLayers = Physics.AllLayers;

    [Header("Car Settings")]

    public int kmh;
    public int speedLimit;
    public float distanceFromObjects = 2f;
    public int recklessnessThreshold = 0;
    public bool despawnForFlippingOver = false;
    public bool taxiMode = false;

    public float acceleration = 100f;
    public float breaking = 1000f;

    private Stopwatch stopwatch = new Stopwatch();
    private Vector3 lastPos;
    private float steerAngle = 0f;
    private bool flipOverCheck = false;


    private void FixedUpdate()
    {
        WheelUpdate(frontRight, frontRightCollider);
        WheelUpdate(frontLeft, frontLeftCollider);
        WheelUpdate(rearRight, rearRightCollider);
        WheelUpdate(rearLeft, rearLeftCollider);

        CalculateKMH();

        SearchForCheckpoints();
       

        if (despawnForFlippingOver && !flipOverCheck)
        {
            flipOverCheck = true;
            StartCoroutine(CheckForFlippingOver());
        }

    }

    IEnumerator CheckForFlippingOver()
    {
        bool deleteCar = isCarFlipedOver();

        if (deleteCar)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!isCarFlipedOver())
                {
                    deleteCar = false;
                }
                yield return new WaitForSeconds(1);
            }

            if (deleteCar)
            {
                UnityEngine.Debug.Log("Car " + gameObject.name + " destroyed for flipping over.");
                Destroy(gameObject);
            }
        }

        yield return new WaitForSeconds(10);

        flipOverCheck = false;

        yield return null;
    }

    private bool isCarFlipedOver()
    {

        if (transform.rotation.eulerAngles.z > 30f || transform.rotation.eulerAngles.z < -30f)
        {
            return true;
        }

        return false;
    }
    private void WheelUpdate(Transform transform, WheelCollider collider)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    public void Accelerate(float value)
    {
        frontRightCollider.motorTorque = value;
        frontLeftCollider.motorTorque = value;
    }

    public void Break(float value)
    {
        frontRightCollider.brakeTorque = value;
        frontLeftCollider.brakeTorque = value;
        rearRightCollider.brakeTorque = value;
        rearLeftCollider.brakeTorque = value;
    }

    public void Turn(float angle)
    {
        frontRightCollider.steerAngle = angle;
        frontLeftCollider.steerAngle = angle;
    }

    private void CalculateKMH()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();

            float distance = (transform.position - lastPos).magnitude;
            float time = stopwatch.Elapsed.Milliseconds / (float)1000;

            kmh = (int)(3600 * distance / time / 1500);

            lastPos = transform.position;
            stopwatch.Reset();
            stopwatch.Start();

        }
        else
        {
            lastPos = transform.position;
            stopwatch.Reset();
            stopwatch.Start();
        }
    }

    public void SetSpeed(int speedLimit)
    {
        if (kmh > speedLimit)
        {
            Break(breaking);
            Accelerate(0);
        }
        else if (kmh < speedLimit)
        {
            Accelerate(acceleration);
            Break(0);
        }
    }

    private void SearchForCheckpoints()
    {
        if (CheckPointSearch && isCarControlledByAI)
        {
            Vector3 nextCheckpointRelative = transform.InverseTransformPoint(nextCheckpoint.position);

            steerAngle = nextCheckpointRelative.x / nextCheckpointRelative.magnitude;
            float xangle = nextCheckpointRelative.y / nextCheckpointRelative.magnitude;

            steerAngle = Mathf.Asin(steerAngle) * 180f / 3.14f;
            xangle = Mathf.Asin(xangle) * 180f / 3.14f;

            Turn(steerAngle);

            float maxDistance = kmh * kmh / 100f + distanceFromObjects;

            RaycastHit carHit = new RaycastHit();

            int objectInFront = 0;

            for (int i = 0; i < checks.Count; i++)
            {
                checks[i].localRotation = Quaternion.Euler(-xangle, steerAngle, 0);
                bool isObjectInFront = Physics.Raycast(checks[i].position, checks[i].forward, out carHit, maxDistance, seenLayers, QueryTriggerInteraction.Ignore);

#if UNITY_EDITOR
             UnityEngine.Debug.DrawRay(checks[i].position, checks[i].forward * maxDistance, Color.green);
#endif

                if (isObjectInFront == true)
                    objectInFront++;
            }

            if (objectInFront > 0)
            {
                SetSpeed(0);
                objectDetected = true;
            }
            else
            {
                objectDetected = false;
                int speed = speedLimit + recklessnessThreshold;
                if (speedLimit == 0)
                {
                    speed = 0;
                }
                if (speed == 0)
                {
                    speed = speedLimit;
                }
                SetSpeed(speed);
            }
        }
    }
 
}

