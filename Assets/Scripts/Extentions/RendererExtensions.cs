using System.Linq;
using UnityEngine;

public static class RendererExtensions
{
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static int GetBlendShapeIndexByName(this SkinnedMeshRenderer skinnedMeshRenderer, string name)
    {
        Mesh m = skinnedMeshRenderer.sharedMesh;
        string[] arr;
        arr = new string[m.blendShapeCount];
        for (int i = 0; i < m.blendShapeCount; i++)
        {
            string s = m.GetBlendShapeName(i);
            arr[i] = s;
        }
        string keyName = arr.FirstOrDefault(x => x == name);
        return arr.ToList().IndexOf(keyName);
    }

    public static void CopySkinnedMesh(this SkinnedMeshRenderer thisMesh, SkinnedMeshRenderer otherMesh, bool copyBlendShapes = true)
    {
        thisMesh.sharedMesh = otherMesh.sharedMesh;
        thisMesh.rootBone = otherMesh.rootBone;
        thisMesh.localBounds = new Bounds(otherMesh.bounds.center, otherMesh.bounds.size);
        thisMesh.bones = otherMesh.bones;

        if (copyBlendShapes)
        {
            for (int i = 0; i < otherMesh.sharedMesh.blendShapeCount; i++)
            {
                if (i == thisMesh.sharedMesh.blendShapeCount) break;
                thisMesh.SetBlendShapeWeight(i, otherMesh.GetBlendShapeWeight(i));
            }
        }
    }

    public static void SetBlendShapeWeight(this SkinnedMeshRenderer renderer, string keyName, float value)
    {
        int index = renderer.GetBlendShapeIndexByName(keyName);
        if (index != -1) renderer.SetBlendShapeWeight(index, value);
    }
}