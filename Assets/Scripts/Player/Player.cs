using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script moves the character controller forward and sideways based on the 
// arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class Player : MonoBehaviour
{
   // character controller handle
   private CharacterController _characterController;

   [Header("Controller Settings")]
   [SerializeField]
   private float _speed = 6.0f;
   [SerializeField]
   private float _jumpHeight = 8.0f;
   [SerializeField]
   private float _gravity = 20.0f;

   Vector3 _direction = Vector3.zero;
   private Vector3 _velocity;

   private Camera _mainCamera;
   [Header("Camera Settings")]
   [SerializeField]
   private float _cameraSensitivity = 1.0f;   // allows player to adjust


   void Start()
   {
      _characterController = GetComponent<CharacterController>();

      if (_characterController == null)
      {
         Debug.LogError("The CharacterController is NULL");
      }

      _mainCamera = Camera.main;

      if (_mainCamera == null)
      {
         Debug.LogError("The Main Camera object is NULL.");
      }

      // lock and hide cursor
      Cursor.lockState = CursorLockMode.Locked;

   }

   void Update()
   {
      CalculateMovement();
      CameraController();

      // unlock cursor withd ESC key
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         Cursor.lockState = CursorLockMode.None;
      }

   }


   void CalculateMovement()
   {
      if (_characterController.isGrounded == true)
      {
         // We are grounded, so recalculate
         // move direction directly from axes
         float horizontal = Input.GetAxis("Horizontal");
         float vertical = Input.GetAxis("Vertical");
         _direction = new Vector3(horizontal, 0f, vertical);

         _velocity = _direction * _speed;

         if (Input.GetKeyDown(KeyCode.Space))
         {
            _velocity.y = _jumpHeight;
         }
      }

      // Apply gravity. Gravity is multiplied by deltaTime twice (once here, 
      // and once below when the direction is multiplied by deltaTime). 
      // This is because gravity should be applied as an acceleration (ms^-2)
      _velocity.y -= _gravity * Time.deltaTime;

      // convert from local space to world space (move in the direction facing)
      _velocity = transform.TransformDirection(_velocity);

      // Move the controller
      _characterController.Move(_velocity * Time.deltaTime);
   }

   void CameraController()
   {
      float mouseX = Input.GetAxis("Mouse X");
      float mouseY = Input.GetAxis("Mouse Y");

      // look left-right: apply mouseX to the player rotation y
      // player rotation (basic idea)
      //transform.rotation.y += mouseX;

      // long way with Euler Angles
      //transform.localEulerAngles += new Vector3(
      //transform.localEulerAngles.x,
      //transform.localEulerAngles.y + mouseX,
      //transform.localEulerAngles.z);

      // short way with Euler Angles
      //Vector3 currentRotation = transform.localEulerAngles;
      //currentRotation.y += mouseX;
      //transform.localEulerAngles = currentRotation;

      // using quaternions to prevent gimble lock
      Vector3 currentRotation = transform.localEulerAngles;
      currentRotation.y += mouseX * _cameraSensitivity;
      transform.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector3.up);

      // look up-down: apply mouseY to the camera rotation X
      // clamp between 0 and 15
      Vector3 currentCameraRotation = _mainCamera.gameObject.transform.localEulerAngles;
      currentCameraRotation.x -= mouseY * _cameraSensitivity;
      currentCameraRotation.x = Mathf.Clamp(currentCameraRotation.x, 0f, 26f);
      _mainCamera.gameObject.transform.localRotation = Quaternion.AngleAxis(currentCameraRotation.x, Vector3.right);
   }
}
