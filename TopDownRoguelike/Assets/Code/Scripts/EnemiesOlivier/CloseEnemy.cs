using UnityEngine;
using System;
using System.Collections;

public class CloseEnemy : BaseEnemy
{
    [Header("Attack Settings")]
    public float attackReload = 1f;
    public float attackDuration = 0.2f;
    public float radiusCircularAttack;
    public float frontAttackRange;
    
    private bool IsAttacking = false; 
    private float TimeBeforeAttack; 

    [Header("Movement Settings")]
    public float movingSpeed = 1f;
    public float trackingSpeed;
    public float RateOfChangeDirection = 2f;
    public bool rotateWhileAttacking;
    public bool moveWhileAttacking;
    
    private bool isChasing = false; 
    private Vector2 randomMovement = new Vector2(0, 0);

    [Header("Unity items")]
    public Material lineMaterial;
    public GameObject tongue;
    public SpriteRenderer tongueRenderer;
    public  BoxCollider2D tongueCollider;
    private Sprite tongueSprite;
    public GameObject enemySprite;
    private CircleCollider2D attackCollider;
    private Collider2D PlayerCollider;

    [Header("Tongue Hit Cooldown")]
    public float tongueHitCooldown = 1f;
    private float lastTongueHitTime = -Mathf.Infinity;

    private void Start()
    {
        base.Start();
        TimeBeforeAttack = attackReload;
        attackCollider = gameObject.AddComponent<CircleCollider2D>();
        attackCollider.radius = frontAttackRange;
        attackCollider.isTrigger = true;
        PlayerCollider = player.GetComponent<Collider2D>();
        tongue.transform.localScale = new Vector3(1, radiusCircularAttack, 0);
        tongueRenderer.enabled = false;
        tongueCollider.enabled = false;
        tongue.SetActive(false);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        enemySprite.transform.localRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
        if ((IsAttacking && !moveWhileAttacking) || getDirectionToPlayer().magnitude < radiusCircularAttack * (2f / 3f))
        {
            body.linearVelocity = new Vector2(0, 0);
            return;
        }
        move();
    }

    private void LateUpdate()
    {
        if (IsAttacking && !rotateWhileAttacking) return;
        if (IsPlayerInlineOfSight())
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            randomMovement = direction;
            timer = RateOfChangeDirection;
            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, trackingSpeed);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
            enemySprite.transform.localRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
        }
        else
        {
            float angle = Mathf.Atan2(randomMovement.y, randomMovement.x) * Mathf.Rad2Deg;
            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, trackingSpeed);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
            enemySprite.transform.localRotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, -transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.z);
        }
    }

    private void followLastKnownPosition()
    {
        isChasing = false;
        timer = RateOfChangeDirection + 3;
        randomMovement = (player.transform.position - transform.position).normalized;
    }

    private void dontSeePlayer()
    {
        if (timer <= 0)
        {
            randomMovement = UnityEngine.Random.insideUnitCircle.normalized;
            timer = RateOfChangeDirection;
        }
        timer -= Time.deltaTime;
        body.linearVelocity = transform.right * movingSpeed;
    }

    private void followPlayer()
    {
        body.linearVelocity = transform.right * movingSpeed;
        isChasing = true;
    }

    private float getDistanceToPlayer()
    {
        return (transform.position - player.transform.position).magnitude;
    }

    public override void Attack()
    {
        if (IsAttacking) return;
        TimeBeforeAttack = attackReload;
        if (getDistanceToPlayer() < radiusCircularAttack)
        {
            IsAttacking = true;
            StartCoroutine(CircularAttack());
        }
        else if (IsPlayerInlineOfSight())
        {
            IsAttacking = true;
            StartCoroutine(FrontAttack());
        }
    }
   
    private IEnumerator FrontAttack(){


        
        float duration = attackDuration;

        float speedRate = 2 * frontAttackRange / duration; 
        tongue.transform.localScale = new Vector3(0.7f, 0, 0); 
        tongueCollider.enabled = true;
        tongueRenderer.enabled = true;
        tongue.SetActive(true);
        bool reachEnd = false;
        while (duration > 0)
        {
            if (reachEnd)
                tongue.transform.localScale -= new Vector3(0, speedRate * Time.deltaTime, 0);
            else
                tongue.transform.localScale += new Vector3(0, speedRate * Time.deltaTime, 0);
            if (tongue.transform.localScale.y >= frontAttackRange)
            {
                reachEnd = true;
                tongue.transform.localScale = new Vector3(tongue.transform.localScale.x, frontAttackRange, tongue.transform.localScale.z);
            }
            if (tongue.transform.localScale.y < 0) break;
            duration -= Time.deltaTime;
            yield return null;
        }
        TimeBeforeAttack = attackReload;
        IsAttacking = false;
        tongueRenderer.enabled = false;
        tongueCollider.enabled = false;

        

        tongue.transform.localScale =new Vector3(0.7f, radiusCircularAttack,0); // return it to normal

    }

    private IEnumerator CircularAttack()
    {
        float duration = attackDuration;
        tongueRenderer.enabled = true;
        tongueCollider.enabled = true;
        tongue.SetActive(true);
        float step = 360f / attackDuration;
        while (duration > 0)
        {
            float angle = step * Time.deltaTime;
            tongue.transform.Rotate(new Vector3(0, 0, angle));
            duration -= Time.deltaTime;
            yield return null;
        }
        TimeBeforeAttack = attackReload;
        IsAttacking = false;
        tongueRenderer.enabled = false;
        tongueCollider.enabled = false;
        tongue.transform.localRotation = Quaternion.Euler(0, 0, -90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsAttacking) return;
        if (!collision.gameObject.CompareTag("Player")) return;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (IsAttacking) return;
        if (Time.time - lastTongueHitTime < tongueHitCooldown) return;
        if (TimeBeforeAttack <= 0)
        {
            Attack();
            lastTongueHitTime = Time.time;
        }
        else TimeBeforeAttack -= Time.deltaTime;
    }

    private RaycastHit2D[] castRayAndGetCollider(Vector2 direction)
    {
        Debug.DrawRay(transform.position, direction);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, viewDistance, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }

    public override void move()
    {
        if (IsPlayerInlineOfSight())
            followPlayer();
        else
        {
            if (isChasing)
                followLastKnownPosition();
            dontSeePlayer();
        }
    }

    public override void updateStatsFromCurrentWave()
    {
        if (!scaleStatsByWave) return;
        int wave = WaveSystem.getCurrentWaveNumber();
        GetComponent<Health>().modifyHealthFromWaveNumber(wave);
        GetComponentInChildren<Tongue>().damage += wave * 3 / 2;
    }
}
