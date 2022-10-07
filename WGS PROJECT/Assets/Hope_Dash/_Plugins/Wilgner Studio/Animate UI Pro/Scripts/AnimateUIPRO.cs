using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if TMP
using TMPro;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AnimationStatus {
    stopped, running, blocked
}

public enum AnimationList {
    None, Bounce, Pulse, Flash, ShakeX, ShakeY, RPGTypingEffect, ZoomIn, ZoomInDown, ZoomInUp, ZoomInLeft, ZoomInRight, ZoomOut, ZoomOutDown, ZoomOutUp,
    ZoomOutLeft, ZoomOutRight, SlideInUp, SlideInDown, SlideInLeft, SlideInRight, SlideOutUp, SlideOutDown, SlideOutLeft, SlideOutRight, SlideOut,
    FadeIn, FadeInUp, FadeInDown, FadeInLeft, FadeInRight, FadeOut, FadeOutUp, FadeOutDown, FadeOutLeft, FadeOutRight, FlipInX, FlipInY, FlipOutX, FlipOutY,
    RollIn, RollOut, FlipRight, FlipLeft

}
[RequireComponent(typeof(CanvasGroup))]
public class AnimateUIPRO : MonoBehaviour {
    public bool log = false;
    public bool startInvisible = false;
    public bool resetAfterEndingAnimation = true;
    public float timeToStart = 0f;
    public bool onStart = true;
    public bool animationInLoop = false;
    public bool inverseFade = false;
    public AnimationList animationList;
    public AnimationStatus animationStatus;
    public UnityEvent afterAnimationIsOver;
    public bool isPreview = false;
    public float previewValue;
    public float previewMaxValue = 1;
    [HideInInspector]
    public CanvasGroup _canvasGroup;

    // Inspector
    public int selectedTab = 0;

    private Vector3 _initPos, _initScale;
    private Quaternion _initRotation;
#if TMP
    private TextMeshProUGUI _textMeshComponent;
#else
    private Text _textMeshComponent;
#endif

    int _index;
    float startRotation;
    float endRotation;

    #region Bounce Settings
    [Header("Bounce Settings")]
    [Rename("Duration Time")]
    public float durationTBounce = 1f;
    [Rename("Height (axis Y)")]
    public float yHeightBounce = 50f;
    #endregion

    #region Pulse Settings
    [Header("Pulse Settings")]
    [Rename("Duration Time")]
    public float durationTPulse = 1f;
    [Rename("Min Scale")]
    public float minScalePulse = 1f;
    [Rename("Max Scale")]
    public float maxScalePulse = 1.2f;
    [Rename("Is Smooth?")]
    public bool smoothPulse = true;
    #endregion

    #region Flash Settings
    [Header("Flash Settings")]
    [Rename("Duration Time")]
    public float durationTFlash = 1f;
    #endregion

    #region Shake X Settings
    [Header("Shake X Settings")]
    [Rename("Duration Time")]
    public float durationTShakeX = 1f;
    [Rename("X Distance")]
    public float xDistanceShakeX = 50f;
    #endregion

    #region Shake Y Settings
    [Header("Shake Y Settings")]
    [Rename("Duration Time")]
    public float durationTShakeY = 1f;
    [Rename("Y Distance")]
    public float yDistanceShakeY = 50f;
    #endregion

    #region RPG Typing Effect Settings
    [Header("RPG Typing Effect")]
    public string[] words;
    [Rename("Is Infinity(loop)")]
    public bool loopWords = true;
    public float typingSpeed = 0.10f;
    #endregion

    #region Zoom In Settings
    [Header("Zoom In Settings")]
    [Rename("Duration Time")]
    public float durationTZoomIn = 1f;
    [Rename("Init Size (in 0 seconds)")]
    public float initSizeZoomIn = 0f;
    [Rename("Max Size (In Duration Time)")]
    public float maxSizeZoomIn = 1f;
    #endregion

    #region Zoom Out Settings
    [Header("Zoom Out Settings")]
    [Rename("Duration Time")]
    public float durationTZoomOut = 1f;
    [Rename("Init Size (in 0 seconds)")]
    public float initSizeZoomOut = 0f;
    [Rename("Max Size (In Duration Time)")]
    public float maxSizeZoomOut = 1f;
    #endregion

    #region Zoom In Settings
    [Header("Zoom In Settings")]
    [Rename("Duration Time")]
    public float durationTZoom = 1f;
    [Rename("Init Size (in 0 seconds)")]
    public float initSizeZoom = 0f;
    [Rename("Max Size (In Duration Time)")]
    public float maxSizeZoom = 1f;
    [Rename("Divide")]
    public int divideZoom = 1;
    [Rename("Width(left, right) / Height(up, down)")]
    public float heightZoom = 200f;
    #endregion

    #region Zoom Out Settings
    [Header("Zoom Out Settings")]
    [Rename("Duration Time")]
    public float durationTZoomOutt = 1f;
    [Rename("Init Size (in 0 seconds)")]
    public float initSizeZoomOutt = 0f;
    [Rename("Max Size (In Duration Time)")]
    public float maxSizeZoomOutt = 1f;
    [Rename("Divide")]
    public int divideZoomOutt = 1;
    [Rename("Width(left, right) / Height(up, down)")]
    public float heightZoomOutt = 200f;
    #endregion

    #region Slide In Settings
    [Header("Slide In Settings")]
    [Rename("Duration Time")]
    public float durationTSlide = 1f;
    [Rename("Width / Height")]
    public float heightSlide = 300f;
    [Rename("Speed")]
    public float speedSlide = 1f;
    #endregion

    #region Slide Out Settings
    [Header("Slide Out Settings")]
    [Rename("Duration Time")]
    public float durationTSlideOut = 1f;
    [Rename("Width / Height")]
    public float heightSlideOut = 300f;
    [Rename("Speed")]
    public float speedSlideOut = 1f;
    #endregion

    #region Fade In Settings
    [Header("Fade In Settings")]
    [Rename("Duration Time")]
    public float durationTFadeIn = 1f;
    [Rename("Width / Height")]
    public float heightFadeIn = 300f;
    [Rename("Speed")]
    public float speedFadeIn = 1f;
    #endregion

