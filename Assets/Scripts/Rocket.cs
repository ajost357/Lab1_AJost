using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Transform myTransform;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float loopsPerSecond = 0.5f; // 0.5 = one full loop every 2 seconds

    private Vector3 startPosition;
    private float angle = 0f;

    void Start()
    {
        Debug.Log("Hello World");
        startPosition = myTransform.position;
    }

    void Update()
    {
        angle += loopsPerSecond * 2f * Mathf.PI * Time.deltaTime;

        // Circle in the X/Y plane that starts and ends at the starting position
        float x = Mathf.Sin(angle) * radius;
        float y = (1f - Mathf.Cos(angle)) * radius;

        myTransform.position = startPosition + new Vector3(x, y, 0f);
    }
}