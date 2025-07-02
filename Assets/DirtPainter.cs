using UnityEngine;

public class DirtPainter : MonoBehaviour
{
    [SerializeField] private RenderTexture dirtTexture;
    [SerializeField] private Material brushMaterial;
    [SerializeField] private Transform drill;
    [SerializeField] private float brushSize = 0.05f;

    private void Update()
    {
        if (dirtTexture == null || brushMaterial == null || drill == null)
            return;

        // Convert drill world position to local space of the dirt layer
        Vector3 localPos = transform.InverseTransformPoint(drill.position);
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
