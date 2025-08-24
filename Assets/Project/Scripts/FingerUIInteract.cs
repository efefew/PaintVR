using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventSystem;
using static UnityEngine.EventSystems.ExecuteEvents;

public class FingerUIInteract : MonoBehaviour
{
    /// <summary>
    /// Максимальное расстояние до UI
    /// </summary>
    [SerializeField] private float _maxDistance = 0.02f; 
    private Camera _camera;
    /// <summary>
    /// Кончик пальца
    /// </summary>
    private Transform _finger;

    private GameObject _lastTarget;

    private void Start()
    {
        _camera = Camera.main;
        _finger = transform;
    }
    private void Update()
    {
        TryClickByFinger();
    }

    private void TryClickByFinger()
    {
        if (!_finger || !_camera) return;

        // Точка пальца на экране
        Vector3 screenPos = _camera.WorldToScreenPoint(_finger.position);

        // Проверяем, есть ли под пальцем UI-объект
        PointerEventData ped = new(current)
        {
            position = screenPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        current.RaycastAll(ped, results);

        Click(results, ped);
    }

    private void Click(List<RaycastResult> results, PointerEventData ped)
    {
        if (results.Count > 0)
        {
            RaycastResult hit = results[0]; // ближайший UI элемент
            if (Vector3.Distance(_finger.position, hit.worldPosition) < _maxDistance)
            {
                if (_lastTarget != hit.gameObject)
                {
                    _lastTarget = hit.gameObject;

                    // "Нажатие" на UI-объект
                    Execute(hit.gameObject, ped, pointerEnterHandler);
                    Execute(hit.gameObject, ped, pointerDownHandler);
                    Execute(hit.gameObject, ped, pointerClickHandler);
                }
            }
        }
        else
        {
            if (_lastTarget)
            {
                // отпускание
                Execute(_lastTarget, new PointerEventData(current), pointerUpHandler);
                _lastTarget = null;
            }
        }
    }
}