using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameEvents.S.OnPlacingSignStart_Invoke();
        MouseController.S.SelectItem(signPrefab);
    }
}
