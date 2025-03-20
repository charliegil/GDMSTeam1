using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnergyBeam))] 
public class BeamAttackHandler : MonoBehaviour , IEventListener {

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float damageTickDelay = 0.5f;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDuration = 5f;
    [SerializeField] private float attackRange;

    [SerializeField] public int numTargets = 1;

    private Coroutine attackCoroutine;
    private GameObject targetEnemy;
    private EnergyBeam beam;

    private float timer;



    private void Awake() {
        
        if (!TryGetComponent(out EnergyBeam _)) {
            gameObject.AddComponent<EnergyBeam>();
        }
        beam = GetComponent<EnergyBeam>();

    }
    void OnEnable()
    {
        subscribe();
    }
    void OnDisable()
    {
        unsubscribe();
    }


    private void Update()
    {
        Attack();
    }
    
    private void Attack(){
        if (Input.GetMouseButton(1))
        {
            if (targetEnemy == null)
            {
                GameObject closestEnemy = GetClosestEnemy();
                if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange)
                {
                    targetEnemy = closestEnemy;
                    
                    
                    beam.SetTarget(targetEnemy.transform);
                }
            }

            if (targetEnemy != null && attackCoroutine == null){
                attackCoroutine = StartCoroutine(DamageOverTime());
            }
        }
        //valid
        else if (attackCoroutine != null)
        {
            CancelAttack();
        }

        // valid
        if (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.transform.position) > attackRange && attackCoroutine != null)
        {
            CancelAttack();
        }
    }
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


    private IEnumerator DamageOverTime() {
        // TODO potentially make player unable to attack while phasing
        timer = attackDuration;
        while (targetEnemy != null && targetEnemy.gameObject != null && timer>0) {
                targetEnemy.GetComponent<Health>().TakeDamage(attackDamage);
                // Play damage tick sound
                //AudioManager.Instance.Play("Damage Tick");
                timer-= damageTickDelay;
                yield return new WaitForSeconds(damageTickDelay);
        }
        CancelAttack();
    }
    private void CancelAttack() {
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        targetEnemy = null;
        EnergyBeam beam = GameObject.FindFirstObjectByType<EnergyBeam>();
        beam.SetTarget(null);
    }

    public void subscribe()
    {
        EventManager.OnBeamDamageTickDelay +=ReduceAttackDelay;
        EventManager.OnBeamAttackDuration +=IncreaseAttackDuration;
        EventManager.OnBeamAttackDamageIncrease += IncreaseAttackDamage;
        EventManager.OnBeamAttackCooldown += lowerAttackCooldown;
        
    }

    public void unsubscribe()
    {
        EventManager.OnBeamDamageTickDelay -=ReduceAttackDelay;
        EventManager.OnBeamAttackDuration -=IncreaseAttackDuration;
        EventManager.OnBeamAttackDamageIncrease -= IncreaseAttackDamage;
        EventManager.OnBeamAttackCooldown -= lowerAttackCooldown;
        
    }

    public void ReduceAttackDelay(float percentDecrease) {
        damageTickDelay /= percentDecrease;
    }
    public void IncreaseNumTargets(float numTargets) {
        this.numTargets+=(int)numTargets;
    }
    public void IncreaseAttackDuration(float addition) {
        attackDuration+= addition;
    }
    public void lowerAttackCooldown(float value){
        attackCooldown/=value;
    }
    public void IncreaseAttackDamage(float value){
        attackDamage+=value;
    }
}