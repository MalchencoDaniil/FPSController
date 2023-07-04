using UnityEngine;

internal enum CursorState
{
    None,
    Locked
}

public class CursorManager : MonoBehaviour
{
    [SerializeField] 
    internal CursorState cursorState;

    private void Update()
    {
        switch (cursorState)
        {
            case CursorState.Locked: Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorState.None: Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}