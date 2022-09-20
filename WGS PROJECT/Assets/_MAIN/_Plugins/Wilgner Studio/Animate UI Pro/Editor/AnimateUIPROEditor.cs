using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AnimateUIPRO))]
public class AnimateUIPROEditor : Editor
{
    private AnimateUIPRO animateUIPRO;
    private SerializedObject script;

    // VARIABLES Global
    public SerializedProperty resetAfterEndingAnimation;
    public SerializedProperty timeToStart;
    public SerializedProperty onStart;
    public SerializedProperty animationInLoop;
    public SerializedProperty animationList;
    public SerializedProperty animationStatus;
    public SerializedProperty afterAnimationIsOver;

    public SerializedProperty isPreview;
    public SerializedProperty previewValue;

    // Bounce Variables
    public SerializedProperty durationTBounce, yHeightBounce;

    // Pulse Variables
    public SerializedProperty durationTPulse, smoothPulse, minScalePulse, maxScalePulse;

    // Flash Variables
    public SerializedProperty durationTFlash;

    // Shake X Variables
    public SerializedProperty durationTShakeX;
    public SerializedProperty xDistanceShakeX;

    // Shake Y Variables
    public SerializedProperty durationTShakeY;
    public SerializedProperty yDistanceShakeY;

    // Rpg Typing Variables
    public SerializedProperty words;
    ReorderableList words2;
    public SerializedProperty loopWords;
    public SerializedProperty typingSpeed;

    // Zoom In Variables
    public SerializedProperty durationTZoomIn;
    public SerializedProperty initSizeZoomIn;
    public SerializedProperty maxSizeZoomIn;

    // Zoom Out Variables
    public SerializedProperty durationTZoomOut;
    public SerializedProperty initSizeZoomOut;
    public SerializedProperty maxSizeZoomOut;

    // Zoom In Others Variables
    public SerializedProperty durationTZoom;
    public SerializedProperty initSizeZoom;
    public SerializedProperty maxSizeZoom;
    public SerializedProperty divideZoom;
    public SerializedProperty heightZoom;

    // Zoom Out Others Variables
    public SerializedProperty durationTZoomOutt;
    public SerializedProperty initSizeZoomOutt;
    public SerializedProperty maxSizeZoomOutt;
    public SerializedProperty divideZoomOutt;
    public SerializedProperty heightZoomOutt;

    // Slide In
    public SerializedProperty durationTSlide;
    public SerializedProperty heightSlide;
    public SerializedProperty speedSlide;

    // Slide Out
    public SerializedProperty durationTSlideOut;
    public SerializedProperty heightSlideOut;
    public SerializedProperty speedSlideOut;

    // Fade In
    public SerializedProperty durationTFadeIn;
    public SerializedProperty heightFadeIn;
    public SerializedProperty speedFadeIn;

    // Fade Out
    public SerializedProperty durationTFadeOut;
    public SerializedProperty heightFadeOut;
    public SerializedProperty speedFadeOut;

    // Flip 
    public SerializedProperty durationTFlip;
    public SerializedProperty leftFlip;

    // Roll In/Out 
    public SerializedProperty durationTRoll;
    public SerializedProperty xdistanceRoll;

    // Textures
    Texture logo;

