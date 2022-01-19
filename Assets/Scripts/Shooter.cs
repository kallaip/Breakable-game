using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float shootingRate = 0.2f;
    public float maxShootDistance = 100f;
    public float shotEffectTime = 0.05f;
    public GameObject gunHole;
    public int initialAmmo = 50;
    public AudioSource shootSound;

    private Camera cam;
    private float shootTimer;
    private bool shootingEnabled;
    private LineRenderer shootLine;

    private WaitForSeconds effectTimer;
    private int ammo;
    
    public int Ammo
    {
        get { return ammo; }
        set { ammo = value; }
    }
    private bool gameFinished = false;
    public bool GameFinished
    {
        get { return gameFinished; }
    }

    private Exploder shootedPlatform;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInParent<Camera>();
        shootLine = GetComponent<LineRenderer>();

        effectTimer = new WaitForSeconds(shotEffectTime);
        ammo = initialAmmo;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1") && ammo > 0)
        {
            //lövünk
            if (shootingEnabled)
            {
                shootingEnabled = false;
                shootTimer = shootingRate;
                processShoot();

            }

        }



        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootingEnabled = true;
            shootTimer = shootingRate;
        }
    }
    private void processShoot()
    {
        shootSound.Play();
        RaycastHit hit;
        ammo--;
        Vector3 targetPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        shootLine.SetPosition(0, gunHole.transform.position);

        if (Physics.Raycast(targetPoint, cam.transform.forward, out hit, maxShootDistance))
        {
            shootLine.SetPosition(1, hit.point);
            StartCoroutine(shotEffect(true));
            if(hit.collider.gameObject.tag == "Console")
            {
                Debug.Log("console hit.");
                Destroy(hit.collider.gameObject);
            } else if (hit.collider.gameObject.tag == "Finish")
            {
                gameFinished = true;
            }
            else if (hit.collider.gameObject.tag == "Breakable")
            {
                shootedPlatform = hit.collider.gameObject.GetComponent<Exploder>();
                shootedPlatform.doExplosion(10);
            }
        }
        else
        {
            shootLine.SetPosition(1, targetPoint + (cam.transform.forward * maxShootDistance));
            StartCoroutine(shotEffect(false));
        }

    }

    private IEnumerator shotEffect(bool impact)
    {

        shootLine.enabled = true;

        yield return effectTimer;
        shootLine.enabled = false;

    }
}
