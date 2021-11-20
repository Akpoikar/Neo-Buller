using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject[] menuItems;
    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
                Pause();
            else 
                UnPause();
        }
    }

    public void Hider(bool flag)
    {
        foreach (var item in menuItems)
        {
            item.SetActive(flag);
        }
    }

    public void Pause()
    {
        pause = true;
        Time.timeScale = 0;
        Hider(true);
    }
    
    public void UnPause()
    {
        pause = false;
        Time.timeScale = 1;
        Hider(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
