using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class SpecialBook : MonoBehaviour
{
    public ParticleSystem sparkles;
    public AudioSource music;
    public AudioClip pickupSound;
    public Transform gripAttach, shelfAttach;
    public Light roomLight;
    public Color grabbedLightColor = Color.magenta;
    public float grabbedLightIntensity = 0.3f;
    public GameObject magicEffects;
    public Material magicSkybox;
    public Renderer floorRenderer;
    public Material grabbedFloorMaterial;

    XRGrabInteractable grab;
    Color origColor;
    float origIntensity;
    Material origSky, origFloor;
    float baseRate = 0.3f;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (gripAttach == null) gripAttach = grab.attachTransform;
        if (roomLight != null) { origColor = roomLight.color; origIntensity = roomLight.intensity; }
        if (floorRenderer != null) origFloor = floorRenderer.sharedMaterial;
        origSky = RenderSettings.skybox;

        grab.hoverEntered.AddListener(_ => SetSparkles(25f));
        grab.hoverExited.AddListener(_ => SetSparkles(1f));
        grab.selectEntered.AddListener(a =>
        {
            if (!(a.interactorObject is XRSocketInteractor)) Apply(true);
        });
        grab.selectExited.AddListener(a =>
        {
            if (!(a.interactorObject is XRSocketInteractor)) Apply(false);
        });
    }

    void Apply(bool on)
    {
        if (on && music != null && pickupSound != null)
            music.PlayOneShot(pickupSound);

        if (roomLight != null)
        {
            roomLight.color = on ? grabbedLightColor : origColor;
            roomLight.intensity = on ? grabbedLightIntensity : origIntensity;
        }

        if (magicEffects != null) magicEffects.SetActive(on);

        RenderSettings.skybox = (on && magicSkybox != null) ? magicSkybox : origSky;

        if (floorRenderer != null)
        {
            var m = on ? grabbedFloorMaterial : origFloor;
            if (m != null) floorRenderer.material = m;
        }
    }

    void SetSparkles(float multiplier)
    {
        if (sparkles == null) return;
        var emission = sparkles.emission;
        emission.rateOverTimeMultiplier = baseRate * multiplier;
    }
}