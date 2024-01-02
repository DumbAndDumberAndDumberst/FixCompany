using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AgentInput : MonoBehaviour
{
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent OnJumpInput;
    public UnityEvent<int> OnWeaponChange;

    public Vector3 dir;

    private void Update()
    {
        Move();
        Jump();
        WeaponChange();
    }


    private void Move()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        OnMovementInput?.Invoke(dir);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnJumpInput?.Invoke();
        }
    }

    private void WeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnWeaponChange?.Invoke(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnWeaponChange?.Invoke(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnWeaponChange?.Invoke(3);
        }
    }
}
