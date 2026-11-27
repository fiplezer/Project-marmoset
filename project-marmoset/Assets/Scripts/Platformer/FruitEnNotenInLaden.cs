using System.Collections;
using System.Threading;
using UnityEngine;

public class FruitEnNotenInLaden : MonoBehaviour
{
    public GameObject[] FruitsAndNuts;
    public GameObject[] RareFruitsAndNuts;

    private int rareChance = 10;
    private int chance;
    private bool timerSpawn = false;
    void Update()
    {   
        //Hier laat ik fruit in spawnen als de timer over is.//
        if (!timerSpawn)
        {
            SpawnRandomFruits();
        }
    }
    // logica voor het laten vallen van het fruit op een willekeurige plek en een willekeurige soort uit de lijst.
    void SpawnRandomFruits()
    {
        chance = (Random.Range(0, 11));
        timerSpawn = true;
        if (chance != rareChance)
        {
            Instantiate(FruitsAndNuts[Random.Range(0, FruitsAndNuts.Length)], new Vector2(Random.Range(-8.8f, 8.8f), 10), Quaternion.identity);
        }
        else
        {
            Instantiate(RareFruitsAndNuts[Random.Range(0, FruitsAndNuts.Length)], new Vector2(Random.Range(-8.8f, 8.8f), 10), Quaternion.identity);

        }
        StartCoroutine(RandomSeconds());
    }

    IEnumerator RandomSeconds()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        timerSpawn = false;
    }
}
