using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine;


public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<TestStruct> a = new(new TestStruct { _int = 17, _bool = true, message = "¾È³ç" }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public struct TestStruct : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public string message;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
        }
    }
    private void Update()
    {
        if (!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            TestServerRPC();
        }
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }
    [ServerRpc]
    private void TestServerRPC()
    {
        Debug.Log("Test ServerRPC | " + OwnerClientId);
    }
}
