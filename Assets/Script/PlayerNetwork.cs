using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode.Components;

public class PlayerNetwork : NetworkBehaviour
{
    private Animator anim;
    private Transform spawnedObject;
    [SerializeField]
    private Transform spawnObjectPrefab;
    private LogBox logBox;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        logBox = FindObjectOfType<LogBox>();
    }
    private void Update()
    {
        if (!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            SpawnObjectServerRPC();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DespawnObjectServerRPC();
        }
        Vector3 moveDir = Vector3.zero;

        anim.SetBool("Run", false);
        int horDir = 0; //none, left, right
        int vertDir = 0;//none, up, down
        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z = +1f;
            vertDir = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir.z = -1f;
            vertDir = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -1f;
            horDir = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = +1f;
            horDir = 2;
        }
        if(moveDir != Vector3.zero)
        {
            anim.SetBool("Run", true);
            if (horDir == 0)
            {
                if (vertDir == 1)
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                else if(vertDir == 2)
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (horDir == 1)
            {
                if (vertDir == 0)
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                else if (vertDir == 1)
                    transform.rotation = Quaternion.Euler(0f, 135f, 0f);
                else if (vertDir == 2)
                    transform.rotation = Quaternion.Euler(0f, 45f, 0f);
            }
            else if (horDir == 2)
            {
                if (vertDir == 0)
                    transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                else if (vertDir == 1)
                    transform.rotation = Quaternion.Euler(0f, -135f, 0f);
                else if (vertDir == 2)
                    transform.rotation = Quaternion.Euler(0f, -45f, 0f);
            }
        }
        float moveSpeed = 3f;
        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }
    [ServerRpc]
    private void SpawnObjectServerRPC()
    {
        spawnedObject = Instantiate(spawnObjectPrefab);
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);
        spawnedObject.position = new Vector3(Random.Range(-5.0f, 5.0f), 1, Random.Range(-10.0f, -5.0f));
    }
    [ServerRpc]
    private void DespawnObjectServerRPC()
    {
        spawnedObject.GetComponent<NetworkObject>().Despawn(true);
    }
}
