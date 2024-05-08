using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GamePlay : MonoBehaviour
{
    TMP_Text timerT;
    string timerGame;
    [HideInInspector]
    public float timeGame;
    [HideInInspector]
    public bool timer = true;
    public Button dragButton; 
    void Awake()
    {
        GameObject button = GameObject.Find("Drag");
        if(button != null)
        {
          dragButton = button.GetComponent<Button>();  
        }
    }
    void Start()
    {
       timerT = GetComponent<TMP_Text>(); 
    }

    void Update()
    {
        if (timer)
		{
			timeGame += Time.deltaTime;
			if (timeGame < 60f)
			{
				timerGame = (int)timeGame + "s";
			}
			else if (timeGame >= 60f)
			{
				float num = timeGame / 60f;
				float num2 = timeGame - (float)((int)num * 60);
				timerGame = string.Concat(new object[]{(int)num,"m ",(int)num2,"s"});
			}
            timerT.text =  timerGame;
		}
    }
}
