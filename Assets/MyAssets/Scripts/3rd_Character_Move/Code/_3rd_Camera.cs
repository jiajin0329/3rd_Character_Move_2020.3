using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3rd_Camera : MonoBehaviour
{
    /// <summary>
    /// 目標角度
    /// </summary>
    [Header("目標角度")]
    [SerializeField]
    private Vector2 angle_target;
    public float angle_target_y
    {
        get { return angle_target.y; }
    }

    /// <summary>
    /// 角度平滑參數
    /// </summary>
    [Header("角度平滑參數")]
    [SerializeField]
    private float smooth_angle = 0.1f;
    private Vector2 angle_smooth;
    private Vector2 aVelocity;
    private Quaternion cameraQuaternion;

    /// <summary>
    /// 攝影機距離
    /// </summary>
    [SerializeField]
    [Header("攝影機距離")]
    private float distence = 3.5f;
    private float distence_smooth;
    private float dVelocity;

    /// <summary>
    /// 攝影機距離平滑參數
    /// </summary>
    [Header("攝影機距離平滑參數")]
    [SerializeField]
    private float smooth_distence = 0.2f;

    /// <summary>
    /// 攝影機最大距離
    /// </summary>
    [SerializeField]
    [Header("攝影機最大距離")]
    private float distence_max = 5f;

    /// <summary>
    /// 攝影機最小距離
    /// </summary>
    [SerializeField]
    [Header("攝影機最小距離")]
    private float distence_min = 2f;

    /// <summary>
    /// 最高上仰角度
    /// </summary>
    [SerializeField]
    [Header("最高上仰角度")]
    private byte angle_x_max = 80;

    /// <summary>
    /// 最低下看角度
    /// </summary>
    [SerializeField]
    [Header("最低下看角度")]
    private byte angle_x_min = 80;

    /// <summary>
    /// 目標Transform
    /// </summary>
    [Header("目標Transform")]
    [SerializeField]
    private Transform tf_target;

    /// <summary>
    /// 目標水平平滑參數
    /// </summary>
    [Header("目標水平平滑參數")]
    [SerializeField]
    private float smmoth_pos_h = 0.2f;

    /// <summary>
    /// 目標垂直平滑參數
    /// </summary>
    [Header("目標垂直平滑參數")]
    [SerializeField]
    private float smmoth_pos_v = 0.5f;

    private Vector3 pos_target;
    private Vector3 pos_smooth;
    private Vector3 tVelocity;

    /// <summary>
    /// Transform
    /// </summary>
    [Header("Transform")]
    [SerializeField]
    private Transform tf;

    private void Start()
    {
        //鎖住滑鼠並隱藏
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        angle_smooth = angle_target = tf.eulerAngles;

        pos_smooth = tf_target.position;

        distence_smooth = distence;
    }

    /// <summary>
    /// 處理輸出的函式
    /// </summary>
    private void Inputer()
    {
        angle_target += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        angle_target.y = Logic.Angle180(angle_target.y);

        angle_target.x = Mathf.Clamp(angle_target.x, -angle_x_max, angle_x_min);

        distence -= Input.GetAxis("Mouse ScrollWheel") * 4f;
        distence = Mathf.Clamp(distence, distence_min, distence_max);
    }

    /// <summary>
    /// 處理平滑效果的函式
    /// </summary>
    private void Smooth()
    {
        angle_smooth.x = Mathf.SmoothDampAngle(angle_smooth.x, angle_target.x, ref aVelocity.x, smooth_angle);
        angle_smooth.y = Mathf.SmoothDampAngle(angle_smooth.y, angle_target.y, ref aVelocity.y, smooth_angle);

        pos_target = tf_target.position;

        pos_smooth.x = Mathf.SmoothDamp(pos_smooth.x, pos_target.x, ref tVelocity.x, smmoth_pos_h);
        pos_smooth.y = Mathf.SmoothDamp(pos_smooth.y, pos_target.y, ref tVelocity.y, smmoth_pos_v);
        pos_smooth.z = Mathf.SmoothDamp(pos_smooth.z, pos_target.z, ref tVelocity.z, smmoth_pos_h);

        distence_smooth = Mathf.SmoothDamp(distence_smooth, distence, ref dVelocity, smooth_distence);
    }

    /// <summary>
    /// 把最終結果設定的函式
    /// </summary>
    private void __3rd_Camera()
    {
        cameraQuaternion = Quaternion.Euler(angle_smooth.x, angle_smooth.y, 0);

        tf.position = cameraQuaternion * new Vector3(0, 0, -distence_smooth) + pos_smooth;

        tf.rotation = cameraQuaternion;
    }

    void Update()
    {
        Inputer();

        Smooth();

        __3rd_Camera();
    }
}
