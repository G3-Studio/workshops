using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Food : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) {
            Vector3 playerPos = col.transform.position;
            PacMan playerScript = col.GetComponent<PacMan>();
            
            var tilePos = GetComponent<Tilemap>().WorldToCell(col.gameObject.transform.position);
            GetComponent<Tilemap>().SetTile(tilePos, null);

            playerScript.addScore(10);
        }
    }

    public int GetFoodCount() {
        return GetComponent<Tilemap>().GetUsedTilesCount();
    }
}
