using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelShader : MonoBehaviour
{

    public int h = 64;
    int w;
    protected void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
    }
    void Update()
    {
 
        float ratio = ((float)Camera.main.pixelWidth) / (float)Camera.main.pixelHeight;
        w = Mathf.RoundToInt(h * ratio);
 
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}
