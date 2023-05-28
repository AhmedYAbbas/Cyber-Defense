using System;
using UnityEngine;

namespace Eissa
{
    public class RotateAroundItself : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        private void Update()
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime),Space.Self);
        }
    }
}