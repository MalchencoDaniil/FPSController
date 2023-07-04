using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private GameManager gameManager;    

    private Vector3 _startPosition;

    internal float _inputMagnitude;

    [Header("Shake Stats")]
    [SerializeField, Range(0, 12)]
    private float frequency = 10.0f;

    [SerializeField, Range(0, 5)]
    private float amount = 0.002f;

    [SerializeField, Range(0, 12)]
    private float smooth = 10.0f;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            return;
        }

        _startPosition = transform.localPosition;
    }

    private void Update()
    {
        CheckForHeadBobTrigger();

        StopHeadBob();
    }

    private void CheckForHeadBobTrigger()
    {
        Vector2 _input = gameManager.inputActions.Player.Movement.ReadValue<Vector2>();

        _inputMagnitude = new Vector3(_input.x, 0, _input.y).magnitude;

        if (_inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        Vector3 _position = Vector3.zero;

        _position.y += Mathf.Lerp(_position.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        _position.x += Mathf.Lerp(_position.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += _position;

        return _position;
    }

    private void StopHeadBob()
    {
        if (transform.localPosition == _startPosition)
            return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, _startPosition, 1 * Time.deltaTime);
    }
}