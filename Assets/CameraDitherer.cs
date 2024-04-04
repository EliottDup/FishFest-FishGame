using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraDitherer : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] int downscaleFactor = 2;

    int lastScreenWidth, lastScreenHeight;
    Camera cam;
    [SerializeField] RawImage imageDisplay;

    [Header("Background Color")]
    [SerializeField] Color bottomColor;
    [SerializeField] Color topColor;

    // Update is called once per frame
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        SetRtSize();
    }

    void Update()
    {
        int width = Screen.width;
        int height = Screen.height;

        bool resChangedEnough = false;

        if (Mathf.Abs(lastScreenHeight - height) > downscaleFactor)
        {
            lastScreenHeight = height;
            resChangedEnough = true;
        }
        if (Mathf.Abs(lastScreenWidth - width) > downscaleFactor)
        {
            lastScreenWidth = width;
            resChangedEnough = true;
        }

        if (resChangedEnough)
        {
            SetRtSize();
        }
    }

    void SetRtSize()
    {
        Destroy(renderTexture);
        renderTexture = new RenderTexture(lastScreenWidth / downscaleFactor, lastScreenHeight / downscaleFactor, 24);
        cam.targetTexture = renderTexture;
        imageDisplay.texture = renderTexture;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            Graphics.Blit(source, renderTexture);
            return;
        }
        Color c = Color.Lerp(bottomColor, topColor, transform.position.y / 30);
        material.SetColor("_BackgroundCol", c);
        Graphics.Blit(source, renderTexture, material);
    }
}
