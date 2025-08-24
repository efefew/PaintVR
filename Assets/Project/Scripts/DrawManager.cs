using UnityEngine;

/// <summary>
/// Менеджер рисования на объекте с помощью мыши в редакторе или пальца на устройстве.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class DrawManager : MonoBehaviour
{
    /// <summary>
    /// Трансформ кончика пальца для взаимодействия с объектом на устройстве.
    /// </summary>
    [SerializeField] private Transform _finger;

    /// <summary>
    /// Максимальная глубина касания при рисовании пальцем.
    /// </summary>
    [SerializeField] private float _drawDistance = 0.02f;

    private GameObject _paintObject;

    /// <summary>
    /// Рендерер объекта, на котором происходит рисование.
    /// </summary>
    private Renderer _renderer;

    /// <summary>
    /// Экземпляр класса Draw, используемый для работы с текстурой.
    /// </summary>
    public Draw Draw { get; private set; }

    /// <summary>
    /// Инициализация рендера и экземпляра Draw в зависимости от платформы.
    /// </summary>
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
#if UNITY_EDITOR
        Draw = new DrawOnMesh(_renderer, 512, 512);
#else
        Draw = new DrawOnMeshByCustomRay(_renderer, _finger, _drawDistance, 512, 512);
#endif
    }

    /// <summary>
    /// Очистка текстуры при уничтожении объекта.
    /// </summary>
    private void OnDestroy()
    {
        DestroyImmediate(Draw.GetTexture());
    }

    /// <summary>
    /// Обновление состояния рисования каждый кадр.
    /// В редакторе — рисование мышью, на устройстве — пальцем.
    /// </summary>
    private void Update()
    {
#if UNITY_EDITOR
        DrawByMouse();
#else
        DrawByFinger();
#endif
    }

    /// <summary>
    /// Выполняет рисование пальцем, если трансформ пальца задан.
    /// </summary>
    private void DrawByFinger()
    {
        if (!_finger) return;
        Draw.TryDraw();
    }

    /// <summary>
    /// Выполняет рисование с помощью мыши в редакторе.
    /// </summary>
    private void DrawByMouse()
    {
        if (Input.GetMouseButtonDown(0)) Draw.StartDraw(Input.mousePosition);
        if (Input.GetMouseButton(0)) Draw.ContinueDraw(Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) Draw.EndDraw();
    }
}
