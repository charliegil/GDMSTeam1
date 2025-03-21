using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour , IEventListener
{
    // ===================== REFERENCES =====================
    
    private InputActionReference pointerPosition;
    private InputSystem_Actions playerInputActions;
    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    


    // ===================== UI =====================
    [Header("UI elements")]
    public UnityEngine.UI.Slider SliderPhaseCooldown;
   
    public GameObject LoseScreen;
 

    // ===================== MOVEMENT =====================
    
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movementDirection;
    private Vector2 currentMovement;
    private Vector2 externalVelocity = new Vector2(0, 0);
    private float currentSpeed;

   private float speedMultiplier = 1;

    private bool canMove = true;

    private Vector2 pointerInput;
    // ===================== PHASING =====================
    [Header("Phase Settings")]
    [SerializeField] private float phaseDuration = 1f;
    [SerializeField] private float phaseFactor = 2f;
    [SerializeField] private float phaseCooldown = 5f;
    private bool isPhasing = false;
    private float phaseInput;

    private float phaseTimer;

    // ===================== DASH =====================
    private bool isDashing = false;
    private float DashTimer;
    private float DashCooldown;
    private float DashVelocity; // will remain constant
    private float Dashduration; // will remain constant

    // ===================== DEFENCE =====================
    private float defenceBoost = 1; 

    public static bool Invincible =  false;

    // ===================== ATTACKING =====================
    // [SerializeField]
    // private GameObject _bulletPrefab;
    // [SerializeField]
    // private float _bulletSpeed;

    // private bool _fireContinuously;
    private float attackInput;
    private GameObject targetEnemy;
    private Coroutine attackCoroutine;

    [Header("Attack Settings")]
   
    public float attackMultiplier = 1f;
    

// ===================== CRITIQUAL =====================
    [Header("critiqual hits Settings")]
    [Range(0f, 1f)] public float critChance = 0.04f; // starting value

    /// <summary>
    /// below this amount of hp, all attacks are critiqual attacks
    /// </summary>
    public static int allCritiqualHits = 0;

    public static int InstantKillHP = 0;

    public float critiqualHitFactor = 2;
    

    // ===================== Health =====================
     [Header("Healths Settings")]
    public float maxHealth;
    private float currentHealth;

    public HealthManager healthManager;
    

    // ===================== DEBUG & TESTING =====================

    // ===================== UNITY CALLBACKS =====================
    public  void Awake() {
        playerInputActions = new InputSystem_Actions();
        //animator = GetComponent<Animator>();
        subscribe();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
        
        healthManager.totalHP = maxHealth;
        healthManager.currentHP = maxHealth;
        currentHealth = maxHealth;

        gameObject.layer = LayerMask.NameToLayer("Player");

    }


    private void OnEnable() {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
        playerInputActions.Player.Phase.performed += OnPhase;
        playerInputActions.Player.Phase.canceled += OnPhase;
        playerInputActions.Player.Attack.performed += OnAttack;
        playerInputActions.Player.Attack.canceled += OnAttack;
    }

    private void OnDisable() {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Phase.performed -= OnPhase;
        playerInputActions.Player.Phase.canceled -= OnPhase;
        playerInputActions.Player.Attack.performed -= OnAttack;
        playerInputActions.Player.Attack.canceled -= OnAttack;
        unsubscribe();
    }

    // ===================== INPUT HANDLING =====================
    private void OnMove(InputAction.CallbackContext context) {
        movementDirection = context.ReadValue<Vector2>();
        //animator.SetFloat("moveX", movementDirection.x);
        //animator.SetFloat("moveY", movementDirection.y);
    }

    private void OnPhase(InputAction.CallbackContext context) {
        phaseInput = context.ReadValue<float>();
    }

    private void OnAttack(InputAction.CallbackContext context) {
       // _fireContinuously = context.performed;
        attackInput = context.ReadValue<float>();
    }

    // ===================== GAMEPLAY LOGIC =====================
    private void Update() {

        AdjustPlayerDirection();
        if (phaseInput > 0 && !isPhasing && phaseTimer<=0 ) {
            StartCoroutine(Phase());
        } 
        else {
            Move();
        }
        if(!isPhasing){
            SliderPhaseCooldown.value = (phaseCooldown-phaseTimer)/phaseCooldown;
            phaseTimer-= Time.deltaTime;
        }
        float currentSpeed = rb.linearVelocity.magnitude;
        animator.SetFloat("speed" , currentSpeed);

    }


    private void Move() {
        currentMovement = movementDirection * currentSpeed * speedMultiplier;
        if(canMove){
            rb.linearVelocity = (currentMovement + externalVelocity);
        }
        else{
            rb.linearVelocity = externalVelocity;
        }
    }

    // TODO clean up code
    
   
    


    

/// <summary>
/// flips the sprite to account for his movement
/// </summary>
    private void AdjustPlayerDirection() {
        spriteRenderer.flipX = movementDirection.x < 0;
    }

    // TODO optimize?
    private GameObject GetClosestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < enemies.Length; i++) {
            float distanceToCurrent = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (distanceToCurrent < minDistance) {
                closest = enemies[i];
                minDistance = distanceToCurrent;
            }
        }

        return closest;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile")) {
            Debug.Log("I'm hit!");
            TakeDamage(3);
        }
        
    }
    public void TakeDamage(float damage) {
    
       
        //Debug.Log("health: " + currentHealth);
        if(damage > 0){
            damage/= defenceBoost;
            
           
            if(Invincible) {
                Debug.Log("the player is invincible now");
                return;}
            
            currentHealth -= damage;
            animator.SetTrigger("takingDamage");
            if(currentHealth <= 0 ) OnDeath();
        }
        else{
            currentHealth -= damage;
            currentHealth = Math.Min(currentHealth,maxHealth);
        }
        //healthManager.addHP(-damage,true);
        healthManager.setHP(currentHealth,true);
        
    }
    

