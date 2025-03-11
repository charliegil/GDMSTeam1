using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ===================== REFERENCES =====================
    private InputSystem_Actions playerInputActions;
    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private PlayerAttackLineRenderer lineRenderer;


    // ===================== UI =====================

    public Slider SliderPhaseCooldown;
    public Slider SliderCurrentHealth;

    // ===================== MOVEMENT =====================
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movementDirection;
    private Vector2 currentMovement;
    private Vector2 externalVelocity = new Vector2(0, 0);
    private float currentSpeed;

    private float SpeedMultiplier = 1;

    private bool canMove = true;


    // ===================== PHASING =====================
    [SerializeField] private float phaseDuration = 1f;
    [SerializeField] private float phaseFactor = 2f;
    [SerializeField] private float phaseCooldown = 5f;
    private bool isPhasing = false;
    private float phaseInput;

    private float phaseTimer;

    // ===================== DODGING =====================
    private bool isDodging = false;
    private float dodgeTimer;
    private float dodgeCooldown;

    // ===================== ATTACKING =====================
    [SerializeField] private float attackRange = 3f;
    private float attackInput;
    private GameObject targetEnemy;
    private Coroutine attackCoroutine;

    public float attackDamage = 10f;
    public float damageTickDelay = 0.5f;
    public float critChance = 0f;
    public int numTargets = 1;

    // ===================== Health =====================
    public float maxHealth;
    private float currentHealth;
    private float healthMultiplier;

    // ===================== DEBUG & TESTING =====================

    // ===================== UNITY CALLBACKS =====================
    private void Awake() {
        playerInputActions = new InputSystem_Actions();
        //animator = GetComponent<Animator>();
        
        //spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
        lineRenderer = gameObject.transform.GetChild(0).GetComponent<PlayerAttackLineRenderer>();
        if (lineRenderer != null) {
            Debug.Log("Found line renderer");
        }
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
    }

    // ===================== INPUT HANDLING =====================
    private void OnMove(InputAction.CallbackContext context) {
        movementDirection = context.ReadValue<Vector2>();
        animator.SetFloat("moveX", movementDirection.x);
        animator.SetFloat("moveY", movementDirection.y);
    }

    private void OnPhase(InputAction.CallbackContext context) {
        phaseInput = context.ReadValue<float>();
    }

    private void OnAttack(InputAction.CallbackContext context) {
        attackInput = context.ReadValue<float>();
    }

    // ===================== GAMEPLAY LOGIC =====================
    private void Update() {
        AdjustPlayerDirection();
        if (phaseInput > 0 && !isPhasing && phaseTimer<=0 ) {
            StartCoroutine(Phase());
        } else {
            Move();
            Attack();
        }
        if(!isPhasing){
            phaseTimer-= Time.deltaTime;
        }
    }

    private void Move() {
        currentMovement = movementDirection * currentSpeed;
        if(canMove){
            rb.linearVelocity = (currentMovement + externalVelocity)* SpeedMultiplier;
        }
        else{
            rb.linearVelocity = externalVelocity* SpeedMultiplier;
        }
    }

    // TODO clean up code
    private void Attack() {

        if (attackInput > 0) {
            if (targetEnemy == null) {

                GameObject closestEnemy = GetClosestEnemy();
                if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange) {
                    targetEnemy = closestEnemy;  // Modify to find closest enemy in range
                    EnergyBeam beam = GameObject.FindFirstObjectByType<EnergyBeam>();
                    beam.SetTarget(targetEnemy.transform);
                }
            }

            if (targetEnemy != null && attackCoroutine == null) {
                attackCoroutine = StartCoroutine(DamageOverTime());
            }
        } 
        
        // Attack button released
        else if (attackCoroutine != null) {
            CancelAttack();
        }

        // Moved too far from enemy
        if (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.transform.position) > attackRange && attackCoroutine != null) {
            CancelAttack();
        }
    }

    private void CancelAttack() {
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        targetEnemy = null;
        EnergyBeam beam = GameObject.FindFirstObjectByType<EnergyBeam>();
        beam.SetTarget(null);
    }

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
            TakeDamage(1);
        }
        
    }
    public void TakeDamage(float damage) {
    
        //healthText.SetText(health.ToString());
        currentHealth -= damage;
        if(currentHealth< 0 ) OnDeath();
        animator.SetTrigger("takingDamage");

        
    }
    public void OnDeath(){

    }
    public void applySpeedModifier(float multiplier){
        SpeedMultiplier *= multiplier;
    }
    public void setSpeedModifier(float multiplier) {
         SpeedMultiplier = multiplier;
    }

    // ===================== COROUTINES =====================
    private IEnumerator Phase() {

        isPhasing = true;

        // Adjust transparency
        Color oldColor = spriteRenderer.color;
        float alpha = 0.5f;
        spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        
        animator.gameObject.transform.rotation = Quaternion.Euler(0,20,0);

        // Increase movement speed
        currentSpeed = moveSpeed * phaseFactor;

        // Allow player to phase through enemies but not environment
        // TODO: See if there is a better way to do this
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            enemy.GetComponent<Collider2D>().enabled = false;
        }

        yield return new WaitForSeconds(phaseDuration);

        // Return player to normal state
        animator.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        spriteRenderer.color = oldColor;
        currentSpeed = moveSpeed;

        foreach (GameObject enemy in enemies) {
            enemy.GetComponent<Collider2D>().enabled = true;
        }
        phaseTimer = phaseCooldown;
        isPhasing = false;
    }

    private IEnumerator DamageOverTime() {
        // TODO potentially make player unable to attack while phasing

        while (true) {
            if (targetEnemy != null && targetEnemy.gameObject != null) {
                targetEnemy.GetComponent<BaseEnemy>().TakeDamage(attackDamage);

                // Play damage tick sound
                AudioManager.Instance.Play("Damage Tick");
            } else {
                yield break;
            }

            yield return new WaitForSeconds(damageTickDelay);
        }
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
    public void applyMultiplier(skillTreeUpgrade sku){
        if(sku.getId() == 1){ // for example, apply multiplier on health

        }
        else if(sku.getId() == 1){ // for example, apply multiplier on the dash speed

        }
        else if(sku.getId() == 1){ // for example, apply multiplier on the dash cooldown

        }
        else if(sku.getId() == 1){ // for example, apply multiplier on overall speed

        }
    }
    // ===================== DEBUGGING =====================
    // void OnDrawGizmos() {
    //     Handles.DrawWireDisc(transform.position, Vector3.forward, attackRange);

    //     if (attackCoroutine != null) {
    //         Gizmos.DrawLine(transform.position, targetEnemy.transform.position);
    //     }
    // }

    // ===================== SKILL TREE UPGRADES =================================
    public void IncreaseAttackDamage(float percentIncrease) {
        attackDamage *= percentIncrease;
    }

    public void ReduceAttackDelay(float percentDecrease) {
        damageTickDelay /= percentDecrease;
    }

    public void ReducePhaseCooldown(float percentDecrease) {
        phaseCooldown /= percentDecrease;
    }

    public void IncreaseNumTargets(int numTargets) {
        this.numTargets = numTargets;
    }
    public void IncreaseMaxHealthByValue(float increase) {
        maxHealth+=increase;
    }
    public void IncreaseMaxHealthByPercentage(float increase) {
        healthMultiplier*=increase;
    }

    public void IncreaseCritChance(float percentIncrease) {
        if (critChance == 0f) {
            critChance = 0.1f;
        }

        else critChance *= percentIncrease;
    }

    public void AddFreeze() {

    }
}
