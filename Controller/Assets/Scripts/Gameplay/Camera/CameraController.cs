using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraController : MonoBehaviour
{
    private float _horizontalAxis, _verticalAxis, axisRotation = 0.0f;

    [SerializeField] 
    private float mouseSensetivity = 10;

    [SerializeField] 
    private Transform target;

    private void Update()
    {
        MyInput();
        HandleMovement();
    }

    private void MyInput()
    {
        Vector2 _mouseLook = GameManager.instance.inputActions.Player.Camera.ReadValue<Vector2>();

        _horizontalAxis = _mouseLook.x * mouseSensetivity * Time.deltaTime;
        _verticalAxis = _mouseLook.y * mouseSensetivity * Time.deltaTime;
    }

    private void HandleMovement()
    {
        axisRotation -= _verticalAxis;
        axisRotation = Mathf.Clamp(axisRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(axisRotation, 0.0f, 0.0f);
        target.Rotate(Vector3.up * _horizontalAxis);
    }
}
