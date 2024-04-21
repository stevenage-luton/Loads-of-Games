using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkyboxCamera : MonoBehaviour
{
    private Transform mainCamera;

    public Camera ourCamera;

    [SerializeField]
    private float skyBoxScale = 20.0f;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = mainCamera.rotation;
        transform.localPosition = mainCamera.position / skyBoxScale;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == ourCamera)
        {
            RenderSettings.fog = true;
        }

    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == ourCamera)
        {
            RenderSettings.fog = false;
        }
        
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }
}
