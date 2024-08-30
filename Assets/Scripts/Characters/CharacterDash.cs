using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDash : MonoBehaviour
{
    public float dashSpeed;
    public float dashTime;

    playerController player;

    private void Awake()
    {
        player = GetComponent<playerController>();
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
            yield return null;
        }
    }
}
