using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public UIManager Intance { get => instance; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance.gameObject);
    }
}
