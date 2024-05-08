using UnityEngine;
using UnityEngine.UI;
public class FPS : MonoBehaviour
{
    float time;
    float fps;
    float hightFps;
    float lovFps = 1000f;
    bool answer;
    public Text fpsText;
    private void Update() {
        if(Time.frameCount % 30 == 0)
            {
                fpsText.text = fps.ToString();
                fps = 1.0f/Time.smoothDeltaTime;
            }
        if(time<100)
        {
            if(lovFps > fps & Time.frameCount > 1000)
            {
                lovFps = fps;
            }
            if(hightFps < fps & Time.frameCount > 1000)
            {
                hightFps = fps;
            }
            time += Time.deltaTime;
        }
        else if(!answer)
        {
            Debug.Log("Кадров за 100 секунд: " + Time.frameCount);
            Debug.Log(" Самый высокий кадр: " + hightFps);
            Debug.Log(" Самый низкий кадр: " + lovFps);
            Debug.Log(" Средний кадр: " + Time.frameCount / 100);
            answer = true;
        }
    }
}
