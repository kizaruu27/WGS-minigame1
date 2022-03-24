using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z6_FadeIn : MonoBehaviour
{
    [Header("Fade In")]
    public Color SplashColor;
    public float StartDelay;
    public float AlphaSpeed;

    [HideInInspector] public float _alpha = 1;
    [HideInInspector] public Texture2D _texture;
    [HideInInspector] public bool _done;
    [HideInInspector] public float _time;
    [HideInInspector] public bool _isfading = false;

    // Start is called before the first frame update
    void Start()
    {
        if (StartDelay > 0)
        {
            _alpha = 1;
            Invoke("ExecFadeIn", StartDelay);
        }
        else
        {
            _isfading = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ExecFadeIn()
    {
        _isfading = true;
    }

    void OnGUI()
    {
        if (_isfading)
        {
            if (_done) return;
            if (_texture == null) _texture = new Texture2D(1, 1);

            _texture.SetPixel(0, 0, new Color(SplashColor.r, SplashColor.g, SplashColor.b, _alpha));
            _texture.Apply();
            _time += Time.deltaTime * AlphaSpeed;
            _alpha -= _time * Time.deltaTime;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
            if (_alpha <= 0) _done = true;
        }
    }

}