    #region Fade Out Settings
    [Header("Fade Out Settings")]
    [Rename("Duration Time")]
    public float durationTFadeOut = 1f;
    [Rename("Width / Height")]
    public float heightFadeOut = 300f;
    [Rename("Speed")]
    public float speedFadeOut = 1f;
    #endregion

    #region Flip Settings
    [Header("Flip Settings")]
    [Rename("Duration Time")]
    public float durationTFlip = 1f;
    [Rename("Is Left")]
    public bool leftFlip = true;
    #endregion

    #region Roll In/Out
    [Header("Roll In/Out Settings")]
    [Rename("Duration Time")]
    public float durationTRoll = 1f;
    [Rename("X Distance")]
    public float xdistanceRoll = 400f;
    #endregion

    // Use this for initialization
    void Awake() {
        _initPos = this.transform.localPosition;
        _initScale = this.transform.localScale;
        _initRotation = this.transform.localRotation;
        _canvasGroup = this.GetComponent<CanvasGroup>();
#if TMP
        _textMeshComponent = this.GetComponent<TextMeshProUGUI>();
#else
        _textMeshComponent = this.GetComponent<Text>();
#endif
        timeToStart = Mathf.Abs(timeToStart);

    }

    void Start() {
        if (animationStatus == AnimationStatus.running)
            animationStatus = AnimationStatus.stopped;

        IsBlocked();
        if (onStart == true) {
            StartCoroutine(ExecuteAnimationWithTimeToStart());
        }

        if (startInvisible)
            _canvasGroup.alpha = 0;

        //if (onStart == true)
        //ExecuteAnimation();
        /*
        StartCoroutine(Timer(CallFlash, () =>
        {
            AnimationEnd();
        }, durationTFlash));
        */
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            TypingNext();
        }

