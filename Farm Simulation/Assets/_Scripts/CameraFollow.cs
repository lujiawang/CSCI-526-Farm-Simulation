using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    public float smoothFactor;
    public Vector3 offset;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    Vector3 desiredPosition;

    // Use this for initialization
    void Start()
    {
        desiredPosition = target.position;
    }

    // Update is called once per frame

    void Update()
    {

    }

    // called every fixed frame rate
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothedPosition;

        //   transform.LookAt(target);
    }
    // called after update and fixed update
    void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);
        }
    }
}

