using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CreditsManager : MonoBehaviour
{
    public UnityEvent OnClick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClick.Invoke();
        }
    }
}
