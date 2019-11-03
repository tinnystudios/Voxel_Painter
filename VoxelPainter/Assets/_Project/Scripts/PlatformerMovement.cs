using UnityEngine;

public class PlatformerMovement : BaseMovement
{
    public Rigidbody Rigidbody;
    private void OnEnable() 
    {
        Rigidbody.useGravity = true;
        Rigidbody.isKinematic = false;
    }

    private void OnDisable() 
    {
        Rigidbody.useGravity = false;
        Rigidbody.isKinematic = true;
    }

    private void Update() 
    {
        var grounded = Physics.Raycast(transform.position, Vector3.down, 1.5f);

        if (!grounded)
            return;

        if (Input.GetButtonUp("Jump")) 
        {
            Rigidbody.velocity = Vector3.up * 5;
        }
    }
}
