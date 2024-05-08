using UnityEngine;
using UnityEngine.UI;
public class AnimationEventUI : MonoBehaviour
{
    GameObject buttonPause;
    Animator background;
    GameObject panelPause;
    Button [] buttonsPause = new Button[3];
    int activityBottonPause;
    int activityBackground;
    int deactivityPanelPause;
    private void Start() {
        buttonsPause[0] = GameObject.Find("/Canvas/Pause/Resume").GetComponent<Button>();
        buttonsPause[1] = GameObject.Find("/Canvas/Pause/Restart").GetComponent<Button>();
        buttonsPause[2] = GameObject.Find("/Canvas/Pause/LevelMenu").GetComponent<Button>();
        panelPause = GameObject.Find("/Canvas/Pause");
        background = GameObject.Find("/Canvas/Pause/Background").GetComponent<Animator>();
        buttonPause = GameObject.Find("/Canvas/Button Pause");
    }
    public void EnableButtonPause()
    {
        activityBottonPause++;
        if(activityBottonPause % 2 == 1)
        {
          buttonPause.SetActive(false);  
        }
        if(activityBottonPause % 2 == 0)
        {
          buttonPause.SetActive(true);  
        }
    }
    public void ActivityAnimatorBackground()
    {
      activityBackground++;
      if(activityBackground % 2 == 1)
      {
        ActivityButtonsPause(true);
      }
      else if(activityBackground % 2 == 0)
      {
        ActivityButtonsPause(false);
        background.SetBool("Activity",false);
      }
    }
    public void DeactivityBackground()
    {
      deactivityPanelPause++;
      if(deactivityPanelPause % 2 == 0)
      {
      Time.timeScale = 1f;
      AudioListener.pause = false;
      panelPause.SetActive(false);
      }
    }
    public void ActivityButtonsPause(bool activityBottons)
    {
      for(int i = 0;i < buttonsPause.Length;i++)
        {
          buttonsPause[i].interactable = activityBottons;
        }
    }
}
