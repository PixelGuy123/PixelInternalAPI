﻿using UnityEngine;

namespace PixelInternalAPI.Components
{
    public class BillboardRotator : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (mainCam != null)
            {
                transform.LookAt(mainCam.transform.position + Vector3.up * (transform.position.y - mainCam.transform.position.y));
            }
            else if (Camera.main != null)
                mainCam = Camera.main;

        }
        Camera mainCam;
    }
}
