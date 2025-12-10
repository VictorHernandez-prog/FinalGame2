using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSetting : MonoBehaviour
{
    //Character stats
    private float _sensitivity;

    private Vector2 _mouseInput;

    private float _pitch;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _mouseInput.x * _sensitivity * Time.deltaTime);

        _pitch -= _mouseInput.y * _sensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        transform.localEulerAngles = new Vector3(_pitch, transform.localEulerAngles.y, 0f);
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        _mouseInput =  context.ReadValue<Vector2>();
    }
}
