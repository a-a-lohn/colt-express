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

// DONT HAVE TO CHANGE ANYTHING ANYMORE FOR TESTING
//// THINGS TO CHANGE FOR RUNNING TESTGAME.CS:


namespace model {
    public class GameManager : SerializableSFSType {
        public static GameManager singleton;
        public string strGameStatus;
        
        public Round currentRound;
        public Bandit currentBandit;
        public ArrayList rounds;
        public Marshal marshalInstance;
        //  CONVENTION FOR DECK: POSITION DECK(SIZE) IS TOP OF DECK, DECK(0) IS BOTTOM OF DECK
        public PlayedPile playedPileInstance;
        //  CONVENTION FOR TRAIN: POSITION TRAIN(0) IS LOCOMOTIVE, TRAIN(TRAINLENGTH - 1) IS CABOOSE
        public ArrayList trainRoof;
        public ArrayList trainCabin;
        public ArrayList stagecoach;
        public int trainLength;
        public ArrayList horses;
        public ArrayList bandits;
        public Hashtable banditmap;
        public Hashtable banditPositions; 
        public ArrayList neutralBulletCard;
        public int banditsPlayedThisTurn;
        public int roundIndex;
        public int banditIndex; // NEVER INITIALIZED IN GM.JAVA
        
        
        public void playTurn() {
            if (currentBandit == null) {
                return;
            }
            Debug.Log("playing turn");
            Debug.Log("currentbandit: "+ currentBandit.getCharacter());
            if(currentBandit.getCharacter().Equals(ChooseCharacter.character) | TestGame.testing) {
            
                Debug.Log("my turn");
                GameBoard.setMyTurn(true);
                if (this.strGameStatus.Equals("SCHEMIN")) {
                    if(TestGame.testing) {
                        if(this.currentRound.getTurnCounter() == 0){
                            if(currentBandit.getCharacter().Equals("DOC")){
                                currentBandit.drawCards(7);
                            }
                            else{
                                currentBandit.drawCards(6);
                            }
                        }
                    }
                    Debug.Log("calling prompt");
                    promptDrawCardsOrPlayCard();
                }
                else if (this.strGameStatus.Equals("STEALIN")) {
                    this.resolveAction(this.currentBandit.getToResolve());
                }
            
           }
            
        }
        
        public void promptDrawCardsOrPlayCard() {
            Debug.Log("setting 'it works' from prompt");
            
            GameBoard.clickable = currentBandit.getHand();
            if(currentBandit.getHand().Count > 0) GameBoard.setDrawCardsButton(true);
            else GameBoard.setDrawCardsButton(false);
            GameBoard.setNextAction("Play a card or draw 3 cards");
            
            // ASSIGN THIS ATTRIBUTE ACCORDINGLY IN EVERY PROMPT;
            TestGame.prompt = "Play a card or draw 3 cards";
        }

        public void resolveAction(ActionCard toResolve) {
           // PlayedPile.getInstance().removePlayedCard(toResolve);
            playedPileInstance.removePlayedCard(toResolve);
            currentBandit.addToDeck(toResolve);

            if (currentBandit.toResolve.getActionTypeAsString().Equals("CHANGEFLOOR")) {
                TestGame.prompt = "Changing floor";
                GameBoard.setNextAction("Changing floors");
                currentBandit.setToResolve(null);
                this.changeFloor();
            }
            else if (currentBandit.toResolve.getActionTypeAsString().Equals("MARSHAL")) {
                TestGame.prompt = "Choose a position for the marshal";
                GameBoard.setNextAction("Choose a position for the marshal");
                currentBandit.setToResolve(null);
                moveMarshalPrompt(calculateMoveMarshal());
            }
            else if (currentBandit.toResolve.getActionTypeAsString().Equals("MOVE")) {
                TestGame.prompt = "Choose a position to move";
                GameBoard.setNextAction("Choose a position to move");
                currentBandit.setToResolve(null);
                movePrompt(calculateMove());
            }
            else if (currentBandit.toResolve.getActionTypeAsString().Equals("PUNCH")) {
                TestGame.prompt = "Choose a bandit to punch";
                GameBoard.setNextAction("Choose a bandit to punch");
                currentBandit.setToResolve(null);
                punchPrompt(calculatePunch());
            }
            else if (currentBandit.toResolve.getActionTypeAsString().Equals("ROB")) {
                TestGame.prompt = "Choose a loot to rob";
                GameBoard.setNextAction("Choose a loot to rob");
                currentBandit.setToResolve(null);
                robPrompt(calculateRob());
            }
            else if (currentBandit.toResolve.getActionTypeAsString().Equals("SHOOT")) {
                TestGame.prompt = "Choose a bandit to shoot";
                GameBoard.setNextAction("Choose a bandit to shoot");
                currentBandit.setToResolve(null);
                shootPrompt(calculateShoot());
            }
            else if(currentBandit.toResolve.getActionTypeAsString().Equals("RIDE")){
                 TestGame.prompt = "Choose a position to ride";
                 GameBoard.setNextAction("Choose a position to ride");
                currentBandit.setToResolve(null);
                ridePrompt(calculateRide());
            }
        }
        
