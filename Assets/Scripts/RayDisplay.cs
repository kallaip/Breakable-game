using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDisplay : MonoBehaviour
{

    public float maxShootDistance;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(targetPoint, cam.transform.forward * maxShootDistance, Color.red);
    }
}
