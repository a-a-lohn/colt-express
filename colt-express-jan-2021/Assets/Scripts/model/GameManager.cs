using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Protocol.Serialization;
// @Instantiation(InstantiationMode.SINGLE_INSTANCE)
// @MultiHandler
namespace model {
    public class GameManager : SerializableSFSType {
        //public Hashtable banditLocation; 
        
        public static GameManager singleton;
        
        public string strGameStatus;
        
        public Round currentRound;
        
        public Bandit currentBandit;
        
        public ArrayList rounds;
        
        //  CONVENTION FOR DECK: POSITION DECK.SIZE() IS TOP OF
        //  DECK, POSITION 0 IS BOTTOM OF DECK
        public Marshal marshalInstance;
        
        public PlayedPile playedPileInstance;
        
        //  CONVENTION FOR DECK: POSITION DECK.SIZE() IS TOP OF DECK, POSITION 0 IS
        //  BOTTOM OF DECK
        public ArrayList trainRoof ;
        
        public ArrayList trainCabin;
        
        public int trainLength;

        public ArrayList horses;

        public ArrayList stagecoach;

        public ArrayList bandits;
        public ArrayList loots; // added 
        
        public Hashtable banditmap;

        public Hashtable banditPositions; 
        
        //public static ColtMultiHandler handler; 
        
        public ArrayList neutralBulletCard;
        
        public int banditsPlayedThisTurn;
        
        public int roundIndex;
        
        public int banditIndex; // NEVER INITIALIZED IN GM.JAVA
        
        // public static void setHandler(ColtMultiHandler handle) {  
        //     handler = handle;
        //     //  ISFSObject rtn = SFSObject.newInstance();
        //     //  handler.updateGameState(rtn);
        // }
        
        public void playTurn() {
            Debug.Log("playing turn");
            Debug.Log("currentbandit: "+ currentBandit.getCharacter());
            if(currentBandit.getCharacter() == ChooseCharacter.character) {
                Debug.Log("my turn");
                if ((this.strGameStatus == "SCHEMIN")) {
                    Debug.Log("calling prompt");
                    promptDrawCardsOrPlayCard();
                }
                else if ((this.strGameStatus == "STEALIN")) {
                    this.resolveAction(this.currentBandit.getToResolve());
                }
            }
            
        }
        
        public void promptDrawCardsOrPlayCard() {
            GameBoard.setWorks();
            //GameBoard.clickable = currentBandit.getHand();
            GameBoard.action = "playcard";
            // TODO
        }

        public void resolveAction(ActionCard toResolve) {
            if (toResolve.getActionTypeAsString().Equals("CHANGEFLOOR")) {
                currentBandit.setToResolve(null);
                this.changeFloor();
            }
            else if (toResolve.getActionTypeAsString().Equals("MARSHAL")) {
                currentBandit.setToResolve(null);
                moveMarshalPrompt(calculateMoveMarshal());
            }
            else if (toResolve.getActionTypeAsString().Equals("MOVE")) {
                currentBandit.setToResolve(null);
                movePrompt(calculateMove());
            }
            else if (toResolve.getActionTypeAsString().Equals("PUNCH")) {
                currentBandit.setToResolve(null);
                punchPrompt(calculatePunch());
            }
            else if (toResolve.getActionTypeAsString().Equals("ROB")) {
                currentBandit.setToResolve(null);
                robPrompt(calculateRob());
            }
            else if (toResolve.getActionTypeAsString().Equals("SHOOT")) {
                currentBandit.setToResolve(null);
                shootPrompt(calculateShoot());
            }
            else if(toResolve.getActionTypeAsString().Equals("RIDE")){
                currentBandit.setToResolve(null);
                ridePrompt(calculateRide());
            }
        }
        
