using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Transform playerPos;       // 플레이어

    [Header("마우스")]
    [SerializeField] private Transform mousePos;        // 마우스 위치

    [Header("카메라 제한")]
    [SerializeField] private float maxCameraX;          // 최대 카메라 좌우 값
    [SerializeField] private float maxCameraZ;          // 최대 카메라 상하 값

    private void LateUpdate()
    {
        Vector3 distance = mousePos.position - playerPos.position;

        float curCameraX = Mathf.Clamp(distance.x, -maxCameraX, maxCameraX);
        float curCameraZ = Mathf.Clamp(distance.z, -maxCameraZ, maxCameraZ);

        mousePos.position = new Vector3(playerPos.position.x + curCameraX, 0, playerPos.position.z + curCameraZ);
    }
}