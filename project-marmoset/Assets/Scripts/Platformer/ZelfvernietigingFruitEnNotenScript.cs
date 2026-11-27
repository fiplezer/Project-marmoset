using System.Collections;
using UnityEngine;

public class ZelfvernietigingFruitEnNotenScript : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == false)
        {
            StartCoroutine(ZelfVernietiging());
        }
    }

    IEnumerator ZelfVernietiging()
    {
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        Destroy(this.gameObject);
    }

}
