using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;

    public GameObject ghost;
    [HideInInspector] public bool makeGhost;

    private playerController player;

    void Start()
    {
        player = GetComponent<playerController>();
        ghostDelaySeconds = ghostDelay;
    }

    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                // Create a ghost object
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 0.25f);
            }
        }
    }
}