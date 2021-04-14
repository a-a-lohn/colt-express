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
using System.Text;

public class GameBoard : MonoBehaviour
{
    private static RestClient client = new RestClient("http://13.72.79.112:4242");
    public static string gameHash = WaitingRoom.gameHash;
    public bool started = false;

    public Button chat;
    bool returningFromChat = false;

    Random r = new Random(10);

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
    public Text currentRoundText;
    public Text turnNum;
    public Text currentPlayer;
    public Text gameStatus;
    public Text resolveCard;
    public Button proceed;
    static bool noChoice;
    static string heldMessage;
    public static string saveGameId = "";
    public static bool saveGameOnLobby = false;

    public Text log;
    int logCounter = 0;

    public static bool myTurn = false;

    public static void setMyTurn(bool turn) {
        myTurn = turn;
    }

    public static void setNextAction(string newActionText) {
        action = newActionText;
        newAction = true;
    }

    public static void setNoChoice(string message) {
        Debug.Log("setting noChoice to true");
        noChoice = true;
        heldMessage = message;
    }

    public Text belleRevolver;
    public Text cheyenneRevolver;
    public Text docRevolver;
    public Text djangoRevolver;
    public Text ghostRevolver;
    public Text tucoRevolver;

    public GameObject canvas;

    public Text exitText;
    
    public static GameManager gm;

    // LIST OF ALL GAME buttonToObject HERE
    public Button cheyenne;
    public Button belle; 
    public Button tuco; 
    public Button doc; 
    public Button ghost; 
    public Button django; 
    public Button marshal;

    public GameObject cheyenneProf;
    public GameObject belleProf; 
    public GameObject tucoProf; 
    public GameObject docProf; 
    public GameObject ghostProf; 
    public GameObject djangoProf; 
    
    private List<Button> playingBandits = new List<Button>();
    private List<Button> allBandits = new List<Button>();

    private List<Button> allGem = new List<Button>();
    private List<Button> allPurse = new List<Button>();
    private List<Button> allBox = new List<Button>();
    private List<Button> allWhiskey = new List<Button>();

    public Button gem1; 
    public Button gem2; 
    public Button gem3; 
    public Button gem4;
    public Button gem5;
    public Button gem6;

    //
    public Button horseBtnOne;
    public Button horseBtnTwo;

    public Button horseOne;
    public Button horseTwo;
    public Button horseThree;
    public Button horseFour;

    public Button stagecoach;

    // public Button ghoLoot;
    public Button purse1;
    public Button purse2;
    public Button purse3;
    public Button purse4;
    public Button purse5;
    public Button purse6;
    public Button purse7;
    public Button purse8;
    public Button purse9;
    public Button purse10;
    public Button purse11;
    public Button purse12;
    public Button purse13;
    public Button purse14;
    public Button purse15;
    public Button purse16;
    public Button purse17;
    public Button purse18;

    public Button box1;
    public Button box2;

    public Button whiskey1;
    public Button whiskey2;
    public Button whiskey3;
    public Button whiskey4;
    public Button whiskey5;
    public Button whiskey6;

    public GameObject bulletCard;

    // propmpt messages 
    public Text promptDrawCardsOrPlayCardMsg;
    public Text promptChooseLoot; 
    public Text promptPunchTarget; 
    public Text promptHorseAttackMsg;

    public static Dictionary<Button, object> buttonToObject = new Dictionary<Button, object>();

    // public Text clickableGOsText;

    private List<Button> goNeutralBulletCards;

    // a list of bullet cards for each and every bandit 
    private List<Button> goBELLEBulletCards; 
    private List<Button> goCHEYENNEBulletCards; 
    private List<Button> goDOCBulletCards; 
    private List<Button> goTUCOBulletCards; 
    private List<Button> goDJANGOBulletCards; 
    private List<Button> goGHOSTBulletCards; 

    // a list of action cards for each and every bandit's hand 
    // private List<Button> goBELLEHand; 
    // private List<Button> goCHEYENNEHand; 
    // private List<Button> goDOCHand; 
    // private List<Button> goGHOSTHand; 
    // private List<Button> goTUCOHand; 
    // private List<Button> goDJANGOHand; 


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
    //bool sleepInvoked = false;
    
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
    public Button trainfourTop; 
    public Button trainfourBtm;
    public Button locoBtm; 
    public Button locoTop;

    public List<Button> trainRoofs; 
    public List<Button> trainCabins; 
    
    public static string punchedBandit;

    public List<ActionCard> actionCardList; 

    bool calledMapTrain = false;

    private List<float> oneBtm = new List<float>() {1311.9F, 838.5F, -364.9F}; 
    private List<float> oneTop = new List<float>() {1314.9F, 885.5F, -364.9F}; 
    private List<float> twoTop = new List<float>() {1147.9F, 884.4F, -364.9F}; 
    private List<float> twoBtm = new List<float>() {1148.9F, 839.6F, -364.9F}; 
    private List<float> threeTop = new List<float>() {982.9F, 883.2F, -364.9F}; 
    private List<float> threeBtm = new List<float>() {982.0F, 835.2F, -364.9F}; 
    private List<float> fourBtm = new List<float>() {831.0F, 838.5F, -364.9F}; 
    private List<float> fourTop = new List<float>() {824.1F, 883.0F, -364.9F}; 
    private List<float> locTop = new List<float>() {1495.9F, 885.5F, -364.9F}; 
    private List<float> locBtm = new List<float>() {1498.6F, 835.2F, -364.9F};

