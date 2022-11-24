using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] public GameObject tilemap;
    private Food food;

    // Start is called before the first frame update
    void Start()
    {
        food = tilemap.GetComponent<Food>();
    }

    // Update is called once per frame
    void Update()
    {
        if(food.GetFoodCount() == 0) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