        if (animationInLoop == true) {
            ExecuteAnimation();
        }
    }

    // Processing

    public delegate void UpdateCallBack(float timePassed, float duration);
    public delegate void EndCallBack();
    public IEnumerator Timer(UpdateCallBack updateCallBack, EndCallBack endCallBack, float durationTime) {
        float start = Time.time;
        bool complete = false;
        do {
            float currentTime = Mathf.Clamp(Time.time - start, 0, durationTime);
            updateCallBack(durationTime, currentTime);
            animationStatus = AnimationStatus.running;
            if (currentTime == durationTime) complete = true;
            yield return null;
        } while (complete != true);
        endCallBack();
        yield return null;
    }

    #region Zoom In
    void ZoomIn(float durationTime, float timePass, float init, float max) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);
        float size = Mathf.Lerp(init, max, time);
        this.transform.localScale = new Vector3(size, size, size);
        _canvasGroup.alpha = inverseFade ? FadeOut(1f, 0f, time) : FadeIn(1f, 0f, time);
    }

    void ZoomInDown(float durationTime, float timePass, float init, float max, float yheight, int divide, Vector3 initPos) {
        if (divide <= 0)
            divide = 1;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);
        float size = Mathf.Lerp(init, max, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y - yheight / divide, initPos.z);
        Vector3 c = new Vector3(initPos.x, initPos.y + yheight, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(c, b, initPos, time);

        this.transform.localScale = new Vector3(size, size, size);
        this.transform.localPosition = Vector3.Lerp(bezier, initPos, time);
        _canvasGroup.alpha = inverseFade ? FadeOut(1f, 0f, time) : FadeIn(1f, 0f, time);
    }

    void ZoomInUp(float durationTime, float timePass, float init, float max, float yheight, int divide, Vector3 initPos) {
        if (divide <= 0)
            divide = 1;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);
        float size = Mathf.Lerp(init, max, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y + yheight / divide, initPos.z);
        Vector3 c = new Vector3(initPos.x, initPos.y - yheight, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(c, b, initPos, time);

        this.transform.localScale = new Vector3(size, size, size);
        this.transform.localPosition = Vector3.Lerp(bezier, initPos, time);
        _canvasGroup.alpha = inverseFade ? FadeOut(1f, 0f, time) : FadeIn(1f, 0f, time);
    }

    void ZoomInLeft(float durationTime, float timePass, float init, float max, float xdistance, Vector3 initPos) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        float size = Mathf.Lerp(init, max, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y, initPos.z);
        Vector3 c = new Vector3(initPos.x - xdistance, initPos.y, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(c, b, initPos, time);

        this.transform.localScale = new Vector3(size, size, size);
        this.transform.localPosition = Vector3.Lerp(bezier, initPos, time);
        _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }

    void ZoomInRight(float durationTime, float timePass, float init, float max, float xdistance, Vector3 initPos) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        float size = Mathf.Lerp(init, max, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y, initPos.z);
        Vector3 c = new Vector3(initPos.x + xdistance, initPos.y, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(c, b, initPos, time);

        this.transform.localScale = new Vector3(size, size, size);
        this.transform.localPosition = Vector3.Lerp(bezier, initPos, time);
        _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }
    #endregion

    #region Zoom Out
    void ZoomOut(float durationTime, float timePass, float init, float max) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = (1f - Mathf.Cos(time * Mathf.PI * 0.5f));
        float size = Mathf.Lerp(max, init, time);
        this.transform.localScale = new Vector3(size, size, size);
        _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void ZoomOutDown(float durationTime, float timePass, float init, float max, float yheight, int divide, Vector3 initPos) {
        if (divide <= 0)
            divide = 1;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = (1f - Mathf.Cos(time * Mathf.PI * 0.5f));
        float size = Mathf.SmoothStep(max, init, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y + yheight / divide, initPos.z);
        Vector3 c = new Vector3(initPos.x, initPos.y - yheight, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(initPos, b, c, time);

        this.transform.localPosition = Vector3.Lerp(initPos, bezier, time);
        this.transform.localScale = new Vector3(size, size, size);
        _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void ZoomOutUp(float durationTime, float timePass, float init, float max, float yheight, int divide, Vector3 initPos) {
        if (divide <= 0)
            divide = 1;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = (1f - Mathf.Cos(time * Mathf.PI * 0.5f));
        float size = Mathf.SmoothStep(max, init, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y - yheight / divide, initPos.z);
        Vector3 c = new Vector3(initPos.x, initPos.y + yheight, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(initPos, b, c, time);

        this.transform.localPosition = Vector3.Lerp(initPos, bezier, time);
        this.transform.localScale = new Vector3(size, size, size);

        _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void ZoomOutLeft(float durationTime, float timePass, float init, float max, float xdistance, Vector3 initPos) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = (1f - Mathf.Cos(time * Mathf.PI * 0.5f));
        float size = Mathf.SmoothStep(max, init, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y, initPos.z + 200f);
        Vector3 c = new Vector3(initPos.x - xdistance, initPos.y, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(initPos, b, c, time);

        this.transform.localScale = new Vector3(size, size, size);
        this.transform.localPosition = Vector3.Lerp(initPos, bezier, time);
        _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void ZoomOutRight(float durationTime, float timePass, float init, float max, float xdistance, Vector3 initPos) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = (1f - Mathf.Cos(time * Mathf.PI * 0.5f));
        float size = Mathf.SmoothStep(max, init, time);

        // Point A is initPos
        Vector3 b = new Vector3(initPos.x, initPos.y, initPos.z + 200f);
        Vector3 c = new Vector3(initPos.x + xdistance, initPos.y, initPos.z);
        Vector3 bezier = QuadrictBezierPoint(initPos, b, c, time);

        this.transform.localScale = new Vector3(size, size, size);
        this.transform.localPosition = Vector3.Lerp(initPos, bezier, time);
        _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    #endregion

    #region Slide and Fade In
    void FadeIn(float durationTime, float timePass) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);
        _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }

    void FadeSlideInUp(float durationTime, float timePass, float yheight, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x, initPos.y - yheight, initPos.z);
        this.transform.localPosition = Vector3.Lerp(newPos, initPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }

    void FadeSlideInDown(float durationTime, float timePass, float yheight, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x, initPos.y + yheight, initPos.z);
        this.transform.localPosition = Vector3.Lerp(newPos, initPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }

    void FadeSlideInLeft(float durationTime, float timePass, float xdistance, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x - xdistance, initPos.y, initPos.z);
        this.transform.localPosition = Vector3.Lerp(newPos, initPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }

    void FadeSlideInRight(float durationTime, float timePass, float xdistance, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x + xdistance, initPos.y, initPos.z);
        this.transform.localPosition = Vector3.Lerp(newPos, initPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }

    #endregion

    #region Slide and Fade Out
    void FadeOut(float durationTime, float timePass) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void FadeSlideOutUp(float durationTime, float timePass, float yheight, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x, initPos.y + yheight, initPos.z);
        this.transform.localPosition = Vector3.Lerp(initPos, newPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void FadeSlideOutDown(float durationTime, float timePass, float yheight, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x, initPos.y - yheight, initPos.z);
        this.transform.localPosition = Vector3.Lerp(initPos, newPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void FadeSlideOutLeft(float durationTime, float timePass, float xdistance, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x - xdistance, initPos.y, initPos.z);
        this.transform.localPosition = Vector3.Lerp(initPos, newPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }

    void FadeSlideOutRight(float durationTime, float timePass, float xdistance, float speed, bool fade, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x + xdistance, initPos.y, initPos.z);
        this.transform.localPosition = Vector3.Lerp(initPos, newPos, time * speed);

        if (fade == true)
            _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }
    #endregion

    #region Miscellaneous 

    void Bounce(float durationTime, float bounceTime, float bounceHeight, Vector3 initPos) {
        float time = Mathf.Clamp(bounceTime / durationTime, 0f, 1f);
        float b = Mathf.Sin(Mathf.Clamp01(time) * Mathf.PI) * bounceHeight;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, initPos.y + b, this.transform.localPosition.z);
    }

    void Pulse(float durationTime, float timePass, float scaleInit, float scaleMax, bool smooth) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        if (float.IsNaN(time))
            time = 0;

        if (smooth == true)
            time = (time * time) * (3f - 2f * time);

        time = Mathf.Sin(time * Mathf.PI);
        float size = Mathf.Lerp(scaleInit, scaleMax, time);

        this.transform.localScale = new Vector3(size, size, size);
    }

    void Flash(float durationTime, float timePass) {
        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = 1f - Mathf.Cos(time * Mathf.PI * 0.5f);
        time = Mathf.Sin(time * Mathf.PI);
        //float time = Mathf.Lerp(scaleInit, scaleMax, time);
        _canvasGroup.alpha = FadeOut(1f, 0f, time);

    }

    void ShakeX(float durationTime, float timePassed, float xdistance, Vector3 initPos) {
        float time = Mathf.Clamp(timePassed / durationTime, 0f, 1f);
        time = 0.5f - ((Mathf.Sin(time * 4f * (Mathf.PI + -1.57f))) / 2);
        //old: 0.5f - (Mathf.Sin(time * 5f * Mathf.PI) / 2);

        Vector3 a = new Vector3(initPos.x - xdistance, initPos.y, initPos.z);
        Vector3 b = new Vector3(initPos.x + xdistance, initPos.y, initPos.z);

        this.transform.localPosition = Vector3.Lerp(a, b, time);
    }

    void ShakeY(float durationTime, float timePassed, float ydistance, Vector3 initPos) {
        float time = Mathf.Clamp(timePassed / durationTime, 0f, 1f);
        time = 0.5f - ((Mathf.Sin(time * 4f * (Mathf.PI + -1.57f))) / 2);
        //old: 0.5f - (Mathf.Sin(time * 5f * Mathf.PI) / 2);

        Vector3 a = new Vector3(initPos.x, initPos.y - ydistance, initPos.z);
        Vector3 b = new Vector3(initPos.x, initPos.y + ydistance, initPos.z);

        this.transform.localPosition = Vector3.Lerp(a, b, time);
    }

    void Flip(float durationTime, float timePassed, bool left) {

    }
    #endregion

    #region Roll In / Out
    void RollIn(float durationTime, float timePass, float xdistance, float speed, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x - xdistance, initPos.y, initPos.z);
        this.transform.localPosition = Vector3.Lerp(newPos, initPos, time * speed);
        _canvasGroup.alpha = FadeIn(1f, 0f, time);
    }

    void RollOut(float durationTime, float timePass, float xdistance, float speed, Vector3 initPos) {
        speed = Mathf.Abs(speed);
        if (speed == 0)
            speed = 1f;

        float time = Mathf.Clamp(timePass / durationTime, 0f, 1f);
        time = Mathf.Sin(time * Mathf.PI * 0.5f);

        Vector3 newPos = new Vector3(initPos.x + xdistance, initPos.y, initPos.z);
        this.transform.localPosition = Vector3.Lerp(initPos, newPos, time * speed);
        _canvasGroup.alpha = FadeOut(1f, 0f, time);
    }
    #endregion

    // Calls

    #region Miscellaneous Call
    public IEnumerator TypingEffect() {
#if TMP
        if (_textMeshComponent == null)
            _textMeshComponent.GetComponent<TextMeshProUGUI>();
#else
        if (_textMeshComponent == null)
            _textMeshComponent = this.GetComponent<Text>();
#endif

        if (_textMeshComponent != null) {
            _textMeshComponent.text = "";
            foreach (char l in words[_index].ToCharArray()) {
                animationStatus = AnimationStatus.running;
                _textMeshComponent.text += l;
                yield return new WaitForSeconds(typingSpeed);
            }

        }
        animationStatus = AnimationStatus.stopped;
    }

    public void CallBounce(float duration, float timePassed) {
        Bounce(duration, timePassed, yHeightBounce, _initPos);
    }

    public void CallPulse(float duration, float timePassed) {
        Pulse(duration, timePassed, minScalePulse, maxScalePulse, smoothPulse);
    }

    public void CallFlash(float duration, float timePassed) {
        Flash(duration, timePassed);
    }

    public void CallShakeX(float duration, float timePassed) {
        ShakeX(duration, timePassed, xDistanceShakeX, _initPos);
    }

    public void CallShakeY(float duration, float timePassed) {
        ShakeY(duration, timePassed, yDistanceShakeY, _initPos);
    }

    #endregion

    #region Zoom IN/OUT call

    public void CallZoomIn(float duration, float timePassed) {
        ZoomIn(duration, timePassed, initSizeZoomIn, maxSizeZoomIn);
    }

    public void CallZoomInDown(float duration, float timePassed) {
        ZoomInDown(duration, timePassed, initSizeZoom, maxSizeZoom, heightZoom, divideZoom, _initPos);
    }

    public void CallZoomInUp(float duration, float timePassed) {
        ZoomInUp(duration, timePassed, initSizeZoom, maxSizeZoom, heightZoom, divideZoom, _initPos);
    }

    public void CallZoomInLeft(float duration, float timePassed) {
        ZoomInLeft(duration, timePassed, initSizeZoom, maxSizeZoom, heightZoom, _initPos);
    }

    public void CallZoomInRight(float duration, float timePassed) {
        ZoomInRight(duration, timePassed, initSizeZoom, maxSizeZoom, heightZoom, _initPos);
    }

    public void CallZoomOut(float duration, float timePassed) {
        ZoomOut(duration, timePassed, initSizeZoomOut, maxSizeZoomOut);
    }

    public void CallZoomOutDown(float duration, float timePassed) {
        ZoomOutDown(duration, timePassed, initSizeZoomOutt, maxSizeZoomOutt, heightZoomOutt, divideZoomOutt, _initPos);
    }

    public void CallZoomOutUp(float duration, float timePassed) {
        ZoomOutUp(duration, timePassed, initSizeZoomOutt, maxSizeZoomOutt, heightZoomOutt, divideZoomOutt, _initPos);
    }

    public void CallZoomOutLeft(float duration, float timePassed) {
        ZoomOutLeft(duration, timePassed, initSizeZoomOutt, maxSizeZoomOutt, heightZoomOutt, _initPos);
    }

    public void CallZoomOutRight(float duration, float timePassed) {
        ZoomOutRight(duration, timePassed, initSizeZoomOutt, maxSizeZoomOutt, heightZoomOutt, _initPos);
    }
    #endregion

    #region Slide In call
    public void CallFadeIn(float duration, float timePassed) {
        FadeIn(duration, timePassed);
    }

    public void CallSlideInUp(float duration, float timePassed) {
        FadeSlideInUp(duration, timePassed, heightSlide, speedSlide, false, _initPos);
    }

    public void CallSlideInDown(float duration, float timePassed) {
        FadeSlideInDown(duration, timePassed, heightSlide, speedSlide, false, _initPos);
    }

    public void CallSlideInLeft(float duration, float timePassed) {
        FadeSlideInLeft(duration, timePassed, heightSlide, speedSlide, false, _initPos);
    }

    public void CallSlideInRight(float duration, float timePassed) {
        FadeSlideInRight(duration, timePassed, heightSlide, speedSlide, false, _initPos);
    }
    #endregion

    #region Fade In call
    public void CallFadeInUp(float duration, float timePassed) {
        FadeSlideInUp(duration, timePassed, heightFadeIn, speedSlide, true, _initPos);
    }

    public void CallFadeInDown(float duration, float timePassed) {
        FadeSlideInDown(duration, timePassed, heightFadeIn, speedSlide, true, _initPos);
    }

    public void CallFadeInLeft(float duration, float timePassed) {
        FadeSlideInLeft(duration, timePassed, heightFadeIn, speedSlide, true, _initPos);
    }

    public void CallFadeInRight(float duration, float timePassed) {
        FadeSlideInRight(duration, timePassed, heightFadeIn, speedSlide, true, _initPos);
    }
    #endregion

    #region Fade Out Call
    public void CallFadeOut(float durationTime, float timePassed) {
        FadeOut(durationTime, timePassed);
    }
    public void CallFadeOutUp(float duration, float timePassed) {
        FadeSlideOutUp(duration, timePassed, heightFadeOut, speedSlide, true, _initPos);
    }
    public void CallFadeOutDown(float duration, float timePassed) {
        FadeSlideOutDown(duration, timePassed, heightFadeOut, speedSlide, true, _initPos);
    }
    public void CallFadeOutLeft(float duration, float timePassed) {
        FadeSlideOutLeft(duration, timePassed, heightFadeOut, speedSlide, true, _initPos);
    }
    public void CallFadeOutRight(float duration, float timePassed) {
        FadeSlideOutRight(duration, timePassed, heightFadeOut, speedSlide, true, _initPos);
    }
    #endregion

    #region Slide Out call
    public void CallSlideOutUp(float duration, float timePassed) {
        FadeSlideOutUp(duration, timePassed, heightSlideOut, speedSlide, false, _initPos);
    }

    public void CallSlideOutDown(float duration, float timePassed) {
        FadeSlideOutDown(duration, timePassed, heightSlideOut, speedSlide, false, _initPos);
    }

    public void CallSlideOutLeft(float duration, float timePassed) {
        FadeSlideOutLeft(duration, timePassed, heightSlideOut, speedSlide, false, _initPos);
    }

    public void CallSlideOutRight(float duration, float timePassed) {
        FadeSlideOutRight(duration, timePassed, heightSlideOut, speedSlide, false, _initPos);
    }



    #endregion

    #region Flip call
    public void CallFlipRight(float duration, float timePassed) {
        Flip(duration, timePassed, false);
        float yRotation = Mathf.Lerp(startRotation, endRotation, timePassed) % 360.0f;
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }

    public void CallFlipLeft(float duration, float timePassed) {
        Flip(duration, timePassed, false);
        float yRotation = Mathf.Lerp(endRotation, startRotation, timePassed) % 360.0f;
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }

    public void CallFlipInX(float duration, float timePassed) {
        FadeIn(duration, timePassed);
        float xRotation = Mathf.Lerp(endRotation, startRotation, timePassed) % 90f;
        this.transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void CallFlipInY(float duration, float timePassed) {
        FadeIn(duration, timePassed);
        float yRotation = Mathf.Lerp(endRotation, startRotation, timePassed) % 90f;
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }

    public void CallFlipOutX(float duration, float timePassed) {
        FadeOut(duration, timePassed);
        float xRotation = Mathf.Lerp(startRotation, endRotation, timePassed) % 90f;
        this.transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void CallFlipOutY(float duration, float timePassed) {
        FadeOut(duration, timePassed);
        float yRotation = Mathf.Lerp(startRotation, endRotation, timePassed) % 90f;
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }
    #endregion

    #region Roll In / Out
    public void CallRollIn(float duration, float timePassed) {
        float time = timePassed;
        time = Mathf.Sin(time * Mathf.PI * 0.5f);
        float zRotation = Mathf.Lerp(endRotation, startRotation, time) % 150f;
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
        RollIn(duration, timePassed, xdistanceRoll, 1f, _initPos);
    }

    public void CallRollOut(float duration, float timePassed) {
        float time = timePassed;
        time = Mathf.Sin(time * Mathf.PI * 0.5f);
        float zRotation = Mathf.Lerp(startRotation, endRotation, time) % 150f * -1;
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
        RollOut(duration, timePassed, xdistanceRoll, 1f, _initPos);
    }
    #endregion

    #region Auxiliary Functions

    bool IsBlocked() {
        return animationStatus == AnimationStatus.blocked ? true : false;
    }

    bool AnimationIsRunning() {
        return animationStatus == AnimationStatus.running ? true : false;
    }

    public void ResetAnim() {
        this.transform.localPosition = _initPos;
        this.transform.localScale = _initScale;
        this.transform.localRotation = _initRotation;
        if (_canvasGroup != null)
            _canvasGroup.alpha = 1f;
    }

    float FadeIn(float min, float max, float duration) {
        float value = Mathf.Lerp(max, min, duration);
        if (value >= 0.95)
            value = 1f;
        return value;
    }

    float FadeOut(float min, float max, float duration) {
        float value = Mathf.Lerp(min, max, duration);
        if (value <= 0.05)
            value = 0f;
        return value;
    }

    Vector3 QuadrictBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        // B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2
        //            u          u        tt

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    public void AnimationEnd() {
        if (resetAfterEndingAnimation == true)
            ResetAnim();

        afterAnimationIsOver.Invoke();
        animationStatus = AnimationStatus.stopped;
    }

    public IEnumerator ExecuteAnimationWithTimeToStart() {
        yield return new WaitForSeconds(timeToStart);
        _initScale = new Vector3(1f, 1f, 1f);
        ExecuteAnimation();
    }

    public void ExecuteAnimation() {
        if (IsBlocked() == true || AnimationIsRunning() == true)
            return;

        _initPos = this.transform.localPosition;

        if (startInvisible)
            _canvasGroup.alpha = 0;

        if (animationList == AnimationList.Bounce) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTBounce;
                Bounce(durationTBounce, previewValue, yHeightBounce, _initPos);
            }
            else {
                StartCoroutine(Timer(CallBounce, () => {
                    AnimationEnd();
                }, durationTBounce));
            }
#else
            StartCoroutine(Timer(CallBounce, () =>
            {
                AnimationEnd();
            }, durationTBounce));
#endif
        }
        else if (animationList == AnimationList.Pulse) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTPulse;
                Pulse(durationTPulse, previewValue, minScalePulse, maxScalePulse, smoothPulse);
            }
            else {
                //StartCoroutine(CallPulse(durationTPulse, minScalePulse, maxScalePulse, smoothPulse));
                StartCoroutine(Timer(CallPulse, () => {
                    AnimationEnd();
                }, durationTPulse));
            }
#else
            StartCoroutine(Timer(CallPulse, () =>
            {
                AnimationEnd();
            }, durationTPulse));
#endif
        }
        else if (animationList == AnimationList.Flash) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                if (_canvasGroup == null)
                    _canvasGroup = this.GetComponent<CanvasGroup>();

                previewMaxValue = durationTFlash;
                Flash(durationTFlash, previewValue);
            }
            else {
                StartCoroutine(Timer(CallFlash, () => {
                    AnimationEnd();
                }, durationTFlash));
            }
