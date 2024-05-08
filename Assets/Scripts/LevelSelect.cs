using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    public static int currentLevel;
    public int quantityLevels;
    public GameObject levelButton;
    public RectTransform ParentPanel;
    //Сколько разблокированных уровней
    static int levelReached;
    public bool isResetLevel;
    public GameObject [] buttons;
    private void Awake() 
    {
        if(isResetLevel)
        {
            PlayerPrefs.SetInt("level",1);
        }
        LevelButtons();    
    }
    void LevelButtons()
    {
        if(PlayerPrefs.HasKey("level"))
        {
            levelReached = PlayerPrefs.GetInt("level");
        }
        else
        {
           PlayerPrefs.SetInt("level",1);
           levelReached = PlayerPrefs.GetInt("level");;
        }
        for(int i = 0;i < quantityLevels;i++)
        {
            int x = new int();
            x = i + 1;
            buttons[i].gameObject.GetComponent<Button>().onClick.AddListener(delegate{LevelSelected(x);});
            if(i + 1 > levelReached)
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
        }
    }
    void LevelSelected(int index)
    {
        currentLevel = index;
        PlayerPrefs.SetInt("levelSelected", index);
        Debug.Log("Level Selected: " + index);
        LoadLevel(index);
    }
    void LoadLevel(int index) 
    {
        FindObjectOfType<AudioManager>().Stop("menu");
        SceneManager.LoadScene("Level" + index);
    }
}
// for(int i = 0;i < quantityLevels;i++)
        // {
        //     // int x = new int();
        //     // x= i + 1;
        //     // GameObject lvlButton = Instantiate(levelButton);
        //     // lvlButton.transform.SetParent(ParentPanel,false);
        //     // TMP_Text buttonText = lvlButton.GetComponentInChildren<TMP_Text>();
        //     // buttonText.text = (i + 1).ToString();
        //     lvlButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate{LevelSelected(x);});
        //     // if(i + 1 > levelReached)
        //     // {
        //     //     lvlButton.GetComponent<Button>().interactable = false;
        //     // }
        // }