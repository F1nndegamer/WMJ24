using UnityEngine;

public class playerscript : MonoBehaviour
{
    public float attractionSpeed = 5f;
    public float repulsionForce = 10f;
    public float interactionRange = 5f;
    public float minimumDistance = 1f;

    private void Update()
    {
        AttractOrRepelObjects();
    }

    private void AttractOrRepelObjects()
    {
        GameObject[] attractors = GameObject.FindGameObjectsWithTag("Attractor");
        GameObject[] repellers = GameObject.FindGameObjectsWithTag("Repeller");
        Vector3 attractionDirection = Vector3.zero;
        Vector3 repulsionDirection = Vector3.zero;

        foreach (GameObject attractor in attractors)
        {
            AttachBlock colliderCheck = attractor.GetComponent<AttachBlock>();
            if (colliderCheck.isColliding == false)
            {
                float distanceToAttractor = Vector3.Distance(transform.position, attractor.transform.position);
                if (distanceToAttractor <= interactionRange && distanceToAttractor > minimumDistance)
                {
                    Vector3 directionToAttractor = (attractor.transform.position - transform.position).normalized;
                    attractionDirection += directionToAttractor / Mathf.Max(distanceToAttractor, 1f);
                }
            }
        }
        foreach (GameObject repeller in repellers)
        {
            RepelBlock colliderCheck = repeller.GetComponent<RepelBlock>();
            if (colliderCheck.isColliding == false)
            {
                float distanceToRepeller = Vector3.Distance(transform.position, repeller.transform.position);
                if (distanceToRepeller <= interactionRange && distanceToRepeller > minimumDistance)
                {
                    Vector3 directionToRepeller = (transform.position - repeller.transform.position).normalized;
                    repulsionDirection += directionToRepeller / Mathf.Max(distanceToRepeller, 1f);
                }
            }
        }
        Vector3 movement = (attractionDirection * attractionSpeed) + (repulsionDirection * repulsionForce);
        transform.position += movement * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        GameObject[] attractors = GameObject.FindGameObjectsWithTag("Attractor");
        foreach (GameObject attractor in attractors)
        {
            float distanceToAttractor = Vector3.Distance(transform.position, attractor.transform.position);
            if (distanceToAttractor <= interactionRange && distanceToAttractor > minimumDistance)
            {
                Gizmos.DrawLine(transform.position, attractor.transform.position);
            }
        }

        Gizmos.color = Color.red;
        GameObject[] repellers = GameObject.FindGameObjectsWithTag("Repeller");
        foreach (GameObject repeller in repellers)
        {
            float distanceToRepeller = Vector3.Distance(transform.position, repeller.transform.position);
            if (distanceToRepeller <= interactionRange && distanceToRepeller > minimumDistance)
            {
                Gizmos.DrawLine(transform.position, repeller.transform.position);
            }
        }
    }
}
