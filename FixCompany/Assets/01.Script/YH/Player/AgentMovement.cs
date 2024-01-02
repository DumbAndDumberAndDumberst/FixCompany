using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float _currentSpeed = 0, _maxSpeed = 50, _rotationSpeed, _accel = 50, _deaccel = 50;
    [SerializeField] private float _jumpPower;
    private Rigidbody _rigid;
    private bool _isJumping;
    private bool _isGround;

    private Vector3 _moveDir;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        if (_isJumping)
        {
            Debug.Log("มกวม");
            return;
        }
        if (dir.sqrMagnitude > 0)
        {
            if (Vector2.Dot(_moveDir, dir) < 0)
            {
                _currentSpeed = 0;
            }
            _moveDir = dir;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        _currentSpeed = CalculateSpeed(dir);
        transform.Translate(Vector3.forward * (Time.deltaTime * _currentSpeed));
    }

    private float CalculateSpeed(Vector3 dir)
    {
        if (dir.sqrMagnitude > 0)
        {
            _currentSpeed += _accel * Time.deltaTime;
        }
        else
        {
            _currentSpeed -= _deaccel * Time.deltaTime;
        }

        return Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
    }

    public void Jump()
    {
        if (_isJumping)
        {
            return;
        }
        _isJumping = true;
        _rigid.AddForce((Vector3.up + transform.position) * _jumpPower, ForceMode.Impulse);
        Action action = () => _isJumping = false;

        StopCoroutine(nameof(WaitUntilGround));
        StartCoroutine(nameof(WaitUntilGround), action);
    }

    private IEnumerator WaitUntilGround(Action onComplete)
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => _isGround);
        onComplete?.Invoke();
        _rigid.velocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }
}
