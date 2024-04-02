using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraDitherer : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] RenderTexture renderTexture;


    // Update is called once per frame
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            Graphics.Blit(source, renderTexture);
            return;
        }
        Graphics.Blit(source, renderTexture, material);
    }
}
