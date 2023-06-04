using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Range : MonoBehaviour
{
    [SerializeField] private TowerModifications baseTowerSo;
    [Range(0, 50)]
    public int segments = 50;
    public float Radius ;
    LineRenderer line;
    private void Awake()
    {
        Radius = baseTowerSo.range;

    }
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * Radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * Radius;
           

            line.SetPosition(i, new Vector3(x, 1, y));

            angle += (360f / segments);
        }
    }
}