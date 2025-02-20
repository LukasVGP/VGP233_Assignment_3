using UnityEngine;

public class CheckpointVisual : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color blinkColor = Color.yellow;
    [SerializeField] private float intensity = 1.5f;

    private Material archMaterial;
    private static readonly string emissionColor = "_EmissionColor";

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        archMaterial = renderer.material;
        archMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float emission = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
        Color finalColor = Color.Lerp(baseColor, blinkColor, emission) * intensity;
        archMaterial.SetColor(emissionColor, finalColor);
    }
}
