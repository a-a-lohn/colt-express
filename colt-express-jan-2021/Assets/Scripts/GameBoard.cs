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
using System.Reflection;
using System;
using Random=System.Random;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour
{
    private static RestClient client = new RestClient("http://13.72.79.112:4242");
    public static string gameHash = WaitingRoom.gameHash;
    public static string savegameId = null;
    public static bool started = false;

    public Button chat;
    bool returningFromChat = false;

    //debug variables
    public static Text debugText;
    public static string debugTextString;
    // public Button button;
    // public Button extension;
    // public Button chooseChar;


    public static ArrayList clickable;
    public static string action = "";
    public Text actionText;
    private static bool newAction = false;
    public Text log;
    int logCounter = 0;

    public static bool myTurn = false;

    public static void setMyTurn(bool turn) {
        //Debug.Log("Your turn!");
        myTurn = turn;
    }

    public static void setNextAction(string newActionText) {
        action = newActionText;
        newAction = true;
    }

    public GameObject canvas;

    public Text exitText;
    
    //public Bandit b;

    public static GameManager gm;

    // LIST OF ALL GAME buttonToObject HERE
    public Button cheyenne;
    public Button belle; 
    public Button tuco; 
    public Button doc; 
    public Button ghost; 
    public Button django; 
    public Button marshal;
    private List<Button> playingBandits = new List<Button>();
    private List<Button> allBandits = new List<Button>();
    private List<Button> allLoot = new List<Button>();

    
    public Button gem1; 
    public Button gem2; 
    public Button gem3; 
    public Button gem4;
    public Button gem5;
    public Button gem6;

    // public Button ghoLoot;

    public GameObject bulletCard;

    // propmpt messages 
    public Text promptDrawCardsOrPlayCardMsg;
    public Text promptChooseLoot; 
    public Text promptPunchTarget; 
    public Text promptHorseAttackMsg;

    public static Dictionary<Button, object> buttonToObject = new Dictionary<Button, object>();

    // public Text clickableGOsText;
    public Text currentRoundText;
    public Text turnNum;
    public Text currentPlayer;

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

    public Button handCard1; 
    public Button handCard2; 
    public Button handCard3; 
    public Button handCard4; 
    public Button handCard5;
    public Button handCard6; 
    public Button handCard7; 
    public Button handCard8; 
    public Button handCard9;  
    public Button handCard10; 
    public Button handCard11; 
    private List<Button> goHandCard = new List<Button>(); 

    public Button drawCardsButton;
    static bool canDrawCards = false;
    
    /* a card has 4 attributes */
    public Text handCardActionType1; 
    // public Text handCardOneSaveForNetRound;
    // public Text handCardOneIsFaceDown; 
    // public Text handCardOneBelongsTo;

    public Text handCardActionType2;
    // public Text handCardTwoSaveForNetRound;
    // public Text handCardTwoIsFaceDown; 
    // public Text handCardTwoBelongsTo;

    public Text handCardActionType3; 
    // public Text handCardThreeSaveForNetRound;
    // public Text handCardThreeIsFaceDown; 
    // public Text handCardThreeBelongsTo;

    public Text handCardActionType4; 
    // public Text handCardFourSaveForNetRound;
    // public Text handCardFourIsFaceDown; 
    // public Text handCardFourBelongsTo;

    public Text handCardActionType5; 
    public Text handCardActionType6; 
    public Text handCardActionType7; 
    public Text handCardActionType8; 
    public Text handCardActionType9; 
    public Text handCardActionType10; 
    public Text handCardActionType11; 

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

    public List<ActionCard> actionCardList; 

    bool calledMapTrain = false;

    private List<float> oneBtm = new List<float>() {706.0F, 816.5F, -364.9F}; 
    private List<float> oneTop = new List<float>() {705.4F, 879.0F, -364.9F}; 
    private List<float> twoTop = new List<float>() {962.9F, 873.7F, -364.9F}; 
    private List<float> twoBtm = new List<float>() {918.2F, 815.7F, -364.9F}; 
    private List<float> threeTop = new List<float>() {1149.4F, 883.2F, -364.9F}; 
    private List<float> threeBtm = new List<float>() {1139.7F, 821.5F, -364.9F}; 
    private List<float> fourTop = new List<float>() {1353.2F, 820.2F, -364.9F}; 
    private List<float> fourBtm = new List<float>() {1357.1F, 873.5F, -364.9F}; 
    private List<float> locTop = new List<float>() {1594.2F, 873.5F, -364.9F}; 
    private List<float> locBtm = new List<float>() {1597.7F, 816.5F, -364.9F};


    void Start(){  
        //Screen.SetResolution(1080, 1920);  
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

        if(!returningFromChat) {
            currentRoundText.text = "";
            exitText.text ="";
            log.text = "";
            currentPlayer.text = "";
            actionText.text = "";
            addAllBandits();
            SFS.setGameBoard();
            initMap();
        }
        if(returningFromChat) Debug.Log("returning from chat");
        EnterGameBoardScene();
    }

    // Update is called once per frame
    void Update()
    {
        //makeEmptyCardsUninteractable();
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
            Debug.Log("currentbandit on mouse: "+ gm.currentBandit.getCharacter());
        }

        if(myTurn) {
            currentPlayer.text = "You!";
        } else {
            currentPlayer.text = gm.currentBandit.characterAsString;
        }
        if(newAction) {
            actionText.text = action;
        }
    }

    public void UpdateGameState(BaseEvent evt) {
        Debug.Log("updategamestate called");
        setAllClickable();
        clearHand();
        //makeEmptyCardsUninteractable();

        ISFSObject responseParams = (SFSObject)evt.Params["params"];
        string logStr = responseParams.GetUtfString("log") + "\n\n";
        if (logStr != null) {
            logCounter++;
            if(logCounter % 3 == 0) {
                log.text = logStr;
            } else {
                log.text += logStr;   
            }
        }
        
        gm = (GameManager)responseParams.GetClass("gm");
        GameManager.replaceInstance(gm);
        reassignReferences();

        if(gm.roundIndex ==  null) {
            Debug.Log("gm.roundIndex ==  null");
        }
        if(gm.currentRound ==  null) {
            Debug.Log(" gm.currentRound ==  null");
        }
        if(gm.currentRound.roundTypeAsString ==  null) {
            Debug.Log("gm.currentRound.roundTypeAsString ==  null");
        }
        if(gm.currentRound.turns ==  null) {
            Debug.Log("gm.currentRound.turns ==  null");
        }
        int num = gm.roundIndex+1;
        currentRoundText.text = "Round #" + num + " - " + gm.currentRound.roundTypeAsString + "\n";
        int ti = 1;
        foreach(Turn t in gm.currentRound.turns) {
            currentRoundText.text += "Turn " + ti + ": " + t.turnTypeAsString + "\n";
            if(t.Equals(gm.currentRound.currentTurn)) {
                turnNum.text = "Current turn #: " + ti;
            }
            ti++;
        }
        
        //addAllBandits();

        ArrayList banditsArray = gm.bandits;
        foreach (Bandit b in banditsArray) {
            if (b.characterAsString == "CHEYENNE") {
                buttonToObject[cheyenne] = b;
                //Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(cheyenne);
            }
            if (b.characterAsString == "BELLE") {
                buttonToObject[belle] = b;
                //Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(belle);
            }
            if (b.characterAsString == "TUCO") {
                buttonToObject[tuco] = b;
                //Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(tuco);
            }
            if (b.characterAsString == "DOC") {
                buttonToObject[doc] = b;
                //Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(doc);
            }
            if (b.characterAsString == "GHOST") {
                buttonToObject[ghost] = b;
                //Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(ghost);
            }
            if (b.characterAsString == "DJANGO") {
                buttonToObject[django] = b;
                //Debug.Log(b.characterAsString + " added as button");
                playingBandits.Add(django);
            }
            
            /* place the bandits in their starting positions */
            mapBandit(gm);

            // foreach(Button ab in allBandits){
            //     if(ab != null) {
            //         if(!playingBandits.Contains(ab)){
            //             Debug.Log("Destroying " + ab.name);
            //             Destroy(ab.gameObject);
            //         }
            //     }
            // }

            if(b.characterAsString == gm.currentBandit.characterAsString){
                /*
                * OBJECTS ARE NEWLY CREATED WHEN SERIALIZED. IF MULTIPLE REFERENCES EXIST FOR THE SAME OBJECT, THEY WILL BE TREATED AS DIFFERENT OBJECTS
                */
                Debug.Log("reassigning current bandit ref to " + b.characterAsString);
                gm.currentBandit = b;
            }

            if(b.characterAsString == ChooseCharacter.character){
                b.updateMainDeck();

                if (gm.strGameStatus.Equals("SCHEMIN")) {
                    if(gm.currentRound.getTurnCounter() == 0 && b.hand.Count == 0){
                        b.drawCards(6);
                        if(b.getCharacter().Equals("DOC")){
                            b.drawCards(1);
                        }
                        b.updateOtherDecks();
                        b.updateOtherHands();
                    }
                }
                
                b.updateMainHand();
                // assign to gameobjects on screen 
                //ArrayList currCards = b.hand;
                int index = 0; 
                ActionCard ac;
                BulletCard bc;
                Debug.Log("num of currcards: " + b.hand.Count);
                Debug.Log("num of currcards b1: " + gm.currentBandit.hand.Count);
                foreach(Card currCard in b.hand){
                    try{
                        ac = (ActionCard) currCard;
                        buttonToObject[goHandCard[index]] = ac;
                        //Debug.Log("trying to cast card as action card");
                    } catch(Exception e) {
                        bc = (BulletCard) currCard;
                        buttonToObject[goHandCard[index]] = bc;
                        Debug.Log("not initializing an action card");
                    }
                    index++;
                }
                mapActionCards(handCard1, handCardActionType1);
                mapActionCards(handCard2, handCardActionType2);
                mapActionCards(handCard3, handCardActionType3);
                mapActionCards(handCard4, handCardActionType4);
                mapActionCards(handCard5, handCardActionType5);
                mapActionCards(handCard6, handCardActionType6);
                mapActionCards(handCard7, handCardActionType7);
                mapActionCards(handCard8, handCardActionType8);
                mapActionCards(handCard9, handCardActionType9);
                mapActionCards(handCard10, handCardActionType10);
                mapActionCards(handCard11, handCardActionType11);
            }
        }
        //Debug.Log(SFS.step);

        // Money m;
        // Whiskey w;
        // int count = 0;
        // foreach (Loot l in gm.loot){
        //     try{
        //         m = (Money) l;
        //         buttonToObject[allLoot[count]] = l;
        //         Debug.Log("casting as Money");
        //     }
        //     catch(Exception e){
        //         w = (Whiskey) l;
        //         buttonToObject[allLoot[count]] = l;
        //         Debug.Log("casting as Whiskey");
        //     }
        //     count++;
        // }

        //mapLoot(gm);

        gm.playTurn();

    }

    void makeEmptyCardsUninteractable(){
        // make all empty cards uninteractable 
        if(handCardActionType1.text == ""){
            handCard1.interactable = false;
        }else if(handCardActionType2.text == ""){
           handCard2.interactable = false;
        }else if(handCardActionType3.text == ""){
            handCard3.interactable = false;
        }else if(handCardActionType4.text == ""){
            handCard4.interactable = false;
        }else if(handCardActionType5.text == ""){
            handCard5.interactable = false;
        }else if(handCardActionType6.text == ""){
            handCard6.interactable = false;
        }else if(handCardActionType7.text == ""){
            handCard7.interactable = false;
        }else if(handCardActionType8.text == ""){
            handCard8.interactable = false;
        }else if(handCardActionType9.text == ""){
            handCard9.interactable = false;
        }else if(handCardActionType10.text == ""){
            handCard10.interactable = false;
        }else if(handCardActionType11.text == ""){
            handCard11.interactable = false;
        }
    }

    void reassignReferences() {
        gm.currentRound = (Round)gm.rounds[gm.roundIndex];
        foreach(Round r in gm.rounds) {
            if(r.turns ==  null) Debug.Log("r.turns is null");
            if(r.turnCounter ==  null) Debug.Log("r.turnCounter is null");
            r.currentTurn = (Turn)r.turns[r.turnCounter];
        }
        if(gm.banditPositions ==  null) Debug.Log("gm.bp is null");
        if(gm.trainRoof ==  null) Debug.Log("gm.trainRoof is null");
        foreach(TrainUnit tr in gm.trainRoof){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tr.carTypeAsString == tu.carTypeAsString & tr.carFloorAsString == tu.carFloorAsString) {
                gm.banditPositions[b.characterAsString] = tr;
                }
            }
        }
        if(gm.trainCabin ==  null) Debug.Log("gm.trainCabin is null");
        foreach(TrainUnit tc in gm.trainCabin){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tc.carTypeAsString == tu.carTypeAsString & tc.carFloorAsString == tu.carFloorAsString) {
                    gm.banditPositions[b.characterAsString] = tc;
                }
            }
        }
    }

    public static void enableDrawCardsButton() {
        canDrawCards = true;
    }

    public void drawCardsClicked() {
        if(canDrawCards) {
            canDrawCards = false;
            gm.drawCards(3);
        }
    }

    public void buttonClicked(Button btn){    
        if(!myTurn) {
            Debug.Log("not my turn!");
        } else {
            Debug.Log( btn.name + " IS CLICKED");
            //Debug.Log("Clickable has " + clickable.Count + "items");
            //promptPunchTarget.text = btn.name + "IS CLICKED"; 
            //punchedBandit = btn.name;
            // if buttonToObject[btn] is an actioncard, call playCard(buttonToObject[btn])
            if(clickable.Contains(buttonToObject[btn])) {
                Debug.Log("this is a clickable item!");
                //all calls back to GM should be here

                newAction = false;
                actionText.text = "";
                try {
                    ActionCard currActionCard = (ActionCard)buttonToObject[btn];
                    gm.playCard(currActionCard); 
                } catch(Exception e) {
                    Debug.Log("not an action card");
                }
            } else Debug.Log("not clickable!");
        }
        btn.interactable = true;
    }

    public void horseBtnOneClicked(){
        promptHorseAttackMsg.text = "Horse Prompt btn one is clicked!";

    }

    public void horseBtnTwoClicked(){
        promptHorseAttackMsg.text = "Horse Prompt btn two is clicked!";

    }

    public void addAllBandits(){
        if(belle != null) allBandits.Add(belle);
        if(cheyenne != null) allBandits.Add(cheyenne);
        if(doc != null) allBandits.Add(doc);
        if(django != null) allBandits.Add(django);
        if(tuco != null) allBandits.Add(tuco);
        if(ghost != null) allBandits.Add(ghost);
        if(marshal != null) allBandits.Add(marshal);
    }

    /* initMap initializes the <Button, object> hashmap */
    public void initMap(){
        /* init. current hand */
        buttonToObject.Add(handCard1, "null"); 
        buttonToObject.Add(handCard2, "null"); 
        buttonToObject.Add(handCard3, "null"); 
        buttonToObject.Add(handCard4, "null"); 
        buttonToObject.Add(handCard5, "null"); 
        buttonToObject.Add(handCard6, "null"); 
        buttonToObject.Add(handCard7, "null"); 
        buttonToObject.Add(handCard8, "null"); 
        buttonToObject.Add(handCard9, "null"); 
        buttonToObject.Add(handCard10, "null"); 
        buttonToObject.Add(handCard11, "null"); 

        /* init. all bandits */
        buttonToObject.Add(belle, "null"); 
        buttonToObject.Add(cheyenne, "null"); 
        buttonToObject.Add(doc, "null"); 
        buttonToObject.Add(django, "null"); 
        buttonToObject.Add(tuco, "null"); 
        buttonToObject.Add(ghost, "null"); 
        buttonToObject.Add(marshal, "null");


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

        buttonToObject.Add(gem1, "null");
        // buttonToObject.Add(gem2, "null");
        // buttonToObject.Add(gem3, "null");
        // buttonToObject.Add(gem4, "null");
        // buttonToObject.Add(gem5, "null");

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

        

        goHandCard.Insert(0, handCard1);
        goHandCard.Insert(1, handCard2);
        goHandCard.Insert(2, handCard3);
        goHandCard.Insert(3, handCard4);
        goHandCard.Insert(4, handCard5);
        goHandCard.Insert(5, handCard6);
        goHandCard.Insert(6, handCard7);
        goHandCard.Insert(7, handCard8);
        goHandCard.Insert(8, handCard9);
        goHandCard.Insert(9, handCard10);
        goHandCard.Insert(10, handCard11);

        allLoot.Insert(0, gem1);
        // allLoot.Insert(1, gem2);
        // allLoot.Insert(2, gem3);
        // allLoot.Insert(3, gem4);
        // allLoot.Insert(4, gem5);

        /* init all action texts */
        handCardActionType1.text = ""; 
        handCardActionType2.text = ""; 
        handCardActionType3.text = ""; 
        handCardActionType4.text = ""; 
        handCardActionType5.text = ""; 
        handCardActionType6.text = ""; 
        handCardActionType7.text = ""; 
        handCardActionType8.text = ""; 
        handCardActionType9.text = ""; 
        handCardActionType10.text = ""; 
        handCardActionType11.text = ""; 
    }

    void clearHand(){
        buttonToObject[handCard1] = "null"; 
        buttonToObject[handCard2] = "null"; 
        buttonToObject[handCard3] = "null"; 
        buttonToObject[handCard4] = "null";
        buttonToObject[handCard5] = "null"; 
        buttonToObject[handCard6] = "null"; 
        buttonToObject[handCard7] = "null"; 
        buttonToObject[handCard8] = "null"; 
        buttonToObject[handCard9] = "null"; 
        buttonToObject[handCard10] = "null"; 
        buttonToObject[handCard11] = "null"; 
    }

    // public void mapTrain(GameManager gm){
    //   public ArrayList trainRoof ;
    //     public ArrayList trainCabin;
    //  foreach(object oneRoof in gm.trainRoof){
    //      buttonToObject[] = oneRoof;

    //  }
    // }

    // public void mapBanditPositions(GameManager gm){
    //  foreach(TrainUnit cabin in gm.trainCabin){
    //      ArrayList occupied = cabin.getBanditsHere();
    //      foreach(Bandit b in occupied) {

    //          TrainUnit t = b.getPosition();
    //          Button button = buttonToObject.FirstOrDefault(x => x.Value.Equals(t)).Key;

    //          if(b.getCharacter().ToLower() == "belle"){
    //              Vector3 temp = button.transform.position;
    //              belle.transform.position = temp; // might need a delta function here 
    //          } else if(b.getCharacter().ToLower() == "cheyenne"){
    //              Vector3 temp = button.transform.position;
    //              cheyenne.transform.position = temp;
    //          } else if(b.getCharacter().ToLower() == "django"){
    //              Vector3 temp = button.transform.position;
    //              django.transform.position = temp;
    //          }else if(b.getCharacter().ToLower() == "doc"){
    //              Vector3 temp = button.transform.position;
    //              doc.transform.position = temp;
    //          }else if(b.getCharacter().ToLower() == "ghost"){
    //              Vector3 temp = button.transform.position;
    //              cheyenne.transform.position = temp;
    //          }else if(b.getCharacter().ToLower() == "tuco"){
    //              Vector3 temp = button.transform.position;
    //              cheyenne.transform.position = temp;
    //          }   
    //      }
    //      }    
    // }

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

    public void mapBandit(GameManager gm){
        foreach(TrainUnit tr in gm.trainRoof){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tr.containsBandit(b)){
                    placeBanditAt(b, tu.carTypeAsString, tu.carFloorAsString);
                };
            }
        }
        foreach(TrainUnit tc in gm.trainCabin){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tc.containsBandit(b)){
                    placeBanditAt(b, tu.carTypeAsString, tu.carFloorAsString);
                };
            }
        }
    }

    public void placeBanditAt(Bandit b, string cartype, string carfloor){
        // places the bandit according to the parameters 
        // Button banditBtn = buttonToObject.FirstOrDefault(x => x.Value.Equals(b)).Key; // DOESN'T WORK 
        // Find the button that corresponds to Bandit b 
        Button banditBtn = allBandits[0];
        //Debug.Log("Bandit passed in is : " + b.characterAsString); 
        foreach(Button aBanditBtn in allBandits){
            if(aBanditBtn.name.ToUpper() == b.characterAsString){
                //playingBandits.Add(aBanditBtn);
                banditBtn = aBanditBtn;
            }
        }
        
        if(carfloor == "CABIN"){
            if(cartype == "LOCOMOTIVE"){
                banditBtn.transform.position = new Vector3 (locBtm[0], locBtm[1], locBtm[2]);
            }else if(cartype == "CAR1"){
                banditBtn.transform.position = new Vector3 (oneBtm[0], oneBtm[1], oneBtm[2]);
            }else if(cartype == "CAR2"){
                banditBtn.transform.position = new Vector3 (twoBtm[0], twoBtm[1], twoBtm[2]);
            }else if(cartype == "CAR3"){
                banditBtn.transform.position = new Vector3 (threeBtm[0], threeBtm[1], threeBtm[2]);
            }else if(cartype == "CAR4"){
                banditBtn.transform.position = new Vector3 (fourBtm[0], fourBtm[1], fourBtm[2]);
            }else if(cartype == "CAR5"){
                banditBtn.transform.position = new Vector3 (706.0F, 816.5F, -364.9F);
            }else if(cartype == "CAR6"){
                banditBtn.transform.position = new Vector3 (706.0F, 816.5F, -364.9F);
            }else{
                // cartype == "STAGECOACH"
                banditBtn.transform.position = new Vector3 (706.0F, 816.5F, -364.9F);
            }
        }else{
            if(cartype == "LOCOMOTIVE"){
                banditBtn.transform.position = new Vector3 (locTop[0], locTop[1], locTop[2]);
            }else if(cartype == "CAR1"){
                banditBtn.transform.position = new Vector3 (oneTop[0], oneTop[1], oneTop[2]);
            }else if(cartype == "CAR2"){
                banditBtn.transform.position = new Vector3 (twoTop[0], twoTop[1], twoTop[2]);
            }else if(cartype == "CAR3"){
                banditBtn.transform.position = new Vector3 (threeTop[0], threeTop[1], threeTop[2]);
            }else if(cartype == "CAR4"){
                banditBtn.transform.position = new Vector3 (fourTop[0], fourTop[1], fourTop[2]);
            }else if(cartype == "CAR5"){
                banditBtn.transform.position = new Vector3 (706.0F, 816.5F, -364.9F);
            }else if(cartype == "CAR6"){
                banditBtn.transform.position = new Vector3 (706.0F, 816.5F, -364.9F);
            }else{
                // cartype == "STAGECOACH"
                banditBtn.transform.position = new Vector3 (706.0F, 816.5F, -364.9F);
            }
        }

    }

    // public void mapLoot(GameManager gm){

    //     foreach(Loot l in gm.loot){

    //         if(l.getBelongsTo() != null){

    //         } else if(l.getPosition() != null){
    //             TrainUnit tr = l.getPosition();
    //             placeLootOnTrain(l, tr.carTypeAsString, tr.carFloorAsString);
    //         }

    //     }


    // }

    public void placeLootOnTrain(Loot l, string carType, string carFloor){



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

    /* setAllNonClickable sets all buttons to be non-clickable */
    public void setAllNonClickable(){
        Button[] allButtons = UnityEngine.Object.FindObjectsOfType<Button>();
        foreach(Button aBtn in allButtons){
            aBtn.interactable = false; 
        }
    }

    public void setAllClickable(){
        Button[] allButtons = UnityEngine.Object.FindObjectsOfType<Button>();
        foreach(Button aBtn in allButtons){
            aBtn.interactable = true; 
        }
    }

    // public static bool isMyTurn() {
    //     if(gm.currentBandit.getCharacter().Equals(ChooseCharacter.character)){
    //         return true;
    //     }
    //     return false;
    // }


    public void mapActionCards(Button button, Text buttonText){

        try {
            string nullstr = (string)buttonToObject[button];
            buttonText.text = "";
            button.interactable = false;
            return;
        } catch(Exception e) {
        }

        try {
            ActionCard card = (ActionCard)buttonToObject[button];
            buttonText.text = card.actionTypeAsString;
            //Debug.Log("setting " + button.name + " to " + card.actionTypeAsString);
        } catch(Exception e) {
            //Debug.Log("not an action card in MAP");
            buttonText.text = "Bullet";
        }
    }

    public void LeaveRoom() {
        SFS.LeaveRoom();
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




    /*
    *
    *
    * SFS COMMUNICATION METHODS
    *
    *
    */




    public void EnterGameBoardScene() {
        Debug.Log("entering scene");
        ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.enterGameBoardScene",obj);
        SFS.Send(req);
        Debug.Log("Sent enter scene message");
    }

    public static void SendNewGameState(string message) {
        ISFSObject obj = SFSObject.NewInstance();
        //Debug.Log("sending new game state");
        obj.PutClass("gm", gm);
        obj.PutUtfString("log", message);
        ExtensionRequest req = new ExtensionRequest("gm.newGameState",obj);
        SFS.Send(req);
        Debug.Log("sent game state");
    }

    public static void trace(string msg) {
    //  debugText.text += (debugText.text != "" ? "\n" : "") + msg;
    }

    public void GoToWaitingRoom(){
        Invoke("GoToWaitingRoom2",5);
    }

    void GoToWaitingRoom2(){
        SceneManager.LoadScene("WaitingRoom");
    }

    public void GoToChat(){
        returningFromChat = true;
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

    public static void promptHorseAttack(int trainIndex) {
        if (gm.bandits.Count == gm.banditPositions.Count) {
            started = true;
            return;
        }
        ISFSObject obj = SFSObject.NewInstance();
        if (1==1) { //gm.banditPositions.Contains()
            return;
        }
        String response;
        //prompt user whether they want to get off at this train (indicated by trainIndex). If yes, response should be "y", if no then "n"
        obj.PutUtfString("ans", response);
        ExtensionRequest req = new ExtensionRequest("gm.choosePosition", obj);
        SFS.Send(req);
    }
}

