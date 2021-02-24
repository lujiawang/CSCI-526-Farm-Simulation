using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    public float smoothFactor;
    public Vector3 offset;

    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottomLimit;
    [SerializeField]
    float topLimit;

    Vector3 desiredPosition;

    // Use this for initialization
    void Start()
    {
    //    desiredPosition = target.position;
    }

    // Update is called once per frame

    void Update()
    {
        if (TouchToMove.isPlayer)
        {
            Vector3 targetPosition = transform.position;
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(targetPosition, desiredPosition, smoothFactor * Time.deltaTime);
            transform.position = smoothedPosition;

            transform.position = new Vector3
                (
                    Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                    Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
                    transform.position.z

                );
        }
    }

    // called every fixed frame rate
    void FixedUpdate()
    {
        
        //   transform.LookAt(target);
    }
    // called after update and fixed update
    void LateUpdate()
    {
   
    }

    void onDrawGizmos()
    {
        // draw around the camera boundary
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, bottomLimit), new Vector2(leftLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(leftLimit, topLimit));
    }
}
