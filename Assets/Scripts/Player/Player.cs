using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(HealthBase))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rigidbody2d;
    public Collider2D collider2d;
    public HealthBase healthBase;
    public SpriteRenderer sprite;    
    public Animator animator;

    [Header("Movement")]
    public float maxSpeed = 24f;   
    public float speedBost = 2f;
    public float groundFriction = 1.5f;
    public float airSpeedMultiplier = 1.2f;
    public float airFriction = 1f;
    private float _velocityX = 0f;
    private float _accelerationX = 0f;

    [Header("Jump")]
    public ParticleSystem jumpVFX;
    public float jumpForce = 5f;
    public float coyoteTime = 0.2f;
    public float jumpDelayTime = 0.1f;

    [Header("Fall")]
    public ParticleSystem landVFX;
    public Vector2 landingCheckBox = Vector2.zero;
    public float heightCheckBoxOffset = 0.5f;
    [Range(-100, 0)]
    public float maxFallingSpeed = -50f;
    public LayerMask groundLayer;
    private bool _grounded = true;
    private bool _doubleJumped = false;
    private bool _jumping = false;
    private float _jumpDelayTimer = 0f;
    // [Header("VFX")]
    // public ParticleSystem moveVFX;

    // public AudioSource deathSfx;
    // public AudioSource jumpSfx;
    // public AudioSource landSfx;

    private Gameplay _inputs;
    private string _animatorJump = "Jump";
    private string _animatorFalling = "Falling";
    private string _animatorRunning = "Running";

    void OnValidate()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        healthBase = GetComponent<HealthBase>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Awake()
    {
        if(healthBase != null){
            healthBase.OnDeath += OnPlayerDeath;
        }
        SetInputs();
        _velocityX = 0f;
    }

    private void SetInputs()
    {
        _inputs = new Gameplay();
        _inputs.Enable();

        _inputs.Keyboard.Move.started += ctx => StartMove(ctx.ReadValue<float>());
        _inputs.Keyboard.Move.canceled += ctx => StopMove();

        _inputs.Keyboard.Jump.started += ctx => JumpUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jumping();
        CheckLanding();
    }

    #region MOVE
    private void Move()
    {
        if(_velocityX != 0)
        {
            sprite.flipX = _velocityX < 0;
            float targetSpeed = maxSpeed * _velocityX;

            if(_grounded)
            {
                if(Mathf.Abs(rigidbody2d.velocity.x) < .2f)
                {
                    rigidbody2d.AddForce(Vector2.right * speedBost * _velocityX, ForceMode2D.Force);
                } 
                else if(Mathf.Sign(rigidbody2d.velocity.x) != Mathf.Sign(_velocityX))
                {
                    rigidbody2d.AddForce(Mathf.Abs(rigidbody2d.velocity.x) * Vector2.right * _velocityX, ForceMode2D.Impulse);
                }
            }

            _accelerationX = (targetSpeed - rigidbody2d.velocity.x) * (_grounded ? 1f : airSpeedMultiplier);


            rigidbody2d.AddForce(Vector2.right * _accelerationX, ForceMode2D.Force);
        }
    }

    private void StartMove(float direction)
    {
        _velocityX = direction;
        animator.SetBool(_animatorRunning, true);
    }

    private void StopMove()
    {
        _velocityX = 0;
        animator.SetBool(_animatorRunning, false);
    }
    #endregion

    #region JUMP

    private void JumpUp()
    {
        if(_grounded || !_doubleJumped)
        {
            if(!_grounded)
            {
                _doubleJumped = true;
            }
            _jumpDelayTimer = 0;
            _jumping = true;
            _grounded = false;
            rigidbody2d.velocity *= Vector2.right;
            rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AnimateJump();
            if(jumpVFX != null)
            {
                jumpVFX.Play();
            }
        } else if(!_grounded) {
            _jumpDelayTimer = jumpDelayTime;
        }
    }

    private void Jumping()
    {
        if(_jumping && rigidbody2d.velocity.y < .2f)
        {
            _jumping = false;
            AnimateFall();
        }

        if(rigidbody2d.velocity.y < maxFallingSpeed)
        {
            Vector2 velocity = rigidbody2d.velocity;
            velocity.y = maxFallingSpeed;
            rigidbody2d.velocity = velocity;
        }

        if(_jumpDelayTimer > 0)
        {
            _jumpDelayTimer -= Time.fixedDeltaTime;
        }
    }
    #endregion

    #region LAND
    private void CheckLanding()
    {        
        if(!_grounded)
        {
            var groundCollision = Physics2D.OverlapBox(
                (Vector2)transform.position - (Vector2.up * heightCheckBoxOffset), 
                landingCheckBox, 
                0, 
                groundLayer
            );

            if(groundCollision != null && !collider2d.IsTouching(groundCollision))
            {
                Land();
            }
        }
    }

    private void Land()
    {   
        _jumping = _doubleJumped = false;
        _grounded = true;     
        AnimateLanding();
        // if(landVFX != null)
        // {
        //     landVFX.Play();
        // }
        
        if(_jumpDelayTimer > 0)
        {
            _jumpDelayTimer = 0;
            rigidbody2d.velocity *= Vector2.right;
            JumpUp();
        }
    }
    #endregion

    #region FRICTION
    private void GroundedFriction()
    {
        _accelerationX = Mathf.Min(Mathf.Abs(rigidbody2d.velocity.x), groundFriction);

        _accelerationX *= Mathf.Sign(rigidbody2d.velocity.x) * -1;

        rigidbody2d.AddForce(Vector2.right * _accelerationX, ForceMode2D.Impulse);
    }
    #endregion

    private void OnPlayerDeath(HealthBase hp){
        healthBase.OnDeath -= OnPlayerDeath;
        // PlayDeathSFX();
        // _currentPlayer.SetTrigger("triggerDie");
    }

    private void AnimateJump(){
        animator?.SetTrigger(_animatorJump);
    }

    private void AnimateFall(){
        if(!_grounded){
            animator?.SetBool(_animatorFalling, true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !_grounded){
            AnimateLanding();
            _grounded = true;
            _doubleJumped = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if((groundLayer.value & (1 << collision.gameObject.layer)) != 0 && _velocityX == 0)
        {
            if(Mathf.Abs(rigidbody2d.velocity.x) > groundFriction/5)
            {
                GroundedFriction();
            } 
            else 
            {
                rigidbody2d.velocity = rigidbody2d.velocity * Vector2.up;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _grounded){
            _grounded = false;
            AnimateFall();
        }
    }

    private void AnimateLanding(){   
        animator?.SetBool(_animatorFalling, false);
        // PlayLandSFX();
        // PlayMoveVFX();
    }

    // private void PlayJumpVFX(){
    //     VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.JUMP, transform.position);
    // }

    // private void PlayJumpSFX(){
    //     if(jumpSfx != null){
    //         jumpSfx.Play();
    //     }
    // }

    // private void PlayLandSFX(){
    //     if(landSfx != null){
    //         landSfx.Play();
    //     }
    // }

    // private void PlayDeathSFX(){
    //     if(deathSfx != null){
    //         deathSfx.Play();
    //     }
    // }

    // private void PlayMoveVFX(){
    //     if(moveVFX != null){
    //         moveVFX.Play();
    //     }
    // }
    // private void StopMoveVFX(){
    //     if(moveVFX != null){
    //         moveVFX.Stop();
    //     }
    // }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube((Vector2)transform.position - (Vector2.up * heightCheckBoxOffset), landingCheckBox);
    }
}
