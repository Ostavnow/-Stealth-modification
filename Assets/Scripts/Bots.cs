using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Bots : MonoBehaviour
{
    [SerializeField] public FieldOfView flashlight;
    [HideInInspector]
    PlayerController playerController;
    [HideInInspector]
    public static bool isBotView;
    public bool botStand;
    AudioManager audioManager;
    MovementPath coordinatePoints;
    private Vector3 playerPosition;
    [Header("Скорость поворота персонажа на месте")]
    public float speedTurn = 5f;
    public GameObject player;
    public int currentMoveng = 0;
    public bool isDeathBot; //Переменная которая потверждает умер ли бот
    // Vector3 mouseRotate;
    Rigidbody rb;
    NavMeshAgent agent;
    [HideInInspector]
    static bool IsThereABotInPosition; //Эта переменная нужна если боты следуя на позицию где в последний раз был игрок,говорила бы другим
    //ботам о том что на позиции есть бот, чтобы они не дрались за неё
    private GameObject panelHp; //Полоска здоровья у персонажа
    [HideInInspector]
    Transform playerT;
    Quaternion rotation;
    float time;
    //Это переменная нужна что если бот чтото обнаружил то другие не бегут ему на подмогу
    static bool check1 = true;
    int count;
    public Material material;
    bool check = true;
    [HideInInspector]
    public GameObject exclamationMark;
    [HideInInspector]
    public GameObject magnifier;
    float y;
    Vector3 point;
    private void Awake() {
    }
    private void Start() {
        Bots.isBotView = false;
        currentMoveng = 0;
        flashlight = GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        audioManager = FindObjectOfType<AudioManager>();
        exclamationMark = GetComponentInChildren<Transform>().GetChild(0).GetComponentInChildren<Transform>().GetChild(0).GetComponentInChildren<Transform>().GetChild(0).gameObject;
        magnifier = GetComponentInChildren<Transform>().GetChild(0).GetComponentInChildren<Transform>().GetChild(0).GetComponentInChildren<Transform>().GetChild(1).gameObject;
        GetComponentInChildren<Transform>().GetChild(2).gameObject.tag = "Bot";
        player = GameObject.Find("Player");
        playerT = player.GetComponent<Transform>();
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        if(botStand)
        {
            rb.isKinematic = true;
        }
        panelHp = GetComponentInChildren<Transform>().GetChild(0).gameObject;
        coordinatePoints = GetComponent<MovementPath>();
        if(!botStand)
        {
          agent.SetDestination(coordinatePoints.PathElements[currentMoveng].position);  
          point = coordinatePoints.PathElements[currentMoveng].position;
        } 
        material.color = new Vector4(255/255f,255/255f,1f,100/255f);
    }
    private void LateUpdate() {
        panelHp.transform.eulerAngles = Vector3.zero;
    }
    public void Update() {       
        if(!isDeathBot)
        {   
        if(botStand)
        {
        } 
        else if(coordinatePoints.PathElements.Length > 1 & !isBotView){
        if(!isBotView)
        {
            if(Vector3.Distance(transform.position,new Vector3(point.x,transform.position.y,point.z))<0.2f)
        {
            if(currentMoveng == coordinatePoints.PathElements.Length - 1)
            {
                currentMoveng = 0;
                point = coordinatePoints.PathElements[currentMoveng].position;
                agent.SetDestination(coordinatePoints.PathElements[currentMoveng].position);
            }
            else
            {
               currentMoveng++; 
               point = coordinatePoints.PathElements[currentMoveng].position;
               agent.SetDestination(coordinatePoints.PathElements[currentMoveng].position);
            }
        }
        }
        }
        else if(Vector3.Distance(transform.position,playerPosition)<0.3f)
        {   
            rb.isKinematic = true;
            playerController.isBot = false;
            IsThereABotInPosition = true;
            agent.speed = 0;
            if(time == 0)
            {
                check = true;
                time = 1;
                speedTurn = 3;
                StartCoroutine(Rotate());
            }
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,speedTurn * Time.deltaTime);
            if(!check1)
            {
                if(audioManager.InformateAudioClip("viewPlayer").isPlaying == true)
                {
                material.color = new Vector4(153/255f,250/255f,1f,100/255f);
                audioManager.InformateAudioClip("background" + playerController.randomBackgroundSound).volume = 1f;
                audioManager.Play("burnedPlayer");
                audioManager.Stop("viewPlayer");
                speedTurn = 1;
                }
                count = 0;
                time = 0;
                check = true;
                IsThereABotInPosition = false;
                agent.speed = 3;
                exclamationMark.SetActive(false);
                magnifier.SetActive(true);
                //Когда бот начинает искать музыка выключается
                Vector3 vector = Random.insideUnitSphere * 25f;
                vector += transform.position;
                NavMeshHit navMeshHit;
                NavMesh.SamplePosition(vector, out navMeshHit, 25f, 1);
                //Присваевается позиция
                playerPosition = new  Vector3(navMeshHit.position.x,transform.position.y,navMeshHit.position.z);
                agent.SetDestination(playerPosition); 
            }
            }
        } 
    }
    public void IsInView(Vector3 positionPB)
    {
        playerPosition = positionPB;
        agent.SetDestination(positionPB);
        if(check){
            if(audioManager.InformateAudioClip("viewPlayer").isPlaying == false)
            {
            material.color = new Vector4(255/255f,80/255f,73/255f,100/255f);
            audioManager.InformateAudioClip("background" + playerController.randomBackgroundSound).volume = 0.1f;
            audioManager.Play("viewPlayer");
            audioManager.Play("burnedPlayer");
            }
            botStand = false;
            check = false;
            count = 0;
            rb.isKinematic = false;
            time = 0;
            IsThereABotInPosition = false;
            flashlight.meshResolution = 1.3f;
            agent.speed = 6;
            playerController.isBot = true;   
            check1 = true;   
            exclamationMark.SetActive(true);
            magnifier.SetActive(false);
            Bots.isBotView = true;
        }
    }
    private IEnumerator Rotate()
	{
        for(int i = 0;i < 6; i++)
        {
            if(check == false)
            {
                yield break;
            }
            yield return new WaitForSeconds(1.5f);
            Debug.Log(count);
            rotation = new Quaternion(0, Random.Range(2,-2),0,Random.Range(2,-2));
            count++;
        }

            check1 = false;
            yield break;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "corpse" )
        {
            y = transform.localPosition.y;
            playerPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        }
        if(other.tag == "Bot" && IsThereABotInPosition)
        {
            y = transform.localPosition.y;
            playerPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        }
    }
}

