using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
   // finite state machine states
   public enum EnemyState
   {
      Idle,
      Chase,
      Attack
   }

   // reference to character controller
   private CharacterController _characterController;
   private Transform _player;

   private Vector3 _direction;
   [SerializeField]
   private float _speed = 2.5f;
   private Vector3 _velocity;
   private float _gravity = 20.0f;

   [SerializeField]
   private EnemyState _currentState = EnemyState.Chase;

   private Health _playerHealth;

   [SerializeField]
   private float _attackDelay = 1.5f;

   [SerializeField]
   private float _nextAttack = -1.0f;

   private void Start()
   {
      _characterController = GetComponent<CharacterController>();
      _player = GameObject.FindGameObjectWithTag("Player").transform;
      _playerHealth = _player.GetComponent<Health>();

      if (_player == null || _playerHealth == null)
      {
         Debug.LogError("Player component(s) is/are NULL.");
      }
   }

   void Update()
   {
      switch (_currentState)
      {
         case EnemyState.Idle:
            break;
         case EnemyState.Chase:
            CalculateMovement();
            break;
         case EnemyState.Attack:
            Attack();
            break;
         default:
            break;
      }
   }

   void CalculateMovement()
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

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Player")
      {
         _currentState = EnemyState.Attack;
      }
   }

   private void OnTriggerExit(Collider other)
   {
      // resume movement
      if (other.tag == "Player")
      {
         _currentState = EnemyState.Chase;
      }
   }

   //private IEnumerator OnTriggerStay(Collider other){}
   // for damage - notice return type IEnumerator (not void)
   // this will not work because we need cool down system, 
   // handle in Update(), instead

   void Attack()
   {
      // cooldown system
      if (Time.time > _nextAttack)
      {
         if (_playerHealth != null)
         {
            _playerHealth.Damage(10);
         }

         _nextAttack = _attackDelay + Time.time;
      }
   }
}
