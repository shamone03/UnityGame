using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed;
    private float maxHealth = 100;
    private int grenades = 1;
    [SerializeField] int score;
    private float runspeed = 20;
    private float walkspeed = 10;
    private float boostspeed = 15f;
    private float airspeed = 7.5f;
    public CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f * 2;

    [SerializeField] private float jumpHeight;
    [SerializeField]
    private float jumps = 2;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject viewPoint;
    public GameObject bulletImpact;
    //public float timeBetweenShots = 0.1f;

    private float shotCounter;
    
    [SerializeField] float maxHeat = 30;
    //[SerializeField] float heatPerShot = 1;
    [SerializeField] float cooldownRate = 4;
    [SerializeField] float overheatCoolRate = 5;
    [SerializeField] bool overheated;
    [SerializeField] float heatCounter;
    public ParticleSystem muzzleFlash;
    [SerializeField] private float health = 100;
    public Gun[] gunArray;
    private int selectedGun;
    Transform groundCheck;
    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject grenade;
    bool isPlayerDead;
    public void Damage(int damage) {
        health -= damage;
        
        if (health <= 0) {
            isPlayerDead = true;
            
        }
    }

    void PlayerDead() {
        if (isPlayerDead) {
            UIController.instance.deadMessage.gameObject.SetActive(true);
            UIController.instance.crosshair.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneManager.LoadScene("Menu");
            }
        }
        
    }

    void DisplayScore() {
        UIController.instance.score.text = "Score: " + score;
    }

    void DisplayHealth() {
        UIController.instance.healthSlider.value = health;
    }
    // Start is called before the first frame update
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        //cam = Camera.main;
        UIController.instance.weaponTempSlider.maxValue = maxHeat;
        UIController.instance.healthSlider.maxValue = maxHealth;
        groundCheck = this.transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");
        SwitchGunModel();
    }

    void SelectGuns() {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
            selectedGun++;
            if (selectedGun >= gunArray.Length) {
                selectedGun = 0;

            }
            SwitchGunModel();
        } else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
            selectedGun--;
            if (selectedGun < 0) {
                selectedGun = gunArray.Length - 1;
            }
            SwitchGunModel();
        }
    }

    void SwitchGunModel() {
        foreach(Gun gun in gunArray) {
            gun.gameObject.SetActive(false);
        }
        gunArray[selectedGun].gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Health")) {
            health += 10;
            score += 5;
        }
        if (other.transform.CompareTag("GrenadeDrop")) {
            grenades++;
            score += 10;
        }
        if (health > maxHealth) {
            health = maxHealth;

        }
        Destroy(other.gameObject);
    }
    
    

    void GroundCheck() {
        if (Physics.Raycast(this.transform.position, Vector3.down, 1.1f)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

    }

    void ThrowGrenade(GameObject grenade) {
        Vector3 direction = (cam.transform.forward * 10) + (cam.transform.up * 3);
        grenade.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }
    void DisplayGrenades() {
        UIController.instance.grenades.text = "Grenades: " + grenades;
    }
    void Update() {
        DisplayGrenades();
        DisplayScore();
        PlayerDead();
        GroundCheck();
        DisplayHealth();
        SelectGuns();   

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 0; 
            SceneManager.LoadScene("Menu");
        }

        if (isPlayerDead) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F) && grenades >= 1) {
            grenades--;
            //Instantiate(grenade, this.transform.position + new Vector3(0, 1.04f, -0.08f), Quaternion.identity);
            GameObject g = Instantiate(grenade, cam.transform.position + transform.forward, Quaternion.identity);
            ThrowGrenade(g);


        }
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        
        if (isGrounded) {
            //if (Input.GetKey(KeyCode.LeftShift)) { 
            //    speed = runspeed;
            //} else {
            //    speed = walkspeed;
            //}
            _ = Input.GetKey(KeyCode.LeftShift) ? speed = runspeed : speed = walkspeed;
            jumps = 2;
            jumpHeight = 3;
        } 

        if (!isGrounded) {
            if (jumps == 1) {
                
                
                speed = airspeed;
                jumpHeight = 4;
            } 
                
            
            if (jumps == 0) {
                speed = boostspeed;
                
            }
        }

        if (Input.GetButtonDown("Jump") && jumps > 0) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            jumps--;
        }
        



        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        controller.Move(move * (speed * Time.deltaTime));
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (!gunArray[selectedGun].overheated) {
            if (Input.GetMouseButtonDown(0)) {
                Shoot();

            }
            if (Input.GetMouseButton(0) && gunArray[selectedGun].isAutomatic) {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0) {
                    Shoot();

                }
            }
            gunArray[selectedGun].heatCounter -= Time.deltaTime * cooldownRate;
        } else {
            gunArray[selectedGun].heatCounter -= Time.deltaTime * overheatCoolRate;
            if (gunArray[selectedGun].heatCounter <= 0) {

                gunArray[selectedGun].overheated = false;
                UIController.instance.overheatedMessage.gameObject.SetActive(false);
            }
        }
        if (gunArray[selectedGun].heatCounter <= 0) {
            gunArray[selectedGun].heatCounter = 0;
        }
        UIController.instance.weaponTempSlider.value = gunArray[selectedGun].heatCounter;

    }
    private void Shoot() {
        muzzleFlash.Play();
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            Debug.Log("Hit " + hit.collider.gameObject.name);
            //GameObject bulletImpactObject = Instantiate(bulletImpact, hit.point + (hit.normal * 0.001f), Quaternion.LookRotation(hit.normal, Vector3.up));
            if (hit.transform.CompareTag("Enemy")) {
                hit.transform.GetComponent<NavMeshAgentBrain>().Damage(gunArray[selectedGun].damage);

            }
            if (hit.transform.CompareTag("Ground")) {
                GameObject bulletImpactObject = Instantiate(bulletImpact, hit.point + (hit.normal * 0.001f), Quaternion.LookRotation(hit.normal, Vector3.up));
                Destroy(bulletImpactObject, 10f);
            }
        }

        shotCounter = gunArray[selectedGun].timeBetweenShots;
        gunArray[selectedGun].heatCounter += gunArray[selectedGun].heatPerShot;
        if (gunArray[selectedGun].heatCounter >= gunArray[selectedGun].maxHeat) {
            gunArray[selectedGun].heatCounter = gunArray[selectedGun].maxHeat;
            gunArray[selectedGun].overheated = true;
            UIController.instance.overheatedMessage.gameObject.SetActive(true);
        }
    }

    private void LateUpdate() {
        cam.transform.position = viewPoint.transform.position;
        cam.transform.rotation = viewPoint.transform.rotation;
    }
}
