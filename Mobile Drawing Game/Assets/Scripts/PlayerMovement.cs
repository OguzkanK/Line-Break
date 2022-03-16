using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Create photon view to differentiate input between clients
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _view.IsMine) // Mouse input for testing
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
        else if (Input.touchCount > 0 && _view.IsMine) // Touch input
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            transform.position = touchPosition;
        }   
    }
}
