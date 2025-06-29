using UnityEngine;

public class DirtInitializer : MonoBehaviour
{
    public RenderTexture dirtTexture;
    public Color dirtColor = new Color(0.4f, 0.2f, 0.1f, 1f); // brown dirt

    private void Start()
    {
        // Create a tiny texture filled with the dirt color
        Texture2D fillTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        fillTexture.SetPixel(0, 0, dirtColor);
        fillTexture.Apply();

        // Blit it onto the RenderTexture
        Graphics.Blit(fillTexture, dirtTexture);

        // Destroy the texture asset to avoid memory leaks
        DestroyImmediate(fillTexture);
    }
}
