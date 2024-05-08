using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [HideInInspector]
    public static bool stop;
    public bool isMenu;
    public bool isPause;
    private GameObject pause;
    private Image pauseI;
    Animator [] animators = new Animator[6];
    bool isResume;
    Button buttonResume;
    Animator panelAnimator;
    private void Start() {
        if(isMenu && !stop)
        {
            FindObjectOfType<AudioManager>().Play("menu");
            MainMenu.stop = true;
        }
        else if(isPause)
        {
            buttonResume = GameObject.Find("/Canvas/Pause/Resume").GetComponent<Button>();
        GameObject [] gameAnim = GameObject.FindGameObjectsWithTag("Pause");
            pause = GameObject.Find("Canvas/Pause");        
        for(int i = 0;i < gameAnim.Length;i++)
        {
            animators[i] = gameAnim[i].GetComponent<Animator>();
        }
        }
        if(pause != null)
        {
            pause.SetActive(false);
        }
    }
    public void SetRestart()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().StopAll();
        AudioListener.pause = false;
        SceneManager.LoadScene("Level" + LevelSelect.currentLevel);
    }
    public void LevelMenu()
    {
        FindObjectOfType<AudioManager>().StopAll();
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelMenu");
    }
    public void MainInLevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }
    public void MainWindow()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void NextLevel()
    {
        FindObjectOfType<AudioManager>().StopAll();
        SceneManager.LoadScene("Level" + ++LevelSelect.currentLevel);
    }
    public void Resume()
    {
        ActivityAnimation(false);
        buttonResume.interactable = false;
        isResume = false;
    }
    public void Pause()
    {
        if(!isResume)
        {
            pause.SetActive(true);
            ActivityAnimation(true);
            AudioListener.pause = true;
            Time.timeScale = 0f;
            buttonResume.interactable = true;
            isResume = true;
        }
    }
    private void ActivityAnimation(bool isActivity)
    {
        for(int i = 0;i < animators.Length;i++)
        {
            if(animators[i].name == "Background")
            {

            }
            else
            {
            animators[i].SetBool("Activity",isActivity);
            }
        }
    }
}
