using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController),typeof(SpellManager))]
public class Player : MonoBehaviour {

    public float maxHitPoints;
    public float hitPoints { get; private set; }
    public PlayerController Controller { get; private set; }
    public Transform _transform { get { return transform; } private set { } }
    public Vector2 spriteSize { get; private set; }
    public bool isFacingRight { get; private set; }

    [SerializeField]
    float 
        gravity, 
        moveVelocity, 
        jumpVelocity;
    SpellManager spellManager;
    Vector2 _velocity;
    bool _isDead;
    Animator animator;
    
    
	void Start ()
    {
        Controller = GetComponent<PlayerController>();
        spellManager = GetComponent<SpellManager>();
        spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        _isDead = false;
        hitPoints = maxHitPoints;
        animator = GetComponent<Animator>();
        isFacingRight = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleSpells();
        
        if (hitPoints <= 0 && !_isDead)
        {
            GameManager.instance.GameOver();
            _isDead = true;
        }
    }

    void HandleMovement()
    {
        Flip();
        var _collisionInfo = Controller.collisionInfo;
        
        //prevent gravity buildup and prevent from sticking top
        if (Controller.collisionInfo.below || _collisionInfo.above)
        {
            _velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _collisionInfo.below)
        {
            _velocity.y = jumpVelocity;            
        }

        _velocity.x = Input.GetAxisRaw("Horizontal") * moveVelocity;
        _velocity.y += gravity * Time.deltaTime;
        Vector2 deltaMovement = _velocity * Time.deltaTime;
        Controller.Move(ref deltaMovement);
        // round deltaMovement to zero as it will get infinitely small and the animator logic wouldnt work
        if (deltaMovement == Vector2.zero)
            animator.SetBool("fly", false);
        else
            animator.SetBool("fly", true);
    }

    void HandleSpells()
    {
        if (Input.GetKey(KeyCode.Q) && spellManager.meleeSpell.CanCast())
        {
            spellManager.meleeSpell.Cast(this);
            animator.SetTrigger("melee");
        }

        if (Input.GetKey(KeyCode.W) && spellManager.blinkSpell.CanCast())
        {
            bool pressLeft = Input.GetKey(KeyCode.LeftArrow);
            bool pressRight = Input.GetKey(KeyCode.RightArrow);

            // check if the right combo is pressed
            bool canBlink = pressLeft & !pressRight | !pressLeft & pressRight;

            if (canBlink)
            {
                Debug.Log("Can blink to " + (pressLeft ? "left" : "right"));
                Vector2 direction = pressLeft ? Vector2.left : Vector2.right;
                spellManager.blinkSpell.Cast(this, direction);
                animator.SetTrigger("blink");
            }
        }

        if (Input.GetKey(KeyCode.E) && spellManager.fireballSpell.CanCast())
        {
            spellManager.fireballSpell.Cast(this);
            animator.SetTrigger("fireball");
        }

        if (Input.GetKey(KeyCode.R) && spellManager.meteorSpell.CanCast())
        {
            spellManager.meteorSpell.Cast(this);
            animator.SetTrigger("ulti");
        }
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
    }

    void Flip()
    {
        Vector2 _localScale = transform.localScale;
        // sprite is facing -x direction by default
        if (!isFacingRight && _velocity.x > 0 || isFacingRight && _velocity.x < 0)
        {
            _localScale.x *= -1;
            transform.localScale = _localScale;
            isFacingRight = !isFacingRight;
        }
    }

    public void IndicateBlink()
    {
        StartCoroutine(_Blink());
    }

    IEnumerator _Blink()
    {
        SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.enabled = true;
    }
}
