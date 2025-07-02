using UnityEngine;

public class DirtPainter : MonoBehaviour
{
    [SerializeField] private RenderTexture dirtTexture;
    [SerializeField] private Material brushMaterial;
    [SerializeField] private Transform drill;
    [SerializeField] private bool useDrillReference = false;
    [SerializeField] private float brushSize = 0.05f;

    private void Update()
    {
        if (useDrillReference && drill != null)
        {
            Paint(drill.position);
        }
    }

    /// <summary>
    /// Paints the dirt texture at the given world position.
    /// </summary>
    /// <param name="worldPosition">World position of the drill/brush.</param>
    public void Paint(Vector3 worldPosition)
    {
        if (dirtTexture == null || brushMaterial == null)
            return;

        // Convert world position to the local space of the dirt layer
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);
        Vector3 scale = transform.lossyScale;
        float u = (localPos.x / scale.x) + 0.5f;
        float v = (localPos.y / scale.y) + 0.5f;
        Vector2 brushPos = new Vector2(u, v);

        brushMaterial.SetVector("_BrushPos", new Vector4(brushPos.x, brushPos.y, 0f, 0f));
        brushMaterial.SetFloat("_BrushSize", brushSize);

        var desc = dirtTexture.descriptor;
        RenderTexture temp = RenderTexture.GetTemporary(desc);
        Graphics.Blit(dirtTexture, temp);
        Graphics.Blit(temp, dirtTexture, brushMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }
}
