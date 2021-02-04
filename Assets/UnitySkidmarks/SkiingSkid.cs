using UnityEngine;

public class SkiingSkid : MonoBehaviour
{
    // INSPECTOR SETTINGS

    [SerializeField] Rigidbody rb;
    public Skidmarks skidmarksController;
    public ICharacter iCharater;


    int lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
    float lastFixedUpdateTime;

    // #### UNITY INTERNAL METHODS ####

    protected void Awake()
    {
        lastFixedUpdateTime = Time.time;
    }

    protected void FixedUpdate()
    {
        lastFixedUpdateTime = Time.time;
    }

    protected void LateUpdate()
    {
        if (iCharater.isGround)
        {
            Ray ray = new Ray(transform.position + new Vector3(0, 1, -0.1f), -transform.up * 100);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                // Account for further movement since the last FixedUpdate
                Vector3 skidPoint = hit.point + (rb.velocity * (Time.time - lastFixedUpdateTime));
                lastSkid = skidmarksController.AddSkidMark(skidPoint, hit.normal, Color.white, lastSkid);
            }
            else
            {
                lastSkid = -1;
            }
        }
        else
        {
            lastSkid = -1;
        }
    }

    // #### PUBLIC METHODS ####

    // #### PROTECTED/PRIVATE METHODS ####
}