using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Food : MonoBehaviour
{

    Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) {
            PacMan playerScript = col.GetComponent<PacMan>();
            
            var tilePos = tilemap.WorldToCell(col.gameObject.transform.position);
            tilemap.SetTile(tilePos, null);

            playerScript.AddScore(10);
        }
    }

    void Update()
    {
        if(tilemap.GetUsedTilesCount() == 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
