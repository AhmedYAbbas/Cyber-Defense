using UnityEngine;

public class SCameraController : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSpeed = 1f;
    public float zoomSpeed = 0.1f;
    public float doubleClickTimeThreshold = 0.3f;

    private Camera mainCamera;
    private bool isTouchDevice;
    private bool isDoubleClicking = false;
    private float doubleClickTimer = 0f;
    private Vector2 lastClickPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        isTouchDevice = Application.isMobilePlatform;
    }

    private void Update()
    {
        if (!isTouchDevice)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (isDoubleClicking)
            {
                // Move the camera in the swipe direction after double-click
                Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
                mainCamera.transform.Translate(-touchDelta.x * rotationSpeed * Time.deltaTime, 0f, -touchDelta.y * rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Rotate the camera around the center point
                Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
                mainCamera.transform.RotateAround(centerPoint.position, Vector3.up, touchDelta.x * rotationSpeed * Time.deltaTime);
            }
        }
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // Detect double-click by measuring time and position difference
            if (!isDoubleClicking && Time.time - doubleClickTimer <= doubleClickTimeThreshold &&
                Vector2.Distance(Input.GetTouch(0).position, lastClickPosition) <= 20f)
            {
                isDoubleClicking = true;
            }
            else
            {
                isDoubleClicking = false;
            }

            doubleClickTimer = Time.time;
            lastClickPosition = Input.GetTouch(0).position;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (isDoubleClicking)
            {
                // Move the camera in the swipe direction after double-click
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                mainCamera.transform.Translate(-mouseX * rotationSpeed * Time.deltaTime, 0f, -mouseY * rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Rotate the camera around the center point
                float mouseX = Input.GetAxis("Mouse X");
                mainCamera.transform.RotateAround(centerPoint.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Detect double-click by measuring time and position difference
            if (!isDoubleClicking && Time.time - doubleClickTimer <= doubleClickTimeThreshold &&
                Vector2.Distance(Input.mousePosition, lastClickPosition) <= 20f)
            {
                isDoubleClicking = true;
            }
            else
            {
                isDoubleClicking = false;
            }

            doubleClickTimer = Time.time;
            lastClickPosition = Input.mousePosition;
        }
    }
}
