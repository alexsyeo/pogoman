using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScoreTextScript : MonoBehaviour
{
    TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    // Update is called once per frame
    void UpdateText()
    {
        score.text = "Final Score: " + PlayerPrefs.GetInt("PLayer Score", 0) + "\n";
        score.text += "\n1st: " + PlayerPrefs.GetInt("first", 0);
        score.text += "\n2nd: " + PlayerPrefs.GetInt("second", 0);
        score.text += "\n3rd: " + PlayerPrefs.GetInt("third", 0);
    }
}