using UnityEngine;
using System.Collections;

public class TowerColorSweep : MonoBehaviour
{
    Renderer[] renderers;
    MaterialPropertyBlock block;

    float minY;
    float maxY;

    float sweep;

    static readonly int SweepID = Shader.PropertyToID("_Sweep");
    static readonly int ColorAID = Shader.PropertyToID("_ColorA");
    static readonly int ColorBID = Shader.PropertyToID("_ColorB");
    static readonly int MinYID = Shader.PropertyToID("_MinY");
    static readonly int MaxYID = Shader.PropertyToID("_MaxY");

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        block = new MaterialPropertyBlock();

        CalculateHeight();
        ApplyHeightToMaterials();
    }

    void CalculateHeight()
    {
        minY = float.MaxValue;
        maxY = float.MinValue;

        foreach (var r in renderers)
        {
            var b = r.bounds;

            if (b.min.y < minY)
                minY = b.min.y;

            if (b.max.y > maxY)
                maxY = b.max.y;
        }
    }

    void ApplyHeightToMaterials()
    {
        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);

            block.SetFloat(MinYID, minY);
            block.SetFloat(MaxYID, maxY);

            r.SetPropertyBlock(block);
        }
    }
    public void SetBaseColor(Color color)
    {
        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);

            block.SetColor(ColorAID, color);
            block.SetColor(ColorBID, color);
            block.SetFloat(SweepID, 0);

            r.SetPropertyBlock(block);
        }
    }

    public void ChangeColor(Color newColor, float duration = 1f)
    {
        StartCoroutine(SweepRoutine(newColor, duration));
    }

    IEnumerator SweepRoutine(Color newColor, float duration)
    {
        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);

            block.SetColor(ColorBID, newColor);

            r.SetPropertyBlock(block);
        }

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            sweep = 1 - Mathf.Clamp01(t / duration);

            foreach (var r in renderers)
            {
                r.GetPropertyBlock(block);

                block.SetFloat(SweepID, sweep);

                r.SetPropertyBlock(block);
            }

            yield return null;
        }

        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);

            block.SetColor(ColorAID, newColor);

            r.SetPropertyBlock(block);
        }
    }
}