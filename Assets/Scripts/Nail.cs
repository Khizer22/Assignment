using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Nail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugText;
    [SerializeField] TextMeshProUGUI completionStatusText;
    CapsuleCollider nailThreadCollider;
    Rigidbody rBody;

    UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable grabScript;
    bool bTouchingBoard = false;
    bool bStuck = false;
    Vector3 nailStuckPosition;
    private const float myHeight = 0.1311f;
    float nailDepthPercent = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        nailThreadCollider = GetComponentInChildren<CapsuleCollider>();
        rBody = GetComponent<Rigidbody>();
        grabScript = GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
    }

    private void OnGUI()
    {
        completionStatusText.SetText((nailDepthPercent * 100).ToString());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            bTouchingBoard = true;
            if (bStuck == false) 
            {
                nailStuckPosition = transform.position;
            }
                
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6)
            bTouchingBoard = false;
    }

    public void HammerHit(Vector3 myImpact)
    {
        if (bTouchingBoard == false)
            return;

        if (bStuck == false)
        {
            bStuck = true;

            //disable thread collider
            nailThreadCollider.enabled = false;

            //Stop and disable transform
            //rBody.freezeRotation = true;
            rBody.constraints = RigidbodyConstraints.FreezeAll;
            rBody.useGravity = false;
            rBody.velocity = (new Vector3(0, 0, 0));

            //disable grab
            grabScript.interactionLayers = 0;

            //Disable force for nailing
            rBody.isKinematic = true;
        }

        Vector3 transformToMove = transform.up * myImpact.magnitude / 1000;
        float distAfterHit = Vector3.Distance(transform.position - transformToMove, nailStuckPosition);
        
        if (distAfterHit < myHeight)
        {
            transform.position -= transformToMove;
            nailDepthPercent = (distAfterHit / myHeight);
        }
        else 
        {
            transform.position -= (transform.up * (myHeight * (1 - nailDepthPercent)));
            nailDepthPercent = 1.0f;
        }
           
    }
}