#else
            StartCoroutine(Timer(CallFlash, () =>
            {
                AnimationEnd();
            }, durationTFlash));
#endif
        }
        else if (animationList == AnimationList.ShakeX) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTShakeX;
                ShakeX(durationTShakeX, previewValue, xDistanceShakeX, _initPos);
            }
            else {
                StartCoroutine(Timer(CallShakeX, () => {
                    AnimationEnd();
                }, durationTShakeX));
            }
#else
            StartCoroutine(Timer(CallShakeX, () =>
            {
                AnimationEnd();
            }, durationTShakeX));
#endif
        }
        else if (animationList == AnimationList.ShakeY) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTShakeX;
                ShakeY(durationTShakeY, previewValue, yDistanceShakeY, _initPos);
            }
            else {
                StartCoroutine(Timer(CallShakeY, () => {
                    AnimationEnd();
                }, durationTShakeY));
            }
#else
                StartCoroutine(Timer(CallShakeY, () =>
                {
                    AnimationEnd();
                }, durationTShakeY));
#endif
        }
        else if (animationList == AnimationList.RPGTypingEffect) {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                StartCoroutine(TypingEffect());
#else

#endif
        }
        else if (animationList == AnimationList.ZoomIn) // Zoom
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                this.transform.localPosition = _initPos;
                previewMaxValue = durationTZoomIn;
                ZoomIn(durationTZoomIn, previewValue, initSizeZoomIn, maxSizeZoomIn);
            }
            else {
                StartCoroutine(Timer(CallZoomIn, () => {
                    AnimationEnd();
                }, durationTZoomIn));
            }