/// <summary>
/// tells if the attack is a critiqual attack
/// </summary>
/// <returns>the multiplier of the attack (critiqual multiplier)</returns>
    public float IsAttackCritiqual(){
            
    /// <summary>
    /// below this amount of hp, all attacks are critiqual attacks
    /// </summary>
        if(currentHealth <= InstantKillHP) return 100; // some sort of last chance, when below 1hp, all attacks are instant kills
        if(currentHealth<=allCritiqualHits) return critiqualHitFactor;
        float random = UnityEngine.Random.Range(0f, 1f);
        //Debug.Log(random );
        if(random <= critChance) return critiqualHitFactor;
        //Debug.Log("no attack mutliplier");
        return 1;
    }
    public float getAttackMultiplier(){
        return attackMultiplier;
    }


    public void OnDeath(){
        Debug.Log("im dead");
        LoseScreen.SetActive(true);
        EventManager.PlayerDied();
        //Time.timeScale = 0;

    }
    

    // ===================== COROUTINES =====================
    private IEnumerator Phase() {

        isPhasing = true;

        // Adjust transparency
        Color oldColor = spriteRenderer.color;
        float alpha = 0.5f;
        spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        
        spriteRenderer.gameObject.transform.rotation = Quaternion.Euler(0,20,0);

        // Increase movement speed
        currentSpeed = moveSpeed * phaseFactor;

        // Allow player to phase through enemies but not environment
        // TODO: See if there is a better way to do this
       int playerLayer = LayerMask.NameToLayer("Player");
       int monsterLayer = LayerMask.NameToLayer("Monster");

       Physics2D.IgnoreLayerCollision(playerLayer,monsterLayer,true);
       
        float timer = phaseDuration;
        while(timer>0){
            SliderPhaseCooldown.value = (timer)/phaseDuration;
            timer-= Time.deltaTime;
            yield return null;
        }
        

        // Return player to normal state
        spriteRenderer.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        spriteRenderer.color = oldColor;
        currentSpeed = moveSpeed;

        
        Physics2D.IgnoreLayerCollision(monsterLayer,playerLayer,false);
        phaseTimer = phaseCooldown;
        isPhasing = false;
    }

    

    public void setExternalVelocity(Vector2 vec){
        externalVelocity = vec;
    }

    public void addExternalVelocity(Vector2 vec){
        externalVelocity+=vec;
    }

    public Vector2 getExternalVelocity(){return externalVelocity;}

    
    
    
    
    
    
    
    
    
    public void SetTimerVelocity(float time, Vector2 direction, bool canMoveDuring){ // this function applies a velocity for a fixed amount of time
        StartCoroutine(SetTimeVelocityEnumerator(time, direction, canMoveDuring));
    }

    private IEnumerator SetTimeVelocityEnumerator(float time, Vector2 direction, bool canMoveDuring){
            bool before = canMove;
            canMove = canMoveDuring;;
            addExternalVelocity(direction);
            
            yield return new WaitForSeconds(time);

            addExternalVelocity(-direction);
            canMove = before;
    }
    
    
    public void MovePlayerTowards(float time, Vector2 point , bool canMoveDuring){
        StartCoroutine(MovePlayerTowardsEnumerator(time,point,canMoveDuring));
    }
    private IEnumerator MovePlayerTowardsEnumerator(float time, Vector3 point, bool canMoveDuring){
        // this function will move the player towards a certain point. it will adjust every delta time to make sure it goes towards that point
        // if time is 0 , this function will stop when it reach that point
        // we cant just use Vector2.MoveTowards() because we are using velocity to move the player
            bool before = canMove;
            canMove = canMoveDuring;
            float currentTime = 0;
            Vector2 oldDirection  = new Vector2(0,0);
            
            while((time ==-1 && (transform.position-point).magnitude > 0.1) || (time != -1 && currentTime < time)){
                Vector3 direction = (transform.position - point).normalized;
                addExternalVelocity(-oldDirection);
                
                addExternalVelocity(direction * Time.deltaTime * 10); // need to make 10 into an actual variable
                oldDirection = (direction * Time.deltaTime * 10);
                currentTime += Time.deltaTime;
                yield return null;

            }
            addExternalVelocity(-oldDirection);
            canMove = before;
    }
    public void setCanMove(bool move){
        canMove = move;
    }
    // ===================== DEBUGGING =====================
    // void OnDrawGizmos() {
    //     Handles.DrawWireDisc(transform.position, Vector3.forward, attackRange);

    //     if (attackCoroutine != null) {
    //         Gizmos.DrawLine(transform.position, targetEnemy.transform.position);
    //     }
    // }

    // ===================== SKILL TREE UPGRADES   ========= POWER UPS BOOSTS =================================
    public void IncreaseAttackDamage(float percentIncrease) {
        Debug.Log("the attack multiplier is getting increased from : " + percentIncrease);
        attackMultiplier *= percentIncrease;
    }

    
    public void IncreaseDefenceBoost(float value){
        Debug.Log("the defence multiplier is getting increased from : " + value);
        defenceBoost *= value;
    }

    
    public void IncreaseMaxHealthByValue(float increase) {
        maxHealth+=increase;
        healthManager.totalHP = maxHealth;
        currentHealth = Math.Min(currentHealth,maxHealth);
        healthManager.setHP(currentHealth,true);
    }

    public void IncreaseMaxHealthByPercentage(float increase) {
       
        maxHealth*=increase;
        healthManager.totalHP = maxHealth;
        currentHealth = Math.Min(currentHealth,maxHealth);
        healthManager.setHP(currentHealth,true);
    }

    public void IncreaseCritChance(float percentIncrease) {
        if (critChance == 0f) {
            critChance = 0.1f;
        }

        else critChance *= percentIncrease;
    }

    public void IncreaseSpeedMultiplier(float value){
        Debug.Log("the speed multiplier is getting increased from : " + value);
        speedMultiplier*= value;
    }
    public void ReducePhaseDuration(float value){
        phaseDuration/=value;
    }
    public void ReducePhaseCooldown(float percentDecrease) {
        phaseCooldown /= percentDecrease;
    }

    
    
    

    public void AddFreeze() {

    }

