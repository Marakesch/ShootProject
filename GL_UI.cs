using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GL_UI : MonoBehaviour {
    #region Variables
    Loop Loop_UpdaterUI;
    bool b_paused = false;    
    public GameObject text;
    [HideInInspector]
    public GameObject button; 

    public static GL_UI S;
    public UICanvas MAIN = new UICanvas();
    public UICanvas PAUSE = new UICanvas();    
    public UICanvas LOSE = new UICanvas();
    
    Camera cam;
    public GameObject go_toggle, go_rockets, go_boom, go_traj;
    List<GameObject> bonus_buttons = new List<GameObject>();
    #endregion    

    #region Routine Methods
    private void Awake()
    {
        if (S == null)
            S = this;
        else
            DestroyImmediate(gameObject);
    }
    void Start()
    {        
        cam = FindObjectOfType<Camera>();
        MAIN.cnvs.enabled = true;
        PAUSE.cnvs.enabled = false;
        LOSE.cnvs.enabled = false;        

        bonus_buttons.Add(go_rockets);
        bonus_buttons.Add(go_boom);
        bonus_buttons.Add(go_traj);
        DisableButtons(bonus_buttons);

        Loop_UpdaterUI += S.UpdtMain;        
    }
    void Update()
    {
        Loop_UpdaterUI();
    }
    #endregion

    #region Main Methods    
    public void PauseGame()
    {
        switch (b_paused)
        {
            case true:
                {
                    Time.timeScale = 1;
                    MAIN.cnvs.enabled = true;
                    PAUSE.cnvs.enabled = false;
                    b_paused = false;
                }
                break;
            case false:
                {
                    Time.timeScale = 0;
                    PAUSE.cnvs.enabled = true;
                    MAIN.cnvs.enabled = false;
                    b_paused = true;
                }
                break;
        }
    }
    public void LoseGame()
    {
        MAIN.cnvs.enabled = false;
        LOSE.cnvs.enabled = true;
        LOSE.text_score.text = "YOUR SCORE IS " + GL_8TYMngr.S.GetScore().ToString("F0");
    }
    void UpdtMain()
    {
        UpdtTime(MAIN);
        MAIN.text_bullets.text = GL_CannonMngr.S.GetWeaponUI();
        MAIN.text_score.text = "SCORE: " + GL_8TYMngr.S.GetScore().ToString();
    }
    void UpdtEvntUI()
    {
        MAIN.text_score.text = "SCORE: " + EVNT_8TYMngr.S.GetScore().ToString();
        UpdtTime(MAIN);
    }
    public void UpdtTime(UICanvas cnvs)
    {
        float time = GL_8TYMngr.S.GetTime();        
        Animator anim = cnvs.text_time.GetComponent<Animator>();
        if (time > 0)
        {
            cnvs.text_time.text = "TIME: " + time.ToString("F0");
        }
        else
        {
            cnvs.text_time.text = "TIME: 0";
        }
        if (time < 5)
        {
            anim.SetTrigger("Ending");
            return;
        }
        if (time > 5)
        {
            anim.SetTrigger("Pause");
        }
    }
    #endregion

    #region Event Methods
    public void StartEvent()
    {        
        Loop_UpdaterUI += S.UpdtEvntUI;
        Loop_UpdaterUI -= S.UpdtMain;
    }
    public void EndEvent()
    {        
        StartCoroutine(ShowText("YOU GOT " + EVNT_8TYMngr.S.GetScore().ToString("F0") + "POINTS!", 2f, MAIN));
        Loop_UpdaterUI -= S.UpdtEvntUI;
        Loop_UpdaterUI += S.UpdtMain;        
    }    
    public void PreStart()
    {            
        StartCoroutine(FlickeringText("IT IS COMING!!!", MAIN, 3f));
    }   
    #endregion

    #region Assisst Methods    
    IEnumerator FlickeringText(string str, UICanvas canvs, float time)
    {        
        GameObject go_new = Instantiate(text);
        Text text_show = go_new.GetComponent<Text>();
        go_new.transform.SetParent(canvs.cnvs.transform);
        text_show.rectTransform.localPosition = Vector3.zero;
        text_show.fontSize = 35;
        text_show.text = str;              
        bool plusminus = true;
        for (float i = 0; i < (time*2); i++)
        {
            if (plusminus)
            {
                text_show.enabled = true;
                plusminus = false;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                text_show.enabled = false;
                plusminus = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
        Destroy(go_new);        
    }
    public IEnumerator ShowText(string s_show, float time, UICanvas cnvs)
    {
        GameObject go_new = Instantiate(text);
        Text text_show = go_new.GetComponent<Text>();
        text_show.transform.SetParent(cnvs.cnvs.transform);
        text_show.rectTransform.localPosition = Vector3.zero;
        text_show.fontSize = 30;
        text_show.rectTransform.localScale = Vector3.zero;
        text_show.text = s_show;
        text_show.rectTransform.DOShakeAnchorPos(time, 10, 9);
        text_show.rectTransform.DOScale(2, time).SetEase(Ease.OutElastic).SetLoops(1, LoopType.Yoyo);
        text_show.rectTransform.DOPlay();
        yield return new WaitForSeconds(time);
        Destroy(go_new);
    }
    public IEnumerator ShowTextByTransform(Transform trans, string str, float time)
    {
        GameObject go_new = Instantiate(text);
        Text text_show = go_new.GetComponent<Text>();
        text_show.rectTransform.anchorMax = Vector2.zero;
        text_show.rectTransform.anchorMin = Vector2.zero;
        text_show.fontSize = 30;
        text_show.text = str;
        text_show.transform.SetParent(MAIN.cnvs.transform);
        Vector3 v3_psn = trans.position;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            Vector3 v3_scrn = cam.WorldToScreenPoint(v3_psn);
            text_show.rectTransform.anchoredPosition = new Vector2(v3_scrn.x, v3_scrn.y);
            yield return null;
        }
        Destroy(go_new);
    }
    public IEnumerator CameraShake(float time)
    {
        cam.transform.DOShakePosition(time, new Vector3(1.5f, 0.5f, 0), 7, 60).Play();
        yield return new WaitForSeconds(time);
    }
    #endregion

    #region Button Methods
    public void ToggleOpen()
    {
        EnableButtons(bonus_buttons);                
        RectTransform rectTr = go_rockets.GetComponent<RectTransform>();        
        rectTr.DOAnchorMin(new Vector2(0.9f, 0.8f), 0.1f).SetEase(Ease.Linear).Play();
        rectTr.DOAnchorMax(new Vector2(0.9f, 0.8f), 0.1f).SetEase(Ease.Linear).Play();
        rectTr = go_traj.GetComponent<RectTransform>();
        rectTr.DOAnchorMin(new Vector2(0.9f, 0.4f), 0.1f).SetEase(Ease.Linear).Play();
        rectTr.DOAnchorMax(new Vector2(0.9f, 0.4f), 0.1f).SetEase(Ease.Linear).Play();
        //Updater += CheckTap;
    }
    public void ToggleClose()
    {
        DisableButtons(bonus_buttons);
        foreach(GameObject go in bonus_buttons)
        {
            go.GetComponent<RectTransform>().anchorMax = new Vector2(0.9f, 0.6f);
            go.GetComponent<RectTransform>().anchorMin = new Vector2(0.9f, 0.6f);
        }
        //Updater -= CheckTap;
    }
    void SwitchOffButton(GameObject go)
    {
        go.GetComponent<Button>().enabled = false;        
        Text[] txt =  go.GetComponentsInChildren<Text>();
        foreach(Text text in txt)
        {
            text.enabled = false;
        }
        go.GetComponent<Image>().enabled = false;
    }
    void SwitchOnButton(GameObject go)
    {
        go.GetComponent<Button>().enabled = true;
        Text[] txt = go.GetComponentsInChildren<Text>();
        foreach (Text text in txt)
        {
            text.enabled = true;
        }
        go.GetComponent<Image>().enabled = true;
    }
    void EnableButtons(List<GameObject> list_go)
    {
        foreach(GameObject go in list_go)
        {
            SwitchOnButton(go);
        }
    }
    void DisableButtons(List<GameObject> list_go)
    {
        foreach (GameObject go in list_go)
        {
            SwitchOffButton(go);
        }
    }
    void CheckTap()
    {
        if(Input.touchCount > 0)
        {
            Touch tch = Input.GetTouch(0);
            
            ToggleClose();
        }
    }
    public void ButtonResponse()
    {
        print("Respond");
    }
    #endregion

    #region Underwork
    public Button CreateButton(string s_title, UICanvas canv)
    {       
        GameObject go_new = Instantiate(button);
        go_new.transform.SetParent(canv.cnvs.transform);
        Button new_button = go_new.GetComponent<Button>();
        Text txt = go_new.GetComponentInChildren<Text>();
        txt.text = s_title;
        return new_button;    
    }
    #endregion
}

#region Private classes
[System.Serializable]
public class UICanvas
{
    public Canvas cnvs;
    public Text text_score;
    public Text text_time;
    public Text text_multi;
    public Text text_bullets;
}
#endregion
