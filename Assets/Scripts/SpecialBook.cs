using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class SpecialBook : MonoBehaviour
{
    public ParticleSystem sparkles;
    public AudioSource music;
    public AudioClip pickupSound;

    [Header("Scene change")]
    public Light roomLight;
    public Color grabbedLightColor = Color.magenta;
    public float grabbedLightIntensity = 0.3f;
    public GameObject magicEffects;
    public Material magicSkybox;

    [Header("Floor change")]
    public Renderer floorRenderer;
    public Material grabbedFloorMaterial;

    XRGrabInteractable grab;
    Color originalLightColor;
    float originalLightIntensity;
    Material originalSkybox;
    Material originalFloorMaterial;
    float baseRate;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        if (sparkles != null)
            baseRate = 0.3f;
        grab.hoverEntered.AddListener(_ => Debug.Log("Hover started"));
        if (roomLight != null)
        {
            originalLightColor = roomLight.color;
            originalLightIntensity = roomLight.intensity;
        }

        if (floorRenderer != null)
            originalFloorMaterial = floorRenderer.sharedMaterial;

        originalSkybox = RenderSettings.skybox;

        grab.hoverEntered.AddListener(_ => SetSparkleIntensity(25f));
        grab.hoverExited.AddListener(_ => SetSparkleIntensity(1f));
        grab.selectEntered.AddListener(_ => OnPickedUp());
        grab.selectExited.AddListener(_ => OnPutAway());
    }

    void OnPickedUp()
    {
        if (music != null && pickupSound != null)
            music.PlayOneShot(pickupSound);

        if (roomLight != null)
        {
            roomLight.color = grabbedLightColor;
            roomLight.intensity = grabbedLightIntensity;
        }

        if (magicEffects != null)
            magicEffects.SetActive(true);

        if (magicSkybox != null)
            RenderSettings.skybox = magicSkybox;

        if (floorRenderer != null && grabbedFloorMaterial != null)
            floorRenderer.material = grabbedFloorMaterial;
    }

    void OnPutAway()
    {
        if (roomLight != null)
        {
            roomLight.color = originalLightColor;
            roomLight.intensity = originalLightIntensity;
        }

        if (magicEffects != null)
            magicEffects.SetActive(false);

        RenderSettings.skybox = originalSkybox;

        if (floorRenderer != null && originalFloorMaterial != null)
            floorRenderer.material = originalFloorMaterial;
    }

    void SetSparkleIntensity(float multiplier)
    {
        if (sparkles == null) return;

        var emission = sparkles.emission;
        emission.rateOverTimeMultiplier = baseRate * multiplier;
    }
}