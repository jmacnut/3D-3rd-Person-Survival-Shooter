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

   [SerializeField]
   private float _speed = 6.0f;
   [SerializeField]
   private float _jumpHeight = 8.0f;
   [SerializeField]
   private float _gravity = 20.0f;

   Vector3 _direction = Vector3.zero;
   private Vector3 _velocity;

   void Start()
   {
      _characterController = GetComponent<CharacterController>();

      if (_characterController == null)
      {
         Debug.LogError("The CharacterController is NULL");
      }
   }

   void Update()
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

      // Move the controller
      _characterController.Move(_velocity * Time.deltaTime);
   }
}
