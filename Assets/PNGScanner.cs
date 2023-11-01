using UnityEngine;
using UnityEngine.Rendering;

public class PNGScanner : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _light;

    private static Texture2D _destinationTexture;
    private static Rect _rect = new Rect(0F, 0F, 64F, 64F);

    public static PNGScanner Current { get; private set; }

    private void Awake()
    {
        Current = this;
    }

    public static Sprite RenderSprite(GameObject target)
    {
        GameObject clone = Instantiate(target);
        clone.layer = LayerMask.NameToLayer("PNGScan");
        Bounds bounds = clone.GetComponent<MeshRenderer>().bounds;
        clone.transform.position -= bounds.center - clone.transform.position;
        float max = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        Current._camera.orthographicSize = max * 0.6F * Mathf.Sqrt(2F);
        Current._light.SetActive(true);
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        Current._camera.Render();
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        Current._light.SetActive(false);
        clone.transform.position = Vector3.one * 100F;
        Destroy(clone);
        Sprite sprite = Sprite.Create(_destinationTexture, _rect, Vector2.zero);
        return sprite;
    }

    private static void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        _destinationTexture = new(64, 64, TextureFormat.RGBA32, false);
        _destinationTexture.ReadPixels(_rect, 0, 0);
        _destinationTexture.Apply();
    }
}