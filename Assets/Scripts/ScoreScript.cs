using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public int scoreValue = 0;
    Text score;
    public static ScoreScript singleton;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //score.text = scoreValue.ToString();
        //PlayerPrefs.SetInt("Player Score", scoreValue);
    }

    public void ResetScore()
    {
        scoreValue = 0;
    }

    public void IncrementScore()
    {
        scoreValue++;
        score.text = scoreValue.ToString();
    }

    public void FinalScore()
    {
        PlayerPrefs.SetInt("Player Score", scoreValue);
    }
}
