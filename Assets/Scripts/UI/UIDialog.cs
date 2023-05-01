using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog : MonoBehaviour
{
    public System.Action OnClose;

    void OnDisable()
    {
        OnClose?.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }
}