    private List<float> belLoot = new List<float>() {834.7F, 1121.2F, -364.9F};
    private List<float> cheLoot = new List<float>() {1047.6F, 1002.1F, -364.9F};
    private List<float> docLoot = new List<float>() {1075.4F, 1000.8F, -364.9F};
    private List<float> djaLoot = new List<float>() {1099.4F, 1000.8F, -364.9F};
    private List<float> ghoLoot = new List<float>() {1061.2F, 1000.3F, -364.9F};
    private List<float> tucLoot = new List<float>() {1126.7F, 1003.6F, -364.9F};

    private List<float> faraway= new List<float>() {117.7F, -373.8F, -364.5F};

    void Start(){  
        /*
        if (SFS.getSFS() == null) {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
            Debug.Log("SFS was null. Setting it now");
        }
        if (!SFS.IsConnected()) {
            SFS.Connect("test");
            Debug.Log("was not connected. Connecting now");
        }*/
        // Screen.SetResolution(1080, 1920, false);  
        // Debug.Log("bel gem: " + belleGemGO.transform.position); 
        // Debug.Log("bel belleWhisGO: " + belleWhisGO.transform.position); 
        // Debug.Log("bel belStrGo: " + belStrGo.transform.position); 
        // Debug.Log("bel belPurGo: " + belPurGo.transform.position); 

        //belle.transform.position = new Vector3(fourTop[0], fourTop[1], fourTop[2]);

        // Debug.Log("locoTop" + locoTop.transform.position); 
        // Debug.Log("locoBTM" + locoBtm.transform.position); 
        // Debug.Log("cartOneTop" + trainOneTop.transform.position); 
        // Debug.Log("cartOneBtm" + trainOneBtm.transform.position); 
        // Debug.Log("cartTwoTop" + trainTwoTop.transform.position); 
        // Debug.Log("cartTwoBtm" + trainTwoBtm.transform.position); 
        // Debug.Log("cartThreeTop" + trainThreeTop.transform.position); 
        // Debug.Log("cartThreeBtm" + trainThreeBtm.transform.position); 
        // Debug.Log("cartFourTop" + trainfourTop.transform.position); 
        // Debug.Log("cartFourBtm" + trainfourBtm.transform.position); 

        // Debug.Log("bel prof" + belleProf.transform.position); 
        // Debug.Log("che prof" + cheyenneProf.transform.position); 
        // Debug.Log("doc prof" + docProf.transform.position); 
        // Debug.Log("dja prof" + djangoProf.transform.position); 
        // Debug.Log("tuc prof" + tucoProf.transform.position); 
        // Debug.Log("gho prof" + ghostProf.transform.position);

        locBtm = new List<float>() {locoBtm.transform.position[0], locoBtm.transform.position[1], locoBtm.transform.position[2]};
        locTop = new List<float>() {locoTop.transform.position[0], locoTop.transform.position[1], locoTop.transform.position[2]};
        oneBtm = new List<float>() {trainOneBtm.transform.position[0], trainOneBtm.transform.position[1], trainOneBtm.transform.position[2]};
        oneTop = new List<float>() {trainOneTop.transform.position[0], trainOneTop.transform.position[1], trainOneTop.transform.position[2]};
        twoBtm = new List<float>() {trainTwoBtm.transform.position[0], trainTwoBtm.transform.position[1], trainTwoBtm.transform.position[2]};
        twoTop = new List<float>() {trainTwoTop.transform.position[0], trainTwoTop.transform.position[1], trainTwoTop.transform.position[2]};
        threeBtm = new List<float>() {trainThreeBtm.transform.position[0], trainThreeBtm.transform.position[1], trainThreeBtm.transform.position[2]};
        threeTop = new List<float>() {trainThreeTop.transform.position[0], trainThreeTop.transform.position[1], trainThreeTop.transform.position[2]};
        fourBtm = new List<float>() {trainfourBtm.transform.position[0], trainfourBtm.transform.position[1], trainfourBtm.transform.position[2]};
        fourTop = new List<float>() {trainfourTop.transform.position[0], trainfourTop.transform.position[1], trainfourTop.transform.position[2]};


        // testing 
        // belle.transform.position = new Vector3(oneTop[0], oneTop[1], oneTop[2]);
        // cheyenne.transform.position = new Vector3(twoTop[0], twoTop[1], twoTop[2]);
        // doc.transform.position = new Vector3(threeTop[0], threeTop[1], threeTop[2]);
        // django.transform.position = new Vector3(fourTop[0], fourTop[1], fourTop[2]);
        // tuco.transform.position = new Vector3(fourBtm[0], fourBtm[1], fourBtm[2]);
        // ghost.transform.position = new Vector3(locTop[0], locTop[1], locTop[2]);
        // marshal.transform.position = new Vector3(locBtm[0], locBtm[1], locBtm[2]);

        locBtm = new List<float>() {locoBtm.transform.position[0], locoBtm.transform.position[1], locoBtm.transform.position[2]};
        locTop = new List<float>() {locoTop.transform.position[0], locoTop.transform.position[1], locoTop.transform.position[2]};
        oneBtm = new List<float>() {trainOneBtm.transform.position[0], trainOneBtm.transform.position[1], trainOneBtm.transform.position[2]};
        oneTop = new List<float>() {trainOneTop.transform.position[0], trainOneTop.transform.position[1], trainOneTop.transform.position[2]};
        twoBtm = new List<float>() {trainTwoBtm.transform.position[0], trainTwoBtm.transform.position[1], trainTwoBtm.transform.position[2]};
        twoTop = new List<float>() {trainTwoTop.transform.position[0], trainTwoTop.transform.position[1], trainTwoTop.transform.position[2]};
        threeBtm = new List<float>() {trainThreeBtm.transform.position[0], trainThreeBtm.transform.position[1], trainThreeBtm.transform.position[2]};
        threeTop = new List<float>() {trainThreeTop.transform.position[0], trainThreeTop.transform.position[1], trainThreeTop.transform.position[2]};
        fourBtm = new List<float>() {trainfourBtm.transform.position[0], trainfourBtm.transform.position[1], trainfourBtm.transform.position[2]};
        fourTop = new List<float>() {trainfourTop.transform.position[0], trainfourTop.transform.position[1], trainfourTop.transform.position[2]};


        // oneBtm = new Vector3(trainOneBtm.transform.position[0], trainOneBtm.transform.position[1], trainOneBtm.transform.position[2]);
        // oneTop = new Vector3(trainOneBtm.transform.position[0], trainOneBtm.transform.position[1], trainOneBtm.transform.position[2]);

        if(!returningFromChat) {
            currentRoundText.text = "";
            exitText.text ="";
            log.text = "";
            currentPlayer.text = "";
            actionText.text = "";
            resolveCard.text = "";
            addAllBandits();
            SFS.setGameBoard();
            initMap();

            resetGB();
        }
        EnterGameBoardScene();
    }

