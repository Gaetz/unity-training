using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

  public int Stone;
  [SerializeField] int maxStone = 100;

  // Use this for initialization
  void Start()
  {
    Stone = 0;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void AddStone(int moreStone)
  {
    Stone += moreStone;
    Stone = Mathf.Min(Stone, maxStone);
  }
}
