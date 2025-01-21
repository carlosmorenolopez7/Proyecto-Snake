using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public float visionAngle = 60f;
    public float visionDistance = 10f;
    public Transform player;
    public float speed = 20f;
    private Light spotLight;

    private void Start()
    {
        spotLight = GetComponent<Light>();
        if (spotLight == null)
        {
            spotLight = gameObject.AddComponent<Light>();
        }
        spotLight.type = LightType.Spot;
        spotLight.spotAngle = visionAngle;
        spotLight.range = visionDistance;
        spotLight.intensity = 500f;
        spotLight.color = Color.yellow;
    }

    private void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle < visionAngle / 2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, visionDistance))
            {
                if (hit.transform == player)
                {
                    spotLight.color = Color.red;
                    ChasePlayer(directionToPlayer);
                }
                else
                {
                    spotLight.color = Color.yellow;
                }
            }
            else
            {
                spotLight.color = Color.yellow;
            }
        }
        else
        {
            spotLight.color = Color.yellow;
        }
    }

    private void ChasePlayer(Vector3 direction)
    {
        Vector3 moveDirection = direction.normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
        transform.LookAt(player);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);

        Vector3 forward = transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + forward * visionDistance);
    }
}