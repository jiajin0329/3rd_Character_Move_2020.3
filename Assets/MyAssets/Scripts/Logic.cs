using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic
{
    /// <summary>
    /// 讓角度在180度以內
    /// </summary>
    static public float Angle180(float angle)
    {
        if (angle > 180)
        {
            angle = angle % 360 - 360;
        }
        else if (angle < -180)
        {
            angle = angle % 360 + 360;
        }
        return angle;
    }
}
