using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("e04");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject main;
    public GameObject inventory;
    public GameObject qrcode;
    public GameObject npc;
    public void main_buttonclick()
    {
        main.SetActive(true);
        inventory.SetActive(false);
        qrcode.SetActive(false);
        npc.SetActive(false);
    }
    public void inventory_buttonclick()
    {
        main.SetActive(false);
        inventory.SetActive(true);
        qrcode.SetActive(false);
        npc.SetActive(false);
    }
    public void qrcode_buttonclick()
    {
        main.SetActive(false);
        inventory.SetActive(false);
        qrcode.SetActive(true);
        npc.SetActive(false);
    }
    public void npc_buttonclick()
    {
        main.SetActive(false);
        inventory.SetActive(false);
        qrcode.SetActive(false);
        npc.SetActive(true);
    }
}
