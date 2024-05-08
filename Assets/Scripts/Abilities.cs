using UnityEngine;
public class Abilities : MonoBehaviour
{
    Camera cam;
    float time;
    bool isZoom;
    bool isPatrollingView = false;
    public LineRenderer line;
    public Transform [] points;
    RectTransform barTimeF;
    GameObject barTime;
    SpriteRenderer shadowMask;
    GameObject PatrylingViewButtonButton;
    GameObject zoomButton;
    float alpha = 0;
    public static event linesActivity lActivity;
    public delegate bool linesActivity(bool isActivity);
    void Start()
    {
        barTimeF = GameObject.Find("BarTime").GetComponentInChildren<Transform>().GetChild(0).GetComponent<RectTransform>();
        barTime = GameObject.Find("BarTime");
        PatrylingViewButtonButton = GameObject.Find("PatrylingViewButton");
        zoomButton = GameObject.Find("Zoom");
        shadowMask = GameObject.Find("ShadowMask").GetComponent<SpriteRenderer>();
        zoomButton.SetActive(false);
        PatrylingViewButtonButton.SetActive(false);
        barTime.SetActive(false);
        cam = Camera.main;  
    }
    void LateUpdate()
    {      
            if(isZoom)
            {
                
                time += Time.deltaTime;
        barTimeF.anchorMax = new Vector2((5 - time) /  5,1); 
                if(time < 2 & cam.orthographicSize < 50)
                {
                    cam.orthographicSize += Time.deltaTime * 65;
                }
                else if(time < 5)
                {
                    time += Time.deltaTime;
                    barTime.SetActive(true);
                }          
                            
                else if(cam.orthographicSize > 12)
                {
                barTime.SetActive(false);
                    cam.orthographicSize -= Time.deltaTime * 65;
                }
                else
                {
                    time = 0;
                    isZoom = false;
                }
            }
            else if(isPatrollingView)
            {
                alpha += Time.deltaTime;
                time += Time.deltaTime;
                barTimeF.anchorMax = new Vector2((5 - time) /  5,1); 
                if(time < 2 & alpha < 141)
                {
                    alpha += Time.deltaTime * 141;
                    shadowMask.color = new Vector4(0,0,0,alpha/255);
                }
                else if(time < 5)
                {
                    time += Time.deltaTime;
                    barTime.SetActive(true);
                }          
                            
                else if(alpha > 0)
                {
                    lActivity(false);
                    barTime.SetActive(false);
                    alpha -= Time.deltaTime * 141;
                    shadowMask.color = new Vector4(0,0,0,alpha/255);
                }
                else
                {
                    time = 0;
                    isPatrollingView = false;
                } 
            }
        }
    public void onZoomCamera()
    {
        zoomButton.SetActive(false);
        PatrylingViewButtonButton.SetActive(false);
        barTime.SetActive(true);
        isZoom = true;  
    }
    public void onPatrolView()
    {
        zoomButton.SetActive(false);
        PatrylingViewButtonButton.SetActive(false);
        isPatrollingView = true;
        barTime.SetActive(true);
        lActivity(true);
    }
    public void OnActionButton()
    {
        zoomButton.SetActive(true);
        PatrylingViewButtonButton.SetActive(true);
    }
}
