
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;

    private float lookAhead;

    private void Update()
    {
        //smoooooooth camera in ere 


        //sets camera pos to players x pos plus a delay
        transform.position = new Vector3 (player.position.x + lookAhead, transform.position.y, transform.position.z);

        // calaculates camera delay
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }
}
