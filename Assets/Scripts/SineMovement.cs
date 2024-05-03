using UnityEngine;

public class SineMovement : MonoBehaviour
{
    public float amplitude = 0.5f; // The amplitude of the sine wave
    public float frequency = 2f; // The frequency of the sine wave

    private Vector3 startPos; // The starting position of the GameObject

    void Start()
    {
        // Store the initial position of the GameObject
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using the sine function
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Update the position of the GameObject
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}