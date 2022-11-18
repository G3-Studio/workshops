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
            int[] direction = col.GetComponent<PacMan>().direction;
            if (direction[0] != 0) {
                float deltaX = direction[0] == -1 ? -1 : 0;
                GetComponent<Tilemap>().SetTile(new Vector3Int((int) Math.Round(playerPos.x + deltaX), (int) Math.Floor(playerPos.y)), null);
            } else {
                float deltaY = direction[1] == -1 ? -1 : 0;
                GetComponent<Tilemap>().SetTile(new Vector3Int((int) Math.Floor(playerPos.x), (int) Math.Round(playerPos.y + deltaY)), null);
            }
        }
    }
}
