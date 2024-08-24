using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamsController : MonoBehaviour
{
    public GameObject playerCam; // The camera that place behind the player
    public CinemachineVirtualCamera playerCamVirtual;
    public GameObject playerFreeLookCam; // The free look camera

    private void Update()
    {
        // Switch between playerCam and playerFreeLookCam
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (playerCam.activeSelf)
            {
                playerCam.SetActive(false);
                playerFreeLookCam.SetActive(true);
            }
            else
            {
                playerCam.SetActive(true);
                playerFreeLookCam.SetActive(false);
                playerCamVirtual.Follow = FindObjectOfType<playerController>().gameObject.transform.GetChild(2);
            }
        }
    }
}