#else
            StartCoroutine(Timer(CallZoomIn, () =>
            {
                AnimationEnd();
            }, durationTZoomIn));
#endif

        }
        else if (animationList == AnimationList.ZoomInDown) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoom;
                ZoomInDown(durationTZoom, previewValue, initSizeZoom, maxSizeZoom, heightZoom, divideZoom, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomInDown, () => {
                    AnimationEnd();
                }, durationTZoom));
            }
#else
            StartCoroutine(Timer(CallZoomInDown, () =>
            {
                AnimationEnd();
            }, durationTZoom));
#endif
        }
        else if (animationList == AnimationList.ZoomInLeft) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoom;
                ZoomInLeft(durationTZoom, previewValue, initSizeZoom, maxSizeZoom, heightZoom, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomInLeft, () => {
                    AnimationEnd();
                }, durationTZoom));
            }
#else
            StartCoroutine(Timer(CallZoomInLeft, () =>
            {
                AnimationEnd();
            }, durationTZoom));
#endif
        }
        else if (animationList == AnimationList.ZoomInRight) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoom;
                ZoomInRight(durationTZoom, previewValue, initSizeZoom, maxSizeZoom, heightZoom, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomInRight, () => {
                    AnimationEnd();
                }, durationTZoom));
            }
#else
            StartCoroutine(Timer(CallZoomInRight, () =>
            {
                AnimationEnd();
            }, durationTZoom));
