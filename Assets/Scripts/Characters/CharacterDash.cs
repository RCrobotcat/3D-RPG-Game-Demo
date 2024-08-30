using System.Collections;
using UnityEngine;

public class CharacterDash : MonoBehaviour
{
    public float dashSpeed;
    public float dashTime;

    playerController player;
    CharacterStatus playerStatus;
    Ghost ghost;

    private void Awake()
    {
        player = GetComponent<playerController>();
        playerStatus = GetComponent<CharacterStatus>();
        ghost = GetComponent<Ghost>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            player.agent.velocity = transform.forward * dashSpeed;
            if (player.agent.velocity != Vector3.zero)
            {
                playerStatus.isInvincible = true;
                ghost.makeGhost = true;
            }
            yield return null;
        }
        ghost.makeGhost = false;
        playerStatus.isInvincible = false;
    }
}
