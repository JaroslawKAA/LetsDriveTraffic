using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }

        public static Transform[] GetAllChildren(this Transform transform)
        {
            Transform[] allChildren = new Transform[transform.childCount];
            int index = 0;
            foreach (Transform children in transform)
            {
                allChildren[index] = children;
                
                index++;
            }

            return allChildren;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(x => Scripts.Utils.Random.Next());
        }
}