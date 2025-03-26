using UnityEngine;


public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    private int _gunOffset;
    private bool _fireContinuously;
    [SerializeField]
    private float _timeBtwShots;
    private float _lastFireTime;
    private bool _fireSingle;
    public Animator animator;

    private GameObject bulletParent;

   private PlayerController playerController;

    public void AddSpeedFire(float reduceIntervalGun){
        _timeBtwShots-=reduceIntervalGun;
    }

    public void AddSpeedBullet(float addBulletSpeed){
        _bulletSpeed+=addBulletSpeed;
    }

   void Start(){
        playerController = GetComponent<PlayerController>();
        bulletParent = new GameObject("bullets");
   }
    void Update()
    {
        _fireContinuously = Input.GetKey(KeyCode.Mouse0);
        if(_fireContinuously){ //|| _fireSingle){
            float timeSinceLastFire = Time.time - _lastFireTime;
            if(timeSinceLastFire >= _timeBtwShots){
                FireBullet();
                animator.SetTrigger("attack");
                _lastFireTime = Time.time;
                //_fireSingle = false;
            }
        }
        
    }
    
    private void FireBullet(){
         // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure it's on the same plane as the player

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Instantiate bullet at player's position
        GameObject bullet = Instantiate(_bulletPrefab, transform.position+ (Vector3)(_gunOffset*direction), Quaternion.identity);
        GameObject bullet2 = Instantiate(_bulletPrefab, transform.position+ (Vector3)(_gunOffset*direction), Quaternion.identity);
        GameObject bullet3 = Instantiate(_bulletPrefab, transform.position+ (Vector3)(_gunOffset*direction), Quaternion.identity);
        directionBullet(bullet, direction);
        directionBullet(bullet2, new Vector2(direction.x+0.2f, direction.y+0.2f));
        directionBullet(bullet3, new Vector2(direction.x-0.2f, direction.y-0.2f));
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
    
}