    // Update is called once per frame
    void Update()
    {
        if(newAction) {
            actionText.text = action;
        }
        if(noChoice) {
            actionText.text += ". Click to proceed";
            proceed.interactable = true;
        }
        if (SFS.IsConnected()) {
            SFS.ProcessEvents();
        }

        if (Input.GetMouseButtonDown(0)){
            Debug.Log("Clicked");
            //Debug.Log("currentbandit on mouse: " + gm.currentBandit.getCharacter());
        }

        if(myTurn) {
            currentPlayer.text = "Your turn!";
        } else if (gm != null & gm.currentBandit != null){
            currentPlayer.text = gm.currentBandit.characterAsString;
        }
    }

    public void onProceed() {
        Debug.Log("sending held game state");
        setMyTurn(false);
        noChoice = false;
        newAction = false;
        actionText.text = "";
        SendNewGameState(heldMessage);
        heldMessage = "";
    }

    /* getRandOffset() picks and returns a random float from a set of pre-defined floats */
    public float getRandOffset(){
        var values = new[] { 20.0F, 40.0F, 50.0F, -20.0F, -30.0F, -50.0F, 46.0F, 35.0F, -46.0F, -35.0F, 20.0F, 40.0F, 50.0F, -20.0F, -30.0F, -50.0F, 46.0F, 35.0F, -46.0F, -35.0F, 20.0F, 40.0F, 50.0F, -20.0F, -30.0F, -50.0F, 46.0F, 35.0F, -46.0F, -35.0F, -20.0F, -30.0F, -50.0F, 46.0F, 35.0F, -46.0F, -35.0F, 20.0F, 40.0F, 50.0F, -20.0F, -30.0F, -50.0F, 46.0F, 35.0F, -46.0F, -35.0F };
        int ri = r.Next(0, values.Length);
        float result = values[ri];
        //Debug.Log("THE RANDOM OFFSET IS: " + result);
        return result; 
    }


    /* removeTrainCarts removes extra train carts */
    public void removeTrainCarts(int numberOfBandits) {
        if(numberOfBandits == 3){
            trainfourTop.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            trainfourBtm.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }else if(numberOfBandits == 2){
            trainThreeBtm.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            trainThreeTop.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            trainfourTop.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            trainfourBtm.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }
    }

    /* removeBandits non-playing bandits */
    public void removeBandits(ArrayList playingBanditsObject) {
        bool belIsPlaying = false; 
        bool cheIsPlaying = false; 
        bool docIsPlaying = false; 
        bool djaIsPlaying = false; 
        bool ghoIsPlaying = false;
        bool tucIsPlaying = false;  

        foreach(object aBandit in playingBanditsObject){
            Bandit currABandit = (Bandit) aBandit;
            if(currABandit.characterAsString == "BELLE"){
                belIsPlaying = true; 
            }else if(currABandit.characterAsString == "CHEYENNE"){
                cheIsPlaying = true;
            }else if(currABandit.characterAsString == "DOC"){
                docIsPlaying = true; 
            }else if(currABandit.characterAsString == "DJANGO"){
                djaIsPlaying = true; 
            }else if(currABandit.characterAsString == "GHOST"){
                ghoIsPlaying = true; 
            }else if(currABandit.characterAsString == "TUCO"){
                tucIsPlaying = true;
            }
        }

        if(belIsPlaying == false){
            belle.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            belleProf.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }
        if(cheIsPlaying == false){
            cheyenne.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            cheyenneProf.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }
        if(djaIsPlaying == false){
            django.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            djangoProf.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }
        if(ghoIsPlaying == false){
            ghost.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            ghostProf.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }
        if(tucIsPlaying == false){
            tuco.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            tucoProf.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }
        if(docIsPlaying == false){
            doc.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
            docProf.transform.position = new Vector3(faraway[0], faraway[1], faraway[2]);
        }
    }


