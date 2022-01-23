using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yuko_Animator : MonoBehaviour
{
    /// <summary>
    /// 第三人稱移動
    /// </summary>
    [Header("第三人稱移動")]
    [SerializeField]
    private _3rd_Character_Move _3cm;

    /// <summary>
    /// Animator
    /// </summary>
    [Header("Animator")]
    [SerializeField]
    private Animator ani;

    /// <summary>
    /// 跳躍特效
    /// </summary>
    [Header("跳躍特效")]
    [SerializeField]
    private ParticleSystem jump_vfx;

    private void Jump()
    {
        _3cm.Jump();
    }

    private void Jump_VFX()
    {
        jump_vfx.transform.position = _3cm._tf.position + new Vector3(0f, 0.01f, 0f);
        jump_vfx.Play();
    }

    private void Set_Can_Move(int _switch)
    {
        _3cm.Set_Can_Move(_switch);
    }

    private void Set_Stay_Air(int _switch)
    {
        _3cm.Set_Stay_Air(_switch);
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("on_ground", _3cm._on_ground);

        ani.SetBool("jump", _3cm._jump);

        ani.SetFloat("move" , _3cm._speed_sum);
    }
}
