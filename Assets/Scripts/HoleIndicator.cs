using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleIndicator : MonoBehaviour
{
    public float pulsatingSpeed = 5.7f;

    SpriteRenderer sr;
    Color colorRef;

    IEnumerator pulsatingCoroutineReference;

    public Text holeNumberText;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        colorRef = sr.color;
        pulsatingCoroutineReference = Pulsate();
    }

    IEnumerator Pulsate()
    {
        while (true)
        {
            colorRef.a = (Mathf.Cos(Time.time * pulsatingSpeed) * 0.5f + 0.5f) * 0.4f;
            sr.color = colorRef;
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartPulsating()
    {
        StartCoroutine(pulsatingCoroutineReference);
    }

    public void EndPulsating()
    {
        StopCoroutine(pulsatingCoroutineReference);
        colorRef.a = 0;
        sr.color = colorRef;
    }

    public void SetHoleNumber(int holeNumber)
    {
        holeNumberText.text = holeNumber.ToString();
    }

    void Update()
    {
        
    }
}
