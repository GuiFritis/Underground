using System.Collections;
using UnityEngine;
using DG.Tweening;
using Sounds;

[RequireComponent(typeof(Rigidbody2D), typeof(HealthBase))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rigidbody2d;
    public Collider2D collider2d;
    public HealthBase healthBase;
    public SpriteRenderer sprite;    
    public Animator animator;

    [Space]
    public float imunityTime = 1f;
    public AudioClip hurtSfx;

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
    public AudioRandomPlayClip jumpSFX;
    public float jumpForce = 5f;
    public float coyoteTime = 0.2f;
    public float jumpDelayTime = 0.1f;

    [Header("Fall")]
    public ParticleSystem landVFX;
    public AudioRandomPlayClip landSFX;
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
    private string _animatorPlatformMoving = "PlatformMoving";
    private string _animatorShoot = "Shoot";

    [Header("Attack")]
    public int damage = 1;
    public Vector2 attackSize = Vector2.zero;
    public float attackRate = .5f;
    public float rechargeRate = 1f;
    public LayerMask targetLayer;
    public ParticleSystem attackVFX;
    public AudioClip shotSFX;
    private float _attackCooldown = 0f;
    private Collider2D[] _hitColliders;

    [Header("Special Attack")]
    public int comboAttack = 3;
    public int specialAttackDamage = 2;
    public Vector2 specialAttackSize = Vector2.zero;
    public ParticleSystem specialAttackVFX;
    public ParticleSystem specialAttackBlastVFX;
    public AudioClip specialShootSFX;
    private int _combo = 1;

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
            healthBase.OnDamage += PlayerDamaged;
        }
        SetInputs();
        _velocityX = 0f;
    }

    void OnDisable()
    {
        _inputs.Disable();
    }

    private void SetInputs()
    {
        _inputs = new Gameplay();
        _inputs.Enable();

        _inputs.Keyboard.Move.started += ctx => StartMove(ctx.ReadValue<float>());
        _inputs.Keyboard.Move.canceled += ctx => StopMove();

        _inputs.Keyboard.Jump.started += ctx => JumpUp();

        _inputs.Keyboard.Attack.started += ctx => Attack();
    }
    
    void Update()
    {
        if(_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }

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
            transform.DORotate(Vector3.up * (_velocityX < 0 ? 180f : 0f), .01f);
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

    private void AnimateJump(){
        animator?.SetTrigger(_animatorJump);
        jumpVFX?.Play();
        jumpSFX?.PlayRandomAudio();
    }
    #endregion

    private void AnimateFall(){
        if(!_grounded){
            animator?.SetBool(_animatorFalling, true);
        }
    }

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
        
        if(_jumpDelayTimer > 0)
        {
            _jumpDelayTimer = 0;
            rigidbody2d.velocity *= Vector2.right;
            JumpUp();
        }
    }

    private void AnimateLanding(){   
        animator?.SetBool(_animatorFalling, false);
        landVFX?.Play();
        landSFX?.PlayRandomAudio();
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

    #region ATTACK
    private void Attack()
    {
        if(_attackCooldown <= 0)
        {
            if(_combo == comboAttack)
            {
                SpecialAttack();
            }
            else
            {
                NormalAttack();
            }
        }
    }

    private void NormalAttack()
    {
        _hitColliders = Physics2D.OverlapCapsuleAll(
            transform.position + attackSize.x/2 * Vector3.right * (transform.rotation.y != 0f ? -1 : 1),
            attackSize,
            CapsuleDirection2D.Horizontal,
            0,
            targetLayer
        );

        if(_hitColliders.Length > 0)
        {
            foreach (var item in _hitColliders)
            {
                item.GetComponent<HealthBase>()?.TakeDamage(damage);
                HitVFX_Pool.Instance.Play(item.transform.position);
            }
        }
        attackVFX?.Play();
        if(shotSFX != null)
        {
            SFX_Pool.Instance.Play(shotSFX);
        }
        StartCoroutine(AttackAnimation());
        _attackCooldown = attackRate;
        _combo++;
    }

    private void SpecialAttack()
    {
        _hitColliders = Physics2D.OverlapCapsuleAll(
            transform.position + specialAttackSize.x/2 * Vector3.right * (transform.rotation.y != 0f ? -1 : 1),
            specialAttackSize,
            CapsuleDirection2D.Horizontal,
            0,
            targetLayer
        );

        if(_hitColliders.Length > 0)
        {
            foreach (var item in _hitColliders)
            {
                item.GetComponent<HealthBase>()?.TakeDamage(specialAttackDamage);
                HitVFX_Pool.Instance.Play(item.transform.position);
            }
        }
        specialAttackVFX?.Play();
        specialAttackBlastVFX?.Play();        
        if(specialShootSFX != null)
        {
            SFX_Pool.Instance.Play(specialShootSFX);
        }
        StartCoroutine(AttackAnimation());
        _attackCooldown = rechargeRate;
        _combo = 1;
    }

    private IEnumerator AttackAnimation()
    {
        animator.SetBool(_animatorShoot, true);
        yield return new WaitForSeconds(.3f);
        animator.SetBool(_animatorShoot, false);
    }
    #endregion

    #region PLATFORM_MOVE
    public void StartPlatformMove()
    {
        _inputs.Disable();
        rigidbody2d.bodyType = RigidbodyType2D.Static;
        animator.SetBool(_animatorPlatformMoving, true);
    }

    public void EndPlatformMove()
    {
        _inputs.Enable();
        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool(_animatorPlatformMoving, false);
    }
    #endregion

    private void PlayerDamaged(HealthBase hp, int damage)
    {
        StartCoroutine(BecomeImune());
        if(hurtSfx != null)
        {
            SFX_Pool.Instance.Play(hurtSfx);
        }
    }

    private IEnumerator BecomeImune()
    {
        healthBase.BecomeImune(true);
        yield return new WaitForSeconds(imunityTime);
        healthBase.BecomeImune(false);
    }

    private void OnPlayerDeath(HealthBase hp){
        healthBase.OnDeath -= OnPlayerDeath;
        _inputs.Disable();
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
