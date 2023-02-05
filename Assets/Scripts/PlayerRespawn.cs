using System.Security.Cryptography;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform checkpoint;
    private PlayerMovement playerHealth;


    private void Awake()
    {
       playerHealth = GetComponent<PlayerMovement>();
    }

    public void Respawn()
    {
        transform.position = checkpoint.position;
        playerHealth.Respawn();

        Camera.main.GetComponent<CameraController>().Respawn();

    }
}