        public void playCard(ActionCard c) {
            Debug.Log("playing card");
            //  Remove card from bandit's hand
            this.currentBandit = c.getBelongsTo();
            this.currentBandit.removeFromHand(c);
            Debug.Log("removed from hand");
            if (this.currentBandit.getCharacter().Equals("GHOST") && this.currentRound.getTurnCounter() == 0) {
                //promptPlayFaceUpOrFaceDown(c); -- COMMENTED OUT FOR NOW
                c.setFaceDown(true);
                Debug.Log("called for ghost");
            }
            else if (this.currentRound.getCurrentTurn().getTurnTypeAsString().Equals("TUNNEL")) {
                //  this.currentRound.getCurrentTurn().getTurnTypeAsString().equals("TUNNEL")
                c.setFaceDown(true);
                 Debug.Log("called for tunnel");
            }
            
            //  Assign card to played pile
            //PlayedPile pile = PlayedPile.getInstance();
            //pile.addPlayedCards(c);
            playedPileInstance.addPlayedCards(c);
            Debug.Log("played card, ending turn");
            string message;
            if(c.getFaceDown()) {
                message = currentBandit.characterAsString + " played a card facedown";
            } else {
                message = currentBandit.characterAsString + " played " + c.actionTypeAsString;
            }
            this.endOfTurn(message);
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        //  The GM method drawCards will draw cards and end turn
        //  The Bandit method drawCards simply moves cards from deck to hand
        public void drawCards(int cardsToDraw) {
            Debug.Log("Reached gm drawCards");
            currentBandit.drawCards(cardsToDraw);
            this.endOfTurn(currentBandit.characterAsString + " drew cards");
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
            endOfTurn("use the endOfTurn method that takes a string arg instead indicating the what happened on that turn");
         }

         public void endOfTurn(string message) {
            endOfTurn(message, false);
         }

        public void endOfTurn(string message, bool noAction) {
            //  SCHEMIN PHASE
            if (this.strGameStatus.Equals("SCHEMIN")) {
                string currentTurnType = this.currentRound.getCurrentTurn().getTurnTypeAsString();
                if(currentRound == null) {
                    Debug.Log("currentRound is null");
                    
                } else {
                    if(currentRound.turns != null) {
                        if(currentRound.turns.Count >0) {
                            Turn t = (Turn)this.currentRound.turns[0];
                            Debug.Log("turn: " + t.getTurnTypeAsString());
                        } else {
                            Debug.Log("turns has no turns");
                        }
                    } else {
                        Debug.Log("turns is null");
                    }
                }
                if(currentRound.getCurrentTurn() == null) {
                    Debug.Log("currentRound.getCurrentTurn()");
                }
                if(currentRound.getCurrentTurn().getTurnTypeAsString() == null) {
                    Debug.Log("currentRound.getCurrentTurn().getTurnTypeAsString() is null");
                }
                Debug.Log("Schemin phase");

                //  STANDARD AND TUNNEL TURN CASE
                if (currentTurnType.Equals("STANDARD") || currentTurnType.Equals("TUNNEL")) {
                    Debug.Log("stand/tunnel");
                    this.banditsPlayedThisTurn++;

                    //  ALL BANDITS HAVE PLAYED
                    if ((this.banditsPlayedThisTurn == this.bandits.Count)) {
                        Debug.Log("all have played");
                        if (this.currentRound.hasNextTurn() == true) {
                            //  THERE ARE MORE TURNS IN THE ROUND - NEXT TURN
                            Debug.Log("THERE ARE MORE TURNS IN THE ROUND - NEXT TURN");
                            this.currentRound.setNextTurn();
                            this.banditIndex = ((this.banditIndex + 1) % this.bandits.Count);
                            this.currentBandit = (Bandit) this.bandits[this.banditIndex];
                            Debug.Log("next bandit assigned: " + currentBandit.characterAsString);
                            banditsPlayedThisTurn = 0;
                        }
                        else { // Added by Aaron
                            //  NO MORE TURNS IN ROUND - END OF SCHEMIN PHASE
                            Debug.Log("NO MORE TURNS IN ROUND - END OF SCHEMIN PHASE");
                            foreach (Bandit b in this.bandits) {
                                b.clearHand();
                            }
                            banditIndex = (banditIndex + 1) % this.bandits.Count;
                            this.banditsPlayedThisTurn = 0;
                            this.setGameStatus("STEALIN");
                        }
                    }
                    
                    //  NOT ALL BANDITS HAVE PLAYED - NEXT BANDIT'S TURN
                    else{
                        Debug.Log("NOT ALL BANDITS HAVE PLAYED - NEXT BANDIT'S TURN");
                        this.banditIndex = (this.banditIndex + 1) % this.bandits.Count;
                        this.currentBandit = (Bandit) this.bandits[this.banditIndex];
                    }
                }

                //  SWITCHING TURN CASE
                else if (currentTurnType.Equals("SWITCHING")) {
                    Debug.Log("switching turn");
				    banditsPlayedThisTurn++;

				    // ALL BANDITS HAVE PLAYED
				    if (banditsPlayedThisTurn == this.bandits.Count) {
                        Debug.Log("all bandits have played");
				    	// THERE ARE MORE TURNS IN THE ROUND - NEXT TURN
				    	if (this.currentRound.hasNextTurn() == true) {
                            Debug.Log("THERE ARE MORE TURNS IN THE ROUND - NEXT TURN");
                            this.currentRound.setNextTurn();
                            banditIndex = (banditIndex + 1) % this.bandits.Count;
				    		this.currentBandit = (Bandit) this.bandits[this.banditIndex];
                            banditsPlayedThisTurn = 0;
				    	}
				    	// NO MORE TURNS IN ROUND - END OF SCHEMIN PHASE
				    	else {
                            Debug.Log("NO MORE TURNS IN ROUND - END OF SCHEMIN PHASE");
                            foreach (Bandit b in this.bandits){
                                b.clearHand();
                            }
                            banditIndex = (banditIndex + 1) % this.bandits.Count;
				    		banditsPlayedThisTurn = 0;
				    		this.setGameStatus("STEALIN");
                            Debug.Log("GAME STATUS: " + this.strGameStatus);
				    	}
				    }
				    // NOT ALL BANDITS HAVE PLAYED - NEXT BANDIT'S TURN
				    else {
                        banditIndex = (banditIndex - 1 + this.bandits.Count) % this.bandits.Count;
				    	this.currentBandit = (Bandit) this.bandits[this.banditIndex];
				    }
                }

                else if (currentTurnType.Equals("SPEEDINGUP")) {

                    //  CURRENT BANDIT COMPLETED 1/2 TURN
                    if (currentBandit.consecutiveTurnCounter == 0) {
					    currentBandit.setConsecutiveTurnCounter(1);
					    promptDrawCardsOrPlayCard();
				    }

                    //  CURRENT BANDIT COMPLETED 2/2 TURN 
                    else if (currentBandit.consecutiveTurnCounter == 1) {
					    currentBandit.setConsecutiveTurnCounter(0);
					    banditsPlayedThisTurn++;

					    // ALL BANDITS HAVE PLAYED
					    if (banditsPlayedThisTurn == this.bandits.Count) {

						    // THERE ARE MORE TURNS IN THE ROUND - NEXT TURN
						    if (this.currentRound.hasNextTurn() == true) {
						    	this.currentRound.setNextTurn();
                                banditIndex = (banditIndex + 1) % this.bandits.Count;
						    	this.currentBandit = (Bandit) this.bandits[this.banditIndex];
                                banditsPlayedThisTurn = 0;
						    }

						    // NO MORE TURNS IN ROUND - END OF SCHEMIN PHASE
						    else {
                                foreach (Bandit b in this.bandits) {
                                    b.clearHand();
                                }
                                banditIndex = (banditIndex + 1) % this.bandits.Count;
						    	banditsPlayedThisTurn = 0;
						    	this.setGameStatus("STEALIN");
                                Debug.Log("GAME STATUS: " + this.strGameStatus);
						    }
					    }

					    // NOT ALL BANDITS HAVE PLAYED - NEXT BANDIT'S TURN
					    else {
                            banditIndex = (banditIndex + 1) % this.bandits.Count;
					    	this.currentBandit = (Bandit) this.bandits[this.banditIndex];
					    }
				    }
                }
            }

            if (this.strGameStatus.Equals("STEALIN")) {
                if(playedPileInstance.getPlayedCards().Count > 0) {
                    ActionCard toResolve = this.playedPileInstance.takeTopCard();
                    Debug.Log("toresolve string for NEXT card: " + toResolve.actionTypeAsString);
                    Bandit b = toResolve.getBelongsTo();
                    Debug.Log("toresolve belongsto for NEXT card: " + b.characterAsString);
                    this.currentBandit = toResolve.getBelongsTo();
                    this.currentBandit.setToResolve(toResolve);
                } else {
                    //  played pile is empty
                    this.roundIndex++;
                    if (this.roundIndex == this.rounds.Count) {
                        this.setGameStatus("COMPLETED");
                    }
                    else {
                        this.currentRound = (Round) this.rounds[this.roundIndex];
                        this.setGameStatus("SCHEMIN");
                        this.banditsPlayedThisTurn = 0;
                    }
                }
            }
            //playedPileInstance = PlayedPile.getInstance();
            marshalInstance = Marshal.getInstance();
            //currentBandit.updateOtherDecks();
            //currentBandit.updateOtherHands();
            
            Debug.Log("ending turn");
            if(!TestGame.testing) {
                if(noAction) {
                    GameBoard.setNoAction(message);
                } else {
                    GameBoard.setMyTurn(false);
                    GameBoard.SendNewGameState(message);
                }
            }
        }
        
        public GameManager() {
            
        }
        
        public static GameManager getInstance() {
            GameManager gm = null;
            if (singleton == null) {
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
        /*public void chosenCharacter(User player, string c, int numPlayers) {
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
        }*/
        
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
        
        public BulletCard popNeutralBullet(){
            BulletCard popped = (BulletCard) this.neutralBulletCard[this.neutralBulletCard.Count-1];
            this.neutralBulletCard.RemoveAt(this.neutralBulletCard.Count-1);
            return popped;
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
                //TRAVERSE TRAIN UNITS TOWARDS RIGHT AND LEFT TO FIND BANDITS IN LINE OF SIGHT
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
            if(possibilities.Count == 0){
                endOfTurn();
            }
            else if(possibilities.Count == 1){
                shoot((Bandit)possibilities[0]);
            }
            else if(possibilities.Count > 1){
                GameBoard.makeShootPossibilitiesClickable(possibilities);
            }
        }
        
	    public void shoot(Bandit toShoot) {

		    if (currentBandit.getSizeOfBullets() > 0) {
		    	toShoot.addToDeck(currentBandit.popBullet()); // TODO <- graphical response
		    }

		    if (currentBandit.getCharacter().Equals("DJANGO")) {
                //DETERMINE IF BANDIT TO SHOOT IS LEFT OR RIGHT OF DJANGO
                bool leftOfDjango = false;
                bool rightOfDjango = false;
			    TrainUnit toLeft = currentBandit.getPosition().getLeft();
                while(toLeft != null){
                    if(toLeft.containsBandit(toShoot)){
                        leftOfDjango = true;
                        break;
                    }
                    else{
                        toLeft = toLeft.getLeft();
                    }
                }
                TrainUnit toRight = currentBandit.getPosition().getRight();
                while(toRight != null){
                    if(toRight.containsBandit(toShoot)){
                        rightOfDjango = true;
                        break;
                    }
                    else{
                        toRight = toRight.getRight();
                    }
                }
                Debug.Assert(leftOfDjango || rightOfDjango);
                if(leftOfDjango && toShoot.getPosition().getLeft() != null){
                    toShoot.setPosition(toShoot.getPosition().getLeft());
                }
                else if(rightOfDjango && toShoot.getPosition().getRight() != null){
                    toShoot.setPosition(toShoot.getPosition().getRight());
                }
                if(toShoot.getPosition().getIsMarshalHere() && toShoot.getPosition().getCarFloorAsString().Equals("CABIN")){
                    toShoot.shotByMarhsal();
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
                Bandit punched = (Bandit) possibilities[0];
                dropPrompt(punched, calculateDrop(punched));
            }
            else{
                //TODO make possibilities clickable (replace new Bandit with the Bandit the client chooses)
                string punchedBanditName = GameBoard.makePunchPossibilitiesClickable(possibilities); 
                Bandit punched = new Bandit(punchedBanditName.ToUpper());
                dropPrompt(punched, calculateDrop(punched));
            }
        }
        
        public ArrayList calculateDrop(Bandit punched){
            return punched.getLoot();
        }

        public void dropPrompt(Bandit punched, ArrayList possibilities){
            if(possibilities.Count == 0){
                knockbackPrompt(punched, null, calculateKnockback(punched));
            }
            else if(possibilities.Count == 1){
                knockbackPrompt(punched, (Loot) possibilities[0], calculateKnockback(punched));
            }
            else{
                //TODO make possibilities clickable (replace new Money with the Loot the client chooses)
                Loot dropped = new Money();
                knockbackPrompt(punched, dropped, calculateKnockback(punched));
            }
        }

        public ArrayList calculateKnockback(Bandit punched){
            ArrayList possibilities = new ArrayList();
            if(punched.getPosition().getLeft() != null){
                possibilities.Add(punched.getPosition().getLeft());
            }
            if(punched.getPosition().getRight() != null){
                possibilities.Add(punched.getPosition().getRight());
            }
            return possibilities;
        }

        public void knockbackPrompt(Bandit punched, Loot dropped, ArrayList possibilities){
            if(possibilities.Count == 0){
                punch(punched, dropped, null);
            }
            else if(possibilities.Count == 1){
                punch(punched, dropped, (TrainUnit)possibilities[0]);
            }
            else{
                //TDOO make possibilities clickable (replace new TrainUnit with the TrainUnit the client chooses)
                TrainUnit knockedTo = new TrainUnit();
                punch(punched, dropped, knockedTo);
            }
        }

        public void punch(Bandit punched, Loot dropped, TrainUnit knockedTo) {
			if (dropped != null) {
			    punched.removeLoot(dropped);
                if(this.currentBandit.getCharacter().Equals("CHEYENNE") && dropped is Money && ((Money)dropped).getMoneyTypeAsString().Equals("PURSE")){
                    this.currentBandit.addLoot(dropped);
                }
                else{
                    punched.getPosition().addLoot(dropped);
                }
			}
			knockedTo.addBandit(punched);
			if (knockedTo.getIsMarshalHere()) {
				punched.shotByMarhsal();
			}
		    endOfTurn();
	    }
        
        //--CHANGE FLOOR--

        public void changeFloor() {
            TrainUnit currentPosition = this.currentBandit.getPosition();
            Debug.Assert(currentPosition.getBelow() == null || currentPosition.getAbove() == null);
            if (currentPosition.getBelow() != null) {
                currentBandit.setPosition(currentPosition.getBelow());
                if(currentBandit.getPosition().getIsMarshalHere()){
                    currentBandit.shotByMarhsal();
                }
            }
            else if (currentPosition.getAbove() != null) {
                currentBandit.setPosition(currentPosition.getAbove());
            }
            TrainUnit newpos = (TrainUnit)banditPositions[currentBandit.characterAsString];
            this.endOfTurn(currentBandit.getCharacter() + " changed floors to the " + newpos.getCarFloorAsString() + " of " + newpos.getCarTypeAsString(), true);
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
            
            if (currentPosition.carFloorAsString.Equals("ROOF")) {
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
        }
        
        public void movePrompt(ArrayList possibilities) {
            if(possibilities.Count == 0){
                endOfTurn();
            }
            else if(possibilities.Count == 1){
                move((TrainUnit)possibilities[0]);
            }
            else if(possibilities.Count > 1){
                //make clickable
            }
        }
        
	    public void move(TrainUnit targetPosition) {
		    targetPosition.addBandit(this.currentBandit);
		    if (targetPosition.isMarshalHere) {
			    currentBandit.shotByMarhsal();
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
            TrainUnit oldp = marshal.getMarshalPosition();
            oldp.setIsMarshalHere(false);
		    marshal.setMarshalPosition(targetPosition);
            targetPosition.setIsMarshalHere(true);
		    foreach (Bandit b in this.getBandits()) {
		    	if (b.getPosition() == targetPosition) {
			    	b.shotByMarhsal();
			    }
		    }
		    endOfTurn(); 
	    }

        //--RIDE--

        public ArrayList calculateRide(){
            TrainUnit currentPosition = currentBandit.getPosition();
            Horse adjacentHorse = null;
            foreach(Horse h in this.horses){
                if(currentPosition.getCarTypeAsString().Equals(h.getAdjacentTo().getCarTypeAsString())){
                    adjacentHorse = h;
                    break;
                }
            }
            if(adjacentHorse == null){
                return new ArrayList();
            }
            else{
                if(currentPosition.getCarFloorAsString().Equals("ROOF")){
                    currentPosition = currentPosition.getBelow();
                }
                ArrayList possibilities = new ArrayList();
                TrainUnit toLeft = currentPosition.getLeft();
                for(int i=0; i<3 && toLeft != null; i++){
                    possibilities.Add(toLeft);
                    toLeft = toLeft.getLeft();
                }
                TrainUnit toRight = currentPosition.getRight();
                for(int i=0; i<3 && toRight != null; i++){
                    possibilities.Add(toRight);
                    toRight = toRight.getRight();
                }
                possibilities.Add(currentPosition);
                return possibilities;
            }
        }

        public void ridePrompt(ArrayList possibilities){
            if(possibilities.Count == 0){
                this.endOfTurn();
            }
            else if(possibilities.Count == 1){
                ride((TrainUnit)possibilities[0]);
            }
            else{
                //TODO make possibilities clickable
                TrainUnit clicked = new TrainUnit();
                ride(clicked);
            }
        }

        public void ride(TrainUnit dest){
            TrainUnit currentPosition = currentBandit.getPosition();
            if(currentPosition.getCarFloorAsString().Equals("ROOF")){
                currentPosition = currentPosition.getBelow();
            }
            Horse adjacentHorse = null;
            foreach(Horse h in horses){
                if(h.getAdjacentTo().getCarTypeAsString().Equals(currentPosition.getCarTypeAsString())){
                    adjacentHorse = h;
                    break;
                }
            }

            currentBandit.setPosition(dest);
            if(dest.getIsMarshalHere()){
                currentBandit.shotByMarhsal();
            }
            adjacentHorse.setAdjacentTo(dest);

            this.endOfTurn();
        }
	
	public ArrayList calculateGunslinger()
        {
            ArrayList bulletCardsLeft = new ArrayList();
            foreach (Bandit b in bandits)
            {
                bulletCardsLeft.Add(b.getSizeOfBullets());
            }
            ArrayList gunslinger = new ArrayList();
            int min = 99;
            foreach (int i in bulletCardsLeft)
            {
                if (i < min)
                {
                    min = i;
                }
            }
            for (int i = 0; i < bulletCardsLeft.Count; i++)
            {
                if ((int)bulletCardsLeft[i] == min)
                {
                    gunslinger.Add(i);
                }
            }
            return gunslinger;
        }

        public ArrayList calculateWinner()
        {
            ArrayList winner = new ArrayList();
            foreach (Bandit b in bandits)
            {
                ArrayList loots = b.getLoot();
                int value = 0;
                foreach (Money money in loots)
                {
                    value = value + money.getValue();
                }
                winner.Add(value);
            }

            ArrayList gunslinger = this.calculateGunslinger();
            for (int i = 0; i < winner.Count; i++)
            {
                foreach (int index in gunslinger)
                {
                    if (i == index)
                    {
                        winner[i] = (int)winner[i] + 1000;
                    }
                }
            }

            // now calculate the hostage ransom
            for (int i = 0; i < bandits.Count; i++)
            {
                Bandit aBandit = (Bandit)bandits[i];
                if (aBandit.getHostageAsString() != null) {
                    string hostage = aBandit.getHostageAsString();
                    if (hostage.Equals("POODLE")){
                        winner[i] = (int)winner[i] + 1000;
                    } else if (hostage.Equals("BANKER")) {  // Get 1000 if the bandit has at least one Strongbox.
                        foreach (Money money in aBandit.getLoot()) {
                            if (money.getMoneyTypeAsString() == "STRONGBOX") {
                                winner[i] = (int)winner[i] + 1000;
                            }
                            break;
                        }
                    } else if (hostage.Equals("MINISTER")) {
                        winner[i] = (int)winner[i] + 900;
                    } else if (hostage.Equals("TEACHER")) {
                        winner[i] = (int)winner[i] + 800;
                    } else if (hostage.Equals("ZEALOT")) {
                        winner[i] = (int)winner[i] + 700;
                    } else if (hostage.Equals("OLDLADY")) {
                        foreach (Money money in aBandit.getLoot())
                        {
                            if (money.getMoneyTypeAsString() == "RUBY")
                            {
                                winner[i] = (int)winner[i] + 500;
                            }
                        }
                    } else if (hostage.Equals("POKERPLAYER")) {
                        foreach (Money money in aBandit.getLoot())
                        {
                            if (money.getMoneyTypeAsString() == "PURSE")
                            {
                                winner[i] = (int)winner[i] + 250;
                            }
                        }
                    } else if (hostage.Equals("PHOTOGRAPHER")) {
                        foreach (BulletCard c in aBandit.getDeck()) {
                            winner[i] = (int)winner[i] + 200;
                        }
                    } else { 
                        
                    }
                }
            }

            return winner;
        }

        public void angryMarshal()
        {
            Marshal marshal = Marshal.getInstance();
            TrainUnit marshalPosition = marshal.getMarshalPosition();
            TrainUnit roofOfMP = marshalPosition.getAbove();
            foreach (Bandit b in this.bandits)
            {
                if (b.getPosition() == roofOfMP)
                {
                    if (this.neutralBulletCard.Count > 0)
                    {
                        b.addToDeck(this.popNeutralBullet());
                    }
                }
            }

            if (marshalPosition != (TrainUnit)this.trainCabin[trainLength-1])
            {
                TrainUnit rightOfMP = marshalPosition.getRight();
                marshal.setMarshalPosition(rightOfMP);
                foreach (Bandit b in this.bandits)
                {
                    if (b.getPosition() == rightOfMP)
                    {
                        b.shotByMarhsal();
                    }
                }
            }
        }

        public void swivelArm()
        {
            foreach (Bandit b in this.bandits)
            {
                TrainUnit banditPosition = b.getPosition();
                int index = this.trainRoof.IndexOf(banditPosition);
                bool isRoof = false;
                if (index > -1)
                {
                    isRoof = true;
                }
                if (isRoof)
                {
                    b.setPosition((TrainUnit)this.trainRoof[trainLength-1]);
                }
            }
        }

        public void braking()
        {
            foreach (Bandit b in this.bandits)
            {
                TrainUnit banditPosition = b.getPosition();
                int index = this.trainRoof.IndexOf(banditPosition);
                bool isRoof = false;
                if (index > -1)
                {
                    isRoof = true;
                }
                if (isRoof)
                {
                    if (banditPosition != (TrainUnit)this.trainRoof[0])
                    {
                        TrainUnit leftOfBandit = banditPosition.getLeft();
                        b.setPosition(leftOfBandit);
                    }
                }
            }
        }

        public void takeItAll()
        {
            Marshal marshal = Marshal.getInstance();
            TrainUnit marshalPosition = marshal.getMarshalPosition();
            Money strongBox = new Money("STRONGBOX", 1000);
            marshalPosition.addLoot(strongBox);
        }

        public void passengersRebellion()
        {
            foreach (Bandit b in this.bandits)
            {
                TrainUnit banditPosition = b.getPosition();
                int index = this.trainCabin.IndexOf(banditPosition);
                bool isCabin = false;
                if (index > -1)
                {
                    isCabin = true;
                }
                if (isCabin)
                {
                    if (this.neutralBulletCard.Count > 0)
                    {
                        b.addToDeck(this.popNeutralBullet());
                    }
                }
            }
        }

        public void marshalsRevenge()
        {
            Marshal marshal = Marshal.getInstance();
            TrainUnit marshalPosition = marshal.getMarshalPosition();
            TrainUnit topOfMP = marshalPosition.getAbove();
            foreach (Bandit b in this.bandits)
            {
                TrainUnit banditPosition = b.getPosition();
                if (banditPosition == topOfMP)
                {
                    ArrayList loots = b.getLoot();
                    int indexx = -1;
                    int value = 2000;
                    foreach (Money money in loots)
                    {
                        if (money.getMoneyTypeAsString() == "PURSE")
                        {
                            if (money.getValue() < value)
                            {
                                value = money.getValue();
                                indexx = loots.IndexOf(money);
                            }
                        }
                    }
                    if (indexx > -1)
                    {
                        b.removeLoot((Money)loots[indexx]);
                    }
                }
            }
        }

        public void pickpocketing()
        {
            foreach (Bandit b in this.bandits)
            {
                TrainUnit banditPosition = b.getPosition();
                if (banditPosition.numOfBanditsHere() == 1)
                { // here add the first purse
                    if (banditPosition.getLootHere().Count != 0)
                    {
                        foreach (Money money in banditPosition.getLootHere())
                        {
                            if (money.getMoneyTypeAsString() == "PURSE")
                            {
                                b.addLoot(money);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void hostageConductor()
        {
            foreach (Bandit b in this.bandits)
            {
                TrainUnit banditPosition = b.getPosition();
                int index = -1;
                index = this.trainCabin.IndexOf(banditPosition);
                if (index == -1)
                {
                    index = this.trainRoof.IndexOf(banditPosition);
                }
                if (index == 0)
                {
                    b.addLoot(new Money("PURSE", 250));
                }
            }
        }

        public void pantingHorses()
        {
            if (this.sizeOfBandits() <= 4)
            {
                for (int index = trainLength-1; index > 0; index--)
                {
                    TrainUnit trainunit = (TrainUnit)this.trainCabin[index];
                    ArrayList horsesHere = trainunit.getHorsesHere();
                    if ((Horse)horsesHere[0] != null)
                    {
                        horsesHere.RemoveAt(0);
                        break;
                    }
                }
            }
            else
            {
                int num = 0;
                for (int index = trainLength-1; index > 0; index--)
                {
                    if (num == 2)
                    {
                        break;
                    }
                    TrainUnit trainunit = (TrainUnit)this.trainCabin[index];
                    ArrayList horsesHere = trainunit.getHorsesHere();
                    if ((Horse)horsesHere[0] != null)
                    {
                        horsesHere.RemoveAt(0);
                        num++;
                    }
                }
            }
        }

        public void aShotOfWhiskeyForTheMarshall()
        {
            Marshal marshal = Marshal.getInstance();
            TrainUnit marshalPosition = marshal.getMarshalPosition();
            ArrayList loots = marshalPosition.getLootHere();
            foreach (Whiskey whiskey in loots)
            {
                string whiskeyType = whiskey.getWhiskeyTypeAsString();
                if (whiskeyType == "NORMAL")
                { // classic
                    TrainUnit RightOfMP = marshalPosition.getRight();
                    marshal.setMarshalPosition(RightOfMP);
                    foreach (Bandit b in RightOfMP.getBanditsHere())
                    {
                        b.shotByMarhsal();
                    }
                    break;
                }
                else if (whiskeyType == "OLD")
                {
                    int index = 0;
                    index = trainCabin.IndexOf(marshalPosition);
                    for (int i = index; i < trainLength; i++)
                    {
                        TrainUnit RightOfMP = marshalPosition.getRight();
                        marshal.setMarshalPosition(RightOfMP);
                        foreach (Bandit b in RightOfMP.getBanditsHere())
                        {
                            b.shotByMarhsal();
                        }
                    }
                }
            }

        }

        public void higherSpeed()
        {
            Marshal marshal = Marshal.getInstance();
            TrainUnit marshalPosition = marshal.getMarshalPosition();
            foreach (Bandit b in this.bandits)
            {
                TrainUnit banditPosition = b.getPosition();
                int index = this.trainRoof.IndexOf(banditPosition);
                bool isRoof = false;
                if (index > -1)
                {
                    isRoof = true;
                }
                if (isRoof)
                {
                    TrainUnit RightOfBandit = banditPosition.getRight();
                    b.setPosition(RightOfBandit);
                    if (RightOfBandit == marshalPosition)
                    {
                        b.shotByMarhsal();
                    }
                }
            }

            foreach (Horse horse in this.horses)
            {
                TrainUnit tu = horse.getAdjacentTo();
                TrainUnit rightOfTU = tu.getRight();
                horse.setAdjacentTo(rightOfTU);
            }

            foreach (TrainUnit tu in this.stagecoach)
            {
                TrainUnit RightOfTU = tu.getRight();
                this.stagecoach.Remove(tu);
                this.stagecoach.Add(RightOfTU);
            }

            // TODO: shotGun move to right 

        }

        public void theShotgunsRage()
        {
            TrainUnit stagecoachCabin = (TrainUnit)this.stagecoach[0];
            TrainUnit stagecoachRoof = (TrainUnit)this.stagecoach[1];
            // TODO: get beside of the stagecoach and shoot the bandits on this 4 trainUnit
            //TrainUnit carCabin = stagecoachCabin
        }

        public void sharingTheLoot()
        {
            foreach (Bandit b in this.bandits)
            {
                ArrayList loots = b.getLoot();
                foreach (Money money in loots)
                {
                    if ((string)money.getMoneyTypeAsString() == "STRONGBOX")
                    {
                        TrainUnit BanditPosition = b.getPosition();
                        if (BanditPosition.numOfBanditsHere() >= 2)
                        {
                            int count = BanditPosition.numOfBanditsHere();
                            b.removeLoot(money);
                            foreach (Bandit bb in BanditPosition.getBanditsHere())
                            {
                                Money m = new Money();
                                int value = 1000 / count;
                                m.setValue(value);
                                bb.addLoot(m);
                            }
                        }
                    }
                    break;
                }
            }
        }

        public void escape()
        {
            ArrayList newBanditList = new ArrayList();
            foreach (Bandit b in this.bandits)
            {
                foreach (ActionCard c in b.getHand())
                {
                    if (c.getActionTypeAsString() == "RIDE")
                    {
                        newBanditList.Add(b);
                    }
                }
                if (!newBanditList.Contains(b))
                {
                    TrainUnit tu = b.getPosition();
                    if (this.stagecoach.Contains(tu))
                    {
                        newBanditList.Add(b);
                    }
                }
            }
            this.bandits = newBanditList;
        }

        public void mortalBullet()
        {
            // TODO : 
            // Each player looses $150 for each Bullet received during 
            // this Round.This includes the Neutral Bullets and those
            // from all the Bandits.In order to facilitate the count of those Bullets, it is 
            // advised to play them apart during this Round.
        }


    }


}
