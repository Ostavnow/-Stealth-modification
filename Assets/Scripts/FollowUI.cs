using UnityEngine;

public class FollowUI : MonoBehaviour
{
    Camera cam;
    [HideInInspector]
    public static RectTransform image;
    RectTransform canvasRect;
    public static Transform target;
    float offset = 20;
    private void Awake() {
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        image = GetComponent<RectTransform>();
        cam = Camera.main;
    }
    void Update()
    {
        if(target != null)
        {
            Vector2 vector = this.cam.WorldToViewportPoint(target.transform.position);
			Vector2 a = new Vector2(vector.x * this.canvasRect.sizeDelta.x - this.canvasRect.sizeDelta.x * 0.5f, vector.y * this.canvasRect.sizeDelta.y - this.canvasRect.sizeDelta.y * 0.5f);
			image.anchoredPosition = a + new Vector2(0f, this.offset);
        }
    }
}
