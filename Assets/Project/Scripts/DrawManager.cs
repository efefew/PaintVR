using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class DrawManager : MonoBehaviour
{
    [SerializeField] private Transform _finger;
    [SerializeField] private float _drawDistance = 0.02f; // глубина касания
    [SerializeField] private bool _testMouse;
    private GameObject _paintObject;

    private Renderer _renderer;
    private DrawOnMeshByCustomRay _draw;
    private DrawOnMesh _drawByMouse;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _draw = new DrawOnMeshByCustomRay(_renderer, _finger, _drawDistance);
        _drawByMouse = new DrawOnMesh(_renderer);
    }

    private void Update()
    {
        DrawByFinger();
#if UNITY_EDITOR
        DrawByMouse();
#endif
    }

    private void DrawByFinger()
    {
        if (!_finger) return;
        _draw.TryDraw();
    }

    private void DrawByMouse()
    {
        if(!_testMouse) return;
        if (Input.GetMouseButtonDown(0)) _drawByMouse.StartDraw(Input.mousePosition);
        if (Input.GetMouseButton(0)) _drawByMouse.ContinueDraw(Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) _drawByMouse.EndDraw();
    }
}
