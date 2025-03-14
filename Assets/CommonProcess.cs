using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonProcess : MonoBehaviour
{
    [SerializeField]
    public GameObject splashScreen;

    //[SerializeField]
    //private PromptManager promptManager;

    public void SetSplashScreenTransparency(float alpha)
    {
        splashScreen.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void SplashIn(Action action = null)
    {
        splashScreen.GetComponent<Animator>().SetTrigger("In");
        splashScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void SplashOut()
    {
        splashScreen.GetComponent<Animator>().SetTrigger("Out");
        splashScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void Start()
    {
        
    }
}
