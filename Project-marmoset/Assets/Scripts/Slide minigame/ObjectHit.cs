using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    Collider objectCollider;
    private float playerpos;

    void Start()
    {
        objectCollider = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider objectCollider)
    {
        if (objectCollider.CompareTag("Player"))
        {
            Debug.Log("Player hit the object!");
            objectCollider.transform.position += Vector3.back * 1f;
        }
    }
}
