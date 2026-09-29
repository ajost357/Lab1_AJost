using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class SpecialBook : MonoBehaviour
{
    public ParticleSystem sparkles;
    public AudioSource music;
    public AudioClip pickupSound;

    [Header("Scene change")]
    public Light roomLight;            // the main light
    public Color grabbedLightColor = Color.magenta;
    public GameObject magicEffects;    // an empty object holding extra particles, fog, etc.
    public Material magicSkybox;       // optional

    XRGrabInteractable grab;
    Color originalLightColor;
    Material originalSkybox;
    float baseRate;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        if (sparkles != null)
            baseRate = sparkles.emission.rateOverTimeMultiplier;

        if (roomLight != null)
            originalLightColor = roomLight.color;

        originalSkybox = RenderSettings.skybox;

        grab.hoverEntered.AddListener(_ => SetSparkleIntensity(3f));
        grab.hoverExited.AddListener(_ => SetSparkleIntensity(1f));
        grab.selectEntered.AddListener(_ => OnPickedUp());
        grab.selectExited.AddListener(_ => OnPutAway());
    }

    void OnPickedUp()
    {
        if (music != null && pickupSound != null)
            music.PlayOneShot(pickupSound);

        if (roomLight != null)
            roomLight.color = grabbedLightColor;

        if (magicEffects != null)
            magicEffects.SetActive(true);

        if (magicSkybox != null)
            RenderSettings.skybox = magicSkybox;
    }

    void OnPutAway()
    {
        if (roomLight != null)
            roomLight.color = originalLightColor;

        if (magicEffects != null)
            magicEffects.SetActive(false);

        RenderSettings.skybox = originalSkybox;
    }

    void SetSparkleIntensity(float multiplier)
    {
        if (sparkles == null) return;

        var emission = sparkles.emission;
        emission.rateOverTimeMultiplier = baseRate * multiplier;
    }
}