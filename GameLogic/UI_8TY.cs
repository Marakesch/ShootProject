using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;



public class UI_8TY : MonoBehaviour
{

    #region Variables
    Loop Loop_UpdaterUI;


    bool b_paused = false;
    public Canvas canvas_ui;
    public Canvas canvas_pause;
    public Canvas canvas_lose;
    public Canvas canvas_event;
    public Text text_weapon;
    public Text text_hit_distance;
    public Text text_score;
    public Text text_time;
    public Text text_uilose_score;
    public Text text_new_level;
    public Text text_event_points, text_event_time;
    public string s_hit_distance;
    string s_weapon;

    public static UI_8TY S;

    DOTweenAnimation dotween;

    Camera cam;

    #endregion

    #region Routine Methods
    private void Awake()
    {
        S = this;
        dotween = text_new_level.GetComponent<DOTweenAnimation>();
    }
    void Start()
    {
        DOTween.Init();
        Loop_UpdaterUI += S.UpdateUI;
        canvas_pause.enabled = false;
        canvas_lose.enabled = false;
        canvas_ui.enabled = true;
        canvas_event.enabled = false;
        text_hit_distance.enabled = false;
        text_new_level.enabled = false;
        cam = FindObjectOfType<Camera>();
        s_hit_distance = "0";
        text_hit_distance.text = s_hit_distance;
        Loop_UpdaterUI();        
    }    
    void Update()
    {
        Loop_UpdaterUI();
    }
    #endregion

    #region Main Methods
    void UpdateUI()
    {
        s_weapon = GL_CannonMngr.S.GetWeaponUI();
        text_weapon.text = s_weapon;
        if (GL_8TYMngr.S != null)
        {
            text_score.text = "Score = " + GL_8TYMngr.S.GetScore().ToString();
            UpdateTime();
        }
    }
    public void PauseGame()
    {
        switch (b_paused)
        {
            case true:
                {
                    Time.timeScale = 1;
                    canvas_ui.enabled = true;
                    canvas_pause.enabled = false;
                    b_paused = false;
                }
                break;
            case false:
                {
                    Time.timeScale = 0;
                    canvas_pause.enabled = true;
                    canvas_ui.enabled = false;
                    b_paused = true;
                }
                break;
        }
    }    
    public void LoseGame()
    {
        canvas_ui.enabled = false;
        canvas_lose.enabled = true;
        text_uilose_score.text = "Your score is " + GL_8TYMngr.S.GetScore().ToString();
    }  
    #endregion

    #region Event Methods
    public void StartEvent()
    {
        canvas_ui.enabled = false;
        canvas_event.enabled = true;
        Loop_UpdaterUI += S.UpdateEventUi;
        Loop_UpdaterUI -= S.UpdateUI;
    }
    public void EndEvent()
    {
        StartCoroutine(ShowEventPoints(EVNT_8TYMngr.S.GetScore()));
        canvas_ui.enabled = true;
        canvas_event.enabled = false;
        Loop_UpdaterUI -= S.UpdateEventUi;
        Loop_UpdaterUI += S.UpdateUI;
    }
    void UpdateEventUi()
    {
        string time = GL_8TYMngr.S.GetTime().ToString("F0");
        string points = EVNT_8TYMngr.S.GetScore().ToString();
        text_event_points.text = "Score: " + points;
        text_event_time.text = "Time: " + time;
    }
    public void PreStart()
    {
        text_new_level.enabled = true;
        text_new_level.text = "IT IS COMING!!!";
        StartCoroutine(FlickeringText(text_new_level));
    }
    public IEnumerator ShowEventPoints(float score)
    {
        text_new_level.enabled = true;
        text_new_level.text = "YOU GOT " + score.ToString() + " SCORE!";
        dotween.DOPlay();
        yield return new WaitForSeconds(3f);
        dotween.DORewind();
        text_new_level.enabled = false;
    }
    #endregion

    #region Assisst Methods
    public IEnumerator ShowHitDistance(Transform t_target, float f_distance)
    {
        Text text = Instantiate(text_hit_distance);
        text.enabled = true;
        text.text = f_distance.ToString("F0");
        text.transform.parent = canvas_ui.transform;
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            if (t_target != null)
            {
                Vector3 v3_tar_pos = cam.WorldToScreenPoint(t_target.position);
                text.rectTransform.anchoredPosition = new Vector2(v3_tar_pos.x, v3_tar_pos.y + 40);
                yield return null;
            }
            continue;
        }
        Destroy(text.gameObject);
    }
    public IEnumerator ShowPoints(Transform t_target, int i_points, float f_time)
    {
        Text text_points = Instantiate(text_hit_distance);
        text_points.text = "Points+" + i_points.ToString() + " Time+" + f_time.ToString();
        text_points.enabled = true;
        text_points.transform.SetParent(canvas_ui.transform);
        Vector3 v3_position = t_target.position;
        for (float i = 0; i < 1.2f; i += Time.deltaTime)
        {
            Vector3 v3_tar_pos = cam.WorldToScreenPoint(v3_position);
            text_points.rectTransform.anchoredPosition = new Vector2(v3_tar_pos.x, v3_tar_pos.y);
            yield return null;
        }
        Destroy(text_points.gameObject);
    }
    public IEnumerator ShowNewLevel(int level)
    {
        text_new_level.enabled = true;
        text_new_level.text = "ACHIEVED LEVEL " + level.ToString() + "!";
        dotween.DOPlay();
        yield return new WaitForSeconds(3f);
        dotween.DORewind();
        text_new_level.enabled = false;
    }
    IEnumerator FlickeringText(Text text)
    {
        bool plusminus = true;
        for (float i = 0; i < 6; i++)
        {
            if (plusminus)
            {
                text.enabled = true;
                plusminus = false;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                text.enabled = false;
                plusminus = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
        text.enabled = false;
    }
    void UpdateTime()
    {
        float time = GL_8TYMngr.S.GetTime();
        if (time > 0)
        {
            float f_time_rou = Mathf.Round(time);
            text_time.text = "Time: " + f_time_rou.ToString();
        }
        else
        {
            text_time.text = "Time: 0";
        }
        if (time < 5)
        {
            text_time.GetComponent<Animator>().SetTrigger("Ending");
        }
    }
    #endregion

    #region UnderWork

    #endregion
}
