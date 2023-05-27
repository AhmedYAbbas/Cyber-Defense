using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public Transform target;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 10f;
    public float panSpeed = 1f;
    public float zoomMin = 1f;
    public float zoomMax = 10f;

    private Camera mainCamera;

    private float currentZoom = 5f;
    private Vector3 lastMousePosition;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Zooming using scroll wheel or 2-finger pinch gesture
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (Input.touchSupported)
        {
            if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                Vector2 touchDeltaPosition0 = touch0.deltaPosition;
                Vector2 touchDeltaPosition1 = touch1.deltaPosition;
                float pinchDelta = (touchDeltaPosition0 - touchDeltaPosition1).magnitude;
                zoomInput = pinchDelta * 0.01f;
            }
        }

        currentZoom -= zoomInput * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);

        // Rotating around the target object
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float rotationAmount = mouseDelta.x * rotationSpeed * Time.deltaTime;
            transform.RotateAround(target.position, Vector3.up, rotationAmount);
        }

        // Panning the camera across its x and z axes
        if (Input.GetMouseButton(1))
        {
            float panInputX = Input.GetAxis("Mouse X");
            float panInputY = Input.GetAxis("Mouse Y");
            Vector3 pan = new Vector3(-panInputX, 0f, -panInputY) * panSpeed * Time.deltaTime;
            transform.Translate(pan, Space.Self);
        }

        // Update the camera's position and rotation
        float currentDistance = Vector3.Distance(transform.position, target.position);
        Vector3 dir = (transform.position - target.position).normalized;
        Vector3 newPos = target.position + dir * currentZoom;
        transform.position = newPos;
        transform.LookAt(target);

        lastMousePosition = Input.mousePosition;
    }
}
