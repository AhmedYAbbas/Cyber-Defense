using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    private static TouchInputManager instance;
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

    public static TouchInputManager Instance
    {
        get
        {
            // If the instance does not exist, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<TouchInputManager>();

                // If it still doesn't exist, create a new GameObject and add the TouchInputManager component to it
                if (instance == null)
                {
                    GameObject obj = new GameObject("TouchInputManager");
                    instance = obj.AddComponent<TouchInputManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        // Ensure that only one instance of TouchInputManager exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasTouchInput()
    {
        return Input.touchCount > 0;
    }

    public Vector3 GetTouchWorldPosition(int touchIndex = 0)
    {
        Vector3 touchWorldPosition = Vector3.zero;

        if (HasTouchInput())
        {
            Touch touch = Input.GetTouch(touchIndex);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,999f, mouseColliderLayerMask))
            {
                touchWorldPosition = hit.point;
            }
            else
            {
                touchWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
        }

        return touchWorldPosition;
    }
    public TouchPhase GetTouchPhase(int touchIndex = 0)
    {
        if (HasTouchInput())
        {
            Touch touch = Input.GetTouch(touchIndex);
            return touch.phase;
        }

        return TouchPhase.Canceled;
    }
    public bool CANBUILD(int touchIndex=0)
    {
        if (HasTouchInput())
        {
            Touch touch = Input.GetTouch(touchIndex);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            bool found = false;
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,999f, mouseColliderLayerMask))
            {
                print(hit.collider.tag);

                if (hit.transform.CompareTag("Buildable"))
                {
                    found = true;
                }
            }
            return found;
        }
        else
        {
            return false;
        }
    }
}
