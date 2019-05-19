using UnityEngine;

public class UnityEventProvider : MonoBehaviour
{
    private void Update()
    {
        UnityEvent.InvokeUpdate();
    }

    private void FixedUpdate()
    {
        UnityEvent.InvokeFixedUpdate();
    }
}
