using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    MovementComponent movementComp;
    InputActions inputActions;
    Animator animator;
    int speedHash = Animator.StringToHash("speed");
    Coroutine BackToIdleCoroutine;
    InGameUI inGameUI;
    CameraManager cameraManager;
    bool isAlive = true;

    [SerializeField] Weapon[] StartWeaponPrefabs;
    [SerializeField] Transform GunSocket;

    [SerializeField] JoyStick moveStick;
    [SerializeField] JoyStick aimStick;

    List<Weapon> Weapons;
    Weapon CurrentWeapon;
    int currentWeaponIndex = 0;
    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void InitializeWeapons()
    {
        Weapons = new List<Weapon>();
        foreach (Weapon weapon in StartWeaponPrefabs)
        {
            Weapon newWeapon = Instantiate(weapon, GunSocket);
            newWeapon.Owner = gameObject;
            newWeapon.UnEquip();
            Weapons.Add(newWeapon);
        }
        EquipWeapon(0);
    }

    void EquipWeapon(int weaponIndex)
    {
        if (Weapons.Count > weaponIndex)
        {
            if(CurrentWeapon!=null)
            {
                CurrentWeapon.UnEquip();
            }

            currentWeaponIndex = weaponIndex;
            Weapons[weaponIndex].Equip();
            CurrentWeapon = Weapons[weaponIndex];
            if(inGameUI!=null)
            {
                inGameUI.SwichedWeaponTo(CurrentWeapon);
            }
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        inGameUI = FindObjectOfType<InGameUI>();
        movementComp = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
        inputActions.Gameplay.CursorPos.performed += CursorPosUpdated;
        inputActions.Gameplay.move.performed += MoveInputUpdated;
        inputActions.Gameplay.move.canceled += MoveInputUpdated;
        //inputActions.Gameplay.MainAction.performed += MainActionButtonDown;
        //inputActions.Gameplay.MainAction.canceled += MainActionReleased;
        inputActions.Gameplay.Space.performed += BigAction;
        inputActions.Gameplay.NextWeapon.performed += NextWeapon;
        animator.SetTrigger("BackToIdle");
        InitializeWeapons();
        cameraManager = FindObjectOfType<CameraManager>();

        WeaponSwitcher switcher = FindObjectOfType<WeaponSwitcher>();
        if(switcher)
        {
            switcher.onWeaponSwitchPressed += NextWeapon;
        }

    }


    private void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
        EquipWeapon(currentWeaponIndex);
        animator.SetFloat("FiringSpeed", CurrentWeapon.GetShootingSpeed());
    }

    private void NextWeapon(InputAction.CallbackContext obj)
    {
        NextWeapon();
    }

    private void BigAction(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("BigAction");
    }

    private void Fire()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
    }

    private void StopFire()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
    }

    private void CursorPosUpdated(InputAction.CallbackContext obj)
    {
        movementComp.SetCursorPos(obj.ReadValue<Vector2>());
    }

    private void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>(); //.normalised
        movementComp.SetMovementInput(input);
        if(input.magnitude==0)
        {
            BackToIdleCoroutine = StartCoroutine(DelayedBackToIdle());
        }else
        {
            if(BackToIdleCoroutine!=null)
            {
                StopCoroutine(BackToIdleCoroutine);
                BackToIdleCoroutine = null;
            }
        }
    }

    public void UpdateMovement(Vector2 input)
    {
        
        movementComp.SetMovementInput(input);
        if (input.magnitude == 0)
        {
            BackToIdleCoroutine = StartCoroutine(DelayedBackToIdle());
        }
        else
        {
            if (BackToIdleCoroutine != null)
            {
                StopCoroutine(BackToIdleCoroutine);
                BackToIdleCoroutine = null;
            }
        }
    }

    IEnumerator DelayedBackToIdle()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("BackToIdle");
    }

    void UpdateAnimation()
    {

        animator.SetFloat(speedHash, GetComponent<CharacterController>().velocity.magnitude);
        Vector3 PlayerForward = movementComp.GetPlayerDesiredLookDir();
        Vector3 PlayerMoveDir = movementComp.GetPlayerDesiredMoveDir();
        Vector3 PlayerLeft = Vector3.Cross(PlayerForward, Vector3.up);
        float forwardAmt = Vector3.Dot(PlayerForward, PlayerMoveDir);
        float leftAmt = Vector3.Dot(PlayerLeft, PlayerMoveDir);

        animator.SetFloat("forwardSpeed", forwardAmt);
        animator.SetFloat("leftSpeed", leftAmt);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (isAlive)
        {
            UpdateAnimation();
            UpdateMoveStickInput();
            UpdateAimStickInput();
        }

        UpdateCamera();
    }

    private void UpdateCamera()
    {
        cameraManager.UpdateCamera(transform.position, moveStick.Input, aimStick.Input.magnitude > 0);
    }
    
    public void FireTimePoint()
    {
        if(CurrentWeapon!=null)
        {
            CurrentWeapon.Fire();
        }
    }

    public void UpdateMoveStickInput()
    {
        if(moveStick != null)
        {
            UpdateMovement(moveStick.Input);
        }
    }

    public void UpdateAimStickInput()
    {
        if (aimStick != null)
        {
            movementComp.SetAimInput(aimStick.Input);

            if(aimStick.Input.magnitude > 0)
            {
                Fire();
            }
            else
            {
                StopFire();
            }
        }
    }

    public override void NoHealthLeft()
    {
        base.NoHealthLeft();
        OnDisable();
        isAlive = false;
    }
}
