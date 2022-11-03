using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugText;
    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject objHit = collision.gameObject;

        Vector3 collisionForce = collision.impulse / Time.deltaTime;

        if (objHit.layer == 7) {
            Nail nailScript = objHit.GetComponent<Nail>();
            nailScript.HammerHit(collisionForce);
        }
    }
}
