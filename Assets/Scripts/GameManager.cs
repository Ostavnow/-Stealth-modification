using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    int randomBackgroundSound;
    public void endGame(string name,float time,int randomBackgroundSound)
    {
        Invoke(name,time);
        this.randomBackgroundSound = randomBackgroundSound;
    }
    void RestartLevel()
    {
        // Bots.isBotView = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // MovementPath.player = GameObject.Find("Player");
    }
    void WinningLoad()
    {
        FindObjectOfType<AudioManager>().Stop("background" + randomBackgroundSound);
        FindObjectOfType<AudioManager>().Stop("viewPlayer");
        FindObjectOfType<AudioManager>().Play("winner");
        int i = PlayerPrefs.GetInt("level");
        if(i == LevelSelect.currentLevel)
        {
           PlayerPrefs.SetInt("level",++i); 
        }
        SceneManager.LoadScene("Winning");
    }
}
