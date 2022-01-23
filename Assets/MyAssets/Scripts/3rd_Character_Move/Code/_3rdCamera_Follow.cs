using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3rdCamera_Follow : MonoBehaviour
{
    /// <summary>
    /// 目標Transform
    /// </summary>
    [Header("目標Transform")]
    [SerializeField]
    private Transform target;

    /// <summary>
    /// 目標pos
    /// </summary>
    private Vector3 target_pos;

    /// <summary>
    /// 平滑參數
    /// </summary>
    [Header("平滑參數")]
    [SerializeField]
    private Vector3 smooth = new Vector3(0.2f, 0.4f, 0.2f);
    private Vector3 Velocity = Vector3.zero;

    /// <summary>
    /// 平滑座標
    /// </summary>
    private Vector3 smooth_pos;

    private Transform tf;

    private void Start()
    {
        tf = transform;
    }

    private void FixedUpdate()
    {
        target_pos = target.position;

        smooth_pos.x = Mathf.SmoothDamp(smooth_pos.x, target_pos.x, ref Velocity.x, smooth.x);
        smooth_pos.y = Mathf.SmoothDamp(smooth_pos.y, target_pos.y, ref Velocity.y, smooth.y);
        smooth_pos.z = Mathf.SmoothDamp(smooth_pos.z, target_pos.z, ref Velocity.z, smooth.z);

        tf.position = smooth_pos;
    }
}
