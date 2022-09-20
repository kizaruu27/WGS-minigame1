using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
#if TMP
using TMPro;
#endif

public class AnimateUIPROControl : MonoBehaviour {

    public AnimateUIPRO mainText;
#if TMP
    public TMP_Dropdown animationsDropDown;
#else
    public Dropdown animationsDropDown;
#endif

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void CallAnimateDropdown()
    {

#if UNITY_EDITOR
        if (EditorApplication.isPlaying) 
            ExecuteAnimButton();
#else
        ExecuteAnimButton();
#endif
    }

    void ExecuteAnimButton() {
        if (animationsDropDown.value == 0) {
            mainText.animationList = AnimationList.Bounce;
        }
        else if (animationsDropDown.value == 1) {
            mainText.animationList = AnimationList.Pulse;
        }
        else if (animationsDropDown.value == 2) {
            mainText.animationList = AnimationList.Flash;
        }
        else if (animationsDropDown.value == 3) {
            mainText.animationList = AnimationList.ShakeX;
        }
        else if (animationsDropDown.value == 4) {
            mainText.animationList = AnimationList.ShakeY;
        }
        else if (animationsDropDown.value == 5) {
            StartCoroutine(mainText.TypingEffect());
        }
        else if (animationsDropDown.value == 6) {
            mainText.animationList = AnimationList.ZoomIn;
        }
        else if (animationsDropDown.value == 7) {
            mainText.animationList = AnimationList.ZoomInDown;
        }
        else if (animationsDropDown.value == 8) {
            mainText.animationList = AnimationList.ZoomInUp;
        }
        else if (animationsDropDown.value == 9) {
            mainText.animationList = AnimationList.ZoomInLeft;
        }
        else if (animationsDropDown.value == 10) {
            mainText.animationList = AnimationList.ZoomInRight;
        }
        else if (animationsDropDown.value == 11) {
            mainText.animationList = AnimationList.ZoomOut;
        }
        else if (animationsDropDown.value == 12) {
            mainText.animationList = AnimationList.ZoomOutDown;
        }
        else if (animationsDropDown.value == 13) {
            mainText.animationList = AnimationList.ZoomOutUp;
        }
        else if (animationsDropDown.value == 14) {
            mainText.animationList = AnimationList.ZoomOutLeft;
        }
        else if (animationsDropDown.value == 15) {
            mainText.animationList = AnimationList.ZoomOutRight;
        }
        else if (animationsDropDown.value == 16) {
            mainText.animationList = AnimationList.SlideInUp;
        }
        else if (animationsDropDown.value == 17) {
            mainText.animationList = AnimationList.SlideInDown;
        }
        else if (animationsDropDown.value == 18) {
            mainText.animationList = AnimationList.SlideInLeft;
        }
        else if (animationsDropDown.value == 19) {
            mainText.animationList = AnimationList.SlideInRight;
        }
        else if (animationsDropDown.value == 20) {
            mainText.animationList = AnimationList.SlideOutUp;
        }
        else if (animationsDropDown.value == 21) {
            mainText.animationList = AnimationList.SlideOutDown;
        }
        else if (animationsDropDown.value == 22) {
            mainText.animationList = AnimationList.SlideOutLeft;
        }
        else if (animationsDropDown.value == 23) {
            mainText.animationList = AnimationList.SlideOutRight;
        }
        else if (animationsDropDown.value == 24) {
            mainText.animationList = AnimationList.FadeIn;
        }
        else if (animationsDropDown.value == 25) {
            mainText.animationList = AnimationList.FadeInUp;
        }
        else if (animationsDropDown.value == 26) {
            mainText.animationList = AnimationList.FadeInDown;
        }
        else if (animationsDropDown.value == 27) {
            mainText.animationList = AnimationList.FadeInLeft;
        }
        else if (animationsDropDown.value == 28) {
            mainText.animationList = AnimationList.FadeInRight;
        }
        else if (animationsDropDown.value == 29) {
            mainText.animationList = AnimationList.FadeOut;
        }
        else if (animationsDropDown.value == 30) {
            mainText.animationList = AnimationList.FadeOutUp;
        }
        else if (animationsDropDown.value == 31) {
            mainText.animationList = AnimationList.FadeOutDown;
        }
        else if (animationsDropDown.value == 32) {
            mainText.animationList = AnimationList.FadeOutLeft;
        }
        else if (animationsDropDown.value == 33) {
            mainText.animationList = AnimationList.FadeOutRight;
        }
        else if (animationsDropDown.value == 34) {
            mainText.animationList = AnimationList.FlipRight;
        }
        else if (animationsDropDown.value == 35) {
            mainText.animationList = AnimationList.FlipLeft;
        }
        else if (animationsDropDown.value == 36) {
            mainText.animationList = AnimationList.FlipInX;
        }
        else if (animationsDropDown.value == 37) {
            mainText.animationList = AnimationList.FlipInY;
        }
        else if (animationsDropDown.value == 38) {
            mainText.animationList = AnimationList.FlipOutX;
        }
        else if (animationsDropDown.value == 39) {
            mainText.animationList = AnimationList.FlipOutY;
        }
        else if (animationsDropDown.value == 40) {
            mainText.animationList = AnimationList.RollIn;
        }
        else if (animationsDropDown.value == 41) {
            mainText.animationList = AnimationList.RollOut;
        }
        mainText.ExecuteAnimation();
    }
}