        public void playCard(ActionCard c) {
            Debug.Log("playing card");
            //  Remove card from bandit's hand
            this.currentBandit = c.getBelongsTo();
            this.currentBandit.removeFromHand(c);
            if (((this.currentBandit.getCharacter() == "GHOST") 
                        && (this.currentRound.getTurnCounter() == 0))) {
                promptPlayFaceUpOrFaceDown(c);
            }
            else if ((this.currentRound.getCurrentTurn().getTurnTypeAsString() == "TUNNEL")) {
                //  this.currentRound.getCurrentTurn().getTurnTypeAsString().equals("TUNNEL")
                c.setFaceDown(true);
            }
            
            //  Assign card to played pile
            PlayedPile pile = PlayedPile.getInstance();
            pile.addPlayedCards(c);
            //  TODO: graphical response
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public void drawCards(int cardsToDraw) {
            for (int i = this.currentBandit.sizeOfDeck()-1; i  >this.currentBandit.sizeOfDeck()-cardsToDraw-1; i--) {
                Card toAdd = this.currentBandit.getFromDeckAt(i);
                this.currentBandit.removeFromDeckAt(i);
                this.currentBandit.addToHand(toAdd);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }

        public void promptPlayFaceUpOrFaceDown(ActionCard c){
            //TODO
            /**
            * if(chooses face up){
                PlayedPile pike = PlayedPile.getInstance();
                pileaddPlayedCards(c);
                
            }
            */
        }

        public void endOfTurn() {
            if ((this.strGameStatus == "SCHEMIN")) {
                if (currentRound == null){
                     Debug.Log("curr round is null");
                }
                if (currentRound.getCurrentTurn() == null){
                     Debug.Log("curr turn is null");
                }
                if (currentRound.getCurrentTurn().getTurnTypeAsString() == null){
                     Debug.Log("curr turn as string is null");
                }
                string currentTurnType = this.currentRound.getCurrentTurn().getTurnTypeAsString();
                if (((currentTurnType == "STANDARD") || (currentTurnType == "TUNNEL"))) {
                    this.banditIndex = ((this.banditIndex + 1) % this.bandits.Count);
                    this.banditsPlayedThisTurn++;
                    //  IF END OF TURN
                    if ((this.banditsPlayedThisTurn > this.bandits.Count)) {
                        //  IF THERE ARE MORE TURNS IN THE ROUND
                        if ((this.currentRound.getNextTurn() != null)) {
                            //  IMPORTANT: getNextTurn() also SETS to next turn
                            this.currentBandit = (Bandit) this.bandits[this.banditIndex];
                        }
                        
                        //  IF THERE ARE NO MORE TURNS IN THE ROUND
                        //
                        foreach (Bandit b in this.bandits) {
                            b.endOfSchemin();
                        }
                        this.banditsPlayedThisTurn = 0;
                        this.setGameStatus("STEALIN");
                    }
                    
                    //  IF NOT END OF TURN
                    this.currentBandit = (Bandit) this.bandits[this.banditIndex];
                }
                else if ((currentTurnType == "SWITCHING")) {
                    // TODO
                }
                else if ((currentTurnType == "SPEEDINGUP")) {
                    // TODO
                }
                
            }
            else if ((this.strGameStatus == "STEALIN")) {
                ActionCard toResolve = this.playedPileInstance.takeTopCard();
                if ((toResolve != null)) {
                    this.currentBandit = toResolve.getBelongsTo();
                }
                else {
                    //  played pile is empty
                    this.roundIndex++;
                    if ((this.roundIndex == this.rounds.Count)) {
                        this.setGameStatus("COMPLETED");
                    }
                    else {
                        this.currentRound = (Round) this.rounds[this.roundIndex];
                        this.setGameStatus("SCHEMIN");
                        this.banditsPlayedThisTurn = 0;
                    }
                    
                }
                
            }
            Debug.Log("sending new game state");
            GameBoard.SendNewGameState();
        }
        
        public GameManager() {
            
        }
        
        public static GameManager getInstance() {
            GameManager gm = null;
            if ((singleton == null)) {
                gm = new GameManager();
            }
            else {
                gm = singleton;
            }
            
            return gm;
        }

        public static void replaceInstance(GameManager gm) {
            singleton = gm;
        } 
        
        public Round getCurrentRound() {
            return this.currentRound;
        }
        
        public void setCurrentRound(Round newObject) {
            this.currentRound = newObject;
        }
        
        public Round getRoundAt(int index) {
            return (Round) this.rounds[index];
        }
        
        public void addRound(Round a) {
            int size = this.rounds.Count;
            this.rounds.Add(a);
        }
        
        public void removeRound(Round a) {
            if (this.rounds.Contains(a)) {
                this.rounds.Remove(a);
            }
            
        }
        
        public bool roundsContains(Round a) {
            bool contains = this.rounds.Contains(a);
            return contains;
        }
        
        public int sizeOfRounds() {
            int size = this.rounds.Count;
            return size;
        }
        
        public ArrayList getRounds() {
            return this.rounds;
        }
        
        public ArrayList createRoundCards(int numOfPlayers) {
            ArrayList RoundCards = new ArrayList();
            if (numOfPlayers >=2 && numOfPlayers <= 6) {
                Round r1 = new Round("AngryMarshal");
                RoundCards.Add(r1);
                Round r2 = new Round("SwivelArm");
                RoundCards.Add(r2);
                Round r3 = new Round("Braking");
                RoundCards.Add(r3);
                Round r4 = new Round("TakeItAll");
                RoundCards.Add(r4);
                Round r5 = new Round("PassengersRebellion");
                RoundCards.Add(r5);
                Round r6 = new Round("Bridge");
                RoundCards.Add(r6);
                Round r7 = new Round("Cave");
                RoundCards.Add(r7);
            }
            Bandit.shuffle(RoundCards);
            return null; ////??
        }
        
        public void setGameStatus(string newStatus) {
            //this.gameStatus = newStatus;
            this.strGameStatus = newStatus;
        }
        
        public string getGameStatus() {
            return this.strGameStatus;
        }
        
        public Bandit getCurrentBandit() {
            return this.currentBandit;
        }
        
        public void setCurrentBandit(Bandit newObject) {
            this.currentBandit = newObject;
        }
        
        public void removeBanditsAt(int index) {
            if ((this.bandits.Count >= index)) {
                this.bandits.Remove(index);
            }
            
        }
        
        public Bandit getBanditsAt(int index) {
            if ((this.bandits.Count >= index)) {
                return (Bandit) this.bandits[index];
            }
            
            return null;
        }
        
        public void addBandit(Bandit a) {
            this.bandits.Add(a);
        }
        
        public void removeBandits(Bandit a) {
            if (this.bandits.Contains(a)) {
                this.bandits.Remove(a);
            }
            
        }
        
        public bool containsBandits(Bandit a) {
            bool contains = this.bandits.Contains(a);
            return contains;
        }
        
        public int sizeOfBandits() {
            int size = this.bandits.Count;
            return size;
        }
        
        public ArrayList getBandits() {
            ArrayList b = ((ArrayList)(this.bandits.Clone()));
            return b;
        }
        
        bool allPlayersChosen(int numPlayers) {
            //  TO DO
            if ((numPlayers == this.bandits.Count)) {
                return true;
            }
            
            //  for all players, return if they are all associated with a bandit or not.
            return false;
        }
        
        int getNumOfPlayers() {
            //  TO DO
            //  Here to get number of players
            return 3;
        }
        
        public void setUpPositions(ArrayList b) {
            int numOfBandit = b.Count;
            if ((numOfBandit == 2)) {
                ((Bandit)b[0]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[1]).setPosition((TrainUnit) this.trainCabin[1]);
            }
            else if ((numOfBandit == 3)) {
                ((Bandit)b[0]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[1]).setPosition((TrainUnit) this.trainCabin[1]);
                ((Bandit)b[2]).setPosition((TrainUnit) this.trainCabin[0]);
            }
            else if ((numOfBandit == 4)) {
                ((Bandit)b[0]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[1]).setPosition((TrainUnit) this.trainCabin[1]);
                ((Bandit)b[2]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[3]).setPosition((TrainUnit) this.trainCabin[1]);
            }
            else if ((numOfBandit == 5)) {
                ((Bandit)b[0]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[1]).setPosition((TrainUnit) this.trainCabin[1]);
                ((Bandit)b[2]).setPosition((TrainUnit) this.trainCabin[2]);
                ((Bandit)b[3]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[4]).setPosition((TrainUnit) this.trainCabin[1]);
            }
            else if ((numOfBandit == 6)) {
                ((Bandit)b[0]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[1]).setPosition((TrainUnit) this.trainCabin[1]);
                ((Bandit)b[2]).setPosition((TrainUnit) this.trainCabin[2]);
                ((Bandit)b[3]).setPosition((TrainUnit) this.trainCabin[0]);
                ((Bandit)b[4]).setPosition((TrainUnit) this.trainCabin[1]);
                ((Bandit)b[4]).setPosition((TrainUnit) this.trainCabin[2]);
            }
            else {
                return;
            }
        }
        
        //  void chosenCharacter(int playerId, Character c) {
        public void chosenCharacter(User player, string c, int numPlayers) {
            Bandit newBandit = new Bandit(c);
            this.bandits.Add(newBandit);
            this.banditmap.Add(newBandit, player);
            //  TO DO.
            //  Here to associate playerId with newBandit.
            bool ready = this.allPlayersChosen(numPlayers);
            if (!ready) {
                //Console.WriteLine("Not all players are ready!");
            }
            else {
                //this.initializeGame();
                this.setGameStatus("SCHEMIN");
            }
            
            //  this.setCurrentRound(this.rounds.get(0));
            //  set waiting for input to be true;
        }
        
        public int getBanditsPlayedThisTurn() {
            return this.banditsPlayedThisTurn;
        }
        
        public void setBanditsPlayedThisTurn(int bp) {
            this.banditsPlayedThisTurn = bp;
        }
        
        public int getRoundIndex() {
            return this.roundIndex;
        }
        
        public void setRoundIndex(int ri) {
            this.roundIndex = ri;
        }
        
        public int getBanditIndex() {
            return this.banditIndex;
        }
        
        public void setBanditIndex(int bi) {
            this.banditIndex = bi;
        }
        

        //--EXECUTE ACTIONS-- BEFORE CALLING ANY OF THESE METHODS, CURRENT BANDIT MUST
	    // BE ASSIGNED CORRECTLY All actions will be called from POV of this.currentBandit

        //--ROB--

        public ArrayList calculateRob(){
            TrainUnit currentPosition = currentBandit.getPosition();
            return currentPosition.getLootHere();
        }
        public void robPrompt(ArrayList possibilities){
            if(possibilities.Count > 0 ){
                this.endOfTurn();
            }
            else{
                //TODO make possibilities clickable
                Loot clicked = new Money();
                rob(clicked);
            }
        }
        public void rob(Loot chosen) {
            this.currentBandit.getPosition().removeLoot(chosen);
            this.currentBandit.addLoot(chosen);
            this.endOfTurn();
        }
        
        //--SHOOT--

        public ArrayList calculateShoot(){
            // TODO REMEMBER BELLE AND TUCO CASES, REMEMBER ROOF AND CABIN CASES
            
		    
		    //ROOF CASE:
		    if (this.currentBandit.getPosition().getCarFloorAsString().Equals("ROOF")) {
                ArrayList possibilities = new ArrayList();
                TrainUnit currentCabin = currentBandit.getPosition();
                if(currentCabin.numOfBanditsHere() > 1){
                    foreach(Bandit b in currentCabin.getBanditsHere()){
                        if(!b.getCharacter().Equals(currentBandit.getCharacter())){
                            possibilities.Add(b);
                        }
                    }
                }
                else{
                    TrainUnit toLeft = currentCabin.getLeft();
                    while(toLeft != null){
                        if(toLeft.numOfBanditsHere()>0){
                            break;
                        }
                        else{
                            toLeft = toLeft.getLeft();
                        }
                    }
                    TrainUnit toRight = currentCabin.getRight();
                    while(toRight != null){
                        if(toRight.numOfBanditsHere()>0){
                            break;
                        }
                        else{
                            toRight = toRight.getRight();
                        }
                    }
                    if(toLeft != null){
                        foreach(Bandit b in toLeft.getBanditsHere()){
                            possibilities.Add(b);
                        }
                    }
                    if(toRight != null){
                        foreach(Bandit b in toRight.getBanditsHere()){
                            possibilities.Add(b);
                        }
                    }
                }
                //TUCO ABILITY
                if(currentBandit.getCharacter().Equals("TUCO")){
                    TrainUnit belowCabin = this.currentBandit.getPosition().getBelow();
                    foreach(Bandit bb in belowCabin.getBanditsHere()){
                        possibilities.Add(bb);
                    }
                }
                //BELLE ABILITY
                foreach (Bandit b in possibilities){
                    if(b.getCharacter().Equals("BELLE") && possibilities.Count > 1){
                        possibilities.Remove(b);
                    }
                }
                return possibilities;
		    } 

            //CABIN CASE:
            else {
                ArrayList possibilities = new ArrayList();
                TrainUnit leftCabin = this.currentBandit.getPosition().getLeft();
                TrainUnit rightCabin = this.currentBandit.getPosition().getRight();
			    foreach (Bandit bl in leftCabin.getBanditsHere()) {
				    possibilities.Add(bl);
			    }
			    foreach (Bandit br in rightCabin.getBanditsHere()) {
				    possibilities.Add(br);
			    }
                //TUCO ABILITY
                if(currentBandit.getCharacter().Equals("TUCO")){
                    TrainUnit aboveCabin = this.currentBandit.getPosition().getAbove();
                    foreach(Bandit ba in aboveCabin.getBanditsHere()){
                        possibilities.Add(ba);
                    }
                }
                //BELLE ABILITY
                foreach (Bandit b in possibilities){
                    if(b.getCharacter().Equals("BELLE") && possibilities.Count > 1){
                        possibilities.Remove(b);
                    }
                }
                return possibilities;
		    }

        }

        public void shootPrompt(ArrayList possibilities){
            //TODO make possibilities clickable
            Bandit clicked = new Bandit();
            shoot(clicked);
        }
        
	    public void shoot(Bandit toShoot) {

		    // Bandit toShoot = new Bandit(); // TODO <- replace with the target chosen by
		    // shootPrompt
		    if (currentBandit.getSizeOfBullets() > 0) {
		    	toShoot.addToDeck(currentBandit.popBullet()); // TODO <- graphical response
		    }

		    if (currentBandit.getCharacter().Equals("DJANGO")) {
			    TrainUnit left = currentBandit.getPosition().getLeft();
			    TrainUnit right = currentBandit.getPosition().getRight();

			    // IF BANDIT IS TO DJANGO'S LEFT, PUSH BANDIT 1 CART TO DJANGO'S LEFT IF
			    // POSSIBLE
			    if (left.containsBandit(toShoot)) {
				    if (left.getLeft() != null) {
				    	toShoot.setPosition(left.getLeft()); // TODO <- graphical response
				    }
			    }
			    // IF BANDIT IS TO DJANGO'S RIGHT, PUSH BANDIT 1 CART TO DJANGO'S RIGHT IF
			    // POSSIBLE
			    else if (right.containsBandit(toShoot)) {
			    	if (right.getRight() != null) {
				    	toShoot.setPosition(right.getRight()); // TODO <- graphical response
				    }

			    }
		    }
		    endOfTurn(); // might have to put this in an if else block for cases like SpeedingUp/Whiskey
	    }
        
        //--PUNCH--

        public ArrayList calculatePunch() {
		    ArrayList possibilities = new ArrayList();
		    foreach (Bandit b in this.currentBandit.getPosition().getBanditsHere()) {
		    	if (!b.getCharacter().Equals(this.currentBandit.getCharacter())) {
		    		possibilities.Add(b);
		    	}
		    }
            return possibilities;
	    }

        public void punchPrompt(ArrayList possibilities) {
            if(possibilities.Count == 0){
                this.endOfTurn();
            }
            else if(possibilities.Count == 1){
                //punch((Bandit)possibilities[0]);
            }
            else{
                //TODO make possibilities clickable
                Bandit clicked = new Bandit();
                //punch(clicked);
            }
        }
        
        public void punch(Bandit target, Loot l, TrainUnit tu) {
		    if (target != null) {
			    if (l != null) {
				    target.removeLoot(l);
				    this.currentBandit.addLoot(l);
			    }
			    tu.addBandit(target);
			    if (tu.isMarshalHere) {
			    	target.addToDeck((BulletCard)this.neutralBulletCard[0]);
                    this.neutralBulletCard.Remove(0);
			    	this.currentBandit.setPosition(target.getPosition().getAbove());
			    	target.getPosition().getAbove().addBandit(this.currentBandit);
			    }
		    }
		    endOfTurn(); // might have to put this in an if else block for cases like SpeedingUp/Whiskey
	    }
        
        //--CHANGE FLOOR--

        public void changeFloor() {
            TrainUnit currentPosition = this.currentBandit.getPosition();
            if (((currentPosition.getAbove() == null) && (currentPosition.getBelow() != null))) {
                currentPosition.getBelow().addBandit(currentBandit);
                /*currentPosition.removeBandit(this.currentBandit);
                currentPosition.getBelow().addBandit(this.currentBandit);
                this.currentBandit.setPosition(currentPosition.getBelow());*/
            }
            else if (((currentPosition.getBelow() == null) && (currentPosition.getAbove() != null))) {
                currentPosition.getAbove().addBandit(currentBandit);
                /*currentPosition.removeBandit(this.currentBandit);
                currentPosition.getAbove().addBandit(this.currentBandit);
                this.currentBandit.setPosition(currentPosition.getAbove());*/
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        //--MOVE--

        public ArrayList calculateMove() {
            ArrayList possibleMoving = new ArrayList();
            TrainUnit currentPosition = currentBandit.getPosition();
            if ((currentPosition.getLeft() != null)) {
                possibleMoving.Add(currentPosition.getLeft());
            }
            
            if ((currentPosition.getRight() != null)) {
                possibleMoving.Add(currentPosition.getLeft());
            }
            
            if ((currentPosition.carFloorAsString == "ROOF")) {
                if ((currentPosition.getLeft().getLeft() != null)) {
                    possibleMoving.Add(currentPosition.getLeft());
                }
                
                if ((currentPosition.getRight().getRight() != null)) {
                    possibleMoving.Add(currentPosition.getLeft());
                }
                
                if ((currentPosition.getLeft().getLeft().getLeft() != null)) {
                    possibleMoving.Add(currentPosition.getLeft());
                }
                
                if ((currentPosition.getRight().getRight().getRight() != null)) {
                    possibleMoving.Add(currentPosition.getLeft());
                }
                
            }
            
            return possibleMoving;
            // call promptMoves(possibleMoving)
           // GameBoard.clickable.Add(possibleMoving);
            //GameBoard.action = "move()";
        }
        
        public TrainUnit movePrompt(ArrayList possibilities) {
            // TODO
            return new TrainUnit();
        }
        
	    public void move(TrainUnit targetPosition) {
		    TrainUnit currentPosition = this.currentBandit.getPosition();
		    currentPosition.addBandit(this.currentBandit);
		    if (targetPosition.isMarshalHere) {
			    this.currentBandit.addToDeck((BulletCard)this.neutralBulletCard[0]);
                this.neutralBulletCard.Remove(0);
			    currentPosition.addBandit(this.currentBandit);
			    this.currentBandit.setPosition(currentPosition);
		    }
		    endOfTurn(); // might have to put this in an if else block for cases like SpeedingUp/Whiskey
	    }
        
        //--MOVE MARSHAL--

	    public ArrayList calculateMoveMarshal() {
		    Marshal marshal = Marshal.getInstance();
		    ArrayList possibilities = new ArrayList();
		    if (marshal.getMarshalPosition().getLeft() != null) {
		    	possibilities.Add(marshal.getMarshalPosition().getLeft());
		    }
		    if (marshal.getMarshalPosition().getRight() != null) {
		    	possibilities.Add(marshal.getMarshalPosition().getRight());
		    }
		    return possibilities;
	    }
        
        public void moveMarshalPrompt(ArrayList possibilities) {
        }
        
	    public void moveMarshal(TrainUnit targetPosition) {
		    Marshal marshal = Marshal.getInstance();
		    marshal.setMarshalPosition(targetPosition);
		    foreach (Bandit b in this.getBandits()) {
		    	if (b.getPosition() == targetPosition) {
			    	if (this.neutralBulletCard[0] != null) {
                        b.addToDeck((BulletCard)neutralBulletCard[0]);
				    	this.neutralBulletCard.Remove(0);
					    targetPosition.getAbove().addBandit(b);
				    }
			    }
		    }
		    endOfTurn(); // might have to put this in an if else block for cases like SpeedingUp/Whiskey
	    }

        //--RIDE--

        public ArrayList calculateRide(){
            //TODO
            return new ArrayList();
        }
        public void ridePrompt(ArrayList possibilities){
            //TODO make possibilities clickable
            TrainUnit clicked = new TrainUnit();
            ride(clicked);
        }
        public void ride(TrainUnit dest){
            this.endOfTurn();
        }
	
	// return an ArrayList <int> where it contains index of gunslinger
	public ArrayList calculateGunslinger(){
            ArrayList bulletCardsLeft = new ArrayList();
            foreach(Bandit b in bandits)
            {
                bulletCardsLeft.Add(b.getSizeOfBullets());
            }
            ArrayList gunslinger = new ArrayList();
            int min = 99;
            foreach(int i in bulletCardsLeft)
            {
                if (i < min){
                    min = i;
                }
            }
            for(int i = 0; i < bulletCardsLeft.Count; i++)
            {
                if (bulletCardsLeft[i] == min){
                    gunslinger.Add(i);
                }
            }
            return gunslinger;
        }

        public ArrayList calculateWinner(){
            ArrayList winner = new ArrayList();
            foreach(Bandit b in bandits)
            {
                ArrayList loots = b.getLoot();
                int value = 0;
                foreach(Money money in loots)
                {
                    value = value + money.getValue();
                }
                winner.Add(value);
            }
        
            ArrayList gunslinger = this.calculateGunslinger();
            for(int i = 0; i < winner.Count; i++)
            {
                foreach(int index in gunslinger)
                {
                    if (i == index){
                        winner[i] = winner[i] + 1000;
                    }
                }
            }        

            return winner;
        }
	    
	    
	    
    }


}
