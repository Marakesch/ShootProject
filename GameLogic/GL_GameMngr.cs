using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GL_GameMngr : MonoBehaviour {
    bool b_pause = false;
    public string s_mainmenu = "MainMenu";
    string s_name_level;
    Scene current;    
    Loop GamePauser;
    GL_GameMngr S;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {            
            DestroyImmediate(gameObject);
            return;
        }        
    }
    void Start () {
        current = SceneManager.GetActiveScene();
        s_name_level = current.name;
        GamePauser += S.PauseGame;        
        GamePauser += GL_UI.S.PauseGame;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            LoadLevel(s_name_level);
        if (Input.GetKeyDown(KeyCode.Escape))
            GamePauser();
    }
    public void LoadLevel(string s_level_name)
    {
        SceneManager.LoadScene(s_level_name);
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        switch (b_pause)
        {
            case true:
                {
                    Time.timeScale = 1;                    
                    b_pause = false;
                }
                break;
            case false:
                {
                    Time.timeScale = 0;                    
                    b_pause = true;
                }
                break;
        }
    }
}
