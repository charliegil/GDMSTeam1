using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnergyBeam))] 
public class BeamAttackHandler : MonoBehaviour , IEventListener {

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float damageTickDelay = 0.5f;

    [SerializeField] private float attackCooldown;
    [SerializeField] private int numberOfHits = 6;
    [SerializeField] private float attackRange;

    [Space(10)]
    [Tooltip("When enabled, the player doesnt have to click to attack with the beam. it automatically activates")]
    [SerializeField] private bool attackAutomatically = true;
    [SerializeField] private Slider SliderCooldown;

/*
 to have multiple beams, just add more components of this type as childs of the Player GameObject

*/




    private Coroutine attackCoroutine;
    private GameObject targetEnemy;
    private EnergyBeam beam;

    private float numberOfHitsLeft;
    
    private float targetSliderValue;

    private bool canAttack = true;


    void OnEnable()
    {
        if (!TryGetComponent(out EnergyBeam _)) {
            gameObject.AddComponent<EnergyBeam>();
        }
        beam = GetComponent<EnergyBeam>();
        if(SliderCooldown!= null) {
            SliderCooldown.maxValue = numberOfHits;
            SliderCooldown.value = numberOfHits;
            targetSliderValue = numberOfHits;
        }


        subscribe();
        if(SliderCooldown!= null ) SliderCooldown.gameObject.SetActive(true);
        if(attackAutomatically && SliderCooldown!= null )SliderCooldown.gameObject.SetActive(false);
        
       
        numberOfHitsLeft = numberOfHits;
    }
    void OnDisable()
    {
        unsubscribe();
        if(SliderCooldown!= null) SliderCooldown.gameObject.SetActive(false);

    }


    private void Update()
    {
        if(!attackAutomatically){
            AttackManually();
            setSliderValue();
                
            if (SliderCooldown != null){
                SliderCooldown.value = Mathf.Lerp(SliderCooldown.value, targetSliderValue, Time.deltaTime * 10);
            }
            canAttack = numberOfHitsLeft >= 1;
        }
        else{
            AttackAutomatically();
            canAttack = canAttack || (numberOfHitsLeft >= numberOfHits);

            if(attackCoroutine == null){
                numberOfHitsLeft += numberOfHits * (Time.deltaTime / attackCooldown);
            }
        }
    }
    
    private void AttackManually(){
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.L)){
            
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
            if(numberOfHitsLeft <=0 || !canAttack){
                CancelAttack();
            }
            if (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.transform.position) > attackRange && attackCoroutine != null){
                CancelAttack();
            }
        }
        //valid
        else {
            CancelAttack();
        }

        // valid
    }
    private void AttackAutomatically(){
        
            if(canAttack){
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
                if(numberOfHitsLeft <=0){
                    CancelAttack();
                }
                if (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.transform.position) > attackRange && attackCoroutine != null){
                    CancelAttack();
                }
            }
            
    }
    private GameObject GetClosestEnemy() {
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    GameObject closest = null;
    GameObject closestTargeted = null;
    float minDistance = Mathf.Infinity;
    float minDistanceTargeted = Mathf.Infinity;

    for (int i = 0; i < enemies.Length; i++) {
        float distanceToCurrent = Vector3.Distance(transform.position, enemies[i].transform.position);

        //Debug.Log(enemies[i]);
        Health component = enemies[i].GetComponent<Health>();
        if(component == null) continue;
        bool isTargeted = component.isTargeted;
        
       
        if (!isTargeted && distanceToCurrent < minDistance) {
            closest = enemies[i];
            minDistance = distanceToCurrent;
        }
        
        else if (isTargeted &&  distanceToCurrent < minDistanceTargeted) {
            closestTargeted = enemies[i];
            minDistanceTargeted = distanceToCurrent;
        }
    }

    return closest;
    //return closest != null ? closest : closestTargeted;
}



    private IEnumerator DamageOverTime() {
        // TODO potentially make player unable to attack while phasing
        bool first = true;// ensure the first hit takes some seconds before landing (spam issue)
        targetEnemy = GetClosestEnemy();
        if(targetEnemy!= null) beam.SetTarget(targetEnemy.transform);     
        targetEnemy.GetComponent<Health>().isTargeted = true;
        while (targetEnemy != null && targetEnemy.gameObject != null && numberOfHitsLeft>0 && canAttack) {
            if (!first) {
                targetEnemy.GetComponent<Health>().TakeDamage(attackDamage);
                targetEnemy.GetComponent<Health>().isTargeted = true;
                numberOfHitsLeft--;;
                if(numberOfHitsLeft<0){
                    canAttack = false;
                }
                
                numberOfHitsLeft = Mathf.Clamp(numberOfHitsLeft,0,numberOfHits);
                
                targetSliderValue = numberOfHitsLeft;
                yield return new WaitForSeconds(damageTickDelay);
                
                if(targetEnemy!= null) targetEnemy.GetComponent<Health>().isTargeted = false;
                targetEnemy = GetClosestEnemy();
                if(targetEnemy!= null) beam.SetTarget(targetEnemy.transform);     
            }
            else{
                first = false;
                yield return new WaitForSeconds(0.3f);     
            }         
        }
        if(attackCoroutine != null) CancelAttack();
    }
    private void CancelAttack() {
        if(attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        if(targetEnemy!= null) targetEnemy.GetComponent<Health>().isTargeted = false;
        targetEnemy = null;

        beam.SetTarget(null);
        
    }
   

private void setSliderValue()
{
    if (SliderCooldown == null) return;

    if (attackCoroutine == null){
           
        targetSliderValue += numberOfHits * (Time.deltaTime / attackCooldown);
        numberOfHitsLeft += numberOfHits * (Time.deltaTime / attackCooldown);   
    }
    

    targetSliderValue = Mathf.Clamp(targetSliderValue,0,numberOfHits);
    
   
}
    

    public void subscribe()
    {
        EventManager.OnBeamDamageTickDelay +=ReduceAttackDelay;
        EventManager.OnBeamAttackDuration +=IncreaseNumberOfHits;
        EventManager.OnBeamAttackDamageIncrease += IncreaseAttackDamage;
        EventManager.OnBeamAttackCooldown += lowerAttackCooldown;
        
    }

    public void unsubscribe()
    {
        EventManager.OnBeamDamageTickDelay -=ReduceAttackDelay;
        EventManager.OnBeamAttackDuration -=IncreaseNumberOfHits;
        EventManager.OnBeamAttackDamageIncrease -= IncreaseAttackDamage;
        EventManager.OnBeamAttackCooldown -= lowerAttackCooldown;
        
    }

    public void ReduceAttackDelay(float percentDecrease) {
        damageTickDelay /= percentDecrease;
    }
    
    public void IncreaseNumberOfHits(float addition) {
        numberOfHits+= (int)addition;
        if(SliderCooldown!= null) SliderCooldown.value = numberOfHits;
    }
    public void lowerAttackCooldown(float value){
        attackCooldown/=value;
    }
    public void IncreaseAttackDamage(float value){
        attackDamage*=value;
    }
}