    public void OnEnable()
    {
        animateUIPRO = (AnimateUIPRO)target;
        script = new SerializedObject(target);
        logo = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Wilgner Studio/Animate UI Pro/Editor/Images/AnimateUILogo.png", typeof(Texture));

        // Objects Settings
        resetAfterEndingAnimation = script.FindProperty("resetAfterEndingAnimation");
        timeToStart = script.FindProperty("timeToStart");
        onStart = script.FindProperty("onStart");
        animationInLoop = script.FindProperty("animationInLoop");
        animationList = script.FindProperty("animationList");
        animationStatus = script.FindProperty("animationStatus");
        afterAnimationIsOver = script.FindProperty("afterAnimationIsOver");

        isPreview = script.FindProperty("isPreview");
        previewValue = script.FindProperty("previewValue");

        // Bounce Settings
        durationTBounce = script.FindProperty("durationTBounce");
        yHeightBounce = script.FindProperty("yHeightBounce");

        // Pulse Settings
        durationTPulse = script.FindProperty("durationTPulse");
        smoothPulse = script.FindProperty("smoothPulse");
        minScalePulse = script.FindProperty("minScalePulse");
        maxScalePulse = script.FindProperty("maxScalePulse");

        // Flash Settings
        durationTFlash = script.FindProperty("durationTFlash");

        // Shake X Settings
        durationTShakeX = script.FindProperty("durationTShakeX");
        xDistanceShakeX = script.FindProperty("xDistanceShakeX");

        // Shake Y Settings
        durationTShakeY = script.FindProperty("durationTShakeY");
        yDistanceShakeY = script.FindProperty("yDistanceShakeY");

        // Rpg Typing Effect Settings
        words = script.FindProperty("words");
        this.words2 = new ReorderableList(script, words);
        loopWords = script.FindProperty("loopWords");
        typingSpeed = script.FindProperty("typingSpeed");

        // Zoom In Settings
        durationTZoomIn = script.FindProperty("durationTZoomIn");
        initSizeZoomIn = script.FindProperty("initSizeZoomIn");
        maxSizeZoomIn = script.FindProperty("maxSizeZoomIn");

        // Zoom Out Settings
        durationTZoomOut = script.FindProperty("durationTZoomOut");
        initSizeZoomOut = script.FindProperty("initSizeZoomOut");
        maxSizeZoomOut = script.FindProperty("maxSizeZoomOut");

        // Zoom In Others Settings
        durationTZoom = script.FindProperty("durationTZoom");
        initSizeZoom = script.FindProperty("initSizeZoom");
        maxSizeZoom = script.FindProperty("maxSizeZoom");
        divideZoom = script.FindProperty("divideZoom");
        heightZoom = script.FindProperty("heightZoom");

        // Zoom Out Others Settings
        durationTZoomOutt = script.FindProperty("durationTZoomOutt");
        initSizeZoomOutt = script.FindProperty("initSizeZoomOutt");
        maxSizeZoomOutt = script.FindProperty("maxSizeZoomOutt");
        divideZoomOutt = script.FindProperty("divideZoomOutt");
        heightZoomOutt = script.FindProperty("heightZoomOutt");

        // Slide In
        durationTSlide = script.FindProperty("durationTSlide");
        heightSlide = script.FindProperty("heightSlide");
        speedSlide = script.FindProperty("speedSlide");

        // Slide Out
        durationTSlideOut = script.FindProperty("durationTSlideOut");
        heightSlideOut = script.FindProperty("heightSlideOut");
        speedSlideOut = script.FindProperty("speedSlideOut");

        // Fade In
        durationTFadeIn = script.FindProperty("durationTFadeIn");
        heightFadeIn = script.FindProperty("heightFadeIn");
        speedFadeIn = script.FindProperty("speedFadeIn");

        // Fade Out
        durationTFadeOut = script.FindProperty("durationTFadeOut");
        heightFadeOut = script.FindProperty("heightFadeOut");
        speedFadeOut = script.FindProperty("speedFadeOut");

        // Flip
        durationTFlip = script.FindProperty("durationTFlip");
        leftFlip = script.FindProperty("leftFlip");

        // Roll
        durationTRoll = script.FindProperty("durationTRoll");
        xdistanceRoll = script.FindProperty("xdistanceRoll");


        this.words2.drawElementCallback = RectNewWords;
        this.words2.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Words");
        };

#if WS_AUIP
        //Debug.Log("Animate UI Pro EXITS!");
#endif

    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector ();

        #region LOGO
        EditorGUILayout.BeginHorizontal();
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.Label(logo, style, GUILayout.Height(50));
        EditorGUILayout.EndHorizontal();
        #endregion

        script.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical("box");
        animateUIPRO.selectedTab = GUILayout.Toolbar(animateUIPRO.selectedTab, new string[] { "General", animateUIPRO.animationList + " Properties", "Preview" }, GUILayout.Height(30));

        EditorGUILayout.BeginVertical("box");
        switch (animateUIPRO.selectedTab)
        {
            case 0:
                GeralGUI();
                break;
            case 1:
                PropertiesGUI();
                break;
            case 2:
                PreviewGUI();
                break;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        /*
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.HelpBox("Note: Settings for a category do not affect settings for other categories. Unique animations (miscellaneous) have their own " +
            "configurable variables", MessageType.Warning);
        EditorGUILayout.HelpBox("Please leave your review and rating, thanks =)", MessageType.Info);
        EditorGUILayout.HelpBox("Suggestions, query or other reasons, please contact: wilgnerstudio.com =)", MessageType.Info);
        */

        if (EditorGUI.EndChangeCheck())
        {
            script.ApplyModifiedProperties();
            if (animateUIPRO._canvasGroup == null)
                animateUIPRO._canvasGroup = animateUIPRO.GetComponent<CanvasGroup>();

            if (animateUIPRO.isPreview)
                animateUIPRO.ExecuteAnimation();
            //GUI.FocusControl (null);
        }
    }

