using System;
using UnityEngine;

namespace Eissa
{
    public class UILookAtCamera : MonoBehaviour
    {
        private Camera _camera;
        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.transform.rotation*Vector3.forward,_camera.transform.rotation*Vector3.up);
        }
    }
}