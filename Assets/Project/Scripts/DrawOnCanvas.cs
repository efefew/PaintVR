using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(RawImage))]
public class DrawOnCanvas : Draw
{
    private RawImage _raw;
    private void Awake()
    {
        _raw = GetComponent<RawImage>();
        _raw.texture = SetBackground();
    }
    protected override bool TryGetUV(Vector2 screenPos, out Vector2 uv)
    {
        PointerEventData ped = new(EventSystem.current) {position = screenPos};

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        foreach (RaycastResult r in results)
        {
            if (r.gameObject == gameObject)
            {
                RectTransform rt = (RectTransform)transform;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPos, _raycastCamera, out Vector2 localPoint))
                {
                    Rect rect = rt.rect;
                    float u = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
                    float v = Mathf.InverseLerp(rect.yMin, rect.yMax, localPoint.y);
                    uv = new Vector2(u, v);
                    return true;
                }
            }
        }

        uv = Vector2.zero;
        return false;
    }
}
