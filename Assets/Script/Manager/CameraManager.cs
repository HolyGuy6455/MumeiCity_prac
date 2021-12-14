using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour {

    [SerializeField] float zoom = 6.0f;
    [SerializeField] float zoomMin = 4.0f;
    [SerializeField] float zoomMax = 12.0f;
    [SerializeField] float zoomSpeed = 1.0f;
    [SerializeField] float zoomAmount = 1.0f;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public void CameraZoom(){
        zoom -= Input.GetAxisRaw("Mouse ScrollWheel")*zoomAmount;
        zoom = (zoom<zoomMin)?zoomMin:(zoom>zoomMax)?zoomMax:zoom;
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoom, Time.deltaTime*zoomSpeed);
    }
    
}