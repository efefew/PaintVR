using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Color _red = Color.red, _blue = Color.blue;
    [SerializeField] private Toggle _changeColorToggle;
    [SerializeField] private Button _saveButton, _loadButton, _clearButton;
    [SerializeField] private DrawManager _drawManager;
    private void Start()
    {
        _drawManager.Draw.BrushColor = _red;
        _changeColorToggle.onValueChanged.AddListener(SwitchColor);
        _saveButton.onClick.AddListener(Save);
        _loadButton.onClick.AddListener(Load);
        _clearButton.onClick.AddListener(_drawManager.Draw.Clear);
    }
    private void OnDestroy()
    {
        _changeColorToggle.onValueChanged.RemoveListener(SwitchColor);
        _saveButton.onClick.RemoveListener(Save);
        _loadButton.onClick.RemoveListener(Load);
        _clearButton.onClick.RemoveListener(_drawManager.Draw.Clear);
    }
    private void Save() => JsonDraw.Save(_drawManager.Draw.GetTexture());
    private void Load() => _drawManager.Draw.SetTexture(JsonDraw.Load());
    private void SwitchColor(bool on) => _drawManager.Draw.BrushColor = on ? _red : _blue;
}
