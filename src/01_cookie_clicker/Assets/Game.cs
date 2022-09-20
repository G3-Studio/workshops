using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Import TextMeshPro
using TMPro;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    
    public GameObject Text;
    public int Score = 0;
    public int NbUpgrade = 0;

    private TMP_Text Tmp;
    private Button Btn;
    private int UpgradeCost = 5;

    // Start is called before the first frame update
    void Start()
    {
        Tmp = Text.GetComponent<TextMeshProUGUI>();
        Btn = GameObject.Find("Button").GetComponent<Button>();

        DisableOrEnableButton();

        InvokeRepeating("AddRecurrentScore", 5.0f, 3.0f);
    }

    // Update is called once per frame while button is hovered
    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) 
            UpdateScore(1);
    }

    void AddRecurrentScore() {
        UpdateScore(NbUpgrade);
    }

    void DisableOrEnableButton() {
        if(Score >= UpgradeCost) {
            Btn.interactable = true;
        } else {
            Btn.interactable = false;
        }
    }

    void UpdateBtn(){
        Btn.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade (" + UpgradeCost.ToString() + ")";
    }

    public void Upgrade() {
        UpdateScore(-UpgradeCost);
        UpgradeCost = Mathf.RoundToInt(UpgradeCost * 1.2f);
        UpdateBtn();
        NbUpgrade += 1;
    }

    void UpdateScore(int value){
        Score += value;
        Tmp.text = "Score : " + Score.ToString();
        DisableOrEnableButton();
    }
}


