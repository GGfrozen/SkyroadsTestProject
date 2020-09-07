using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : MonoBehaviour
{
    [SerializeField] int point = 5;
    [SerializeField] int pointPerAsteroid = 1;

    private Quaternion originalRotation;
    private float rotateAngle;

    private void Start()
    {
        originalRotation = transform.rotation;
    }
    private void FixedUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {
        //independent rotation on two axes
        rotateAngle++;
        Quaternion rotationY = Quaternion.AngleAxis(rotateAngle, Vector3.up);
        Quaternion rotationX = Quaternion.AngleAxis(rotateAngle, Vector3.right);
        transform.rotation = originalRotation * rotationY * rotationX;
    }
    private void OnBecameInvisible()
    {
        LevelManager.Instance.AddPointToScore(point);
        LevelManager.Instance.AddAsteroidsCount(pointPerAsteroid);
        Destroy(gameObject);
    }
}
