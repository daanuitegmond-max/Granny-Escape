using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWin : MonoBehaviour
{
    void Start()
    {   
        winTextObject.gameObject.SetActive(false);
    }

    [SerializeField] private TextMeshProUGUI winTextObject;
    [SerializeField] private TextMeshProUGUI survivedTimeText;
    private float survivedTime;
    private bool gameOver = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Stop the timer
            gameOver = true;

            // Show "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.text = "You Lose!";

            // Show survived time
            survivedTimeText.gameObject.SetActive(true);
            survivedTimeText.text = "Survived: " + survivedTime.ToString("F1") + " seconds";
        }
    }

    private void Update()
    {
        if (!gameOver)
        {
            survivedTime += Time.deltaTime;
        }
    }
}

