using UnityEngine;
using UnityEngine.InputSystem;

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

   private PlayerController playerController;

   void Start(){
        playerController = GetComponent<PlayerController>();
   }
    void Update()
    {
        _fireContinuously = Input.GetKey(KeyCode.Mouse0);
        if(_fireContinuously){ //|| _fireSingle){
            float timeSinceLastFire = Time.time - _lastFireTime;
            if(timeSinceLastFire >= _timeBtwShots){
                FireBullet();
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
    
        bullet.GetComponent<Bullet>().damage*= (playerController.getAttackMultiplier()*playerController.IsAttackCritiqual());
        // Rotate bullet to face the mouse direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        // Apply velocity in the calculated direction
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * _bulletSpeed;
        // GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, transform.rotation);
        // Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        // rigidbody.linearVelocity = _bulletSpeed * transform.up;
    }
    // if(InputValue.isPressed){
    //     _fireSingle = true;
    // }
    // private void OnFire(InputValue inputValue){
    //     //Debug.Log("you click e");
        
    //     _fireContinuously = inputValue.isPressed;

    // }
}