    void GeralGUI()
    {
        EditorGUILayout.PropertyField(resetAfterEndingAnimation);
        EditorGUILayout.PropertyField(timeToStart);
        EditorGUILayout.PropertyField(onStart);
        EditorGUILayout.PropertyField(animationInLoop);
        EditorGUILayout.PropertyField(animationList);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(animationStatus);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(afterAnimationIsOver);
    }

    void PropertiesGUI()
    {
        if (animateUIPRO.animationList == AnimationList.None)
        {
            EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("No Animation Selected", MessageType.Warning);
        }
        else if (animateUIPRO.animationList == AnimationList.Bounce)
        {
            BounceGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.Pulse)
        {
            PulseGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.Flash)
        {
            FlashGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.ShakeX)
        {
            ShakeXGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.ShakeY)
        {
            ShakeYGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.RPGTypingEffect)
        {
            RPGTypingEffectGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.ZoomIn)
        {
            ZoomInGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.ZoomInDown || animateUIPRO.animationList == AnimationList.ZoomInLeft || animateUIPRO.animationList == AnimationList.ZoomInLeft || animateUIPRO.animationList == AnimationList.ZoomInRight || animateUIPRO.animationList == AnimationList.ZoomInUp)
        {
            ZoomInOthersGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.ZoomOut)
        {
            ZoomOutGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.ZoomOutDown || animateUIPRO.animationList == AnimationList.ZoomOutLeft || animateUIPRO.animationList == AnimationList.ZoomOutLeft || animateUIPRO.animationList == AnimationList.ZoomOutRight || animateUIPRO.animationList == AnimationList.ZoomOutUp)
        {
            ZoomOuttOthersGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.SlideInDown || animateUIPRO.animationList == AnimationList.SlideInLeft || animateUIPRO.animationList == AnimationList.SlideInRight || animateUIPRO.animationList == AnimationList.SlideInUp)
        {
            SlideGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.SlideOut)
        {
            EditorGUILayout.PropertyField(durationTSlideOut);
        }
        else if (animateUIPRO.animationList == AnimationList.SlideOutDown || animateUIPRO.animationList == AnimationList.SlideOutLeft || animateUIPRO.animationList == AnimationList.SlideOutRight || animateUIPRO.animationList == AnimationList.SlideOutUp)
        {
            SlideOutGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.FadeIn)
        {
            EditorGUILayout.PropertyField(durationTFadeIn);
        }
        else if (animateUIPRO.animationList == AnimationList.FadeInDown || animateUIPRO.animationList == AnimationList.FadeInLeft || animateUIPRO.animationList == AnimationList.FadeInRight || animateUIPRO.animationList == AnimationList.FadeInUp)
        {
            FadeInGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.FadeOut)
        {
            EditorGUILayout.PropertyField(durationTFadeOut);
        }
        else if (animateUIPRO.animationList == AnimationList.FadeOutDown || animateUIPRO.animationList == AnimationList.FadeOutLeft || animateUIPRO.animationList == AnimationList.FadeOutRight || animateUIPRO.animationList == AnimationList.FadeOutUp)
        {
            FadeOutGUI();
        }
        else if (animateUIPRO.animationList == AnimationList.FlipLeft || animateUIPRO.animationList == AnimationList.FlipRight) // Flip
        {
            EditorGUILayout.PropertyField(durationTFlip);
            EditorGUILayout.PropertyField(leftFlip);
        }
        else if (animateUIPRO.animationList == AnimationList.FlipInX || animateUIPRO.animationList == AnimationList.FlipInY || animateUIPRO.animationList == AnimationList.FlipOutX || animateUIPRO.animationList == AnimationList.FlipOutY)
        {
            EditorGUILayout.PropertyField(durationTFlip);
        }
        else if (animateUIPRO.animationList == AnimationList.RollIn || animateUIPRO.animationList == AnimationList.RollOut) // Roll In / Out
        {
            EditorGUILayout.PropertyField(durationTRoll);
            EditorGUILayout.PropertyField(xdistanceRoll);
        }
    }

