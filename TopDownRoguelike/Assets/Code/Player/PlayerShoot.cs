using UnityEngine;


public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private static float _bulletSpeed = 10;
    [SerializeField]
    private int _gunOffset;
    private bool _fireContinuously;
    
    [Range(0f, 2f)] public float timeBetweenShoot = 0.7f;
    public static float _timeBtwShots = 0.7f;


    private static float _timeBtwShots = 0.25f;

    private bool _fireSingle;
    public Animator animator;

    private GameObject bulletParent;

    private PlayerController playerController;

    public int angleBetweenBullets = 8;
    private float timer;

    public static int numPojectile =0;

    public static void ReduceTimeBetweenShots(float reduceIntervalGun){
        _timeBtwShots-=reduceIntervalGun;
    }

    public static void AddSpeedBullet(float addBulletSpeed){
        _bulletSpeed+=addBulletSpeed;
    }
    public static void AddDamage(int dmg){
        BulletState.AddDamage(dmg);
    }
    public static void AddBullet(){
        //something add bullet
    }

   void Start(){
        playerController = GetComponent<PlayerController>();
        bulletParent = new GameObject("bullets");
        timer =0;
        _timeBtwShots = timeBetweenShoot;
        
   }
    void Update()
    {
        //Debug.Log("time between shoots: "+_timeBtwShots);
        _fireContinuously = Input.GetKey(KeyCode.Mouse0);
        if(_fireContinuously){ //|| _fireSingle){
            if(timer <= 0){  
                FireBullet();
                timer = _timeBtwShots;
                //animator.SetTrigger("attack");
                //_fireSingle = false;
            }
        }
        timer-=Time.deltaTime;
        
    }
    
    private void FireBullet(){
         // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure it's on the same plane as the player

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Instantiate bullet at player's position

        GameObject bullet = Instantiate(_bulletPrefab, transform.position+ (Vector3)(_gunOffset*direction), Quaternion.identity);
        directionBullet(bullet, direction);
        int change = 1;
        for(int i=1; i<=numPojectile;i++){
            
            GameObject bullet2 = Instantiate(_bulletPrefab, transform.position+ (Vector3)(_gunOffset*direction), Quaternion.identity);
            int sign = i % 2 == 0 ? -1 : 1;
            float angleOffset = change*angleBetweenBullets * sign;
            if(sign == -1) change++;

            Vector2 newDirection = RotateVector(new Vector2(direction.x, direction.y),angleOffset);
            directionBullet(bullet2,newDirection);
        }

    }
    private void directionBullet(GameObject bullet, Vector2 direction){
        //bullet.GetComponent<Bullet>().damage*= playerController.getAttackMultiplier()*playerController.IsAttackCritiqual();
        // Rotate bullet to face the mouse direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        // Apply velocity in the calculated direction
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * _bulletSpeed + playerController.GetComponent<Rigidbody2D>().linearVelocity* 0.3f; // Get player's velocity
        AudioManager.instance.PlaySound("Fire");
        bullet.transform.SetParent(bulletParent.transform);

    }
    Vector2 RotateVector(Vector2 v, float angle){
    float rad = angle * Mathf.Deg2Rad;
    float cos = Mathf.Cos(rad);
    float sin = Mathf.Sin(rad);
    return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
    
}