/// <summary>
/// subscribing to the events in this class must be done by this method
/// </summary>
    public void subscribe()
    {
        EventManager.OnPlayerTakeDamage += TakeDamage;
        EventManager.OnCriticalHitBoost += IncreaseCritChance;
        EventManager.OnAttackBoost += IncreaseAttackDamage;
        EventManager.OnSpeedBoost += IncreaseSpeedMultiplier;
        EventManager.OnIncreaseMaxHealth += IncreaseMaxHealthByPercentage;
        EventManager.OnDefenceBoost += IncreaseDefenceBoost;
        EventManager.OnPhaseCooldownBoost += ReducePhaseCooldown;
        EventManager.OnPhaseDurationBoost += ReducePhaseDuration;

     
    }
/// <summary>
/// unsubscribing to the events in this class must be done by this method
/// </summary>
    public void unsubscribe()
    {
        EventManager.OnPlayerTakeDamage -= TakeDamage;
        EventManager.OnCriticalHitBoost -= IncreaseCritChance;
        EventManager.OnAttackBoost -= IncreaseAttackDamage;
        EventManager.OnSpeedBoost -= IncreaseSpeedMultiplier;
        EventManager.OnIncreaseMaxHealth -= IncreaseMaxHealthByPercentage;
        EventManager.OnDefenceBoost -= IncreaseDefenceBoost;
        EventManager.OnPhaseCooldownBoost -= ReducePhaseCooldown;
        EventManager.OnPhaseDurationBoost -= ReducePhaseDuration;

    }
    
}