#endif
        }
        else if (animationList == AnimationList.ZoomInUp) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoom;
                ZoomInUp(durationTZoom, previewValue, initSizeZoom, maxSizeZoom, heightZoom, divideZoom, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomInUp, () => {
                    AnimationEnd();
                }, durationTZoom));
            }
#else
            StartCoroutine(Timer(CallZoomInUp, () =>
            {
                AnimationEnd();
            }, durationTZoom));
#endif
        }
        else if (animationList == AnimationList.ZoomOut) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                this.transform.localPosition = _initPos;
                previewMaxValue = durationTZoomOut;
                ZoomOut(durationTZoomOut, previewValue, initSizeZoomOut, maxSizeZoomOut);
            }
            else {
                StartCoroutine(Timer(CallZoomOut, () => {
                    AnimationEnd();
                }, durationTZoomOut));
            }
#else
            StartCoroutine(Timer(CallZoomOut, () =>
            {
                AnimationEnd();
            }, durationTZoomOut));
#endif
        }
        else if (animationList == AnimationList.ZoomOutDown) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoomOutt;
                ZoomOutDown(durationTZoomOutt, previewValue, initSizeZoomOut, maxSizeZoomOut, heightZoomOutt, divideZoomOutt, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomOutDown, () => {
                    AnimationEnd();
                }, durationTZoomOutt));
            }
#else
            StartCoroutine(Timer(CallZoomOutDown, () =>
            {
                AnimationEnd();
            }, durationTZoomOutt));
#endif
        }
        else if (animationList == AnimationList.ZoomOutLeft) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoomOutt;
                ZoomOutLeft(durationTZoomOutt, previewValue, initSizeZoomOut, maxSizeZoomOut, heightZoomOutt, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomOutLeft, () => {
                    AnimationEnd();
                }, durationTZoomOutt));
            }
#else
            StartCoroutine(Timer(CallZoomOutLeft, () =>
            {
                AnimationEnd();
            }, durationTZoomOutt));
#endif
        }
        else if (animationList == AnimationList.ZoomOutRight) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoomOutt;
                ZoomOutRight(durationTZoomOutt, previewValue, initSizeZoomOut, maxSizeZoomOut, heightZoomOutt, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomOutRight, () => {
                    AnimationEnd();
                }, durationTZoomOutt));
            }
#else
            StartCoroutine(Timer(CallZoomOutRight, () =>
            {
                AnimationEnd();
            }, durationTZoomOutt));
#endif
        }
        else if (animationList == AnimationList.ZoomOutUp) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTZoomOutt;
                ZoomOutUp(durationTZoomOutt, previewValue, initSizeZoomOut, maxSizeZoomOut, heightZoomOutt, divideZoomOutt, _initPos);
            }
            else {
                StartCoroutine(Timer(CallZoomOutUp, () => {
                    AnimationEnd();
                }, durationTZoomOutt));
            }
