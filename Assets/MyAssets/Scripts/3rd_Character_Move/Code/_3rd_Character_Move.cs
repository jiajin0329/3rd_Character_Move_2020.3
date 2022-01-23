using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3rd_Character_Move : MonoBehaviour {
    #region 變數宣告 ===============================================================================================================
    /// <summary>
    /// 是否在地上
    /// </summary>
    [Header("是否在地上")]
    [SerializeField]
    private bool on_ground;
    /// <summary>
    /// 踩到地板數
    /// </summary>
    public bool _on_ground {
        get { return on_ground; }
    }

    /// <summary>
    /// 是否輸入中
    /// </summary>
    [Header("是否輸入中")]
    [SerializeField]
    private bool input = false;

    /// <summary>
    /// 鍵盤輸入量
    /// </summary>
    [Header("鍵盤輸入量")]
    [SerializeField]
    private Vector2 input_keypad;
    /// <summary>
    /// 輸入角度
    /// </summary>
    public Vector2 _input_keypad {
        get { return input_keypad; }
    }

    /// <summary>
    /// 輸入角度
    /// </summary>
    [Header("輸入角度")]
    [SerializeField]
    private float input_angle;
    /// <summary>
    /// 輸入角度
    /// </summary>
    public float _input_angle {
        get { return input_angle; }
    }

    /// <summary>
    /// 是否可以移動
    /// </summary>
    [Header("是否可以移動")]
    public bool can_move = true;

    /// <summary>
    /// 是否有移動量
    /// </summary>
    [Header("是否有移動量")]
    private bool move = false;
    /// <summary>
    /// 是否有移動量
    /// </summary>
    public bool _move {
        get { return move; }
    }

    /// <summary>
    /// 移動量
    /// </summary>
    [Header("移動量")]
    private Vector2 move_pos;
    /// <summary>
    /// 移動量
    /// </summary>
    public Vector2 _move_pos {
        get { return move_pos; }
    }

    /// <summary>
    /// 放大完移動量
    /// </summary>
    [Header("放大完移動量")]
    private Vector2 move_pos_sum;
    /// <summary>
    /// 放大完移動量
    /// </summary>
    public Vector2 _move_pos_sum {
        get { return move_pos_sum; }
    }

    /// <summary>
    /// 是否走路中
    /// </summary>
    [Header("是否走路中")]
    [SerializeField]
    private bool walk = false;

    /// <summary>
    /// 移動角度
    /// </summary>
    [Header("移動角度")]
    [SerializeField]
    private float move_angle;
    /// <summary>
    /// 移動角度
    /// </summary>
    public float _move_angle {
        get { return move_angle; }
    }

    /// <summary>
    /// 移動平滑角度
    /// </summary>
    [Header("移動角度平滑")]
    [SerializeField]
    private float move_angle_smooth;

    /// <summary>
    /// 移動平滑角度
    /// </summary>
    [Header("移動平滑角度參數")]
    [SerializeField]
    private float smooth_angle = 0.2f;
    private float yVelocity = 0f;

    /// <summary>
    /// 速度
    /// </summary>
    [Header("速度")]
    [SerializeField]
    private float speed = 3f;

    /// <summary>
    /// 幾禎到最高速
    /// </summary>
    [Header("幾禎到最高速")]
    [SerializeField]
    private byte speed_to_max = 12;

    /// <summary>
    /// 加速度
    /// </summary>
    private float addspeed;

    /// <summary>
    /// 加速完的速度
    /// </summary>
    private float speed_add;

    /// <summary>
    /// 幾禎到最高速
    /// </summary>
    [Header("幾禎到停止")]
    [SerializeField]
    private byte speed_to_zero = 12;

    /// <summary>
    /// 阻力
    /// </summary>
    private float drag;

    /// <summary>
    /// 儲存計算減速玩速度
    /// </summary>
    private float speed_sum;
    /// <summary>
    /// 儲存計算減速玩速度
    /// </summary>
    public float _speed_sum {
        get { return speed_sum; }
    }

    private float speed_max = 1f;

    /// <summary>
    /// 是否按到跳躍
    /// </summary>
    [Header("是否按到跳躍")]
    [SerializeField]
    private bool jump = false;
    /// <summary>
    /// 是否按到跳躍
    /// </summary>
    public bool _jump {
        get { return jump; }
    }

    /// <summary>
    /// 是否可以滯空
    /// </summary>
    [Header("是否可以滯空")]
    public bool stay_air = false;

    /// <summary>
    /// 跳躍力道
    /// </summary>
    [Header("跳躍力道")]
    [SerializeField]
    private byte jump_pow = 5;
    
    /// <summary>
    /// 3rd_Camera
    /// </summary>
    [Header("3rd_Camera")]
    [SerializeField]
    private _3rd_Camera _3rd_cam;

    /// <summary>
    /// 自己Rigidbody
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// 自己的Transform
    /// </summary>
    [SerializeField]
    private Transform tf;
    /// <summary>
    /// 自己的Transform
    /// </summary>
    public Transform _tf {
        get { return tf; }
    }

    /// <summary>
    /// 自己的CapsuleCollider
    /// </summary>
    private CapsuleCollider capsule_collider;

    private bool is_hit;
    private Vector3 spherecast_pos;
    private float spherecast_radius;
    private RaycastHit hit;
    private float spherecast_dis;

    #endregion =============================================================================================================== 變數宣告

    #region 函式 ===============================================================================================================
    private void Start() {
        drag = 60f/speed_to_zero * 0.017f;
        addspeed = drag + 60f/speed_to_max * 0.017f;

        rb = GetComponent<Rigidbody>();
        tf = transform;
        capsule_collider = GetComponent<CapsuleCollider>();

        spherecast_radius = capsule_collider.radius * 0.9f;
        spherecast_dis = capsule_collider.radius*1.1f;
    }

    //固定迴圈
    private void FixedUpdate() {
        OnGround();
        Inputer();
        Move();
    }

    private void Inputer() {
        //用LeftAlt切換是否走路狀態
        if(Input.GetKeyDown(KeyCode.LeftAlt)) {
            walk = !walk;
            ChangeAddSpeed();
        }

        input_keypad = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if(input_keypad != Vector2.zero) {
            input = true;

            //計算輸入角度
            input_angle = Mathf.Atan2(input_keypad.x, input_keypad.y);
        }
        else {
            input = false;
        }

        if(Input.GetButtonDown("Jump")) {
            jump = true;
        }
    }

    /// <summary>
    /// 切換加速度和阻力
    /// </summary>
    private void ChangeAddSpeed() {
        if(walk) {
            addspeed *= 0.5f;
            drag *= 0.5f;
        }
        else {
            addspeed *= 2f;
            drag *= 2f;
        }
    }

    /// <summary>
    /// 角色Transform
    /// </summary>
    private void Move() {
        if(walk) {
            if(speed_max > 0.33f) {
                speed_max -= drag;
            }
            else {
                speed_max = 0.33f;
            }
        }
        else {
            if(speed_max < 1f) {
                speed_max += drag;
            }
            else {
                speed_max = 1f;
            }
        }

        if(input && can_move) {
            float input_cam_angle = input_angle +_3rd_cam.angle_target_y * Mathf.Deg2Rad;

            //計算移動座標
            move_pos += addspeed * new Vector2(Mathf.Sin(input_cam_angle), Mathf.Cos(input_cam_angle));
        }

        //計算加速完速度
        speed_add = Vector2.Distance(move_pos, Vector2.zero);
        //計算減速完的速度
        speed_sum = speed_add - drag;

        //把速度限制在0~1之間
        if(speed_sum > speed_max) {
            speed_sum *= speed_max/speed_sum;
        }
        else if(speed_sum < 0)
            speed_sum = 0f;

        move_pos *= speed_sum > 0f ? speed_sum/speed_add : 0f;

        if(move_pos != Vector2.zero) {
            move = true;

            //計算移動角度，要拿來角色的面相角度
            move_angle = Mathf.Atan2(move_pos.x, move_pos.y)  / (Mathf.PI/180f);
            //使用平滑補間看起來比較有質感
            move_angle_smooth = Mathf.SmoothDampAngle(move_angle_smooth, move_angle, ref yVelocity, smooth_angle);

            //設定角度
            rb.rotation = Quaternion.Euler(0, move_angle_smooth, 0);

            move_pos_sum = move_pos * speed;
            //設定移動量
            rb.velocity = new Vector3(move_pos_sum.x, rb.velocity.y, move_pos_sum.y);
        }
        else {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            move = false;
        }

        if(stay_air) {
            //設定移動量
            rb.velocity = new Vector3(move_pos_sum.x, 0, move_pos_sum.y);
        }
    }

    /// <summary>
    /// 判斷是否在地上
    /// </summary>
    private void OnGround() {
        spherecast_pos = tf.position + tf.up * capsule_collider.radius;

        is_hit = Physics.SphereCast(spherecast_pos, spherecast_radius, -tf.up, out hit, spherecast_dis);

        if (is_hit) {
            on_ground = true;
        }
        else {
            on_ground = false;
        }
    }

    void OnDrawGizmos() {
        if (is_hit) {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(spherecast_pos, - tf.up * hit.distance);
            Gizmos.DrawWireSphere(spherecast_pos - tf.up * hit.distance, spherecast_radius);
        }
        else {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(spherecast_pos, - tf.up * spherecast_dis);
            Gizmos.DrawWireSphere(spherecast_pos - tf.up * spherecast_dis, spherecast_radius);
        }
    }

        
    
    /// <summary>
    /// 跳躍函式(給外部程式)
    /// </summary>
    public void Jump() {
        jump = false;

        rb.velocity = new Vector3(rb.velocity.x, jump_pow, rb.velocity.z);
    }

    /// <summary>
    /// 設定是否可以跳躍(給外部程式)
    /// </summary>
    public void Set_Can_Move(int _switch) {
        can_move = _switch > 0 ? true : false;
    }

    /// <summary>
    /// 設定是否可以滯空(給外部程式)
    /// </summary>
    public void Set_Stay_Air(int _switch) {
        stay_air = _switch > 0 ? true : false;
    }
    #endregion =============================================================================================================== 函式
}
