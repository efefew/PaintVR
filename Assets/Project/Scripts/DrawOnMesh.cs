using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Renderer))]
public class DrawOnMesh : Draw
{
    private Renderer _rend;
    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _rend.material.mainTexture = SetBackground();
    }
    protected override bool TryGetUV(Vector2 screenPos, out Vector2 uv)
    {
        Ray ray = _raycastCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                uv = hit.textureCoord;
                return true;
            }
        }
        uv = Vector2.zero;
        return false;
    }
}
