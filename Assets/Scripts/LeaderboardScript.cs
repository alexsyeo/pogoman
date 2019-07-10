using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScript : MonoBehaviour
{
    private static int first;
    private static int second;
    private static int third;

    // Start is called before the first frame update
    void Start()
    {
        first = PlayerPrefs.GetInt("first", 0);
        second = PlayerPrefs.GetInt("second", 0);
        third = PlayerPrefs.GetInt("third", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void NewScore(int newScore)
    {
        if (newScore > first)
        {
            third = second;
            second = first;
            first = newScore;
        }
        else if (newScore > second)
        {
            third = second;
            second = newScore;
        }
        else if (newScore > third)
        {
            third = newScore;
        }

        PlayerPrefs.SetInt("first", first);
        PlayerPrefs.SetInt("second", second);
        PlayerPrefs.SetInt("third", third);
    }

    //public Rect windowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 300, 400, 600);

    //void OnGUI()
    //{
    //    if (playerDead)
    //    {
    //        // Register the window. Notice the 3rd parameter
    //        windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
    //    }
    //}

    //// Make the contents of the window
    //void DoMyWindow(int windowID)
    //{
    //    if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
    //    {
    //        print("Got a click");
    //    }
    //}
}
