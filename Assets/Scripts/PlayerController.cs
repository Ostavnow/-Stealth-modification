using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    //Характеристики персонажа
    public Joystick joystick;
    private List<Transform> dragEnemyList = new List<Transform>();
    public float speed = 5f;
    private Rigidbody rb;
    Vector3 moveInput;
    int countStars = 0;
    [HideInInspector]
    //Видит ли нас бот
    public bool isBot;
    public bool NeardyBot;
    public bool isDrag;
    Button dragButton;
    Button cameraZoomButton;
    Button visionButton;
    float timer;
    TMP_Text scopeTime;
    public GameObject portal;
    private GamePlay gamePlay;
    public Animator[] starsAnimation = new Animator[3];
    Vector3 botDistance;
    CapsuleCollider coliders;
    GameObject parentColiders;
    public UnityEvent HitActionButton;
    private Transform dragBot;
    int deactiveEnemy = 200;
    AudioManager audioManager;
    [HideInInspector]
    public int randomBackgroundSound;
    void Start()
    {
        MainMenu.stop = false;
        audioManager = FindObjectOfType<AudioManager>();
        randomBackgroundSound = Random.Range(0,4);
        audioManager.Play("background" + randomBackgroundSound);
        audioManager.InformateAudioClip("background" + randomBackgroundSound).volume = 1f;
        GameObject stars;
        stars = GameObject.FindGameObjectWithTag("UIStars");
        for(int i = 0;i < 3;i++)
        {
            starsAnimation[i] = stars.GetComponentInChildren<Transform>().GetChild(i).GetComponent<Animator>();
        }
        rb = gameObject.GetComponent<Rigidbody>();
        FollowUI.image.gameObject.SetActive(false);
        dragButton = FindObjectOfType<GamePlay>().dragButton;
        dragButton.gameObject.SetActive(false);
        parentColiders = GetComponentInChildren<Transform>().GetChild(1).gameObject;
        coliders = GetComponentInChildren<Transform>().GetChild(1).GetComponent<CapsuleCollider>();
    }

    void Update()
    {  
        if (ListEnemy.instance._listTransform.Count != 0)
		{
			Transform closest = GetClosest(ListEnemy.instance._listTransform);
			if ((double)Vector3.Distance(transform.position, closest.position) < 1.8f)
			{
				FollowUI.target = closest;
				if (timer > 0f)
				{
					timer -= Time.deltaTime;
					if (timer < 1.6f)
					{
                        FollowUI.image.gameObject.SetActive(true);
                        FollowUI.image.GetComponent<Image>().fillAmount = timer / 2f;
					}
				}
                else
                {
                    audioManager.Play("killBot");
                    ListEnemy.instance._listTransform.Remove(closest);
                    Bots bot = closest.GetComponent<Bots>();
                    Destroy(closest.GetComponent<NavMeshAgent>());
                    Destroy(closest.GetComponent<Rigidbody>());
                    FollowUI.image.gameObject.SetActive(false);
                    Destroy(bot.flashlight);
                    Destroy(bot.GetComponent<MeshRenderer>());
                    Destroy(bot.GetComponent<MeshFilter>());
                    bot.isDeathBot = true;
                    bot.magnifier.SetActive(false);
                    bot.exclamationMark.SetActive(false);
                    Destroy(closest.GetComponent<Bots>());
                    closest.gameObject.layer = 9;
                    closest.GetComponentInChildren<Transform>().GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.7921f,0.0235f,0,1);
                    closest.GetComponentInChildren<Transform>().GetChild(2).gameObject.tag = "corpse";
                    closest.gameObject.layer = 3;
                    closest.gameObject.tag = "corpse";
                    dragEnemyList.Add(closest);
                }
            }
            else
            {
                timer = 2f;
                FollowUI.image.gameObject.SetActive(false);
            }
        }
        else
        {
            FollowUI.target = null;
            timer = 2f;
        }
        if (this.dragEnemyList.Count != 0)
		{
			if ((double)Vector3.Distance(base.transform.position, this.GetClosest(this.dragEnemyList).position) < 1.8f)
			{
				dragButton.gameObject.SetActive(true);
			}
			else if ((double)Vector3.Distance(base.transform.position, this.GetClosest(this.dragEnemyList).position) > 1.8f & !isDrag)
			{
				dragButton.gameObject.SetActive(false);

			}
		}
		else
		{
			dragButton.gameObject.SetActive(false);
		}
        if(isDrag)
        {
            dragBot.position = transform.position + botDistance;
        }
        moveInput = new Vector3(joystick.Horizontal,0,joystick.Vertical);
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("stars"))
        {
            audioManager.Play("stars");
            if(countStars == 0)
            {
                starsAnimation[0].SetTrigger("StarsActivity");
            }
            if(countStars == 1)
            {
                starsAnimation[1].SetTrigger("StarsActivity");
            }
            if(countStars == 2)
            {
                starsAnimation[2].SetTrigger("StarsActivity");
                portal.SetActive(true);
            }
            Destroy(other.gameObject);
            countStars++;
        }

        if(other.CompareTag("corpse"))
        {
            NeardyBot = true;
        }
        if(other.CompareTag("eat"))
        {
             HitActionButton.Invoke();
             audioManager.Play("stars");
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Bot"))
        {
            if(isBot)
            {
                audioManager.Stop("viewPlayer");
                audioManager.Stop("background" + randomBackgroundSound);
                audioManager.Play("gameOver");
                AnimationUI.isWinning = false;
                SceneManager.LoadScene("GameOver");
            }
        }
        if(other.CompareTag("Portal"))
        {
            AnimationUI.second = (int)FindObjectOfType<GamePlay>().timeGame;
            FindObjectOfType<GamePlay>().timer = false;
            int num = 6000 - 10 * (int)FindObjectOfType<GamePlay>().timeGame;
		    num -= deactiveEnemy;
		    if (num < 100)
		    {
			num = 100;
		    }
            AnimationUI.score = num;
            AnimationUI.isWinning = true;
            FindObjectOfType<GameManager>().endGame(nameof(WinningLoad),2,randomBackgroundSound);
            Destroy(gameObject);
            audioManager.Play("finish");
        }
    }
    public void OnDrag() {
        if(!isDrag)
        {
            dragBot = this.GetClosest(this.dragEnemyList);
            //Рачёт градуса поворота объекта другому объекту
            Vector3 targetPos = dragBot.transform.position;
            targetPos.y = transform.position.y;
            botDistance = targetPos - transform.position;
            speed = 2;
            parentColiders.transform.eulerAngles = new Vector3(0,Vector3.SignedAngle(parentColiders.transform.forward,botDistance,Vector3.up),0);
            coliders.height += Vector3.Distance(transform.position, dragBot.transform.position);
            coliders.center = new Vector3(0,0,Vector3.Distance(transform.position, dragBot.transform.position) / 2);
            dragBot.GetComponent<CapsuleCollider>().enabled = false;
          isDrag = true;  
        }
        else
        {
            Transform bot = this.GetClosest(this.dragEnemyList);
            isDrag = false;  
            speed = 5;
            float centerZ = coliders.center.z;
            parentColiders.transform.eulerAngles = new Vector3(0,0,0.7f);
            coliders.height = 1;
            coliders.center = new Vector3(0,0,0);
            dragBot.GetComponent<CapsuleCollider>().enabled = true;
        }
        }
    public void OnVisionPostulation()
    { 
        cameraZoomButton.enabled = false;
            visionButton.enabled = false;

    }

    public void CameraZoom()
    {
            cameraZoomButton.enabled = false;
            visionButton.enabled = false;

    }
    private void FixedUpdate() {
        rb.velocity = moveInput * speed;
    }
    public Transform GetClosest(List<Transform> joing)
	{
		Transform result = null;
		float num = float.PositiveInfinity;
		int num2 = 0;
		Vector3 position = base.transform.position;
		foreach (Transform transform in joing)
		{
			float num3 = Vector3.Distance(transform.position, position);
			if (num3 < num)
			{
				result = transform;
				num = num3;
			}
			num2++;
		}
		return result;
	}
    void WinningLoad()
    {
        audioManager.Stop("background" + randomBackgroundSound);
        audioManager.Play("winner");
        int i = PlayerPrefs.GetInt("level");
        if(i == LevelSelect.currentLevel)
        {
           PlayerPrefs.SetInt("level",++i); 
        }
        SceneManager.LoadScene("Winning");
    }
}