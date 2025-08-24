using UnityEngine;
public class TextureData
{
    public int Width;
    public int Height;
    public string Base64;
}
public static class JsonDraw
{
    private const string KEY = "draw"; 
    public static void Delete() => PlayerPrefs.DeleteKey(KEY);
    public static TextureData Load()
    {
        return JsonUtility.FromJson<TextureData>(PlayerPrefs.GetString(KEY));
    }
    public static void Save(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToJPG();

        // Создаём объект для сериализации
        TextureData data = new()
        {
            Width = texture.width,
            Height = texture.height,
            Base64 = System.Convert.ToBase64String(bytes)
        };

        PlayerPrefs.SetString(KEY, JsonUtility.ToJson(data));
    }
}
