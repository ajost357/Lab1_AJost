using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class SpecialBook : MonoBehaviour
{
    public ParticleSystem magicSparkles;
    public AudioSource music;
    public AudioClip pickupSound;

    XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.hoverEntered.AddListener(_ => SetSparkleIntensity(3f));
        grab.hoverExited.AddListener(_ => SetSparkleIntensity(1f));
        grab.selectEntered.AddListener(_ => music.PlayOneShot(pickupSound));
    }

    void SetSparkleIntensity(float multiplier)
    {
        var emission = magicSparkles.emission;
        emission.rateOverTimeMultiplier = 10f * multiplier;
    }
}