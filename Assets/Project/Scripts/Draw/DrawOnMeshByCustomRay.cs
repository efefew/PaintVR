using UnityEngine;

public class DrawOnMeshByCustomRay : DrawOnMesh
{
    private Transform  _raySource;
    private float _rayDistance;
    public DrawOnMeshByCustomRay(Renderer renderer, Transform  raySource, float rayDistance, int textureWidth = 1024, int textureHeight = 1024) : base(renderer, textureWidth, textureHeight)
    {
        _raySource = raySource;
        _rayDistance = rayDistance;
    }
    protected override bool TryGetUV(Vector2 screenPos, out Vector2 uv)
    {
        Ray ray = new(_raySource.position, _raySource.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance))
        {
            if (hit.collider.gameObject == _drawPlace)
            {
                uv = hit.textureCoord;
                return true; 
            }
        }
        else
        {
            EndDraw();
        }
        
        uv = Vector2.zero;
        return false;
    }
}
