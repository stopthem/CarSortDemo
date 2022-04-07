using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void CopyTransform(this Transform t, Transform toTransform, bool local = false, bool useToScale = true)
    {
        if (local)
        {
            t.localPosition = toTransform.localPosition;
            t.localRotation = toTransform.localRotation;
        }
        else
        {
            t.rotation = toTransform.rotation;
            t.position = toTransform.position;
        }

        if (useToScale) t.localScale = toTransform.localScale;
    }
}
