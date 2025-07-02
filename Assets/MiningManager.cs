using UnityEngine;
using System;

/// <summary>
/// Handles the mining level logic. Responsible for enabling the dirt painter,
/// tracking objectives and clearing the dirt texture when required.
/// </summary>
public class MiningManager : MonoBehaviour
{
    public static MiningManager Instance { get; private set; }

    [Header("Dirt Settings")]
    [SerializeField] private DirtPainter dirtPainter;
    [SerializeField] private RenderTexture dirtTexture;
    [SerializeField] private bool clearDirtOnStart = true;

    [Header("Objectives")]
    [SerializeField] private int oilRequired = 1;

    public event Action OilFound;
    public event Action AllObjectivesCompleted;

    private int oilFoundCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (clearDirtOnStart)
            ClearDirtTexture();

        EnablePainter(false); // disabled until mining starts
    }

    /// <summary>
    /// Enables or disables the <see cref="DirtPainter"/> component.
    /// </summary>
    public void EnablePainter(bool state)
    {
        if (dirtPainter != null)
            dirtPainter.enabled = state;
    }

    /// <summary>
    /// Call when an oil deposit has been found.
    /// </summary>
    public void RegisterOilFound()
    {
        oilFoundCount++;
        OilFound?.Invoke();

        if (oilFoundCount >= oilRequired)
            AllObjectivesCompleted?.Invoke();
    }

    /// <summary>
    /// Clears the dirt render texture.
    /// </summary>
    public void ClearDirtTexture()
    {
        if (dirtTexture == null)
            return;

        RenderTexture active = RenderTexture.active;
        RenderTexture.active = dirtTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = active;
    }

    /// <summary>
    /// Resets objective progress and optionally clears the texture.
    /// </summary>
    public void ResetMining(bool clearTexture)
    {
        oilFoundCount = 0;
        if (clearTexture)
            ClearDirtTexture();
    }
}
