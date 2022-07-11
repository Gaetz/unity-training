using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Transform _cameraTransform;
    private CharacterController _controller;
    private Vector2 _mouseDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = transform.GetChild(0);
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = Vector3.Normalize(transform.forward * vertical + transform.right * horizontal) * Time.deltaTime;
        _controller.Move(movement);
    }

    private void HandleRotation()
    {
        float vertical = Input.GetAxisRaw("Mouse Y");
        float horizontal = Input.GetAxisRaw("Mouse X");
        Vector2 mouseMove = new Vector2(horizontal, vertical);
        _mouseDirection += mouseMove;
        _cameraTransform.localRotation = Quaternion.AngleAxis(-_mouseDirection.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(_mouseDirection.x, Vector3.up);
    }
}
