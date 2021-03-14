// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Login : MonoBehaviour
// {
//     public InputField UsernameInput;
//     public InputField PasswordInput;
//     public Button LoginButton;

//     // Start is called before the first frame update
//     void Start()
//     {
//         LoginButton.onClick.AddListener(() => {
            
//         });
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour{
    public void Start(){

    }
    public void Update(){

    }
    
    public void Host(){
        SceneManager.LoadScene("HostGame"); // load all scenes named "HostGame"
    }

    public void Join(){
        SceneManager.LoadScene("JoinGame"); // load all scenes named "JoinGame"
    }

    public void Quit(){
        Application.Quit(); // quit the game
    }
}


