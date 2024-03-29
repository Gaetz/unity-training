using UnityEditor;
using UnityEngine;

enum DeadZoneStatus
{
    In, Out, CatchingUp
}

enum CameraStatus
{
    ThirdPerson, FirstPerson, Camera1
}


public class SpringArm : MonoBehaviour
{
    #region Rotation Settings

    [Space]
    [Header("Rotation Settings \n-----------------------")]
    [Space]
    [SerializeField] private bool useControlRotation = true;
    [SerializeField] private float mouseSensitivity = 500f;
    
    // For mouse inputs
    private float pitch;
    private float yaw;
    
    #endregion
    
    #region Follow Settings

    [Space]
    [Header("Follow Settings \n--------------------")]
    [Space]
    [SerializeField] private Transform target;
    [SerializeField] private float movementSmoothTime = 0.2f;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 1.8f, 0);
    
    [SerializeField] private float targetArmLength = 3f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0.5f, 0, -0.3f);

    // refs for SmoothDamping
    private Vector3 moveVelocity;
    
    private Vector3 endPoint;
    private Vector3 cameraPosition;
    
    [SerializeField] private float deadZoneSize = 2.0f;
    [SerializeField] private float targetZoneSize = 0.1f;
    private DeadZoneStatus deadZoneStatus = DeadZoneStatus.In;
    
    #endregion

    #region Collisions
    
    [Space]
    [Header("Collision Settings \n-----------------------")]
    [Space]
    
    [SerializeField] private bool doCollisionTest = true;
    [Range(2, 20)] [SerializeField] private int collisionTestResolution = 4;
    [SerializeField] private float collisionProbeSize = 0.3f;
    [SerializeField] private float collisionSmoothTime = 0.05f;
    [SerializeField] private LayerMask collisionLayerMask = ~0;
    
    private RaycastHit[] hits;
    private Vector3[] raycastPositions;

    #endregion

    #region Debug
    
    [Space]
    [Header("Debugging \n--------------")]
    [Space]
    
    [SerializeField] private bool visualDebugging = true;
    [SerializeField] private Color springArmColor = new Color(0.75f, 0.2f, 0.2f, 0.75f);
    [Range(1f, 10f)] [SerializeField] private float springArmLineWidth = 6f;
    [SerializeField] private bool showRaycasts;
    [SerializeField] private bool showCollisionProbe;
    
    private readonly Color collisionProbeColor = new Color(0.2f, 0.75f, 0.2f, 0.15f);

    #endregion
    
    #region Camera Transition

    [Space]
    [Header("Camera Transition \n--------------")]
    [Space]
    
    [SerializeField] private Transform camera1;
    private CameraStatus cameraStatus = CameraStatus.ThirdPerson;
    
    #endregion
    
    
    // Start is called before the first frame update
    void Start()
    {
        raycastPositions = new Vector3[collisionTestResolution];
        hits = new RaycastHit[collisionTestResolution];
    }
    
    private void OnValidate()
    {
        raycastPositions = new Vector3[collisionTestResolution];
        hits = new RaycastHit[collisionTestResolution];
    }

    void Update()
    {
        // If target is null, return from here: NullReference check
        if(!target)
            return;

        Vector3 targetPosition = Vector3.zero;

        if (Input.GetKey(KeyCode.Alpha1))
        {
            cameraStatus = CameraStatus.ThirdPerson;
        } 
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            cameraStatus = CameraStatus.Camera1;
        }

        switch (cameraStatus)
        {
            case CameraStatus.Camera1:
                targetPosition = UpdateCamera1();
                break;
            
            case CameraStatus.ThirdPerson:
                targetPosition = UpdateThirdPerson();
                break;
        }
        
        // Follow the target applying targetOffset
        transform.position = Vector3.SmoothDamp(transform.position, 
            targetPosition, ref moveVelocity, movementSmoothTime);
    }

    Vector3 UpdateThirdPerson()
    {
        Vector3 targetPosition = Vector3.zero;

        // Collision check
        if (doCollisionTest) 
            CheckCollisions();
        SetCameraTransform();

        // Handle mouse inputs for rotations
        if (useControlRotation && Application.isPlaying)
            Rotate();

            
        float distanceToTarget = Vector3.Distance(transform.position, target.position + targetOffset);
        if (distanceToTarget > deadZoneSize)
        {
            deadZoneStatus = DeadZoneStatus.Out;
            targetPosition = target.position + targetOffset;
        }
        else
        {
            switch (deadZoneStatus)
            {
                case DeadZoneStatus.In:
                    targetPosition = transform.position;
                    break;
                case DeadZoneStatus.Out:
                    targetPosition = target.position + targetOffset;
                    deadZoneStatus = DeadZoneStatus.CatchingUp;
                    break;
                case DeadZoneStatus.CatchingUp:
                    targetPosition = target.position + targetOffset;
                    if (distanceToTarget <= targetZoneSize)
                    {
                        deadZoneStatus = DeadZoneStatus.In;
                    }
                    break;
            }
        }

        return targetPosition;
    }
    
    Vector3 UpdateCamera1()
    {
        transform.LookAt(target);
        return camera1.position;
    }
    
    /// <summary>
    /// Handle rotations
    /// </summary>
    private void Rotate()
    {
        // Increment yaw by Mouse X input
        yaw += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        // Decrement pitch by Mouse Y input
        pitch -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // Clamp pitch so that we can't invert the the gameobject by mistake
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        
        // Set the rotation to new rotation
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void SetCameraTransform()
    {
        // Cache transform as it is used quite often
        Transform trans = transform;

        // Offset a point in z direction of targetArmLength by camera offset and translating it into world space.
        Vector3 targetArmOffset = cameraOffset - new Vector3(0, 0, targetArmLength);
        endPoint = trans.position + (trans.rotation * targetArmOffset);
    
        // If collisionTest is enabled
        if (doCollisionTest)
        {
            // Finds the minDistance
            float minDistance = targetArmLength;
            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider)
                    continue;
            
                float distance = Vector3.Distance(hit.point, trans.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                }
            }
        
            // Calculate the direction of children movement
            Vector3 dir = (endPoint - trans.position).normalized;
            // Get vector for movement
            Vector3 armOffset = dir * (targetArmLength - minDistance);
            // Offset it by endPoint and set the cameraPositionValue
            cameraPosition = endPoint - armOffset;
        }
        // If collision is disabled
        else
        {
            // Set cameraPosition value as endPoint
            cameraPosition = endPoint;
        }

        // iterate through all children and set their position as cameraPosition, using SmoothDamp to smoothly translate the vectors.
        Vector3 cameraVelocity = Vector3.zero;
        foreach (Transform child in trans)
        {
            child.position = Vector3.SmoothDamp(child.position, 
                cameraPosition, ref cameraVelocity, collisionSmoothTime);
        }
    }
    
    /// <summary>
    /// Checks for collisions and fill the raycastPositions and hits array
    /// </summary>
    private void CheckCollisions()
    {
        // Cache transform as it is used quite often
        Transform trans = transform;
        
        // iterate through raycastPositions and hits and set the corresponding data
        for (int i = 0, angle = 0; i < collisionTestResolution; i++, angle += 360 / collisionTestResolution)
        {
            // Calculate the local position of a point w.r.t angle
            Vector3 raycastLocalEndPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * collisionProbeSize;
            // Convert it to world space by offsetting it by origin: endPoint, and push in the array
            raycastPositions[i] = endPoint + (trans.rotation * raycastLocalEndPoint);
            // Sets the hit struct if collision is detected between this gameobject's position and calculated raycastPosition
            Physics.Linecast(trans.position, raycastPositions[i], out hits[i], collisionLayerMask);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if(!visualDebugging)
            return;

        // Draw main LineTrace or LineTraces of RaycastPositions, useful for debugging
        Handles.color = springArmColor;
        if(showRaycasts)
            foreach (Vector3 raycastPosition in raycastPositions)
                Handles.DrawAAPolyLine(springArmLineWidth, 2, transform.position, raycastPosition);
        else
            Handles.DrawAAPolyLine(springArmLineWidth, 2, transform.position, endPoint);
        
        // Draw collisionProbe, useful for debugging
        Handles.color = collisionProbeColor;
        if(showCollisionProbe)
            Handles.SphereHandleCap(0, cameraPosition, Quaternion.identity, 2 * collisionProbeSize, EventType.Repaint);
    }
}
