using UnityEngine;
using System.Collections;
using Assets.Scripts.Enum;

public class TowerColorSweep : MonoBehaviour
{
    Renderer[] renderers;
    MaterialPropertyBlock block;

    float minY;
    float maxY;
    float sweep;

    static readonly int SweepID = Shader.PropertyToID("_Sweep");
    static readonly int ColorIndexID = Shader.PropertyToID("_ColorIndex");
    static readonly int TargetIndexID = Shader.PropertyToID("_TargetIndex");
    static readonly int MinYID = Shader.PropertyToID("_MinY");
    static readonly int MaxYID = Shader.PropertyToID("_MaxY");

    int GetColorIndex(Color color)
    {
        if (color == Color.black) return 0;
        else if (color == Color.white) return 1;
        else if (color == Color.red) return 2;
        else if (color == Color.purple) return 3;
        else if (color == Color.orange) return 4;
        else if (color == Color.yellow) return 5;
        else if (color == Color.green) return 6;
        else if (color == Color.blue) return 8;

        return 0;
    }

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
            if (b.min.y < minY) minY = b.min.y;
            if (b.max.y > maxY) maxY = b.max.y;
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
        int index = GetColorIndex(color);

        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);
            block.SetInt(ColorIndexID, index);
            block.SetInt(TargetIndexID, index);
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
        int index = GetColorIndex(newColor);

        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);
            block.SetInt(TargetIndexID, index);
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
            block.SetInt(ColorIndexID, index);
            r.SetPropertyBlock(block);
        }
    }
}