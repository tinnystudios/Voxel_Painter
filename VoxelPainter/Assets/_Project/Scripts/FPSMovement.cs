using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{

}

public class FPSMovement : BaseMovement
{
    public float LookAtSpeed = 3;
    public float MoveSpeed = 5;

    private Vector2 _rotation;
    private Vector3 _startAngle;

    private void OnEnable()
    {
        _rotation = Vector2.zero;
        _startAngle = transform.eulerAngles;
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var deltaMovement = new Vector3(horizontal, 0, vertical);

        transform.position += transform.forward * vertical * Time.deltaTime * MoveSpeed;
        transform.position += transform.right * horizontal * Time.deltaTime * MoveSpeed;

        if (Input.GetMouseButton(1))
        {
            _rotation.y += Input.GetAxis("Mouse X");
            _rotation.x += -Input.GetAxis("Mouse Y");

            Vector3 newAngle = _rotation * LookAtSpeed;
            transform.eulerAngles = _startAngle + newAngle;
        }
    }
}
