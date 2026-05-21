using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float checkRadius = 0.01f;
    public string groundLayer = "Ground";

    [HideInInspector] public bool isOnGround = false;

    private void FixedUpdate()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, checkRadius);
        if (colliderArray.Length <= 0) isOnGround = false;
        foreach (Collider collider in colliderArray)
        {
            if (collider.gameObject.CompareTag(groundLayer))
            {
                isOnGround = true;
                return;
            }
        }
        isOnGround = false;
    }
}