    public void UpdateGameState(BaseEvent evt) {
        Debug.Log("updategamestate called");

        setAllClickable();
        proceed.interactable = false;
        canDrawCards = false;

        clearHand();

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
        displayGameInfo();
        
        int numberOfBandits = gm.bandits.Count;

        Debug.Log("THERE ARE " + numberOfBandits + " IN THE GAME!");
        removeTrainCarts(numberOfBandits);
        // removeBandits(gm.bandits);
        
        //addAllBandits();

        ArrayList banditsArray = gm.bandits;
        foreach (Bandit b in banditsArray) {
            if (b.characterAsString == "CHEYENNE") {
                buttonToObject[cheyenne] = b;
                Debug.Log(b.characterAsString + "'s bullets " + b.getSizeOfBullets());
                playingBandits.Add(cheyenne);
                cheyenneRevolver.text = b.getSizeOfBullets().ToString();
            }
            if (b.characterAsString == "BELLE") {
                buttonToObject[belle] = b;
                Debug.Log(b.characterAsString + "'s bullets " + b.getSizeOfBullets());
                playingBandits.Add(belle);
                belleRevolver.text = b.getSizeOfBullets().ToString();
            }
            if (b.characterAsString == "TUCO") {
                buttonToObject[tuco] = b;
                Debug.Log(b.characterAsString + "'s bullets " + b.getSizeOfBullets());
                playingBandits.Add(tuco);
                tucoRevolver.text = b.getSizeOfBullets().ToString();
            }
            if (b.characterAsString == "DOC") {
                buttonToObject[doc] = b;
                Debug.Log(b.characterAsString + "'s bullets " + b.getSizeOfBullets());
                playingBandits.Add(doc);
                docRevolver.text = b.getSizeOfBullets().ToString();
            }
            if (b.characterAsString == "GHOST") {
                buttonToObject[ghost] = b;
                Debug.Log(b.characterAsString + "'s bullets " + b.getSizeOfBullets());
                playingBandits.Add(ghost);
                ghostRevolver.text = b.getSizeOfBullets().ToString();
            }
            if (b.characterAsString == "DJANGO") {
                buttonToObject[django] = b;
                Debug.Log(b.characterAsString + "'s bullets " + b.getSizeOfBullets());
                playingBandits.Add(django);
                djangoRevolver.text = b.getSizeOfBullets().ToString();
            }
            
            /* place the bandits in their starting positions */
            mapBandit(gm);
            removeBandits(gm.bandits);

            if(b.characterAsString == gm.currentBandit.characterAsString){
                /*
                * OBJECTS ARE NEWLY CREATED WHEN SERIALIZED. IF MULTIPLE REFERENCES EXIST FOR THE SAME OBJECT, THEY WILL BE TREATED AS DIFFERENT OBJECTS
                */
                Debug.Log("reassigning current bandit ref to " + b.characterAsString);
                gm.currentBandit = b;
            }

            if(b.characterAsString == ChooseCharacter.character){

                if (gm.strGameStatus.Equals("SCHEMIN")) {
                    if(gm.currentRound.getTurnCounter() == 0 && b.hand.Count == 0){
                        b.drawCards(6);
                        if(b.getCharacter().Equals("DOC")){
                            b.drawCards(1);
                        }
                    }
                }

                int index = 0; 
                ActionCard ac;
                BulletCard bc;
                //Debug.Log("num of currcards: " + b.hand.Count);
                //Debug.Log("num of currcards b1: " + gm.currentBandit.hand.Count);
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

        mapTrain(gm);

        Money m;
        Whiskey w;
        int gemCount = 0;
        int purseCount = 0;
        int boxCount = 0;
        int whiskeyCount = 0;
    
        foreach(Bandit b in gm.bandits){
            foreach(Loot l in b.loot){
                try{
                    m = (Money) l;
                    if (m.moneyTypeAsString == "JEWEL"){
                        buttonToObject[allGem[gemCount]] = m;
                        Debug.Log("casting as Gem " + gemCount);
                        gemCount++;
                        moveLootToBanditPos(allGem[gemCount], b.characterAsString); 
                    } else if (m.moneyTypeAsString == "PURSE"){
                        buttonToObject[allPurse[purseCount]] = m;
                        //Debug.Log("casting as Purse " + purseCount);
                        purseCount++;
                        moveLootToBanditPos(allPurse[purseCount], b.characterAsString); 
                    } else if (m.moneyTypeAsString == "STRONGBOX"){
                        buttonToObject[allBox[boxCount]] = m;
                        Debug.Log("casting as Box " + boxCount);
                        boxCount++;
                        moveLootToBanditPos(allBox[boxCount], b.characterAsString); 
                    }
                }
                catch(Exception e){
                    w = (Whiskey) l;
                    buttonToObject[allWhiskey[whiskeyCount]] = w;
                    //Debug.Log("casting as Whiskey" + whiskeyCount);
                    whiskeyCount++;
                    moveLootToBanditPos(allWhiskey[whiskeyCount], b.characterAsString); 
                }
            }
        }

        foreach(TrainUnit tr in gm.trainRoof){
            foreach(Loot l in tr.lootHere){
            try{
                m = (Money) l;
                if (m.moneyTypeAsString == "JEWEL"){
                buttonToObject[allGem[gemCount]] = m;
                placeLootOnTrain(allGem[gemCount], tr.carTypeAsString, tr.carFloorAsString);
                //Debug.Log("casting as Gem " + gemCount);
                gemCount++;
                } else if (m.moneyTypeAsString == "PURSE"){
                buttonToObject[allPurse[purseCount]] = m;
                placeLootOnTrain(allPurse[purseCount], tr.carTypeAsString, tr.carFloorAsString);
                //Debug.Log("casting as Purse " + purseCount);
                purseCount++;
                } else if (m.moneyTypeAsString == "STRONGBOX"){
                buttonToObject[allBox[boxCount]] = m;
                placeLootOnTrain(allBox[boxCount], tr.carTypeAsString, tr.carFloorAsString);
                //Debug.Log("casting as Box" + boxCount);
                boxCount++;
                }
            }
            catch(Exception e){
                w = (Whiskey) l;
                buttonToObject[allWhiskey[whiskeyCount]] = w;
                placeLootOnTrain(allWhiskey[whiskeyCount], tr.carTypeAsString, tr.carFloorAsString);
                Debug.Log("casting as Whiskey " + whiskeyCount);
                whiskeyCount++;
                }
            }
        }

        foreach(TrainUnit tc in gm.trainCabin){
            foreach(Loot l in tc.lootHere){
                try{
                    m = (Money) l;
                    if (m.moneyTypeAsString == "JEWEL"){
                    buttonToObject[allGem[gemCount]] = m;
                    placeLootOnTrain(allGem[gemCount], tc.carTypeAsString, tc.carFloorAsString);
                   // Debug.Log("casting as Gem " + gemCount);
                    gemCount++;
                    } else if (m.moneyTypeAsString == "PURSE"){
                    buttonToObject[allPurse[purseCount]] = m;
                    placeLootOnTrain(allPurse[purseCount], tc.carTypeAsString, tc.carFloorAsString);
                    //Debug.Log("casting as Purse " + purseCount);
                    purseCount++;
                    } else if (m.moneyTypeAsString == "STRONGBOX"){
                    buttonToObject[allBox[boxCount]] = m;
                    placeLootOnTrain(allBox[boxCount], tc.carTypeAsString, tc.carFloorAsString);
                    //Debug.Log("casting as Box" + boxCount);
                    boxCount++;
                    }
                }
                catch(Exception e){
                    w = (Whiskey) l;
                    buttonToObject[allWhiskey[whiskeyCount]] = w;
                    placeLootOnTrain(allWhiskey[whiskeyCount], tc.carTypeAsString, tc.carFloorAsString);
                    //Debug.Log("casting as Whiskey " + whiskeyCount);
                    whiskeyCount++;
                }
            }
        }

        gm.playTurn();

    }



    void displayGameInfo() {
        gameStatus.text = gm.getGameStatus();
        if(gm.currentBandit.getToResolve() != null) {
            resolveCard.text = gm.currentBandit.getToResolve().getActionTypeAsString();
        }
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

        if(gm.getGameStatus() == "STEALIN") {
            turnNum.text = "";
        } else {
            int ti = 1;
            foreach(Turn t in gm.currentRound.turns) {
                currentRoundText.text += "Turn " + ti + ": " + t.turnTypeAsString + "\n";
                if(t.Equals(gm.currentRound.currentTurn)) {
                    turnNum.text = "Current turn #: " + ti;
                }
                ti++;
            }
        }
    }

    void reassignReferences() {
        if(gm.playedPileInstance == null) gm.playedPileInstance = PlayedPile.getInstance();
        Marshal.instance = gm.marshalInstance;
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
                if (gm.banditPositions.Contains(b.getCharacter())) {
                    TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                    if(tr.carTypeAsString == tu.carTypeAsString & tr.carFloorAsString == tu.carFloorAsString) {
                        gm.banditPositions[b.characterAsString] = tr;
                    }
                }
            }
        }
        if(gm.trainCabin ==  null) Debug.Log("gm.trainCabin is null");
        foreach(TrainUnit tc in gm.trainCabin){
            foreach (Bandit b in gm.bandits){
                if (gm.banditPositions.Contains(b.getCharacter())) {
                    TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                    if(tc.carTypeAsString == tu.carTypeAsString & tc.carFloorAsString == tu.carFloorAsString) {
                        gm.banditPositions[b.characterAsString] = tc;
                    }
                }
            }
        }
    }

