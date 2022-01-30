using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraDecolorizeEffectController : MonoBehaviour
{
    [Range(0, 1)] public float Intensity;
    [SerializeField] private Material _material;
    private static readonly int Intensity1 = Shader.PropertyToID("_bwBlend");

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        _material.SetFloat(Intensity1, Intensity);
        Graphics.Blit(src, dest, _material);
    }
}
