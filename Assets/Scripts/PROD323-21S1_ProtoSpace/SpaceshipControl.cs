using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class SpaceshipControl : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float burstSpeed;
    public int maxQueue;
    public GameObject laserBeam;
    public GameObject beamer_R;
    public GameObject beamer_L;
    public GameObject cameraPlacement;
    public GameObject furthestPlacement;
    public ParticleSystem booster;
    public GameObject explosion;
    public Slider healthbar;
    public int health;
    public GameObject gameOverText;
    public GameObject victoriousText;

    private SimpleControls m_Controls;
    private bool m_Charging;
    private Vector2 m_Rotation;
    private Rigidbody shipRB;
    private Queue<Vector3> posFilter = new Queue<Vector3>();
    private Queue<Quaternion> rotFilter = new Queue<Quaternion>();
    private float shipSpeed;
    private Vector3 v1, v2, v3;
    private Vector3 contactPos;
    private Quaternion contactRot;
    private bool gameover = false;

    public void Awake()
    {
        m_Controls = new SimpleControls();

        m_Controls.gameplay.fire.performed +=
            ctx =>
            {
                if (ctx.interaction is SlowTapInteraction)
                {
                    StartCoroutine(BurstFire((int)(ctx.duration * burstSpeed)));
                }
                else
                {
                    Fire();
                }
                m_Charging = false;
            };
        m_Controls.gameplay.fire.started +=
            ctx =>
            {
                if (ctx.interaction is SlowTapInteraction)
                    m_Charging = true;
            };
        m_Controls.gameplay.fire.canceled +=
            ctx =>
            {
                m_Charging = false;
            };
    }

    private void Start()
    {
        shipRB = gameObject.GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        m_Controls.Enable();
    }

    public void OnDisable()
    {
        m_Controls.Disable();
    }

    public void OnGUI()
    {
        if (m_Charging)
            GUI.Label(new Rect(100, 100, 200, 100), "Charging...");
    }

    public void Update()
    {
        if(!gameover)
        {
            var look = m_Controls.gameplay.look.ReadValue<Vector2>();
            var move = m_Controls.gameplay.move.ReadValue<Vector2>();

            // Update orientation first, then move. Otherwise move orientation will lag
            // behind by one frame.
            Look(look);
            Move(move);
            CheckHealth();
            CheckCannon();
        }
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01) {
            if(shipRB.velocity.magnitude > 0)
                shipRB.velocity -= shipRB.velocity * Time.deltaTime;
            booster.Stop();
            return;
        }    

        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        //Add forward thrust
        if(direction.y > 0)
        {
            shipRB.AddForce(transform.forward * scaledMoveSpeed * direction.y, ForceMode.Acceleration);
            shipSpeed = shipRB.velocity.magnitude;
            shipSpeed = shipSpeed < moveSpeed ? shipSpeed : moveSpeed;
            Vector3 v1 = cameraPlacement.transform.position;
            Vector3 v2 = furthestPlacement.transform.position;
            float lerpVal = shipSpeed / moveSpeed;
            Camera.main.transform.position = new Vector3(Mathf.Lerp(v1.x, v2.x, lerpVal), Mathf.Lerp(v1.y, v2.y, lerpVal), Mathf.Lerp(v1.z, v2.z, lerpVal));
            booster.Play();
        }

        //Add side thrust
        if (direction.x != 0)
            //shipRB.AddForce(transform.right * scaledMoveSpeed * direction.x, ForceMode.Acceleration);
            shipRB.velocity = transform.right * scaledMoveSpeed * direction.x;
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += rotate.x * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation / 2;
    }

    private IEnumerator BurstFire(int burstAmount)
    {
        for (var i = 0; i < burstAmount; ++i)
        {
            Fire();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Fire()
    {
        var transform = this.transform;
        var laserR = Instantiate(laserBeam, beamer_R.transform.position, Quaternion.identity);
        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(90, 0, 0);
        laserR.transform.rotation = transform.rotation * rot;
        var laserL = Instantiate(laserBeam, beamer_L.transform.position, Quaternion.identity);
        laserL.transform.rotation = transform.rotation * rot;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "LaserEnemy")
        {
            health -= 1;
            healthbar.value = health;
            ContactPoint contact = collision.contacts[0];
            contactRot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            contactPos = contact.point;
            Destroy(collision.gameObject);
        }
    }

    private void CheckHealth()
    {
        if(health < 0)
        {
            booster.Stop();
            Instantiate(explosion, contactPos, contactRot);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            healthbar.gameObject.SetActive(false);
            gameOverText.SetActive(true);
            StartCoroutine(StopGame(2f));
            gameover = true;
        }
    }

    private void CheckCannon()
    {
        if(GameObject.FindGameObjectWithTag("cannon") == null)
        {
            victoriousText.SetActive(true);
            StartCoroutine(StopGame(2f));
        }
    }

    IEnumerator StopGame(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0;
    }
}
