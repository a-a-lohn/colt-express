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
        //public TrainUnit[] trainRoof;
        public ArrayList trainRoof ;
        
        public ArrayList trainCabin;
        
        //public TrainUnit[,] train;
        
        //public ArrayList stagecoach;

        public ArrayList bandits;
        
        public Hashtable banditmap;
        
        //public static ColtMultiHandler handler; 
        
        public ArrayList neutralBulletCard;
        
        public int banditsPlayedThisTurn;
        
        public int roundIndex;
        
        public int banditIndex;
        
        // public static void setHandler(ColtMultiHandler handle) {  
        //     handler = handle;
        //     //  ISFSObject rtn = SFSObject.newInstance();
        //     //  handler.updateGameState(rtn);
        // }
        
        //  SOME OF THESE FIELDS SHOULD BE AUTOMATICALLY INITIALIZED, NOT PASSED AS
        //  PARAMS
        //  this method should only be called from if-else block in chosenCharacter
        // public void initializeGame() {
        //     //  set train-related attributes
        //     //  this.stagecoach = TrainUnit.createStagecoach();
        //     //  this.train = TrainUnit.createTrain(bandits.size());
        //     this.trainRoof = TrainUnit.createTrainRoof(this.getNumOfPlayers());
        //     this.trainCabin = TrainUnit.createTrainCabin(this.getNumOfPlayers());
        //     ArrayList bandits = this.getBandits();
        //     foreach (Bandit b in this.bandits) {
        //         //  initialize each bandit cards, purse
        //         b.createStartingCards();
        //         //  also the hand for bandits
        //         b.createBulletCards();
        //         b.createStartingPurse();
        //     }
            
        //     this.marshalInstance = Marshal.getInstance();
        //     //  initialize round cards, round attributes/create round constructor
        //     this.rounds = this.createRoundCards(this.getNumOfPlayers());
        //     //System.Collections.shuffle(this.bandits); 
        //     //  <- to decide who goes first, shuffle bandit list
        //     this.currentBandit = (Bandit) this.bandits[0];
        //     this.currentRound = (Round) this.rounds[0];
        //     this.setUpPositions(this.bandits);
        //     // 
        //     Marshal marshal = new Marshal();
        //     Money strongbox = new Money("STRONGBOX", 1000);
        //     // marshal.setMarshalPosition(this.trainCabin[this.getNumOfPlayers()]);
        //     // strongbox.setPosition(this.trainCabin[this.getNumOfPlayers()]);
        //     // 
        //     //  create netural bullet card
        //     Card NBullet1 = new BulletCard();
        //     Card NBullet2 = new BulletCard();
        //     Card NBullet3 = new BulletCard();
        //     Card NBullet4 = new BulletCard();
        //     Card NBullet5 = new BulletCard();
        //     Card NBullet6 = new BulletCard();
        //     Card NBullet7 = new BulletCard();
        //     Card NBullet8 = new BulletCard();
        //     Card NBullet9 = new BulletCard();
        //     Card NBullet10 = new BulletCard();
        //     Card NBullet11 = new BulletCard();
        //     Card NBullet12 = new BulletCard();
        //     Card NBullet13 = new BulletCard();
        //     this.neutralBulletCard.Add(NBullet1);
        //     this.neutralBulletCard.Add(NBullet2);
        //     this.neutralBulletCard.Add(NBullet3);
        //     this.neutralBulletCard.Add(NBullet4);
        //     this.neutralBulletCard.Add(NBullet5);
        //     this.neutralBulletCard.Add(NBullet6);
        //     this.neutralBulletCard.Add(NBullet7);
        //     this.neutralBulletCard.Add(NBullet8);
        //     this.neutralBulletCard.Add(NBullet9);
        //     this.neutralBulletCard.Add(NBullet10);
        //     this.neutralBulletCard.Add(NBullet11);
        //     this.neutralBulletCard.Add(NBullet12);
        //     this.neutralBulletCard.Add(NBullet13);
        //     this.roundIndex = 0;
        //     //  this.currentRound = this.rounds.get(roundIndex);
        //     //  currentRound and currentRound.currentTurn must be initialized
        //     this.banditsPlayedThisTurn = 0;
        //     //this.gameStatus = GameStatus.SCHEMIN;
        //     this.strGameStatus = "SCHEMIN";
        //     this.currentBandit = (Bandit) this.bandits[0];
        // }
        
        public void playTurn() {
            Debug.Log("playing turn");
            if(currentBandit.banditNameAsString == ChooseCharacter.character) {
                if ((this.strGameStatus == "SCHEMIN")) {
                    Debug.Log("calling prompt");
                    PlayerLog.promptDrawCardsOrPlayCard();
                }
                else if ((this.strGameStatus == "STEALIN")) {
                    this.resolveAction(this.currentBandit.getToResolve());
                }
            }
            
        }
        
        /*public void promptDrawCardsOrPlayCard() {
            // TODO
        }*/
        
        public void resolveAction(ActionCard toResolve) {
            if ((toResolve.getActionTypeAsString() == "CHANGEFLOOR")) {
                this.changeFloor();
            }
            else if ((toResolve.getActionTypeAsString() == "MARSHAL")) {
                ArrayList possibilities = this.calculateMoveMarshal();
                TrainUnit to = this.moveMarshalPrompt(possibilities);
                this.moveMarshal(to);
            }
            else if ((toResolve.getActionTypeAsString() == "MOVE")) {
                ArrayList possibilities = this.calculateMove();
                TrainUnit to = this.movePrompt(possibilities);
                this.move(to);
            }
            else if ((toResolve.getActionTypeAsString() == "PUNCH")) {
                
            }
            else if ((toResolve.getActionTypeAsString() == "ROB")) {
                
            }
            else if ((toResolve.getActionTypeAsString() == "SHOOT")) {
                
            }
            
        }
        
        public void playCard(ActionCard c) {
            Debug.Log("playing card");
            //  Remove card from bandit's hand
            this.currentBandit = c.getBelongsTo();
            this.currentBandit.removeHand(c);
            //  Prompt playing face down
            if (((this.currentBandit.getCharacter() == "GHOST") 
                        && (this.currentRound.getTurnCounter() == 0))) {
                //  TODO: prompt choice;
                //  TODO: receive choice;
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
            for (int i = (this.currentBandit.sizeOfDeck() - 1); (i 
                        > (this.currentBandit.sizeOfDeck() 
                        - (cardsToDraw - 1))); i--) {
                //Card toAdd = this.currentBandit.removeDeckAt(i);
                Card toAdd = this.currentBandit.getDeckAt(i);
                this.currentBandit.removeDeckAt(i);
                this.currentBandit.addHand(toAdd);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public void endOfTurn() {
            if ((this.strGameStatus == "SCHEMIN")) {
                string currentTurnType = this.currentRound.getCurrentTurn().getTurnTypeAsString();
                if (((currentTurnType == "STANDARD") 
                            || (currentTurnType == "TUNNEL"))) {
                    this.banditIndex = ((this.banditIndex + 1) 
                                % this.bandits.Count);
                    
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
                Card toResolve = this.playedPileInstance.takeTopCard();
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
            if (((numOfPlayers == 2) 
                        || ((numOfPlayers == 3) 
                        || (numOfPlayers == 4)))) {
                Round r1 = new Round("AngryMarshal");
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("TUNNEL"));
                r1.addTurn(new Turn("SWITCHING"));
                RoundCards.Add(r1);
                Round r2 = new Round("SwivelArm");
                r2.addTurn(new Turn("STANDARD"));
                r2.addTurn(new Turn("TUNNEL"));
                r2.addTurn(new Turn("STANDARD"));
                r2.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r2);
                Round r3 = new Round("Braking");
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r3);
                Round r4 = new Round("TakeItAll");
                r4.addTurn(new Turn("STANDARD"));
                r4.addTurn(new Turn("TUNNEL"));
                r4.addTurn(new Turn("SPEEDINGUP"));
                r4.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r4);
                Round r5 = new Round("PassengersRebellion");
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("TUNNEL"));
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r5);
                Round r6 = new Round("SIX");
                r6.addTurn(new Turn("STANDARD"));
                r6.addTurn(new Turn("SPEEDINGUP"));
                r6.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r6);
                Round r7 = new Round("SEVEN");
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                r7.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r7);
            }
            else if (((numOfPlayers == 5) 
                        || (numOfPlayers == 6))) {
                Round r1 = new Round("AngryMarshal");
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("SWITCHING"));
                RoundCards.Add(r1);
                Round r2 = new Round("SwivelArm");
                r2.addTurn(new Turn("STANDARD"));
                r2.addTurn(new Turn("TUNNEL"));
                r2.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r2);
                Round r3 = new Round("Braking");
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("TUNNEL"));
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                RoundCards.Add(r3);
                Round r4 = new Round("TakeItAll");
                r4.addTurn(new Turn("STANDARD"));
                r4.addTurn(new Turn("SPEEDINGUP"));
                r4.addTurn(new Turn("SWITCHING"));
                RoundCards.Add(r4);
                Round r5 = new Round("PassengersRebellion");
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("TUNNEL"));
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("SWITCHING"));
                RoundCards.Add(r5);
                Round r6 = new Round("SIX");
                r6.addTurn(new Turn("STANDARD"));
                r6.addTurn(new Turn("SPEEDINGUP"));
                RoundCards.Add(r6);
                Round r7 = new Round("SEVEN");
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                RoundCards.Add(r7);
            }
            else {
                return null;
            }
            
            return Bandit.shuffle(RoundCards);
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
        
        public Loot RobPrompt(Bandit b, ArrayList l) {
            //  TO DO
            //  ask b to choose loot from l
            //return l.IEnumerator.MoveNext; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            return null;
        }
        
        public void Rob() {
            if (!(this.currentBandit.getPosition().lootHere.Count == 0)) {
                Loot l = this.RobPrompt(this.currentBandit, this.currentBandit.getPosition().lootHere);
                this.currentBandit.addLoot(l);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public Bandit RoofShootPrompt(Bandit b, ArrayList ab) {
            //  TO DO
            //  ask b to choose one target from ab
            return (Bandit) ab[0];
        }
        
        public Bandit CarShootPrompt(Bandit b, ArrayList ab) {
            //  TO DO
            //  ask b to choose one target from ab
            return (Bandit) ab[0];
        }
        
        public void shoot() {
            ArrayList roofShootTarget = new ArrayList();
            ArrayList carShootTarget = new ArrayList();
            if (this.currentBandit.getPosition().carFloorAsString == "ROOF") {
                foreach (Bandit b in this.bandits) {
                    if ((b != this.currentBandit)) {
                        if ((b.getPosition().carFloorAsString == "ROOF")) {
                            roofShootTarget.Add(b);
                        }
                        
                    }
                    
                }
                
                if ((roofShootTarget[0] != null)) {
                    //  ask the current player to choose which bandit to shoot
                    Bandit b = this.RoofShootPrompt(this.currentBandit, roofShootTarget);
                    BulletCard bc = (BulletCard) this.currentBandit.bullets[0];
                    this.currentBandit.bullets.Remove(0);
                    if ((bc != null)) {
                        b.addDiscardPile(bc);
                    }
                    
                }
                
            }
            else {
                foreach (Bandit bl in this.currentBandit.getPosition().getLeft().banditsHere) {
                    carShootTarget.Add(bl);
                }
                
                foreach (Bandit br in this.currentBandit.getPosition().getRight().banditsHere) {
                    carShootTarget.Add(br);
                }
                
                if ((carShootTarget[0] != null)) {
                    //  ask the current player to choose which bandit to shoot
                    Bandit b = this.CarShootPrompt(this.currentBandit, carShootTarget);
                    BulletCard bc =(BulletCard) this.currentBandit.bullets[0];
                    this.currentBandit.bullets.Remove(0);
                    if ((bc != null)) {
                        b.addDiscardPile(bc);
                    }
                    
                }
                
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public Bandit punchBanditPrompt(Bandit b, ArrayList ab) {
            //  TO DO
            //  ask b to choose one target from ab
            return (Bandit) ab[0];
        }
        
        public Loot punchLootPrompt(Bandit b, Bandit b2) {
            //  TO DO
            //  ask b to choose one loot from b2
            return (Loot) b2.loot[0];
        }
        
        public TrainUnit punchPositionPrompt(Bandit b, Bandit b2) {
            //  TO DO
            //  ask b to choose one position that b2 can be punched to
            return b2.getPosition().left;
        }
        
        public void punch() {
            ArrayList otherBandits = new ArrayList();
            foreach (Bandit b in this.currentBandit.getPosition().banditsHere) {
                if ((b != this.currentBandit)) {
                    otherBandits.Add(b);
                }
                
            }
            
            if ((otherBandits[0] != null)) {
                //  ask the player to choose target
                Bandit target = this.punchBanditPrompt(this.currentBandit, otherBandits);
                if (target.getLoot().Count>0) {
                    //  ask the player to choose 1 loot
                    Loot l = this.punchLootPrompt(this.currentBandit, target);
                    target.removeLoot(l);
                    this.currentBandit.addLoot(l);
                }
                
                //  ask where to punch to
                TrainUnit tu = this.punchPositionPrompt(this.currentBandit, target);
                target.setPosition(tu);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public void changeFloor() {
            TrainUnit currentPosition = this.currentBandit.getPosition();
            if (((currentPosition.getAbove() == null) 
                        && (currentPosition.getBelow() != null))) {
                currentPosition.removeBandit(this.currentBandit);
                currentPosition.getBelow().addBandit(this.currentBandit);
                this.currentBandit.setPosition(currentPosition.getBelow());
            }
            else if (((currentPosition.getBelow() == null) 
                        && (currentPosition.getAbove() != null))) {
                currentPosition.removeBandit(this.currentBandit);
                currentPosition.getAbove().addBandit(this.currentBandit);
                this.currentBandit.setPosition(currentPosition.getAbove());
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public static ArrayList moveAlgorithm(Bandit b) {
            // use currentBandit instead of parameter //void method
            ArrayList possibleMoving = new ArrayList();
            TrainUnit currentPosition = b.getPosition();
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
        
        public ArrayList calculateMove() {
            return new ArrayList();
        }
        
        public TrainUnit movePrompt(ArrayList possibilities) {
            // TODO
            return new TrainUnit();
        }
        
        public void move(TrainUnit targetPosition) {
            TrainUnit currentPosition = this.currentBandit.getPosition();
            this.currentBandit.setPosition(targetPosition);
            if (targetPosition.isMarshalHere) {
                this.currentBandit.addDiscardPile((Card) this.neutralBulletCard[0]);
                this.neutralBulletCard.Remove(0);
                this.currentBandit.setPosition(currentPosition);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public ArrayList calculateMoveMarshal() {
            // TODO
            return new ArrayList();
        }
        
        public TrainUnit moveMarshalPrompt(ArrayList possibilities) {
            // TODO
            return new TrainUnit();
        }
        
        public void moveMarshal(TrainUnit targetPosition) {
            // TODO
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
    }
}