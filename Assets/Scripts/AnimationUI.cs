using UnityEngine;
using TMPro;
public class AnimationUI : MonoBehaviour
{
    [HideInInspector]
    public static bool isWinning;
    [HideInInspector]
    public static int second;
    [HideInInspector]
    public static int score;
    TMP_Text secondT;
    TMP_Text scoreT;
    void Start()
    {
        if(isWinning)
        {
        secondT = GameObject.Find("Second").GetComponent<TMP_Text>();
        secondT.text = second.ToString() + "s";      
        scoreT = GameObject.Find("score").GetComponent<TMP_Text>();
        scoreT.text = score.ToString() + "s"; 
        }
    }
}
