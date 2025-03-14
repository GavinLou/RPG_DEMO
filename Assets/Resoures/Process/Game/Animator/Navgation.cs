using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navgation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject hidescreen;

    //[SerializeField]
    //private PromptManager promptManager;

    public static bool hidemod = false;

    public void HideOn()
    {
        hidescreen.GetComponent<Animator>().SetTrigger("On");
    }
    public void HideOff()
    {
        hidescreen.GetComponent<Animator>().SetTrigger("Off");
    }

    public void onbuttonclick()
    {
        Debug.Log("Buttontest");
        if (hidemod)
        {
            hidemod = false;
            HideOff();
        }
        else 
        {
            hidemod = true;
            HideOn();
        } 
    }
}
