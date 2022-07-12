using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSplash : MonoBehaviour {
    public Camera mainCamera;
    public Texture2D splashTexture;
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
            {
                SplashableSurface surface = hit.collider.gameObject.GetComponent<SplashableSurface>();
                if (null != surface)
                    surface.PaintOn(hit.textureCoord, splashTexture);
            }
        }
	}
}
