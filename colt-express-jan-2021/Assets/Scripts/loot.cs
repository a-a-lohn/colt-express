using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loot : MonoBehaviour
{
    public GameObject gem; 

    void OnMouseDown(){
        // gameObject.active = false; 
        // Destroy(gameObject);
        gem.SetActive(false);
        Destroy(gem);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
