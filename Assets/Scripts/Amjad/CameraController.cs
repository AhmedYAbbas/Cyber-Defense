using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 1f;
    public float zoomSpeed = 1f;
    public float minZoomDistance = 5f;
    public float maxZoomDistance = 20f;
    public float rotateDelay = 1f; // Delay in seconds before allowing rotation after zooming
    public float minSwipeDistance = 1; // Minimum swipe distance required for rotation
    public Camera camera;

    private Vector3 previousMousePosition;
    private bool isDragging;
    private float rotateTimer;
    private bool shouldRotate;

    private void Update()
    {
        if (/*!EventSystem.current.IsPointerOverGameObject() &&*/ GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO() == null)
        {
            HandleCameraRotation();

            HandleCameraZoom();
        }
    }

    private void HandleCameraRotation()
    {
        if (rotateTimer > 0f)
        {
            rotateTimer -= Time.deltaTime;
            shouldRotate = false; // Disable rotation during the delay
        }
        else
        {
            shouldRotate = true; // Enable rotation after the delay
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && shouldRotate && !HasZoomInput())
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 deltaMousePosition = currentMousePosition - previousMousePosition;

            // Check if the delta position exceeds the minimum swipe distance for rotation
            if (deltaMousePosition.magnitude > minSwipeDistance)
            {
                // Rotate camera around the target
                transform.RotateAround(target.position, Vector3.up, deltaMousePosition.x * rotationSpeed);
                transform.RotateAround(target.position, transform.right, -deltaMousePosition.y * rotationSpeed);
            }
        }

        previousMousePosition = Input.mousePosition;
    }

    private void HandleCameraZoom()
    {
        float zoomInput = GetZoomInput();
        ZoomCamera(zoomInput);

        // Start the rotate timer when zooming occurs
        if (Mathf.Abs(zoomInput) > 0.001f)
        {
            rotateTimer = rotateDelay;
        }
    }

    private bool HasZoomInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            return Mathf.Abs(touchDeltaMag - prevTouchDeltaMag) > 0.001f;
        }
        else
        {
            return Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.001f;
        }
    }

    private float GetZoomInput()
    {
        float zoomInput = 0f;

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            zoomInput = prevTouchDeltaMag - touchDeltaMag;
        }
        else
        {
            zoomInput = -Input.GetAxis("Mouse ScrollWheel") * 100;
        }

        return zoomInput;
    }

    private void ZoomCamera(float zoomInput)
    {
        Vector3 cameraToTarget = target.position - transform.position;
        float currentDistance = cameraToTarget.magnitude;

        float newDistance = currentDistance + zoomInput * zoomSpeed;
        newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);

        Vector3 newCameraPosition = target.position - cameraToTarget.normalized * newDistance;
        transform.position = newCameraPosition;
    }
}
