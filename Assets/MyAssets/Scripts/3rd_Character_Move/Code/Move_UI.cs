using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_UI : MonoBehaviour
{
    /// <summary>
    /// 第三人稱移動
    /// </summary>
    [Header("第三人稱移動")]
    [SerializeField]
    private _3rd_Character_Move _3cm;

    /// <summary>
    /// 箭頭
    /// </summary>
    [Header("箭頭")]
    [SerializeField]
    private RectTransform arrow;

    /// <summary>
    /// 邊界距離
    /// </summary>
    [Header("邊界距離")]
    [SerializeField]
    private float dis;


    // Update is called once per frame
    void Update()
    {
        arrow.eulerAngles = new Vector3(0, 180, _3cm._move_angle);
        arrow.localPosition = _3cm._move_pos * dis;
    }
}
