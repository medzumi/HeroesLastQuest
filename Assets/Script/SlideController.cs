using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SlideController : MonoBehaviour
{
    public List<Sprite> Slides;
    public Image currentSlide;
    public float interval;
    public string nextScene;

    float timeLeft;
    int sildeNumber = 0;

    void Start() {
        currentSlide.sprite = Slides[sildeNumber];
        timeLeft = interval;
    }

    void Update() {
     timeLeft -= Time.deltaTime;
     if (timeLeft < 0) {
        NextSlide();
    }
 }

    public void NextSlide() {
        sildeNumber++;
        if(sildeNumber >= Slides.Count) {
            SceneManager.LoadScene (sceneName: nextScene);
            return;
        }
        currentSlide.sprite = Slides[sildeNumber];
        timeLeft = interval;
    }
}