#else
            StartCoroutine(Timer(CallZoomOutUp, () =>
            {
                AnimationEnd();
            }, durationTZoomOutt));
#endif
        }
        else if (animationList == AnimationList.SlideInDown) // Slide
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlide;
                FadeSlideInDown(durationTSlide, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideInDown, () => {
                    AnimationEnd();
                }, durationTSlide));
            }
#else
            StartCoroutine(Timer(CallSlideInDown, () =>
            {
                AnimationEnd();
            }, durationTSlide));
#endif
        }
        else if (animationList == AnimationList.SlideInLeft) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlide;
                FadeSlideInLeft(durationTSlide, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideInLeft, () => {
                    AnimationEnd();
                }, durationTSlide));
            }
#else
            StartCoroutine(Timer(CallSlideInLeft, () =>
            {
                AnimationEnd();
            }, durationTSlide));
#endif
        }
        else if (animationList == AnimationList.SlideInRight) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlide;
                FadeSlideInRight(durationTSlide, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideInRight, () => {
                    AnimationEnd();
                }, durationTSlide));
            }
#else
            StartCoroutine(Timer(CallSlideInRight, () =>
            {
                AnimationEnd();
            }, durationTSlide));
#endif
        }
        else if (animationList == AnimationList.SlideInUp) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlide;
                FadeSlideInUp(durationTSlide, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideInUp, () => {
                    AnimationEnd();
                }, durationTSlide));
            }
#else
            StartCoroutine(Timer(CallSlideInUp, () =>
            {
                AnimationEnd();
            }, durationTSlide));
#endif
        }
        else if (animationList == AnimationList.SlideOut) // Slide Out
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlideOut;
                FadeOut(durationTSlideOut, previewValue);
            }
            else {
                StartCoroutine(Timer(CallFadeOut, () => {
                    AnimationEnd();
                }, durationTSlideOut));
            }
#else
            StartCoroutine(Timer(CallFadeOut, () =>
            {
                AnimationEnd();
            }, durationTSlideOut));
#endif
        }
        else if (animationList == AnimationList.SlideOutDown) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlideOut;
                FadeSlideOutDown(durationTSlideOut, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideOutDown, () => {
                    AnimationEnd();
                }, durationTSlideOut));
            }
#else
            StartCoroutine(Timer(CallSlideOutDown, () =>
                {
                    AnimationEnd();
                }, durationTSlideOut));
#endif
        }
        else if (animationList == AnimationList.SlideOutLeft) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlideOut;
                FadeSlideOutLeft(durationTSlideOut, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideOutLeft, () => {
                    AnimationEnd();
                }, durationTSlideOut));
            }
#else
            StartCoroutine(Timer(CallSlideOutLeft, () =>
            {
                AnimationEnd();
            }, durationTSlideOut));
#endif
        }
        else if (animationList == AnimationList.SlideOutRight) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlideOut;
                FadeSlideOutRight(durationTSlideOut, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideOutRight, () => {
                    AnimationEnd();
                }, durationTSlideOut));
            }
#else
            StartCoroutine(Timer(CallSlideOutRight, () =>
            {
                AnimationEnd();
            }, durationTSlideOut));
#endif
        }
        else if (animationList == AnimationList.SlideOutUp) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTSlideOut;
                FadeSlideOutUp(durationTSlideOut, previewValue, heightSlide, speedSlide, false, _initPos);
            }
            else {
                StartCoroutine(Timer(CallSlideOutUp, () => {
                    AnimationEnd();
                }, durationTSlideOut));
            }
#else
            StartCoroutine(Timer(CallSlideOutUp, () =>
            {
                AnimationEnd();
            }, durationTSlideOut));
#endif
        }
        else if (animationList == AnimationList.FadeIn) // Fade In
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeIn;
                FadeIn(durationTFadeIn, previewValue);
            }
            else {
                StartCoroutine(Timer(CallFadeIn, () => {
                    AnimationEnd();
                }, durationTFadeIn));
            }
#else
            StartCoroutine(Timer(CallFadeIn, () =>
            {
                AnimationEnd();
            }, durationTFadeIn));
#endif
        }
        else if (animationList == AnimationList.FadeInDown) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeIn;
                FadeSlideInDown(durationTFadeIn, previewValue, heightFadeIn, speedFadeIn, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeInDown, () => { AnimationEnd(); }, durationTFadeIn));
            }
#else
            StartCoroutine(Timer(CallFadeInDown, () => { AnimationEnd(); }, durationTFadeIn));
#endif
        }
        else if (animationList == AnimationList.FadeInLeft) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeIn;
                FadeSlideInLeft(durationTFadeIn, previewValue, heightFadeIn, speedFadeIn, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeInLeft, () => { AnimationEnd(); }, durationTFadeIn));
            }
#else
            StartCoroutine(Timer(CallFadeInLeft, () => { AnimationEnd(); }, durationTFadeIn));
#endif
        }
        else if (animationList == AnimationList.FadeInRight) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeIn;
                FadeSlideInRight(durationTFadeIn, previewValue, heightFadeIn, speedFadeIn, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeInRight, () => { AnimationEnd(); }, durationTFadeIn));
            }
#else
            StartCoroutine(Timer(CallFadeInRight, () => { AnimationEnd(); }, durationTFadeIn));
#endif
        }
        else if (animationList == AnimationList.FadeInUp) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeIn;
                FadeSlideInUp(durationTFadeIn, previewValue, heightFadeIn, speedFadeIn, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeInUp, () => { AnimationEnd(); }, durationTFadeIn));
            }
#else
            StartCoroutine(Timer(CallFadeInUp, () => { AnimationEnd(); }, durationTFadeIn));
#endif
        }
        else if (animationList == AnimationList.FadeOut) // Fade Out
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                this.transform.localPosition = _initPos;
                previewMaxValue = durationTFadeOut;
                FadeOut(durationTFadeOut, previewValue);
            }
            else {
                StartCoroutine(Timer(CallFadeOut, () => {
                    AnimationEnd();
                }, durationTSlide));
            }
