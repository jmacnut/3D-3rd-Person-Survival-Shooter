﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
   void Start()
   {

   }

   void Update()
   {
      ShootCube();
   }

   void ShootCube()
   {
      // left click to fire
      // cast a ray from the center of the screen (viewport)
      // debug the name of the object you hit
      if (Input.GetMouseButtonDown(0))
      {
         // vector3 start position
         // direction (toward cube)
         Vector3 center = new Vector3(0.5f, 0.5f, 0);
         Ray rayOrigin = Camera.main.ViewportPointToRay(center);
         RaycastHit hitInfo;

         if (Physics.Raycast(rayOrigin, out hitInfo))
         {
            Debug.Log("Hit: " + hitInfo);

         }
      }
   }
}
