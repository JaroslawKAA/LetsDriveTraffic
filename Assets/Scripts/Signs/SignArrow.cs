using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignArrow : MonoBehaviour
{
    public void SetRotation(float degrees)
    {
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x,
            degrees,
            transform.localRotation.eulerAngles.z);
    }
}
