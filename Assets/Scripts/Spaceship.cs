using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public Action OnBoost = delegate { };

    [Header("Move")]
    [SerializeField] float moveForwardSpeed = 10f;
    [SerializeField] float moveRightSpeed = 10f;
    [SerializeField] float moveLimit = 2.5f;
    [SerializeField] float rotateAngleCoeff = 10f;

    [Header("Acceleration")]
    [SerializeField] float accelerationСoeff = 2f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] AudioClip accelerationSound;
    [SerializeField] SmoothFollow mainCamera;

    [Header("Die")]
    [SerializeField] GameObject diePartical;
    [SerializeField] AudioClip dieSound;


    private Rigidbody rb;

    private float startBoostFilling = 3f;
    private float boostFilling = 3f;
    private bool readyForSpeedUp;
    private bool readyForFillBoost;
    private bool isAlive;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }
    public float BoostFilling
    {
        get
        {
            return boostFilling;
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        readyForSpeedUp = true;
        readyForFillBoost = true;
        isAlive = true;
    }

    private void FixedUpdate()
    {
        Move();
        SetSpeedBoost();
        FeelBoost();
    }

    private void Move()
    {
        if (isAlive)
        {
            float inputX = Input.GetAxis("Horizontal");
            Vector3 direction = new Vector3(inputX, 0, 0);
            rb.velocity = Vector3.forward * moveForwardSpeed + direction * moveRightSpeed;
            float clampedPositionX = Mathf.Clamp(transform.position.x, -moveLimit, moveLimit);
            transform.position = new Vector3(clampedPositionX, transform.position.y, transform.position.z);
            transform.rotation = Quaternion.Euler(0, 0, -inputX * rotateAngleCoeff);
        }
    }

    private void SetSpeedBoost()
    {
        //give acceleration
        if (readyForSpeedUp && isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelManager.Instance.DoublePoints = true;
                AudioManager.Instance.PlayEffect(accelerationSound);
                mainCamera.BoostZoom();
                StartCoroutine(SetAcceleration(accelerationTime, accelerationСoeff));
            }
        }

    }
    private void FeelBoost()
    {
        //provided that the player is alive, change the filling of the acceleration scale
        if (isAlive)
        {
            OnBoost();

            if (!readyForFillBoost)
            {
                boostFilling -= Time.deltaTime;
            }
            else if (readyForFillBoost && boostFilling < startBoostFilling)
            {
                LevelManager.Instance.DoublePoints = false;
                boostFilling += Time.deltaTime;
                if (boostFilling >= startBoostFilling)
                {                    
                    readyForSpeedUp = true;
                }
            }
        }
    }

    private IEnumerator SetAcceleration(float time, float coeff)
    {
        //add acceleration for 3 seconds
        moveForwardSpeed *= coeff;
        readyForSpeedUp = false;
        readyForFillBoost = false;
        yield return new WaitForSeconds(time);
        moveForwardSpeed /= coeff;
        mainCamera.SetNormalZoom();
        readyForFillBoost = true;

    }
    private void OnCollisionEnter(Collision collision)
    {
        //stop the object and apply effects to it at the time of death
        isAlive = false;
        rb.velocity = Vector3.zero;
        AudioManager.Instance.PlayEffect(dieSound);
        Instantiate(diePartical, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        LevelManager.Instance.SetBestScore();
        UImanager.Instance.ActivateLoseGamePanel();
    }
}