    void PreviewGUI()
    {
        EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
        if (animateUIPRO.animationList != AnimationList.None)
        {
            EditorGUILayout.PropertyField(isPreview);
            EditorGUILayout.Slider(previewValue, 0f, animateUIPRO.previewMaxValue);
        }
        else if (animateUIPRO.animationList == AnimationList.RPGTypingEffect)
        {
            if (GUILayout.Button("Next Word"))
            {
                animateUIPRO.TypingNext();
            }
        }
        if (GUILayout.Button("Set Initial Position"))
        {
            animateUIPRO.SetInitPos();
        }
    }

    void BounceGUI()
    {
        EditorGUILayout.PropertyField(durationTBounce);
        EditorGUILayout.PropertyField(yHeightBounce);
    }

    void PulseGUI()
    {
        EditorGUILayout.PropertyField(durationTPulse);
        EditorGUILayout.PropertyField(minScalePulse);
        EditorGUILayout.PropertyField(maxScalePulse);
        EditorGUILayout.PropertyField(smoothPulse);
    }

    void FlashGUI()
    {
        EditorGUILayout.PropertyField(durationTFlash);
    }

    void ShakeXGUI()
    {
        EditorGUILayout.PropertyField(durationTShakeX);
        EditorGUILayout.PropertyField(xDistanceShakeX);
    }

    void ShakeYGUI()
    {
        EditorGUILayout.PropertyField(durationTShakeY);
        EditorGUILayout.PropertyField(yDistanceShakeY);
    }

    void RPGTypingEffectGUI()
    {
        //EditorGUILayout.PropertyField(words);
        words2.DoLayoutList();
        EditorGUILayout.PropertyField(loopWords);
        EditorGUILayout.PropertyField(typingSpeed);
    }

    void ZoomInGUI()
    {
        EditorGUILayout.PropertyField(durationTZoomIn);
        EditorGUILayout.PropertyField(initSizeZoomIn);
        EditorGUILayout.PropertyField(maxSizeZoomIn);
    }

    void ZoomOutGUI()
    {
        EditorGUILayout.PropertyField(durationTZoomOut);
        EditorGUILayout.PropertyField(initSizeZoomOut);
        EditorGUILayout.PropertyField(maxSizeZoomOut);
    }

    void ZoomInOthersGUI()
    {
        EditorGUILayout.PropertyField(durationTZoom);
        EditorGUILayout.PropertyField(initSizeZoom);
        EditorGUILayout.PropertyField(maxSizeZoom);
        EditorGUILayout.PropertyField(divideZoom);
        EditorGUILayout.PropertyField(heightZoom);
    }

    void ZoomOutOthersGUI()
    {
        EditorGUILayout.PropertyField(durationTZoom);
        EditorGUILayout.PropertyField(initSizeZoom);
        EditorGUILayout.PropertyField(maxSizeZoom);
        EditorGUILayout.PropertyField(divideZoom);
        EditorGUILayout.PropertyField(heightZoom);
    }

    void ZoomOuttOthersGUI()
    {
        EditorGUILayout.PropertyField(durationTZoomOutt);
        EditorGUILayout.PropertyField(initSizeZoomOutt);
        EditorGUILayout.PropertyField(maxSizeZoomOutt);
        EditorGUILayout.PropertyField(divideZoomOutt);
        EditorGUILayout.PropertyField(heightZoomOutt);
    }

    void SlideGUI()
    {
        EditorGUILayout.PropertyField(durationTSlide);
        EditorGUILayout.PropertyField(heightSlide);
        EditorGUILayout.PropertyField(speedSlide);
    }

    void SlideOutGUI()
    {
        EditorGUILayout.PropertyField(durationTSlideOut);
        EditorGUILayout.PropertyField(heightSlideOut);
        EditorGUILayout.PropertyField(speedSlideOut);
    }

    void FadeInGUI()
    {
        EditorGUILayout.PropertyField(durationTFadeIn);
        EditorGUILayout.PropertyField(heightFadeIn);
        EditorGUILayout.PropertyField(speedFadeIn);
    }

    void FadeOutGUI()
    {
        EditorGUILayout.PropertyField(durationTFadeOut);
        EditorGUILayout.PropertyField(heightFadeOut);
        EditorGUILayout.PropertyField(speedFadeOut);
    }

    void GuiLine(int i_height = 1)
    {

        Rect rect = EditorGUILayout.GetControlRect(false, i_height);

        rect.height = i_height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

    }

    private void RectNewWords(Rect rect, int index, bool active, bool focus)
    {
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 16), words.GetArrayElementAtIndex(index));
    }

}
