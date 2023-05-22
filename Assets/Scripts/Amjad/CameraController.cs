using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 1f;
    public float zoomSpeed = 1f;
    public float minZoomFOV = 20f;
    public float maxZoomFOV = 60f;

    private Vector3 previousMousePosition;
    private bool isDragging;

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO() == null)
        {
            // Handle camera rotation
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                isDragging = true;
                previousMousePosition = Input.mousePosition;
            }
            else if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
            {
                isDragging = false;
            }

            if (isDragging && !HasZoomInput())
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 deltaMousePosition = currentMousePosition - previousMousePosition;

                // Rotate camera around the target
                transform.RotateAround(target.position, Vector3.up, deltaMousePosition.x * rotationSpeed);
                //transform.RotateAround(target.position, transform.right, -deltaMousePosition.y * rotationSpeed);
            }

            // Store the current mouse position for the next frame
            previousMousePosition = Input.mousePosition;

            // Handle camera zoom
            float zoomInput = GetZoomInput();
            ZoomCamera(zoomInput);
        }
    }

    private bool HasZoomInput()
    {
        // Check for pinch gesture on mobile devices
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // Return true if there is zoom input on mobile devices
            return Mathf.Abs(touchDeltaMag - prevTouchDeltaMag) > 0.001f;
        }
        // Check for scroll wheel input on other platforms
        else
        {
            // Return true if there is zoom input from the scroll wheel
            return Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.001f;
        }
    }

    private float GetZoomInput()
    {
        float zoomInput = 0f;

        // Check for pinch gesture on mobile devices
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // Reverse the zoom input for mobile devices
            zoomInput = prevTouchDeltaMag - touchDeltaMag;
        }
        // Check for scroll wheel input on other platforms
        else
        {
            zoomInput = -Input.GetAxis("Mouse ScrollWheel") * 100;
        }

        return zoomInput;
    }

    private void ZoomCamera(float zoomInput)
    {
        // Adjust the camera's field of view for zooming
        float newFOV = Camera.main.fieldOfView + zoomInput * zoomSpeed;
        newFOV = Mathf.Clamp(newFOV, minZoomFOV, maxZoomFOV);
        Camera.main.fieldOfView = newFOV;

        // Calculate the distance between the camera and the target
        Vector3 cameraToTarget = target.position - transform.position;
        float currentDistance = cameraToTarget.magnitude;

        // Calculate the new distance based on the field of view change
        float zoomRatio = currentDistance / cameraToTarget.magnitude;
        float newDistance = currentDistance * (newFOV / Camera.main.fieldOfView);

        // Move the camera towards or away from the target based on the zoom input
        Vector3 newCameraPosition = target.position - transform.forward * newDistance * zoomRatio;
        transform.position = newCameraPosition;
    }
}
