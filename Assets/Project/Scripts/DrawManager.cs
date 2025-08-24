using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class DrawManager : MonoBehaviour
{
    [SerializeField] private Transform _finger;
    [SerializeField] private float _drawDistance = 0.02f; // глубина касания
    private GameObject _paintObject;

    private Renderer _renderer;
    public Draw Draw {get; private set;}
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
#if UNITY_EDITOR
        Draw = new DrawOnMesh(_renderer);
#else
        Draw = new DrawOnMeshByCustomRay(_renderer, _finger, _drawDistance);
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        DrawByMouse();
#else
        DrawByFinger();
#endif
    }

    private void DrawByFinger()
    {
        if (!_finger) return;
        Draw.TryDraw();
    }

    private void DrawByMouse()
    {
        if (Input.GetMouseButtonDown(0)) Draw.StartDraw(Input.mousePosition);
        if (Input.GetMouseButton(0)) Draw.ContinueDraw(Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) Draw.EndDraw();
    }
}
