using model;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Reflection;
using Sfs2X.Protocol.Serialization;

using System.Collections;
using Random=System.Random;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour
{
	private static RestClient client = new RestClient("http://13.72.79.112:4242");
	public static string gameHash = WaitingRoom.gameHash;
	public static string savegameId = null;

	//debug variables
	public static Text debugText;
	public static string debugTextString;
    public Button button;
	public Button extension;
	public Button chooseChar;

	public static bool works = false;
	public Text doesItWork;

	public static void setWorks() {
		works = true;
	}

	public GameObject canvas;

	public Text Round;
	//public GameObject exit;
	public Text exitText;
	
    public Bandit b;

	public static GameManager gm;

	// LIST OF ALL GAME buttonToObject HERE
    public Button cheyenne;
	public Button belle; 
	public Button tuco; 
	public Button doc; 
	public Button ghost; 
	public Button django; 
	public Button marshal;
	
	public Button gem1; 
	public Button gem2; 
	public Button gem3; 
	public Button gem4;
	public Button gem5;
	public Button gem6;

	public Button ghoLoot;

	public GameObject bulletCard;

	// propmpt messages 
	public Text promptDrawCardsOrPlayCardMsg;
	public Text promptChooseLoot; 
	public Text promptPunchTarget; 

	public static Dictionary<Button, object> buttonToObject = new Dictionary<Button, object>();

	public static ArrayList clickable = new ArrayList();
	public static string action = "";

	public Text cardNewAText;
	public Text cardNewABext;
	public Text cardNewCText;

	public GameObject playerE;

	public Button BelleBulletCard1; 
	public Button BelleBulletCard2;
	public Button BelleBulletCard3;
	public Button BelleBulletCard4;
	public Button BelleBulletCard5;
	public Button BelleBulletCard6;    

	public Button CheyenneBulletCard1; 
	public Button CheyenneBulletCard2;
	public Button CheyenneBulletCard3;
	public Button CheyenneBulletCard4;
	public Button CheyenneBulletCard5;
	public Button CheyenneBulletCard6;     
	
	public Button DocBulletCard1; 
	public Button DocBulletCard2;
	public Button DocBulletCard3;
	public Button DocBulletCard4;
	public Button DocBulletCard5;
	public Button DocBulletCard6;   
		
	public Button TucoBulletCard1; 
	public Button TucoBulletCard2;
	public Button TucoBulletCard3;
	public Button TucoBulletCard4;
	public Button TucoBulletCard5;
	public Button TucoBulletCard6;   

	public Button DjangoBulletCard1; 
	public Button DjangoBulletCard2;
	public Button DjangoBulletCard3;
	public Button DjangoBulletCard4;
	public Button DjangoBulletCard5;
	public Button DjangoBulletCard6;   

	public Button GhostBulletCard1; 
	public Button GhostBulletCard2;
	public Button GhostBulletCard3;
	public Button GhostBulletCard4;
	public Button GhostBulletCard5;
	public Button GhostBulletCard6;   

	public Button BelleActionMove; 
	public Button BelleActionChangeFloor; 
	public Button BelleActionPunch; 
	public Button BelleActionShoot;

	public Text clickableGOsText;
	public Text currentRound; 
	public Text currentBandit; 

	private List<Button> goNeutralBulletCards;

	// a list of bullet cards for each and every bandit 
	private List<Button> goBELLEBulletCards; 
	private List<Button> goCHEYENNEBulletCards; 
	private List<Button> goDOCBulletCards; 
	private List<Button> goTUCOBulletCards; 
	private List<Button> goDJANGOBulletCards; 
	private List<Button> goGHOSTBulletCards; 

	// a list of action cards for each and every bandit's hand 
	private List<Button> goBELLEHand; 
	private List<Button> goCHEYENNEHand; 
	private List<Button> goDOCHand; 
	private List<Button> goGHOSTHand; 
	private List<Button> goTUCOHand; 
	private List<Button> goDJANGOHand; 

	private List<GameObject> clickableGOs; 
	public List<object> clickablebuttonToObject;  

	public Button ghoCard1; 
	public Button ghoCard2; 
	public Button ghoCard3; 
	public Button ghoCard4; 
	public Button ghoCard5;
	public Button ghoCard6;  
	public Button ghoCard7; 

	public Button docCard1; 
	public Button docCard2; 
	public Button docCard3; 
	public Button docCard4; 
	public Button docCard5;
	public Button docCard6;  

	public Button tucCard1; 
	public Button tucCard2; 
	public Button tucCard3; 
	public Button tucCard4; 
	public Button tucCard5;
	public Button tucCard6;  

	public Button belCard1; 
	public Button belCard2; 
	public Button belCard3; 
	public Button belCard4; 
	public Button belCard5;
	public Button belCard6;  

	public Button cheCard1; 
	public Button cheCard2; 
	public Button cheCard3; 
	public Button cheCard4; 
	public Button cheCard5;
	public Button cheCard6;  

	public Button djaCard1; 
	public Button djaCard2; 
	public Button djaCard3; 
	public Button djaCard4; 
	public Button djaCard5;
	public Button djaCard6;  

	/* TrainUnit */
	public Button trainOneBtm; 
	public Button trainOneTop; 
	public Button trainTwoBtm; 
	public Button trainTwoTop;
	public Button trainThreeBtm; 
	public Button trainThreeTop;
	public Button trainFourBtm; 
	public Button trainFourTop;
	// public Button trainFiveBtm; 
	// public Button trainFiveTop;
	public Button locoBtm; 
	public Button locoTop;

	public List<Button> trainRoofs; 
	public List<Button> trainCabins; 

	/* horses ?*/
	
    public static string punchedBandit; 

    void Start(){
		setAllNonClickable();
		Debug.Log("hiii");
		// GameObject.Find("GameBoardGO").GetComponent<GameBoard>().promptPlayOrDraw.text = "HIII";

		/* DUMMY BANDITS FOR TESTING PURPOSES */
		Bandit b1 = new Bandit("GHOST");
		Bandit b2 = new Bandit("BELLE");
		Bandit b3 = new Bandit("CHEYENNE");	

		ArrayList banditsArr = new ArrayList(); 
		banditsArr.Add(b1); 
		banditsArr.Add(b2); 
		banditsArr.Add(b3); 

		/* @TEST makeShootPossibilitiesClickable*/
		buttonToObject.Add(ghost, b1);

		ArrayList shootArr = new ArrayList(); 
		shootArr.Add(b1);
		makeShootPossibilitiesClickable(shootArr);
	
		/* @OUTPUT now only GHOST is clickable 🎉*/

		/* @TEST makePunchPossibilitiesClickable */
		var selectedBanditName = EventSystem.current.currentSelectedGameObject;
         if (selectedBanditName != null)
             promptPunchTarget.text = "ahh" + selectedBanditName.name;
         else
             promptPunchTarget.text = "ahh NULLL POINTERR";
		// promptPunchTarget.text = "ahh" + selectedBanditName;
		promptPunchTarget.text = "ahh" + selectedBanditName.name;
		 

		string selectedPunchBandit = makePunchPossibilitiesClickable(shootArr);
		Debug.Log("YOU PUNCHED " + selectedPunchBandit);
		promptPunchTarget.text = selectedPunchBandit + "IS PUNCHED";

		// initCards();
		Round.text = "ROUND 1:\n-Standard turn\n-Tunnel turn\n-Switching turn";
		SFS.setGameBoard();

		exitText.text ="";
		doesItWork.text = "";
		//Invoke("LeaveRoom",5);
		/*if (SFS.getSFS() == null) {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
        }
        if (!SFS.IsConnected()) {
            SFS.Connect("test");
        }*/
		EnterGameBoardScene();
    }

	/* add all Belle bullet cards to list*/
	public void addAllBelleBullets(){
		goBELLEBulletCards.Add(BelleBulletCard1); 
		goBELLEBulletCards.Add(BelleBulletCard2); 
		goBELLEBulletCards.Add(BelleBulletCard3); 
		goBELLEBulletCards.Add(BelleBulletCard4);
		goBELLEBulletCards.Add(BelleBulletCard5);  
		goBELLEBulletCards.Add(BelleBulletCard6); 
	}

	/* add all Cheyenne bullet cards to list*/
	public void addAllCheyenneBullets(){
		goCHEYENNEBulletCards.Add(CheyenneBulletCard1); 
		goCHEYENNEBulletCards.Add(CheyenneBulletCard2); 
		goCHEYENNEBulletCards.Add(CheyenneBulletCard3); 
		goCHEYENNEBulletCards.Add(CheyenneBulletCard4);
		goCHEYENNEBulletCards.Add(CheyenneBulletCard5);  
		goCHEYENNEBulletCards.Add(CheyenneBulletCard6); 
	}

	/* add all Doc bullet cards to list*/
	public void addAllDocBullets(){
		goDOCBulletCards.Add(DocBulletCard1); 
		goDOCBulletCards.Add(DocBulletCard2); 
		goDOCBulletCards.Add(DocBulletCard3); 
		goDOCBulletCards.Add(DocBulletCard4);
		goDOCBulletCards.Add(DocBulletCard5);  
		goDOCBulletCards.Add(DocBulletCard6); 
	}

	/* add all Tuco bullet cards to list*/
	public void addAllTucoBullets(){
		goTUCOBulletCards.Add(TucoBulletCard1); 
		goTUCOBulletCards.Add(TucoBulletCard2); 
		goTUCOBulletCards.Add(TucoBulletCard3); 
		goTUCOBulletCards.Add(TucoBulletCard4);
		goTUCOBulletCards.Add(TucoBulletCard5);  
		goTUCOBulletCards.Add(TucoBulletCard6); 
	}

	/* add all Django bullet cards to list*/
	public void addAllDjangoBullets(){
		goDJANGOBulletCards.Add(DjangoBulletCard1); 
		goDJANGOBulletCards.Add(DjangoBulletCard2); 
		goDJANGOBulletCards.Add(DjangoBulletCard3); 
		goDJANGOBulletCards.Add(DjangoBulletCard4);
		goDJANGOBulletCards.Add(DjangoBulletCard5);  
		goDJANGOBulletCards.Add(DjangoBulletCard6); 
	}

	/* add all Ghost bullet cards to list*/
	public void addAllGhostBullets(){
		goGHOSTBulletCards.Add(GhostBulletCard1); 
		goGHOSTBulletCards.Add(GhostBulletCard2); 
		goGHOSTBulletCards.Add(GhostBulletCard3); 
		goGHOSTBulletCards.Add(GhostBulletCard4);
		goGHOSTBulletCards.Add(GhostBulletCard5);  
		goGHOSTBulletCards.Add(GhostBulletCard6); 
	}

	/* initMap initializes the <Button, object> hashmap */
	public void initMap(){

		/* init. all bandits */
		buttonToObject.Add(belle, "null"); 
		buttonToObject.Add(cheyenne, "null"); 
		buttonToObject.Add(doc, "null"); 
		buttonToObject.Add(django, "null"); 
		buttonToObject.Add(tuco, "null"); 
		buttonToObject.Add(ghost, "null"); 
		buttonToObject.Add(marshal, "null");

		/* init all bullet cards */
		addNullListToMap(goBELLEBulletCards);
		addNullListToMap(goCHEYENNEBulletCards);
		addNullListToMap(goDOCBulletCards);
		addNullListToMap(goGHOSTBulletCards);
		addNullListToMap(goDJANGOBulletCards);
		addNullListToMap(goTUCOBulletCards);

		/* init all bandits' hands */
		goBELLEHand = new List<Button>(){belCard1, belCard2, belCard3, belCard4, belCard5, belCard5, belCard6};
		goCHEYENNEHand = new List<Button>(){cheCard1, cheCard2, cheCard3, cheCard4, cheCard5, cheCard5, cheCard6};
		goTUCOHand = new List<Button>(){tucCard1, tucCard2, tucCard3, tucCard4, tucCard5, tucCard5, tucCard6};
		goDOCHand = new List<Button>(){docCard1, docCard2, docCard3, docCard4, docCard5, docCard5, docCard6};
		goGHOSTHand = new List<Button>(){ghoCard1, ghoCard2, ghoCard3, ghoCard4, ghoCard5, ghoCard5, ghoCard6, ghoCard7};
		goDJANGOHand = new List<Button>(){djaCard1, djaCard2, djaCard3, djaCard4, djaCard5, djaCard5, djaCard6};
		addNullListToMap(goBELLEHand);
		addNullListToMap(goCHEYENNEHand);
		addNullListToMap(goTUCOHand);
		addNullListToMap(goDOCHand);
		addNullListToMap(goGHOSTHand);
		addNullListToMap(goDJANGOHand);

		/* init all action cards ? */

		/* init all traincarts */
		buttonToObject.Add(trainOneBtm, "null"); 
		buttonToObject.Add(trainOneTop, "null"); 
		buttonToObject.Add(trainTwoBtm, "null"); 
		buttonToObject.Add(trainTwoTop, "null"); 
		buttonToObject.Add(trainThreeBtm, "null"); 
		buttonToObject.Add(trainThreeTop, "null"); 
		buttonToObject.Add(trainFourBtm, "null"); 
		buttonToObject.Add(trainFourTop, "null");
		buttonToObject.Add(locoBtm, "null"); 
		buttonToObject.Add(locoTop, "null");

		trainCabins.Insert(0, locoBtm);
		trainCabins.Insert(1, trainOneBtm);
		trainCabins.Insert(2, trainTwoBtm);
		trainCabins.Insert(3, trainThreeBtm);
		trainCabins.Insert(4, trainFourBtm);

		trainRoofs.Insert(0, locoTop);
		trainRoofs.Insert(1, trainOneTop);
		trainRoofs.Insert(2, trainTwoTop);
		trainRoofs.Insert(3, trainThreeTop);
		trainRoofs.Insert(4, trainFourTop);
	}

<<<<<<< HEAD
	// public void mapTrain(GameManager gm){
	// 	 public ArrayList trainRoof ;
    //     public ArrayList trainCabin;
	// 	foreach(object oneRoof in gm.trainRoof){
	// 		buttonToObject[] = oneRoof;

	// 	}
	// }

	public void mapBanditPositions(GameManager gm){
		foreach(TrainUnit cabin in gm.trainCabin){
			ArrayList occupied = cabin.getBanditsHere();
			foreach(Bandit b in occupied) {

				TrainUnit t = b.getPosition();
				Button button = buttonToObject.FirstOrDefault(x => x.Value.Equals(t)).Key;

				if(b.getCharacter().ToLower() == "belle"){
					Vector3 temp = button.transform.position;
					belle.transform.position = temp;
				} else if(b.getCharacter().ToLower() == "cheyenne"){
					Vector3 temp = button.transform.position;
					cheyenne.transform.position = temp;
				} else if(b.getCharacter().ToLower() == "django"){
					Vector3 temp = button.transform.position;
					django.transform.position = temp;
				}else if(b.getCharacter().ToLower() == "doc"){
					Vector3 temp = button.transform.position;
					doc.transform.position = temp;
				}else if(b.getCharacter().ToLower() == "ghost"){
					Vector3 temp = button.transform.position;
					cheyenne.transform.position = temp;
				}else if(b.getCharacter().ToLower() == "tuco"){
					Vector3 temp = button.transform.position;
					cheyenne.transform.position = temp;
				}	

			}
			 
	}
=======
	public void mapTrain(GameManager gm){
		int index = 0; 
		foreach(object oneRoof in gm.trainRoof){
			buttonToObject[trainRoofs[index]] = oneRoof;
			index++;
		}
		index = 0;
		foreach(object oneCab in gm.trainCabin){
			buttonToObject[trainCabins[index]] = oneCab;
			index++;
		}
	}

	public void mapBanditBullet(GameManager gm){
		// for all the bandits passed in via gm, map their bullet cards 
		foreach(Bandit aBandit in gm.bandits){
			ArrayList aBanditBullets = aBandit.bullets;
			if(aBandit.characterAsString == "BELLE"){
				buttonToObject[BelleBulletCard1] = aBanditBullets[0]; 
				buttonToObject[BelleBulletCard2] = aBanditBullets[1]; 
				buttonToObject[BelleBulletCard3] = aBanditBullets[2]; 
				buttonToObject[BelleBulletCard4] = aBanditBullets[3]; 
				buttonToObject[BelleBulletCard5] = aBanditBullets[4]; 
				buttonToObject[BelleBulletCard6] = aBanditBullets[5]; 
			}
			if(aBandit.characterAsString == "CHEYENNE"){
				buttonToObject[CheyenneBulletCard1] = aBanditBullets[0]; 
				buttonToObject[CheyenneBulletCard2] = aBanditBullets[1]; 
				buttonToObject[CheyenneBulletCard3] = aBanditBullets[2]; 
				buttonToObject[CheyenneBulletCard4] = aBanditBullets[3]; 
				buttonToObject[CheyenneBulletCard5] = aBanditBullets[4]; 
				buttonToObject[CheyenneBulletCard6] = aBanditBullets[5]; 
			}
			if(aBandit.characterAsString == "DOC"){
				buttonToObject[DocBulletCard1] = aBanditBullets[0]; 
				buttonToObject[DocBulletCard2] = aBanditBullets[1]; 
				buttonToObject[DocBulletCard3] = aBanditBullets[2]; 
				buttonToObject[DocBulletCard4] = aBanditBullets[3]; 
				buttonToObject[DocBulletCard5] = aBanditBullets[4]; 
				buttonToObject[DocBulletCard6] = aBanditBullets[5]; 
			}	
			if(aBandit.characterAsString == "DJANGO"){
				buttonToObject[DjangoBulletCard1] = aBanditBullets[0]; 
				buttonToObject[DjangoBulletCard2] = aBanditBullets[1]; 
				buttonToObject[DjangoBulletCard3] = aBanditBullets[2]; 
				buttonToObject[DjangoBulletCard4] = aBanditBullets[3]; 
				buttonToObject[DjangoBulletCard5] = aBanditBullets[4]; 
				buttonToObject[DjangoBulletCard6] = aBanditBullets[5]; 
			}
			if(aBandit.characterAsString == "TUCO"){
				buttonToObject[TucoBulletCard1] = aBanditBullets[0]; 
				buttonToObject[TucoBulletCard2] = aBanditBullets[1]; 
				buttonToObject[TucoBulletCard3] = aBanditBullets[2]; 
				buttonToObject[TucoBulletCard4] = aBanditBullets[3]; 
				buttonToObject[TucoBulletCard5] = aBanditBullets[4]; 
				buttonToObject[TucoBulletCard6] = aBanditBullets[5]; 
			}
			if(aBandit.characterAsString == "GHOST"){
				buttonToObject[GhostBulletCard1] = aBanditBullets[0]; 
				buttonToObject[GhostBulletCard2] = aBanditBullets[1]; 
				buttonToObject[GhostBulletCard3] = aBanditBullets[2]; 
				buttonToObject[GhostBulletCard4] = aBanditBullets[3]; 
				buttonToObject[GhostBulletCard5] = aBanditBullets[4]; 
				buttonToObject[GhostBulletCard6] = aBanditBullets[5]; 
			}
		}
>>>>>>> 61b796e0063e0941defba7e6263ce182dcade7c1
	}

	/* promptDrawOrPlayMessage displays the prompt message on gameboard*/
	public static void promptDrawOrPlayMessage(){
		// promptDrawCardsOrPlayCardMsg.text = "Please play a card or draw 3 cards!";
		// GameObject GameBoardGameObject = GameObject.Find("GameBoardGO");
		// GameBoardGameObject.
	}

	public void addNullListToMap(List<Button> aBtnList){
		foreach(Button aBtn in aBtnList){
			buttonToObject.Add(aBtn, "null");
		}
	}

 	public void buttonClicked(Button btn){
        promptPunchTarget.text = btn.name + "IS CLICKED"; 
        punchedBandit = btn.name;
        // btn.interactable = false;
    }
	
	/*
	public static void testSerial() {
		ISFSObject obj = SFSObject.NewInstance();
		ExtensionRequest req = new ExtensionRequest("gm.testSerial",obj);
		SFS.Send(req);
		//EnterGameBoardScene();
		exitText.text = ""; 
		clickableGOsText.text = "";
		// init clickables should be called on update
		initClickables();
		exitText.text =""; 
    }
	*/

	/* setAllNonClickable sets all buttons to be non-clickable */
	public void setAllNonClickable(){
		Button[] allButtons = UnityEngine.Object.FindObjectsOfType<Button>();
		foreach(Button aBtn in allButtons){
			aBtn.interactable = false; 
		}
	}


	// THIS IS THE FIRST METHOD CALLED FOR RECEIVING NEW GAME STATE
    public void UpdateGameState(BaseEvent evt) {
        Debug.Log("updategamestate called");
        
        ISFSObject responseParams = (SFSObject)evt.Params["params"];
		gm = (GameManager)responseParams.GetClass("gm");
		
		// REASSIGN ALL GAME buttonToObject USING DICTIONARY
		ArrayList banditsArray = gm.bandits;
		foreach (Bandit b in banditsArray) {
			ArrayList currCards = b.hand; 
            if (b.characterAsString == "CHEYENNE") {
				buttonToObject[cheyenne] = b;
				int index = 0;
				foreach(Card c in currCards){
					 buttonToObject.Add(goCHEYENNEHand[index], c);
					 index++;
				}
            }
			if (b.characterAsString == "BELLE") {
                buttonToObject[belle] = b;
               	int index = 0;
				foreach(Card c in currCards){
					 buttonToObject.Add(goBELLEHand[index], c);
					 index++;
				}
            }
			if (b.characterAsString == "TUCO") {
                buttonToObject[tuco] = b;
				int index = 0;
				foreach(Card c in currCards){
					 buttonToObject.Add(goTUCOHand[index], c);
					 index++;
				}
            }
			if (b.characterAsString == "DOC") {
                buttonToObject[doc] = b;
                int index = 0;
				foreach(Card c in currCards){
					 buttonToObject.Add(goDOCHand[index], c);
					 index++;
				}
            }
			if (b.characterAsString == "GHOST") {
                buttonToObject[ghost] = b;
               	int index = 0;
				foreach(Card c in currCards){
					 buttonToObject.Add(goGHOSTHand[index], c);
					 index++;
				}
            }
			if (b.characterAsString == "DJANGO") {
                buttonToObject[django] = b;
                int index = 0;
				foreach(Card c in currCards){
					 buttonToObject.Add(goDJANGOHand[index], c);
					 index++;
				}
            }
		}
		Debug.Log(SFS.step);

		mapTrain(gm);

		/* map bullet cards*/


		// map the 13 neutral bullet cards
		ArrayList neuturalBulletCards = gm.neutralBulletCard; 
		for(int i=1; i<14; i++){
			Button goBulletCard = goNeutralBulletCards[i];
			buttonToObject[goBulletCard] = neuturalBulletCards[i];
		}

		//Invoke("LeaveRoom",5);
		/*if (SFS.getSFS() == null) {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
        }
        if (!SFS.IsConnected()) {
            SFS.Connect("test");
        }*/

		EnterGameBoardScene();

    }


	public void mapActionCards(List<Button> goHand, string actionName, Card c, string banditName){
		foreach(Button g in goHand){
			string goName = banditName + g.name;
			if(actionName == goName.ToUpper()){
            	buttonToObject[g] = c;
			}
		}
	}

	public void LeaveRoom() {
        SFS.LeaveRoom();
    }

	public void playCard(GameObject selectedCard){
		// draws 3 cards randomly and put in the hand
		Destroy(selectedCard);
	}

	/* Map all Buttons to their GM objects counterparts */
	public void mapAll(){
		
	}
	
	/* makeShootPossibilitiesClickable makes all possibilities clickable */
	public static void makeShootPossibilitiesClickable(ArrayList possibilities){
		Debug.Log("HELLO FROM makeShootPossibilitiesClickable");

		foreach(Bandit b in possibilities){
			foreach(Button oneBtn in buttonToObject.Keys){
				if(b.characterAsString == oneBtn.name.ToUpper()){
					oneBtn.interactable = true; 
				}
			}
		}
	}

    /* makePunchPossibilitiesClickable makes all possibilities clickable AND returns the clicked Bandit's name as a string */
    public static string makePunchPossibilitiesClickable(ArrayList possibilities){
        foreach(Bandit b in possibilities){
            foreach(Button oneBtn in buttonToObject.Keys){
                if(b.characterAsString == oneBtn.name.ToUpper()){
                    oneBtn.interactable = true; 
                }
            }
        }

        // user clicks on one of the highlighted bandits 
        while(punchedBandit is null){
            makePunchPossibilitiesClickable(possibilities);
        }   

        Debug.Log("PASSED BACK TO GM");
        return punchedBandit; 
    }


    // Update is called once per frame
    void Update()
    {

		// var selectedBanditName = EventSystem.current.currentSelectedGameObject;
        //  if (selectedBanditName != null)
        //      promptPunchTarget.text = "ahh" + selectedBanditName.name;
        //  else
        //      promptPunchTarget.text = "ahh NULLL POINTERR";

        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

		if (Input.GetMouseButtonDown(0)){
			// MouseDown();
			Debug.Log("Clicked");
			works = false;
			Debug.Log("currentbandit on mouse: "+ gm.currentBandit.getCharacter());
			if(gm != null && gm.currentBandit.getCharacter() == ChooseCharacter.character) {
				Debug.Log("ending my turn");
				Bandit b = (Bandit) gm.bandits[0];
				if (b.getCharacter() == gm.currentBandit.getCharacter()) {
					gm.currentBandit = (Bandit) gm.bandits[1];
				} else {
					gm.currentBandit = (Bandit) gm.bandits[0];
				}
				SendNewGameState();
				//gm.endOfTurn();
			}
		}

		if(works) {
			doesItWork.text = "it works!";
		} else {
			doesItWork.text = "";
		}

		/*if (SFS.debugText != debugText.text) {
            debugText.text = SFS.debugText;
        }
        clickableGOsText.text += "==== NOW GHOST IS SET TO NONACTIVE ===";
        // ghost.SetActive(false);
        foreach(GameObject go in allObjects){
            if(go.activeSelf == true){
                clickableGOsText.text += go.name;
            }
        }

		// for debugging
		if (SFS.moreText) {
            debugTextString += SFS.debugText;
            SFS.moreText = false;
        }
        if (debugTextString != debugText.text) {
            debugText.text = debugTextString;
        }*/
    }

	public void EnterGameBoardScene() {
		Debug.Log("entering scene");
		ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.enterGameBoardScene",obj);
        SFS.Send(req);
        Debug.Log("Sent enter scene message");
	}

	public static void SendNewGameState() {
		ISFSObject obj = SFSObject.NewInstance();
		//Debug.Log("sending new game state");
		obj.PutClass("gm", gm);
        ExtensionRequest req = new ExtensionRequest("gm.newGameState",obj);
        SFS.Send(req);
        Debug.Log("sent game state");
	}

	/*private void ChooseCharacter() {
        ISFSObject obj = SFSObject.NewInstance();
		obj.PutUtfString("chosenCharacter", "TUCO");
        ExtensionRequest req = new ExtensionRequest("gm.chosenCharacter",obj);
        SFS.Send(req);
        trace("chose Tuco");
    }*/

    public static void trace(string msg) {
	//	debugText.text += (debugText.text != "" ? "\n" : "") + msg;
	}

	public void GoToWaitingRoom(){
		Invoke("GoToWaitingRoom2",5);
	}

	void GoToWaitingRoom2(){
		SceneManager.LoadScene("WaitingRoom");
	}

	public void GoToChat(){
		SceneManager.LoadScene("Chat");
	}

    void OnApplicationQuit() {
		ChooseCharacter.RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}


	/*
	 The methods below implements the save game and launch saved game features
	
	*/
	private static string GetAdminToken() {
        var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", "admin")
            .AddParameter("password", "admin")
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
            
        var obj = JObject.Parse(response.Content);
        string adminToken = (string)obj["access_token"];
        adminToken = adminToken.Replace("+", "%2B");

		return adminToken;
    }

	public void TestSave() {
		SaveGameState("test");
	}

	public static void SaveGameState(string savegameID) {

		//ONLY NEED TO SEND THE SAVEGAME REQUEST TO THE LS ONCE
		//(although making the same call multiple times can't hurt, and is simpler)
		/*var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        var JObj = JObject.Parse(response.Content);
        Dictionary<string, object> sessionDetails = JObj.ToObject<Dictionary<string, object>>();

		dynamic j = new JObject();
		var temp = JsonConvert.SerializeObject(sessionDetails["gameParameters"]);
		var gameParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(temp);

		string gameName = gameParameters["name"];
		j.gamename = gameName; // can replace with "ColtExpress"
		//below I deserialize a JSON object to a collection		
		temp = JsonConvert.SerializeObject(sessionDetails["players"]);
		j.players = JsonConvert.DeserializeObject<List<string>>(temp);//In case it doesn't work, debug by adding a .ToArray()
		j.savegameid = savegameID;

		request = new RestRequest("api/gameservices/" + gameName + "/savegames/" + savegameID + "?access_token=" + GetAdminToken(), Method.POST)
            .AddParameter("application/json", j.ToString(), ParameterType.RequestBody)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");

        response = client.Execute(request);*/

		// After saving the game, store the information to the server

		ISFSObject obj = SFSObject.NewInstance();
		Debug.Log("saving the current game state on the server");
		obj.PutUtfString("savegameId", savegameID);
		obj.PutClass("gm", gm);
        ExtensionRequest req = new ExtensionRequest("gm.saveGameState",obj);
        SFS.Send(req);
	}
}



	/* 
	public void executeHardCoded(int step) {
		Debug.Log(SFS.step);
		announcement.text += "\n";

		if(step % 3 == 0){
			announcement.text = ""; 
		}
		announcement.text += logMessages[SFS.step];

		switch(step) {
			case 0:
				//round,turn info
				//"Angry Marshal Round! 1 Standard turns, 1 Tunnel turn, and 1 Switching turn",
				//Its yyy's turn to play a card or draw 3 cards.
				break;
			case 1: 
				// yyyy played a ___ card / yyy chose to draw 3 cards
				//"Standard Turn: Ghost played a MOVE card",
				//Its xxx's turn to play a card or draw 3 cards.
				if(ChooseCharacter.character == "GHOST"){
					playCard(cardA);
				}
				break;
			case 2:
				//"Standard Turn: Cheyenne played a CHANGEFLOOR card",
				if(ChooseCharacter.character == "CHEYENNE"){
					playCard(cardD);
				}
				break;
			case 3:
				//Standard Turn: Django chose to draw cards",
				// drawCards("DJANGO", step);
				// DRAW CARDS 
				if(ChooseCharacter.character == "DJANGO"){
					GoToChat();
					drawCards(); 
				}
				break;
			case 4:
				//"Tunnel Turn: Ghost played an action card which is hidden",
				if(ChooseCharacter.character == "GHOST"){
					GoToChat();
					playCard(cardB);
				}
				break;
			case 5:
				//"Tunnel Turn: Cheyenne played an action card which is hidden",
				if(ChooseCharacter.character == "CHEYENNE"){
					playCard(cardC);
				}
				break;
			case 6:
				//"Tunnel Turn: Django played an action card which is hidden",
				if(ChooseCharacter.character == "DJANGO"){
					playCard(cardF);
				}
				break;
			case 7:
				//"Switching Turn: ",
				if(ChooseCharacter.character == "GHOST"){
					drawCards();
				}
				//"Switching Turn: Ghost chose to draw cards",
				break;
			case 8:
				if(ChooseCharacter.character == "DJANGO"){
					playCard(cardE);
				}
				break;
			case 9:
				if(ChooseCharacter.character == "CHEYENNE"){
					drawCards();
				}
				//"Switching Turn: Cheyenne chose to draw cards",
				break;
			case 10:
				//"Stealin, Resolving Move: Ghost moved to the adjacent car",
				ghost.transform.position = new Vector3 (cartOneBtm[0] - 1F, cartOneBtm[1], cartOneBtm[2]);
                		ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f;
			        // cheyenne.transform.position = new Vector3 (cartZeroTop[0], cartZeroTop[1], cartZeroTop[2]);
                    		// cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;
				break;
			case 11:
				//"Stealin, Resolving ChangeFloor: Cheyenne moved to the top of the car",
				cheyenne.transform.position = new Vector3 (cartZeroTop[0] + 5F, cartZeroTop[1], cartZeroTop[2]);
                		cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;
			        // Destroy(gem3);
				break;
			case 12:
				//"Stealin, Resolving Rob: Ghost chooses one gem to add to his loot",
				// TODO: GHOST CLICK ON GEM 3 
				Destroy(gem3);
				// gem4.SetActive(true);
				break;
			case 13:
				//"Stealin, Resolving MoveMarshal: Cheyenne moved the Marshal",
				marshal.transform.position = new Vector3 (cartTwoBtm[0], cartTwoBtm[1], cartTwoBtm[2]);
                		marshal.transform.position += marshal.transform.forward * Time.deltaTime * 5f;
				break;
			case 14:
				// "Stealin, Resolving Punch: Django must punch Ghost,Time for Django to choose which loot to force Ghost to drop"
				// gem2.SetActive(true); //purse appears
				Destroy(ghoLoot);
				break;
			case 15:
				//"Stealin, Resolving Punch: Django chose the loot.\nTime for Django to choose where to punch Ghost to\n",//15
				//"Punch: Django chooses to punch Ghost to the last train car",
				punch(); //moves ghost
				break;
			case 16:
			//"Punch: Django chooses to punch Ghost to the last train car\nTime for Django to choose who to shoot\n",
				// "Stealin, Resolving Shoot: Django shoots Ghost",// "New Round, SpeedingUp! 1 SpeedingUp turn",
			   	// shoot();
				Round.text = "ROUND 2:\n-SpeedingUp turn";
				if(ChooseCharacter.character == "DJANGO"){
					Destroy(cardA);
				}
				else if(ChooseCharacter.character == "CHEYENNE"){
					Destroy(CardNewC);
				} else if(ChooseCharacter.character == "GHOST") {
					Destroy(cardE);
				}
				break;
			case 17:	
				// "SpeedingUp Turn 1 (Cheyenne): Cheyenne played a MOVE card",  
				if(ChooseCharacter.character == "CHEYENNE"){
					playCard(cardA);
				}
				break;
			case 18:
				// "SpeedingUp Turn 2 (Cheyenne): Cheyenne chose to draw cards",
				if(ChooseCharacter.character == "CHEYENNE"){
					drawCardsSecond();
				}
				break;
			case 19:
				if(ChooseCharacter.character == "DJANGO"){
							playCard(CardNewB);
				}
				// "SpeedingUp Turn 1 (Django): Django played a CHANGEFLOOR card", 
				break;
			case 20:
				if(ChooseCharacter.character == "DJANGO"){
					drawCardsSecond();
				}
				// "SpeedingUp Turn 2 (Django): Django chose to draw cards",
				break;
			case 21:
				if(ChooseCharacter.character == "GHOST"){
						drawCardsSecond();
				}
				// "SpeedingUp Turn 1 (Ghost): Ghost chose to draw cards",
				break;
			case 22:
				if(ChooseCharacter.character == "GHOST"){
					playCard(CardNewB);
				}
				// "SpeedingUp Turn 2 (Ghost): Ghost played a CHANGEFLOOR card",
				break;
			case 23:
				// "Stealin, Resolving Move: Cheyenne moves to the adjacent train car",
			        cheyenne.transform.position = new Vector3 (cartOneTop[0] + 5F, cartOneTop[1], cartOneTop[2]);
                   		cheyenne.transform.position += cheyenne.transform.forward * Time.deltaTime * 5f;	
				break;
			case 24:
				// "Stealin, Resolving ChangeFloor: Django is moved to the top of the car",
			        django.transform.position = new Vector3 (cartOneTop[0] - 5F, cartOneTop[1], cartOneTop[2]);
                    		django.transform.position += django.transform.forward * Time.deltaTime * 10f;	
				break;
			case 25:
				// "Stealin, Resolving ChangeFloor: Ghost is moved to the top of the car",
			        ghost.transform.position = new Vector3 (cartZeroTop[0] - 1F, cartZeroTop[1], cartZeroTop[2]);
                    		ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f;
				break;
			case 26: 
				// "Results: Game has ended. ADD SCORES Django is the winner!" 
				break;
			case 27:
				Debug.Log("Leaving room");
				//LeaveRoom();
				break;
		}
    }
	*/ 

	/* 
	public void punch(){
		// Django punches Ghost, Ghost is punched back to last train car and with 
		// his initial purse is left in the second last train car. 
		// move ghost to the last train car
		// check if the obj being clicked on is the loot/bandit that we want to move 
		Debug.Log("GHOST IS PUNCHED");
        float posX = cartZeroBtm[0]; 
        float posY = cartZeroBtm[1]; 
        float posZ = cartZeroBtm[2]; 
        ghost.transform.position = new Vector3 (posX, posY, posZ);
        ghost.transform.position += ghost.transform.forward * Time.deltaTime * 5f; // can be any float number
		// shoot();  
	}
	*/ 