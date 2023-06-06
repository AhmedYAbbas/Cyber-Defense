using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Variables")]
    [SerializeField] private List<GameObject> popups = new List<GameObject>();
    public int _currentIndex = 0;
    private static TutorialManager _instance;
    public static TutorialManager Instance => _instance;

    #region Camera Variabels
    [Space]
    [Header("Camera Variables")]
    public Transform target;

    private float rotationSpeed = 1f;
    private float zoomSpeed = 1f;
    private float minZoomDistance = 5f;
    private float maxZoomDistance = 20f;
    private float rotateDelay = 1f; // Delay in seconds before allowing rotation after zooming
    private float minSwipeDistance = 1; // Minimum swipe distance required for rotation
    private Camera camera;

    private Vector3 previousMousePosition;
    private bool isDragging;
    private float rotateTimer;
    private bool shouldRotate;
    private bool isZoomed = false;
    private bool isRotate = false;

    #endregion

    #region Spwaning Malwares Variabels

    [Space] [Header("Spwaning Malwares")] 
    [SerializeField] private List<GameObject> malware = new List<GameObject>();

    public int malwareIndex = int.MaxValue;
    public int currentMalwareCost;

    #endregion


    [Space] [Header("Energy Variables")] 
    [SerializeField] private int maxEnergy;
    private int _currentEnergy;


    private void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }
        ResettingTheGameVariables();
        SwitchThePopupsOnAndOff();
        camera = Camera.main;
    }

    private void Update()
    {
        switch (_currentIndex)
        {
            case 0: // for teaching camera zoom
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    HandleCameraZoom();
                    print("in case 0");
                    print(isZoomed);
                }
                break;
            case 1: // for teaching camera rotation
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    HandleCameraRotation();
                    HandleCameraZoom();
                    print("in case 1");
                }
                break;
            case 2: // for teaching camera panning
                _currentIndex = 4;
                print("in case 2");
                break;
            case 3: // for teaching the player about the two side
                _currentIndex = 4;
                print("in case 3");
                break;
            case 4: // for teaching Spawning the malwares
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    SpawnObject(Input.mousePosition);
                }
                else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject())
                {
                    SpawnObject(Input.GetTouch(0).position);
                }
                print("in case 4");
                break;
            case 5: // for teaching the player about the two side
                _currentIndex = 4;
                print("in case 5");
                break;
        }
        print(_currentIndex);
    }

    public void SwitchThePopupsOnAndOff()
    {
        for (int i = 0; i < popups.Count; i++)
        {
            if (i == _currentIndex)
            {
                popups[i].gameObject.SetActive(true);
            }
            else
            {
                popups[i].gameObject.SetActive(false);
            }
        }
    }

    #region camera functions

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
                //transform.RotateAround(target.position, transform.right, -deltaMousePosition.y * rotationSpeed);
                if (!isRotate)
                {
                    StartCoroutine(FinishTheCurrentTutorial());
                    isRotate = true;
                }
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
            if (isZoomed == false)
            {
                StartCoroutine(FinishTheCurrentTutorial());
                isZoomed = true;
            }

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

    #endregion

    #region spawning malwares functions

    void SpawnObject(Vector3 position)
    {
            
        Ray ray = camera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Road"))
            {
                Vector3 spawnPosition = hit.point;
                spawnPosition.y += .3f;
                print("hit road");
                if (malwareIndex < malware.Count &&currentMalwareCost <= _currentEnergy)
                {
                    Instantiate(malware[malwareIndex], spawnPosition, Quaternion.identity);
                    _currentEnergy -= currentMalwareCost;
                }
            }
        }
    }
    #endregion

    void ResettingTheGameVariables()
    {
        _currentEnergy = maxEnergy;
        malwareIndex = int.MaxValue;
    }

    IEnumerator FinishTheCurrentTutorial()
    {
        yield return new WaitForSeconds(2);
        _currentIndex++;
        SwitchThePopupsOnAndOff();
    }
}
