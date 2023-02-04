using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool selectedCharacter = true;
    public GameObject spotLight;
    public LayerMask selectable;
    public Transform position1, position2;
    public void SwitchCharacter()
    {
        selectedCharacter = !selectedCharacter;
        spotLight.transform.position = selectedCharacter?position1.position:position2.position;
    }


    public void UpdateSelectionOnClick()
    {
        RaycastHit hit;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction*10000, Color.green);
        if(Physics.Raycast(ray, out hit, 10000, selectable))
        {
            selectedCharacter = hit.transform.name.Contains("Woman");
            spotLight.transform.position = selectedCharacter?position1.position:position2.position;
        }
    }

    public void GoToGame()
    {
        PlayerPrefs.SetInt("Character",selectedCharacter?0:1);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
