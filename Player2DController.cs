using UnityEngine;
using UnityEngine.UI;

public class Player2DController: MonoBehaviour
{
    /* 
        Use this script on the parent object of the u.i of your player
    */
    
    // UI
    [SerializeField] private Image UIPlayer;

    // Bools
    private bool IsLastDirectionPositive;
    private bool CanSprint;
    private bool CanJump;
    private bool IsJumping;

    // Vectors
    private Vector2 movement;

    // Movement Properties
    [Header("Movement Properties")]
    [SerializeField] private float MoveSpeed = 250f;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float SprintSpeedRatio = 1.5f;
    [SerializeField] private float JumpPower = 35f;
    private float _jumpPower = 0;

    // Defaults
    private float DefaultMoveSpeed;

    // Key Mapping
    [Header("Key Mapping")]
    [SerializeField] private KeyCode SprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode JumpKey = KeyCode.Space;

    // Time & Cooldowns

    /* Used to prevent glitchy jumping */
    private float JumpCooldown = 0;

    private void Start()
    {
        /* 
          Sprint speed is just an amplified move speed 
          Lowering the ratio will make the movement slower but higher values will amplify it
          - Feel free to change it -
        */
        SprintSpeed = MoveSpeed * SprintSpeedRatio;
        DefaultMoveSpeed = MoveSpeed;
    }

    // Player Input
    private Vector2 GetMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
       
        return new Vector2(Horizontal, Vertical) * MoveSpeed;
    }

    private void Sprint()
    {
        if(Mathf.Abs(movement.x) > 0.2f || Mathf.Abs(movement.y) > 0.2f){
            if (Input.GetKey(SprintKey))
            {
                CanSprint = true;
            }
            else
            {
                CanSprint = false;
            }
        }
    }

    private void Jump()
    {
        if(Input.GetKeyDown(JumpKey) && Time.time > JumpCooldown)
        {
            CanJump = true;
            _jumpPower = 0 ;
            JumpCooldown = Time.time + 2f;
        }
    }

    private void MovePlayer()
    {
        movement = GetMovement();

        Sprint();
        Jump();

        if(movement.x < 0)
        {
            IsLastDirectionPositive = false;
        }
        else if(movement.x > 0)
        {
            IsLastDirectionPositive = true;
        }

        if(CanSprint)
        {
            MoveSpeed = SprintSpeed;
        }
        else
        {
            MoveSpeed = DefaultMoveSpeed;
        }

        if(CanJump)
        {
            CanJump = false;
            IsJumping = true;
        }

        if(IsJumping && _jumpPower < JumpPower)
        {
            _jumpPower += 2f;
        }
        else
        {
            _jumpPower = 0;
            IsJumping = false;
        }

        if(_jumpPower > 0)
        {
            movement.y += _jumpPower;
        }
        
        transform.Translate(movement * Time.deltaTime);
    }

    private void UpdateUI()
    {
        if (!IsLastDirectionPositive)
        {
            UIPlayer.rectTransform.rotation
                 = new Quaternion(UIPlayer.rectTransform.rotation.x, 180, 0, 0);
        }
        else
        {
            UIPlayer.rectTransform.rotation
                = new Quaternion(0, 0, 0, 0);
        }
    }

    private void Update()
    {
        MovePlayer();
        /*
            ~ Use 'UpdateUI' for updating the direction of player in the game
            it depends on your input if it's a negative direction the player
            will rotate 180°, for positive direction 0°
        */ 
        UpdateUI();
    }
}
