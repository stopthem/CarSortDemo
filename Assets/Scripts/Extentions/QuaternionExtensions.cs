using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuaternionExtensions

{
    public static Vector3 FixReturnEuler(this Quaternion rot)
    {
        Vector3 rotEuler = rot.eulerAngles;
        rotEuler.y = rotEuler.y > 180 ? rotEuler.y - 360 : rotEuler.y;
        rotEuler.x = rotEuler.x > 180 ? rotEuler.x - 360 : rotEuler.x;
        rotEuler.z = rotEuler.z > 180 ? rotEuler.z - 360 : rotEuler.z;
        return rotEuler;
    }
}