    public static void setDrawCardsButton(bool canDraw) {
        canDrawCards = canDraw;
    }

    public void drawCardsClicked() {
        if(canDrawCards) {
            canDrawCards = false;
            if(gm.currentBandit.getDeck().Count >= 3) gm.drawCards(3);
            else gm.drawCards(gm.currentBandit.getDeck().Count);
            newAction = false;
            actionText.text = "";
            Debug.Log("drawing cards");
        }
    }

    public void buttonClicked(Button btn){
        if(!myTurn) {
            Debug.Log("not my turn!");
        } else {
            Debug.Log( btn.name + " IS CLICKED");

            if(clickable.Contains(buttonToObject[btn])) {
                Debug.Log("this is a clickable item!");
                //all calls back to GM should be here
                if(actionText.text == "Play a card or draw cards") {
                    try {
                        ActionCard currActionCard = (ActionCard)buttonToObject[btn];
                        gm.playCard(currActionCard); 
                    } catch(Exception e) {
                        Debug.Log("not casting to card");
                    }
                } else if(actionText.text == "Choose a position to move the Marshal") {
                    Debug.Log("choose a marshal pos");
                    try {
                        TrainUnit clickedTU = (TrainUnit)buttonToObject[btn]; 
                        gm.moveMarshal(clickedTU);
                        Debug.Log("moving marshal");
                    } catch(Exception e) {
                        Debug.Log("not moving the marshal");
                    }
                } else if(actionText.text == "Choose a position to move") {
                    Debug.Log("choose a moving pos");
                    try {
                        TrainUnit clickedTU = (TrainUnit)buttonToObject[btn]; 
                        gm.move(clickedTU);
                        Debug.Log("moving");
                    } catch(Exception e) {
                        Debug.Log("not moving");
                    }
                } else if(actionText.text == "Choose a loot to rob") {
                    Debug.Log("choose a loot");
                    try {
                        Loot clickedLoot = (Loot)buttonToObject[btn]; 
                        gm.rob(clickedLoot);
                        Debug.Log("loot chosen for rob");
                    } catch(Exception e) {
                        Debug.Log("wrong btn for rob?");
                    }
                } else if(actionText.text == "Choose a bandit to shoot") {
                    Debug.Log("choose a bandit to shoot");
                    try {
                        Bandit clickedB = (Bandit)buttonToObject[btn]; 
                        gm.shoot(clickedB);
                        Debug.Log("shooting");
                    } catch(Exception e) {
                        Debug.Log("not shooting");
                    }
                }

                newAction = false;
                actionText.text = "";

            } else Debug.Log("not clickable!");
        }
        btn.interactable = true;
    }

