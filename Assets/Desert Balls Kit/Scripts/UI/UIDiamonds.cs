using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// fill the text with the number of diamonds
public class UIDiamonds : MonoBehaviour
{
    Text diamonds;

    
    void Start()
    {
        diamonds = GetComponent<Text>();
        diamonds.text = GameSettings.getDiamonds().ToString();
    }

    void FixedUpdate()
    {
        diamonds.text = GameSettings.getDiamonds().ToString();
    }
}
