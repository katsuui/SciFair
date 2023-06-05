using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float spinSpeed = 10f; // adjust the speed of rotation
    public float maxSpinTime = 5f; // adjust the maximum time for each spin
    public float noiseScale = 1f; // adjust the scale of the Perlin noise

    private float spinTime; // keep track of the time for each spin
    private Vector3 spinDirection; // keep track of the direction to spin in

    // Start is called before the first frame update
    void Start()
    {
        // initialize the spin direction to a random direction
        spinDirection = GetRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // check if it's time to change spin direction
        if (spinTime >= maxSpinTime)
        {
            // choose a new random spin direction
            spinDirection = GetRandomDirection();
            spinTime = 0f;
        }

        // increment the spin time
        spinTime += Time.deltaTime;

        // smoothly rotate the object in the current spin direction
        transform.Rotate(spinDirection * spinSpeed * Time.deltaTime);
    }

    private Vector3 GetRandomDirection()
    {
        // generate a random Perlin noise value for each axis
        float x = Mathf.PerlinNoise(Random.Range(0f, 100f), Time.time * 0.1f) * 2f - 1f;
        float y = Mathf.PerlinNoise(Random.Range(0f, 100f), Time.time * 0.1f) * 2f - 1f;
        float z = Mathf.PerlinNoise(Random.Range(0f, 100f), Time.time * 0.1f) * 2f - 1f;

        // create a vector from the noise values and scale it
        Vector3 direction = new Vector3(x, y, z) * noiseScale;

        // normalize the vector to get a random direction
        direction.Normalize();

        return direction;
    }
}