    public void horseBtnClicked(Button btn) {
        String response;
        if (btn.name == "horseBtnOne") {
            response = "y";
            GameObject.Find("horseBtnOne").SetActive(false);
            GameObject.Find("horseBtnTwo").SetActive(false);
        }
        else {
            response = "n";
        }
        horseBtnOne.interactable = false;
        horseBtnTwo.interactable = false;

        promptHorseAttackMsg.text="Waiting for other players..";
        ISFSObject obj = SFSObject.NewInstance();
        obj.PutUtfString("ans", response);
        ExtensionRequest req = new ExtensionRequest("gm.choosePosition", obj);
        SFS.Send(req);
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
        buttonToObject.Add(trainfourTop, "null"); 
        buttonToObject.Add(trainfourBtm, "null");
        buttonToObject.Add(locoBtm, "null"); 
        buttonToObject.Add(locoTop, "null");

        buttonToObject.Add(gem1, "null");
        buttonToObject.Add(gem2, "null");
        buttonToObject.Add(gem3, "null");
        buttonToObject.Add(gem4, "null");
        buttonToObject.Add(gem5, "null");
        buttonToObject.Add(gem6, "null");

        buttonToObject.Add(purse1, "null");
        buttonToObject.Add(purse2, "null");
        buttonToObject.Add(purse3, "null");
        buttonToObject.Add(purse4, "null");
        buttonToObject.Add(purse5, "null");
        buttonToObject.Add(purse6, "null");
        buttonToObject.Add(purse7, "null");
        buttonToObject.Add(purse8, "null");
        buttonToObject.Add(purse9, "null");
        buttonToObject.Add(purse10, "null");
        buttonToObject.Add(purse11, "null");
        buttonToObject.Add(purse12, "null");
        buttonToObject.Add(purse13, "null");
        buttonToObject.Add(purse14, "null");
        buttonToObject.Add(purse15, "null");
        buttonToObject.Add(purse16, "null");
        buttonToObject.Add(purse17, "null");
        buttonToObject.Add(purse18, "null");

        buttonToObject.Add(box1, "null");
        buttonToObject.Add(box2, "null");

        buttonToObject.Add(whiskey1, "null");
        buttonToObject.Add(whiskey2, "null");
        buttonToObject.Add(whiskey3, "null");
        buttonToObject.Add(whiskey4, "null");
        buttonToObject.Add(whiskey5, "null");
        buttonToObject.Add(whiskey6, "null");


        trainCabins.Insert(0, locoBtm);
        trainCabins.Insert(1, trainOneBtm);
        trainCabins.Insert(2, trainTwoBtm);
        trainCabins.Insert(3, trainThreeBtm);
        trainCabins.Insert(4, trainfourTop);

        trainRoofs.Insert(0, locoTop);
        trainRoofs.Insert(1, trainOneTop);
        trainRoofs.Insert(2, trainTwoTop);
        trainRoofs.Insert(3, trainThreeTop);
        trainRoofs.Insert(4, trainfourBtm);


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

        allGem.Insert(0, gem1);
        allGem.Insert(1, gem2);
        allGem.Insert(2, gem3);
        allGem.Insert(3, gem4);
        allGem.Insert(4, gem5);
        allGem.Insert(5, gem6);

        allPurse.Insert(0, purse1);
        allPurse.Insert(1, purse2);
        allPurse.Insert(2, purse3);
        allPurse.Insert(3, purse4);
        allPurse.Insert(4, purse5);
        allPurse.Insert(5, purse6);
        allPurse.Insert(6, purse7);
        allPurse.Insert(7, purse8);
        allPurse.Insert(8, purse9);
        allPurse.Insert(9, purse10);
        allPurse.Insert(10, purse11);
        allPurse.Insert(11, purse12);
        allPurse.Insert(12, purse13);
        allPurse.Insert(13, purse14);
        allPurse.Insert(14, purse15);
        allPurse.Insert(15, purse16);
        allPurse.Insert(16, purse17);
        allPurse.Insert(17, purse18);

        allBox.Insert(0, box1);
        allBox.Insert(1, box2);

        allWhiskey.Insert(0, whiskey1);
        allWhiskey.Insert(1, whiskey2);
        allWhiskey.Insert(2, whiskey3);
        allWhiskey.Insert(3, whiskey4);
        allWhiskey.Insert(4, whiskey5);
        allWhiskey.Insert(5, whiskey6);

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

    public void mapTrain(GameManager gm){
        int index = 0;
        foreach(object oneRoof in gm.trainRoof){
            buttonToObject[trainRoofs[index]] = (TrainUnit)oneRoof;
            index++;
        }
        index = 0;
       // Debug.Log("ALL TRAIN ROOFS ARE MAPPED");
        foreach(object oneCab in gm.trainCabin){
            buttonToObject[trainCabins[index]] = (TrainUnit)oneCab;
            index++;
        }
       // Debug.Log("ALL TRAIN CABINS ARE MAPPED");
    }

    public void mapBandit(GameManager gm){
        foreach(TrainUnit tr in gm.trainRoof){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tr.containsBandit(b)){
                    placeBanditAt(b, tu.carTypeAsString, tu.carFloorAsString);
                }
            }
        }
        foreach(TrainUnit tc in gm.trainCabin){
            foreach (Bandit b in gm.bandits){
                TrainUnit tu = (TrainUnit)gm.banditPositions[b.characterAsString];
                if(tc.containsBandit(b)){
                    placeBanditAt(b, tu.carTypeAsString, tu.carFloorAsString);
                }
            }
            if(tc.getIsMarshalHere()) {
                gm.marshalInstance.marshalPosition = tc;
                placeMarshalAt(tc.carTypeAsString);
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
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (locBtm[0] + newRandOffset, locBtm[1], locBtm[2]);
            }else if(cartype == "CAR1"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (fourBtm[0] + newRandOffset, fourBtm[1], fourBtm[2]);
            }else if(cartype == "CAR2"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (threeBtm[0] + newRandOffset, threeBtm[1], threeBtm[2]);
            }else if(cartype == "CAR3"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (twoBtm[0] + newRandOffset, twoBtm[1], twoBtm[2]);
            }else if(cartype == "CAR4"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (oneBtm[0] + newRandOffset, oneBtm[1], oneBtm[2]);
            }else if(cartype == "CAR5"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else if(cartype == "CAR6"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else{
                // cartype == "STAGECOACH"
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }
        }else{
            if(cartype == "LOCOMOTIVE"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (locTop[0] + newRandOffset, locTop[1], locTop[2]);
            }else if(cartype == "CAR1"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (fourTop[0] + newRandOffset, fourTop[1], fourTop[2]);
            }else if(cartype == "CAR2"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (threeTop[0] + newRandOffset, threeTop[1], threeTop[2]);
            }else if(cartype == "CAR3"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (twoTop[0] + newRandOffset, twoTop[1], twoTop[2]);
            }else if(cartype == "CAR4"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (oneTop[0] + newRandOffset, oneTop[1], oneTop[2]);
            }else if(cartype == "CAR5"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else if(cartype == "CAR6"){
                float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else{
                // cartype == "STAGECOACH"
                 float newRandOffset = getRandOffset(); 
                banditBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }
        }

    }

    public void placeMarshalAt(string cartype){
        if(cartype == "LOCOMOTIVE"){
            float newRandOffset = getRandOffset(); 
            marshal.transform.position = new Vector3 (locBtm[0] + newRandOffset, locBtm[1], locBtm[2]);
        }else if(cartype == "CAR1"){
            float newRandOffset = getRandOffset(); 
            marshal.transform.position = new Vector3 (fourBtm[0] + newRandOffset, fourBtm[1], fourBtm[2]);
        }else if(cartype == "CAR2"){
            float newRandOffset = getRandOffset(); 
            marshal.transform.position = new Vector3 (threeBtm[0] + newRandOffset, threeBtm[1], threeBtm[2]);
        }else if(cartype == "CAR3"){
            float newRandOffset = getRandOffset(); 
            marshal.transform.position = new Vector3 (twoBtm[0] + newRandOffset, twoBtm[1], twoBtm[2]);
        }else if(cartype == "CAR4"){
             float newRandOffset = getRandOffset(); 
            marshal.transform.position = new Vector3 (oneBtm[0] + newRandOffset, oneBtm[1], oneBtm[2]);
        }else if(cartype == "CAR5"){
             float newRandOffset = getRandOffset(); 
            marshal.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
        }else if(cartype == "CAR6"){
             float newRandOffset = getRandOffset(); 
            marshal.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
        }
    }


    public void placeLootOnTrain(Button lootBtn, string cartype, string carfloor){
            if(carfloor == "CABIN"){
            if(cartype == "LOCOMOTIVE"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (locBtm[0] + newRandOffset, locBtm[1], locBtm[2]);
            }else if(cartype == "CAR1"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (fourBtm[0] + newRandOffset, fourBtm[1], fourBtm[2]);
            }else if(cartype == "CAR2"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (threeBtm[0] + newRandOffset, threeBtm[1], threeBtm[2]);
            }else if(cartype == "CAR3"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (twoBtm[0] + newRandOffset, twoBtm[1], twoBtm[2]);
            }else if(cartype == "CAR4"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (oneBtm[0] + newRandOffset, oneBtm[1], oneBtm[2]);
            }else if(cartype == "CAR5"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else if(cartype == "CAR6"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else{
                // cartype == "STAGECOACH"
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }
        }else{
            if(cartype == "LOCOMOTIVE"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (locTop[0] + newRandOffset, locTop[1], locTop[2]);
            }else if(cartype == "CAR1"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (fourTop[0] + newRandOffset, fourTop[1], fourTop[2]);
            }else if(cartype == "CAR2"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (threeTop[0]+ newRandOffset, threeTop[1], threeTop[2]);
            }else if(cartype == "CAR3"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (twoTop[0] + newRandOffset, twoTop[1], twoTop[2]);
            }else if(cartype == "CAR4"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (oneTop[0] + newRandOffset, oneTop[1], oneTop[2]);
            }else if(cartype == "CAR5"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else if(cartype == "CAR6"){
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }else{
                // cartype == "STAGECOACH"
                float newRandOffset = getRandOffset(); 
                lootBtn.transform.position = new Vector3 (706.0F + newRandOffset, 816.5F, -364.9F);
            }
        }
    }

    public void moveLootToBanditPos(Button chosenLootBtn, string banditName) {
        //Bandit currBandit = gm.currentBandit; 
        if(banditName == "BELLE"){
            chosenLootBtn.transform.position = new Vector3(belLoot[0], belLoot[1], belLoot[2]); 
        }else if(banditName == "CHEYENNE"){
            chosenLootBtn.transform.position = new Vector3(cheLoot[0], cheLoot[1], cheLoot[2]); 
        }else if(banditName == "DOC"){
            chosenLootBtn.transform.position = new Vector3(docLoot[0], docLoot[1], docLoot[2]); 
        }else if(banditName == "DJANGO"){
            chosenLootBtn.transform.position = new Vector3(djaLoot[0], djaLoot[1], djaLoot[2]); 
        }else if(banditName == "GHOST"){
            chosenLootBtn.transform.position = new Vector3(ghoLoot[0], ghoLoot[1], ghoLoot[2]); 
        }else if(banditName == "TUCO"){
            chosenLootBtn.transform.position = new Vector3(tucLoot[0], tucLoot[1], tucLoot[2]); 
        }
    }


    // public void placeLootOnBandit(Button lootBtn, string lootType, string bandit){
    //         if(bandit == "BELLE"){
    //             // move to belle's loot position
    //             Debug.Log("MOVE LOOT OF TYPE" + lootType + " TO BELLE'S NAME'S POSITION"); 
    //         }else if(bandit == "CHEYENNE"){
    //             Debug.Log("MOVE LOOT OF TYPE" + lootType + " TO CHEYENNE'S NAME'S POSITION"); 
    //         }else if(bandit == "DOC"){
    //             Debug.Log("MOVE LOOT OF TYPE" + lootType + " TO DOC'S NAME'S POSITION"); 
    //         }else if(bandit == "DJANGO"){
    //             Debug.Log("MOVE LOOT OF TYPE" + lootType + " TO DJANGO'S NAME'S POSITION"); 
    //         }else if(bandit == "GHOST"){
    //             Debug.Log("MOVE LOOT OF TYPE" + lootType + " TO GHOST'S NAME'S POSITION"); 
    //         }else if(bandit == "TUCO"){
    //             Debug.Log("MOVE LOOT OF TYPE" + lootType + " TO TUCO'S NAME'S POSITION"); 
    //         }
    // }


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
        ChooseCharacter.RemoveLaunchedSession();
        returningFromChat = false;
        playingBandits.Clear();
        allGem.Clear();
        allPurse.Clear();
        allBox.Clear();
        allWhiskey.Clear();

        SFS.LeaveRoom(); // triggers exit to WaitingRoom scene
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


    private void resetGB() {
        gameHash = WaitingRoom.gameHash;
        returningFromChat = false;
        newAction = false;
        newAction = false;
        myTurn = false;
        saveGameId = "";
        saveGameOnLobby = false;
        // these cause issues
        // playingBandits = new List<Button>();
        // allGem = new List<Button>();
        // allPurse = new List<Button>();
        // allBox = new List<Button>();
        // allWhiskey = new List<Button>();
        canDrawCards = false;
        calledMapTrain = false;
    }

    /*
    *
    *
    * SFS COMMUNICATION METHODS
    *
    *
    */




    public void EnterGameBoardScene() {
        gm = GameManager.getInstance();
        //gm.currentBandit = new Bandit();
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

    public static void TestSave() {
        if(saveGameId == ""){
            Debug.Log("creating new savegame");
            saveGameOnLobby = true;
            saveGameId = generateRandomString(7);
        }

        SaveGameState(saveGameId);
    }

    public static void SaveGameState(string savegameID) {
        Debug.Log("SaveGameState is called!"); 

        if (saveGameOnLobby){

            var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            var JObj = JObject.Parse(response.Content);
            Dictionary<string, object> sessionDetails = JObj.ToObject<Dictionary<string, object>>();

            var temp = JsonConvert.SerializeObject(sessionDetails["gameParameters"]);
            var gameParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(temp);

            string gameName = gameParameters["name"];
            Debug.Log("gamename: " + gameName);
            //below I deserialize a JSON object to a collection
            List<string> players = JsonConvert.DeserializeObject<List<string>>(sessionDetails["players"].ToString());
        
            Dictionary<string, object> body = new Dictionary<string, object>
            {
                { "gamename", gameName },
                { "players", players },
                { "savegameid", savegameID }
            };

            string json = JsonConvert.SerializeObject(body, Formatting.Indented);


            JObject jObjectbody = new JObject();
            jObjectbody.Add("gamename", gameName);
            jObjectbody.Add("players", JsonConvert.SerializeObject(players));
            jObjectbody.Add("savegameid", savegameID);

            var request1 = new RestRequest("api/gameservices/" + gameName + "/savegames/" + savegameID + "?access_token=" + GetAdminToken(), Method.PUT)
                .AddParameter("application/json", json, ParameterType.RequestBody)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");

            IRestResponse response2 = client.Execute(request1);
            Debug.Log("Here is the game saving return: "+ response2.ErrorMessage + "   " + response2.StatusCode);

            saveGameOnLobby = false;

        }
       
        // Save game on the server
        ISFSObject obj = SFSObject.NewInstance();
        Debug.Log("saving the current game state on the server");
        obj.PutUtfString("savegameId", savegameID);
        obj.PutClass("gm", gm);
        ExtensionRequest req = new ExtensionRequest("gm.saveGameState",obj);
        SFS.Send(req);
    }

    public void promptHorseAttack() {
        Debug.Log("prompt horse attack called");
        horseBtnOne.interactable = true;
        horseBtnTwo.interactable = true;

        promptHorseAttackMsg.text = "pha called";
        if (gm.bandits.Count == gm.banditPositions.Count) {
            Debug.Log("ending horse attack");

            foreach(DictionaryEntry s in gm.banditPositions) {
                String b = (String)s.Key;
                TrainUnit u = (TrainUnit)s.Value;
                Debug.Log("bandit: " + b + " is in " + u.carTypeAsString);
            }
            promptHorseAttackMsg.text = "";
            started = true;
            Destroy(GameObject.Find("horseBtnOne"));
            Destroy(GameObject.Find("horseBtnTwo"));
            return;
        }

        foreach(DictionaryEntry s in gm.banditPositions) {
            String b = (String)s.Key;
            if (b.Equals(ChooseCharacter.character)) {
                promptHorseAttackMsg.text = "";
                return;
            }
        }
        //prompt user whether they want to get off at this train (indicated by trainIndex). If yes, response should be "y", if no then "n"
        //promptHorseAttackMsg.text = "Would you like to get on the train at cabin number "+gm.trainIndex+"?";
    }

    public static String generateRandomString(int length)
    {
        StringBuilder str_build = new StringBuilder();  
        Random random = new Random();  

        char letter;  

        for (int i = 0; i < length; i++)
        {
            double flt = random.NextDouble();
            int shift = Convert.ToInt32(Math.Floor(25 * flt));
            letter = Convert.ToChar(shift + 65);
            str_build.Append(letter);  
        }
        
        return str_build.ToString();
        
    }
}

