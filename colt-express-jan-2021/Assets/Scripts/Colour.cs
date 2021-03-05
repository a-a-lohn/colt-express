using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colour : MonoBehaviour
{
    public Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
         rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Color startcolor;
    void OnMouseEnter()
 	{
     startcolor = rend.material.color;
     rend.material.color = Color.grey;
 	}
     void OnMouseExit()
 	{
     rend.material.color = startcolor;
 	}
 	void OnMouseDown()
 	{
     rend.material.color = Color.black;
 	}
}
