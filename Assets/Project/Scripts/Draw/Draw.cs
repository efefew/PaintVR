using UnityEngine;

public abstract class Draw
{
    private const int MIN_BRUSH_SIZE = 1;
    private const int MAX_BRUSH_SIZE = 128;
    public int BrushSize
    {
        get => _brushSize;
        private set => _brushSize = Mathf.Clamp(value, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);
    }
    public Color Background
    {
        get => _background;
        set
        {
            _background = value;
            SetBackground();
        }
    }
    public Color BrushColor
    {
        get => _brushColor;
        set => _brushColor = value;
    }

    protected Camera _raycastCamera = Camera.main;
    private Texture2D _tex;
    private bool _drawing;
    private Vector2 _prevUV;
    private Color32[] _bgCache;
    
    private int _textureWidth;
    private int _textureHeight;
    
    private Color _background = Color.white;
    private Color _brushColor = Color.black;
    private int _brushSize = 16;

    protected Draw(int textureWidth, int textureHeight)
    {
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        _tex.SetPixels32(_bgCache);
        _tex.Apply();
    }
    public Texture2D GetTexture() => _tex;
    public void SetTexture(TextureData data)
    {
        if (data == null || data.Width != _textureWidth || data.Height != _textureHeight)
        {
            return;
        }
        byte[] bytes = System.Convert.FromBase64String(data.Base64);
        _tex.LoadImage(bytes);
    }

    protected Texture2D SetBackground()
    {
        if (_tex == null)
        {
            _tex = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp
            };
        }
        

        _bgCache = new Color32[_textureWidth * _textureHeight];
        for (int i = 0; i < _bgCache.Length; i++) _bgCache[i] = Background;

        _tex.SetPixels32(_bgCache);
        _tex.Apply();
        return _tex;
    }
    protected abstract bool TryGetUV(Vector2 screenPos, out Vector2 uv);
    public void TryDraw()
    {
        ContinueDraw(Vector2.zero);
    }
    public void StartDraw(Vector2 screenPos)
    {
        if (!TryGetUV(screenPos, out Vector2 uv)) return;
        

        _drawing = true;
        _prevUV = uv;
        DrawDotUV(uv);
        _tex.Apply();
    }
    public void ContinueDraw(Vector2 screenPos)
    {
        if (!_drawing)
        {
            StartDraw(screenPos);
            return;
        }

        if (!TryGetUV(screenPos, out Vector2 uv)) return;
        
        DrawLineUV(_prevUV, uv);
        _prevUV = uv;
        _tex.Apply();
    }
    public void EndDraw() => _drawing = false;
    
    private void DrawDotUV(Vector2 uv) => DrawDotPx(UVtoPx(uv));
    private void DrawDotPx(Vector2Int px) => DrawCircle(px, BrushSize, BrushColor);
    private void DrawLineUV(Vector2 a, Vector2 b) => DrawLinePx(UVtoPx(a), UVtoPx(b));
    private void DrawLinePx(Vector2Int p0, Vector2Int p1)
    {
        int steps = Mathf.CeilToInt(Vector2.Distance(p0, p1));
        for (int i = 0; i <= steps; i++)
        {
            float t = steps == 0 ? 0 : (float)i / steps;
            Vector2Int p = Vector2Int.RoundToInt(Vector2.Lerp(p0, p1, t));
            DrawCircle(p, BrushSize, BrushColor);
        }
    }
    private Vector2Int UVtoPx(Vector2 uv) => new (
        Mathf.Clamp(Mathf.RoundToInt(uv.x * (_tex.width - 1)), 0, _tex.width - 1),
        Mathf.Clamp(Mathf.RoundToInt(uv.y * (_tex.height - 1)), 0, _tex.height - 1)
    );
    private void DrawCircle(Vector2Int center, int radius, Color color)
    {
        int r2 = radius * radius;
        int x0 = Mathf.Max(center.x - radius, 0);
        int x1 = Mathf.Min(center.x + radius, _tex.width - 1);
        int y0 = Mathf.Max(center.y - radius, 0);
        int y1 = Mathf.Min(center.y + radius, _tex.height - 1);

        for (int y = y0; y <= y1; y++)
        {
            int dy = y - center.y;
            for (int x = x0; x <= x1; x++)
            {
                int dx = x - center.x;
                if (dx * dx + dy * dy <= r2)
                    _tex.SetPixel(x, y, color);
            }
        }
    }
}
