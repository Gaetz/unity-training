using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

  [SerializeField] float panSpeed;
  [SerializeField] float rotateSpeed;
  [SerializeField] float rotateAmount;

  Quaternion rotation;
  float panDetect = 30f;
	float minHeight = 10f;
  float maxHeight = 50f;

  GameObject selectedObject;
  ObjectInfo selectedInfo;

  void Start() {
    rotation = Camera.main.transform.rotation;
  }

	void Update () {
    MoveCamera();
    RotateCamera();
    if(Input.GetMouseButton(0)) {
      Selection();
    }
	}

  void Selection() {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if(Physics.Raycast(ray, out hit, 100)) {
      if (hit.collider.tag == "Ground") {
        selectedObject = null;
      } else if (hit.collider.tag == "Selectable") {
        selectedObject = hit.collider.gameObject;
        selectedInfo = selectedObject.GetComponent<ObjectInfo>();
        selectedInfo.IsSelected = true;
      }
    }
  }

  void MoveCamera() {
    float moveX = Camera.main.transform.position.x;
    float moveY = Camera.main.transform.position.y;
    float moveZ = Camera.main.transform.position.z;

    float mouseX = Input.mousePosition.x;
    float mouseY = Input.mousePosition.y;

    if( Input.GetKey(KeyCode.Q) || (mouseX > 0 && mouseX < panDetect) ) {
      moveX -= panSpeed * Time.deltaTime;
    } else if( Input.GetKey(KeyCode.D) || (mouseX < Screen.width && mouseX > Screen.width - panDetect) ) {
      moveX += panSpeed * Time.deltaTime;
    }
    
    if( Input.GetKey(KeyCode.S) || (mouseY > 0 && mouseY < panDetect) ) {
      moveZ -= panSpeed * Time.deltaTime;
    } else if( Input.GetKey(KeyCode.Z) || (mouseY < Screen.height && mouseY > Screen.height - panDetect) ) {
      moveZ += panSpeed * Time.deltaTime;
    }
    
    moveY += Input.GetAxis("Mouse ScrollWheel") * panSpeed;
    moveY = Mathf.Clamp(moveY, minHeight, maxHeight);

    Vector3 newPosition = new Vector3(moveX, moveY, moveZ);
    Camera.main.transform.position = newPosition;
  }

  void RotateCamera() {
    Vector3 origin = Camera.main.transform.rotation.eulerAngles;
    Vector3 destination = origin;

    if (Input.GetMouseButton(2)) {
      destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
      destination.y += Input.GetAxis("Mouse X") * rotateAmount;
    }

    if (destination != origin) {
      Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, rotateSpeed * Time.deltaTime);
    }

    if ( Input.GetKeyDown(KeyCode.Space) ) {
      Camera.main.transform.rotation = rotation;
    }
  }
}
