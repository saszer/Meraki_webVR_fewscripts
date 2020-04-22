
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    void Start()
    {
        // Sets the position to be somewhere inside a sphere
        // with radius 5 and the center at zero.

        transform.position = Random.insideUnitSphere * 20;
    }

		
}
