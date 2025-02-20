using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonProcess : MonoBehaviour
{
    [SerializeField]
    private GameObject splashScreen;


    //[SerializeField]
    //private PromptManager promptManager;

    public void SetSplashScreenTransparency(float alpha)
    {
        splashScreen.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void SplashIn(Action action = null)
    {
        splashScreen.GetComponent<Animator>().Play("Splash@In");
        splashScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (action != null)
        {
            StartCoroutine(SplashAction(action));
        }
    }

    private IEnumerator SplashAction(Action action)
    {
        yield return new WaitForSeconds(2);
        action.Invoke();
    }

    public void SplashOut()
    {
        splashScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
        splashScreen.GetComponent<Animator>().Play("Splash@Out");
    }
    void Start()
    {
        
    }
}
