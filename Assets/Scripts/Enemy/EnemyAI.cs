using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
   // reference to character controller
   private CharacterController _characterController;
   private Transform _player;

   private Vector3 _direction;
   [SerializeField]
   private float _speed = 2.5f;
   private Vector3 _velocity;
   private float _gravity = 20.0f;

   private void Start()
   {
      _characterController = GetComponent<CharacterController>();
      _player = GameObject.FindGameObjectWithTag("Player").transform;
   }

   void Update()
   {
      // check if grounded
      if (_characterController.isGrounded == true)
      {
         // calculate direction = destination (player or target) - source (self's transform.position)
         _direction = _player.position - transform.position;


         // cut the direction length to 1 so enemy doesn't rush up to player
         // calculate the unit vector move 7 units in one second
         _direction.Normalize();
         _direction.y = 0;
         // rotate towards the player
         transform.localRotation = Quaternion.LookRotation(_velocity);
         _velocity = _direction * _speed;


      }

      // for character to move in direction it is facing
      //   see CalculateMovement() method
      //   need to adjust the transform inverse

      // local to world space
      //_velocity = transform.TransformDirection(_velocity);


      // subtract gravity
      _velocity.y -= _gravity * Time.deltaTime;

      // move to velocity
      _characterController.Move(_velocity * Time.deltaTime);

   }
}
