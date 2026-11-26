using UnityEngine;

public class SlideGround : MonoBehaviour
{

public float speed = 2f;
public float acceleration = 0.05f;

void Update()
{
    // Snelheid over tijd
    speed += acceleration * Time.deltaTime;

    // Beweeg de grond naar links
    transform.position += Vector3.left * speed * Time.deltaTime;
}
}