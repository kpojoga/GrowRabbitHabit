using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// custom switch
public class ToggleController : MonoBehaviour
{
    public bool isOn;

    public Color onColorBg;
    public Color offColorBg;

    public Image toggleBgImage;
    public RectTransform toggle;

    public GameObject handle;
    private RectTransform handleTransform;

    private float handleSize;
    private float onPosX;
    private float offPosX;

    public float handleOffset;

    public GameObject onIcon;
    public GameObject offIcon;


    public float speed;
    private float t = 0.0f;

    private bool switching = false;

    [System.Serializable]
    public class OnToggle : UnityEvent<bool> { };
    public OnToggle onToggle = null;


    void Awake()
    {
        handleTransform = handle.GetComponent<RectTransform>();
        handleSize = handleTransform.sizeDelta.x;
        float toggleSizeX = toggle.sizeDelta.x;
        onPosX = (toggleSizeX / 2) - (handleSize / 2) - handleOffset;
        offPosX = onPosX * -1;
    }


    void Start()
    {
        if (isOn)
        {
            toggleBgImage.color = onColorBg;
            handleTransform.localPosition = new Vector3(onPosX, handleTransform.localPosition.y, 0f);
            onIcon.gameObject.SetActive(true);
            offIcon.gameObject.SetActive(false);
        }
        else
        {
            toggleBgImage.color = offColorBg;
            handleTransform.localPosition = new Vector3(offPosX, handleTransform.localPosition.y, 0f);
            onIcon.gameObject.SetActive(false);
            offIcon.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (switching)
        {
            Toggle(isOn);
        }
    }

    public void DoYourStaff()
    {
        onToggle.Invoke(isOn);
    }

    public void Switching()
    {
        switching = true;
    }



    public void Toggle(bool toggleStatus)
    {
        if (!onIcon.activeSelf || !offIcon.activeSelf)
        {
            onIcon.SetActive(true);
            offIcon.SetActive(true);
        }

        if (toggleStatus)
        {
            toggleBgImage.color = SmoothColor(onColorBg, offColorBg);
            Transparency(onIcon, 1f, 0f);
            Transparency(offIcon, 0f, 1f);
            handleTransform.localPosition = SmoothMove(onPosX, offPosX);
        }
        else
        {
            toggleBgImage.color = SmoothColor(offColorBg, onColorBg);
            Transparency(onIcon, 0f, 1f);
            Transparency(offIcon, 1f, 0f);
            handleTransform.localPosition = SmoothMove(offPosX, onPosX);
        }
    }


    Vector3 SmoothMove(float startPosX, float endPosX)
    {
        Vector3 position = new Vector3(Mathf.Lerp(startPosX, endPosX, t += speed * Time.deltaTime), handleTransform.localPosition.y, 0f);
        StopSwitching();
        return position;
    }

    Color SmoothColor(Color startCol, Color endCol)
    {
        return Color.Lerp(startCol, endCol, t += speed * Time.deltaTime);
    }

    CanvasGroup Transparency(GameObject alphaObj, float startAlpha, float endAlpha)
    {
        CanvasGroup alphaVal = alphaObj.gameObject.GetComponent<CanvasGroup>();
        alphaVal.alpha = Mathf.Lerp(startAlpha, endAlpha, t += speed * Time.deltaTime);
        return alphaVal;
    }

    void StopSwitching()
    {
        if (t > 1.0f)
        {
            switching = false;

            t = 0.0f;
            switch (isOn)
            {
                case true:
                    isOn = false;
                    DoYourStaff();
                    break;

                case false:
                    isOn = true;
                    DoYourStaff();
                    break;
            }

        }
    }

}