#else
            StartCoroutine(Timer(CallFadeOut, () =>
            {
                AnimationEnd();
            }, durationTSlide));
#endif
        }
        else if (animationList == AnimationList.FadeOutDown) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeOut;
                FadeSlideOutDown(durationTFadeOut, previewValue, heightFadeOut, speedFadeOut, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeOutDown, () => {
                    AnimationEnd();
                }, durationTFadeOut));
            }
#else
            StartCoroutine(Timer(CallFadeOutDown, () =>
            {
                AnimationEnd();
            }, durationTFadeOut));
#endif
        }
        else if (animationList == AnimationList.FadeOutLeft) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeOut;
                FadeSlideOutLeft(durationTFadeOut, previewValue, heightFadeOut, speedFadeOut, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeOutLeft, () => {
                    AnimationEnd();
                }, durationTFadeOut));
            }
#else
            StartCoroutine(Timer(CallFadeOutLeft, () =>
            {
                AnimationEnd();
            }, durationTFadeOut));
#endif
        }
        else if (animationList == AnimationList.FadeOutRight) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeOut;
                FadeSlideOutRight(durationTFadeOut, previewValue, heightFadeOut, speedFadeOut, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeOutRight, () => {
                    AnimationEnd();
                }, durationTFadeOut));
            }
#else
            StartCoroutine(Timer(CallFadeOutRight, () =>
            {
                AnimationEnd();
            }, durationTFadeOut));
#endif
        }
        else if (animationList == AnimationList.FadeOutUp) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                previewMaxValue = durationTFadeOut;
                FadeSlideOutUp(durationTFadeOut, previewValue, heightFadeOut, speedFadeOut, true, _initPos);
            }
            else {
                StartCoroutine(Timer(CallFadeOutUp, () => {
                    AnimationEnd();
                }, durationTFadeOut));
            }
#else
            StartCoroutine(Timer(CallFadeOutUp, () =>
            {
                AnimationEnd();
            }, durationTFadeOut));
#endif
        }
        else if (animationList == AnimationList.FlipLeft) // Flip Left
        {
            startRotation = transform.eulerAngles.y;
            endRotation = startRotation + 360.0f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallFlipLeft, () => {
                    AnimationEnd();
                }, durationTFlip));
            }
#else
            StartCoroutine(Timer(CallFlipLeft, () =>
            {
                AnimationEnd();
            }, durationTFlip));
#endif
        }
        else if (animationList == AnimationList.FlipRight) // Flip Right
        {
            startRotation = transform.eulerAngles.y;
            endRotation = startRotation + 360.0f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallFlipRight, () => {
                    AnimationEnd();
                }, durationTFlip));
            }
#else
            StartCoroutine(Timer(CallFlipRight, () =>
            {
                AnimationEnd();
            }, durationTFlip));
#endif
        }
        else if (animationList == AnimationList.FlipInX) {
            startRotation = transform.eulerAngles.x;
            endRotation = startRotation + 90.0f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallFlipInX, () => {
                    AnimationEnd();
                }, durationTFlip));
            }
#else
            StartCoroutine(Timer(CallFlipInX, () =>
            {
                AnimationEnd();
            }, durationTFlip));
#endif
        }
        else if (animationList == AnimationList.FlipInY) {
            startRotation = transform.eulerAngles.y;
            endRotation = startRotation + 90.0f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallFlipInY, () => {
                    AnimationEnd();
                }, durationTFlip));
            }
#else
            StartCoroutine(Timer(CallFlipInY, () =>
            {
                AnimationEnd();
            }, durationTFlip));
#endif
        }
        else if (animationList == AnimationList.FlipOutX) {
            startRotation = transform.eulerAngles.x;
            endRotation = startRotation + 90.0f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallFlipOutX, () => {
                    AnimationEnd();
                }, durationTFlip));
            }
#else
            StartCoroutine(Timer(CallFlipOutX, () =>
            {
                AnimationEnd();
            }, durationTFlip));
#endif
        }
        else if (animationList == AnimationList.FlipOutY) {
            startRotation = transform.eulerAngles.y;
            endRotation = startRotation + 90.0f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallFlipOutY, () => {
                    AnimationEnd();
                }, durationTFlip));
            }
#else
            StartCoroutine(Timer(CallFlipOutY, () =>
            {
                AnimationEnd();
            }, durationTFlip));
#endif
        }
        else if (animationList == AnimationList.RollIn) // Roll In
        {
            startRotation = transform.eulerAngles.z;
            endRotation = startRotation + 150f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallRollIn, () => {
                    AnimationEnd();
                }, durationTRoll));
            }
#else
            StartCoroutine(Timer(CallRollIn, () =>
            {
                AnimationEnd();
            }, durationTRoll));
#endif
        }
        else if (animationList == AnimationList.RollOut) // Roll Out
        {
            startRotation = transform.eulerAngles.z;
            endRotation = startRotation + 150f;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {

            }
            else {
                StartCoroutine(Timer(CallRollOut, () => {
                    AnimationEnd();
                }, durationTRoll));
            }
#else
            StartCoroutine(Timer(CallRollOut, () =>
            {
                AnimationEnd();
            }, durationTRoll));
#endif
        }

    }

    public void TypingNext() {
        if (animationStatus == AnimationStatus.stopped) {
            if (_index < words.Length - 1) {
                _index++;
                StartCoroutine(TypingEffect());
            }
            else if (loopWords == true) {
                _index = 0;
                StartCoroutine(TypingEffect());
            }
        }
    }

    public void SetInitPos() {
        _initPos = this.transform.localPosition;
    }

    public void LogAUI(string nameAni, bool running) {
        if (log == true) {
            if (running)
                Debug.Log(string.Format("WS-AUIPRO_Log: {0} executed {1}", name, nameAni));
            else
                Debug.Log(string.Format("WS-AUIPRO_Log: {0} is running {1}", name, nameAni));
        }
    }
    #endregion
}
