using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Prime31;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D.CharacterCollisionState2D flags;
    public float walkSpeed = 6.0f;
    public float jumpSpeed = 16.0f;
    public float gravity = 20.0f;
    public float doublejumpSpeed = 10.0f;
    public float wallXJumpAmount = 0.5f;
    public float wallYJumpAmount = 0.5f;
    public float WallRunAmount = 2f;



    //player ability toggle
    public bool canDoubleJump = true;
    public bool canWallJupm = true;
    public bool canJumpAfterWallJump = false;
    public bool canWallRun = true;
    public bool canRunAfterWallJump = true;

    //player state vaeiables
    public bool isGrounded;
    public bool isJumping;
    public bool isFactingRight;
    public bool doubleJumped;
    public bool wallJumped;
    public bool isWallRunning;


    
    //private variable
    
    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController2D _characterController;
    private bool _lastJumpWasLeft;


    // Start is called before the first frame update

    void Start()
    {
        _characterController = GetComponent<CharacterController2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (wallJumped == false) 
        {
            _moveDirection.x = Input.GetAxis("Horizontal");
            _moveDirection.x *= walkSpeed;
        }


        if (isGrounded) //player is grounded
        {
            _moveDirection.y = 0;
            isJumping = false;
            doubleJumped = false;


            if(_moveDirection.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isFactingRight = false;
            }
            else if(_moveDirection.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isFactingRight = true;

            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _moveDirection.y = jumpSpeed;
                isJumping = true;

                isWallRunning = true;

            }
        }
        else //player is in the air
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if(_moveDirection.y > 0)
                {
                    _moveDirection.y = _moveDirection.y * 0.5f;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (canDoubleJump)
                {
                    if (!doubleJumped)
                    {
                        _moveDirection.y = doublejumpSpeed;
                        doubleJumped = true;
                         
                    }
                }
            }

        }
        _moveDirection.y -= gravity * Time.deltaTime;

        _characterController.move(_moveDirection * Time.deltaTime);

        flags = _characterController.collisionState;

        isGrounded = flags.below;

        if (flags.above)
        {
            _moveDirection.y -= gravity * Time.deltaTime;    
        }
        if (flags.left || flags.right)
        {
            if (canWallRun)
            {
                if(Input.GetAxis("Vertical") > 0 && isWallRunning == true)
                {
                    _moveDirection.y = jumpSpeed / WallRunAmount;
                    StartCoroutine(WallRunWaiter());
                }
            }
            if (canWallJupm)
            {
                if(Input.GetKeyDown(KeyCode.UpArrow) || wallJumped == false && isGrounded == false)
                {
                    if(_moveDirection.x < 0)
                    {
                        _moveDirection.x = jumpSpeed * wallXJumpAmount;
                        _moveDirection.y = jumpSpeed * wallYJumpAmount;
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        _lastJumpWasLeft = false;

                    }
                    else if(_moveDirection.x > 0)
                    {
                        _moveDirection.x = -jumpSpeed * wallXJumpAmount;
                        _moveDirection.y = jumpSpeed * wallYJumpAmount;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        _lastJumpWasLeft = true;

                    }
                    StartCoroutine(WallJumpWaiter());
                    if (canJumpAfterWallJump)
                    {
                        doubleJumped = false;
                    }
                }
            }
        }
   /*     else
        {
            if (canRunAfterWallJump)
            {
                StopCoroutine(WallRunWaiter());
                isWallRunning = true ;
            }
        }*/
    }
    IEnumerator WallJumpWaiter()
    {
        wallJumped = true;
        yield return new WaitForSeconds(0.5f);
        wallJumped = false;

    }
    IEnumerator WallRunWaiter()
    {
        isWallRunning = true;
        yield return new WaitForSeconds(0.5f);
        isWallRunning = false;
    }
}
