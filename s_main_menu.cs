using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class s_main_menu : MonoBehaviour {
    public Canvas canvas_main;
    public Canvas canvas_lvls;
	// Use this for initialization
	void Start () {
        canvas_lvls.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadLevel(string s_level_name)
    {
        SceneManager.LoadScene(s_level_name);
    }
    public void Switch()
    {
        if (canvas_main.enabled == true)
        {
            canvas_main.enabled = false;
            canvas_lvls.enabled = true;
        }
        else
        {
            canvas_lvls.enabled = false;
            canvas_main.enabled = true;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
