using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICannonMngr
{
    void AddBullets(int i, int k);
    string GetWeaponUI();
    void AutoFire();
}

public class GL_CannonMngr : MonoBehaviour, ICannonMngr {

    #region Variables
    bool b_is_firing = false;
    bool b_firstview = true;
    public bool b_canControl = true;
    public float f_smooth_rot = 0.5f;
    public float f_mag = 75.5f;
    public List<Bullet> bullets = new List<Bullet>();    
    public Camera camera_maincamera;
    public Transform t_cam_default, t_cam_aim, t_bullet_pos, t_dulo, t_platform;    
    public static GL_CannonMngr S;
    public Animator animator;      
    GameObject go_bullet;       
    int i = 0;
    float f_input_x
    {
        get { return _f_input_x; }
        set { _f_input_x = Mathf.Clamp(value, 0, 40); }
    }
    private float _f_input_x=0;
    float f_intput_y
    {
        get { return _f_input_y; }
        set { _f_input_y = Mathf.Clamp(value, -50, 50); }
    }
    private float _f_input_y = 0;

    #endregion

    #region Routine Methods
    private void Awake()
    {
        if (S == null)
            S = this;
        else
            DestroyImmediate(gameObject);
    }
    void Start () {               
        camera_maincamera = FindObjectOfType<Camera>();
        go_bullet = bullets[0].go_projectile;        
        camera_maincamera.transform.position = t_cam_default.position;
        camera_maincamera.transform.rotation = t_cam_default.rotation;        
    }    	
	void Update () {
        CannonControl();           
    }
    #endregion

    #region Controls
    void SetX(float x)
    {
        t_dulo.localRotation = Quaternion.Euler(new Vector3(-x, t_dulo.localRotation.eulerAngles.y, 0));
    }
    void SetY(float y)
    {
        t_platform.localRotation = Quaternion.Euler(new Vector3(t_platform.localRotation.eulerAngles.x, y, 0));         
    }
    public void Fire()
    {
        if (b_is_firing == false)
        {
            if(bullets[i].i_qty > 0 || bullets[i].i_qty == -1)
            {
                b_is_firing = true;
                Instantiate(go_bullet, t_bullet_pos.position, t_bullet_pos.rotation);
                if(bullets[i].i_qty > 0)
                {
                    bullets[i].i_qty--;
                }
                animator.SetTrigger("Shoot");
                StartCoroutine(CoolDownBullet(bullets[i].f_cool_down));
            }
        }          
    }    
    void CannonControl()
    {
        if (b_canControl == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ChangeWeapon();
            if (Input.GetKeyDown(KeyCode.F))
                b_firstview = !b_firstview;
            UpdateCameraPosition();
            if (Input.GetAxis("Horizontal") != 0)
            {
                SetY(f_intput_y += (Input.GetAxis("Horizontal") * f_smooth_rot * Time.deltaTime));
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                SetX(f_input_x += (Input.GetAxis("Vertical") * f_smooth_rot * Time.deltaTime));
            }
            if (Input.GetAxis("Jump") != 0)
            {
                Fire();
            }            
        }
    }
    public void EasyTouchInput(Vector2 v2_input)
    {
        if (b_canControl == true)
        {
            SetY(f_intput_y += v2_input.x * f_smooth_rot * Time.deltaTime);
            SetX(f_input_x += v2_input.y * f_smooth_rot * Time.deltaTime * -1);
        }
    }
    #endregion

    #region Assist methods
    IEnumerator CoolDownBullet(float f_cool)
    {
        yield return new WaitForSeconds(f_cool);
        b_is_firing = false;
    }
    void ChangeWeapon()
    {
        i++;
        if (i >= bullets.Count)
            i = 0;
        go_bullet = bullets[i].go_projectile;
    }
    void UpdateCameraPosition()
    {
        if (!b_firstview)
        {
            camera_maincamera.transform.position = t_cam_aim.position;
            camera_maincamera.transform.rotation = t_cam_aim.rotation;
        }
        else
        {
            camera_maincamera.transform.position = t_cam_default.position;
            camera_maincamera.transform.rotation = t_cam_default.rotation;
        }
    }    
    public void AutoFire()
    {
        Fire();
    }
    public void SwitchOnTrajectory()
    {
        IBonus trajectory = GetComponent<GL_Trajectory>();
        trajectory.SwitchOn();
    }
    public void SwitchOffTrajectory()
    {
        IBonus trajectory = GetComponent<GL_Trajectory>();
        trajectory.SwitchOff();
    }
    #endregion

    #region Access methods
    public string GetWeaponUI()
    {        
        string s_weapon = bullets[i].s_name + " = " + bullets[i].i_qty.ToString();
        if (bullets[i].i_qty == -1)
        {
            s_weapon = bullets[i].s_name + " безконечны";
        }
        return s_weapon;
    }
    public int GetCurrentBullet()
    {
        return i;
    }
    #endregion

    #region Setter methods
    public void AddBullets(int k, int i_add)
    {
        bullets[k].i_qty += i_add;
    }
    public void SetCurrentWeapon(int k)
    {
        i = k;
        go_bullet = bullets[k].go_projectile;
    }
    #endregion

    #region Data class
    [System.Serializable]
    public class Bullet
    {
        public GameObject go_projectile;
        public int i_qty;
        public float f_cool_down;
        public string s_name;
    }
    #endregion

}
