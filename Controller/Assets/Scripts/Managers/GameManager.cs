using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    internal InputActions inputActions;

    private void Awake()
    {
        instance = this;
        
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
