using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject platformPrefab;

    public int platformCount = 20;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPosition = new Vector3();

        for(int i = 0; i < platformCount; i++)
        {
            spawnPosition.y += Random.Range(0.25f, 1.5f);
            spawnPosition.x = Random.Range(-2.5f, 2.5f);

            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
