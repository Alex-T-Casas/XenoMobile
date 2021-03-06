using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    AbilityComponent abilityComp;
    AbilityWheel abilityWheel;
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

    List<Weapon> Weapons = new List<Weapon>();
    Weapon CurrentWeapon;
    int currentWeaponIndex = 0;
    private void Awake()
    {
        inputActions = new InputActions();
        abilityComp = GetComponent<AbilityComponent>();
        abilityWheel = FindObjectOfType<AbilityWheel>();
        if(abilityComp!=null)
        {
            abilityComp.onNewAbilityInitialzed += NewAbilityAdded;
            abilityComp.onStaminaUpdated += StaminaUpdated;
        }
    }

    internal void AquireNewWeapon(Weapon weapon, bool Equip = false)
    {
        int weaponHash = weapon.gameObject.GetHashCode();


        Weapon newWeapon = Instantiate(weapon, GunSocket);
        newWeapon.Owner = gameObject;
        newWeapon.UnEquip();
        Weapons.Add(newWeapon);
        if (Equip)
        {
            EquipWeapon(Weapons.Count - 1);
        }
    }


    private void StaminaUpdated(float newValue)
    {
        abilityWheel.UpdateStamina(newValue);         
    }

    private void NewAbilityAdded(AbilityBase NewAbility)
    {
        AbilityWheel abilityWheel = FindObjectOfType<AbilityWheel>();
        abilityWheel.AddNewAbility(NewAbility);
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
        foreach (Weapon weapon in StartWeaponPrefabs)
        {
            AquireNewWeapon(weapon, false);
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
            animator.SetFloat("FiringSpeed", CurrentWeapon.GetShootingSpeed());
            if (inGameUI!=null)
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
        abilityWheel.UpdateStamina(abilityComp.GetSteminaLevel());

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

    public override void NoHealthLeft(GameObject killer)
    {
        base.NoHealthLeft();
        OnDisable();
        isAlive = false;
    }

    public PlayerSaveGameData GenerateSaveData()
    {
        List<string> weaponsNames = new List<string>();
        foreach(Weapon weapon in Weapons)
        {
            weaponsNames.Add(weapon.GetWeaponInfo().name);
        }
        return new PlayerSaveGameData(transform.position,
            GetComponent<HealthComponent>().GetHealth(),
            GetComponent<AbilityComponent>().GetSteminaLevel(),
            weaponsNames.ToArray());
    }

    public void UpdateFromSaveData(PlayerSaveGameData data)
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = data.Location;
        GetComponent<CharacterController>().enabled = true;


        //apply health
        HealthComponent healthComp = GetComponent<HealthComponent>();
        healthComp.ChangeHealth(data.Health - healthComp.GetHealth());

        //stamina
        AbilityComponent ablilityComp = GetComponent<AbilityComponent>();
        abilityComp.ChangeStamina(data.Stamina - ablilityComp.GetSteminaLevel());

        var shops = Resources.FindObjectsOfTypeAll<ShopSystem>();
        if(shops.Length>0)
        {
            ShopSystem shop = shops[0];
            Weapon[] allWeapons = shop.GetWaponsOnSale();
            List<string> weaponInData = new List<string>(data.Weapons);
            foreach (Weapon weapon in allWeapons)
            {
                bool HadWeapon = weaponInData.Contains(weapon.GetWeaponInfo().name);
                bool AlreadyHave = false;
                foreach(Weapon weaponAlreadyHave in StartWeaponPrefabs)
                {
                    if(weaponAlreadyHave.GetWeaponInfo().name == weapon.GetWeaponInfo().name)
                    {
                        AlreadyHave = true;
                    }

                }

                if(HadWeapon && !AlreadyHave)
                {
                    AquireNewWeapon(weapon);
                }
            }
        }
    }
}

[Serializable]

public struct PlayerSaveGameData
{
    public PlayerSaveGameData(Vector3 playerLoc, float playerHealth, float playerStamina, string[] weapons)
    {
        Location = playerLoc;
        Health = playerHealth;
        Stamina = playerStamina;
        Weapons = weapons;
    }
    public Vector3 Location;
    public float Health;
    public float Stamina;
    public string[] Weapons;
}