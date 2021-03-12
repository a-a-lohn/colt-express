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
        
        public static GameManager singleton;
        
        //public GameStatus gameStatus = GameStatus.SETUP;
        
        public String strGameStatus = "SETUP";
        
        public Round currentRound;
        
        public Bandit currentBandit;
        
        public ArrayList<Round> rounds = new ArrayList<Round>();
        
        //  CONVENTION FOR DECK: POSITION DECK.SIZE() IS TOP OF
        //  DECK, POSITION 0 IS BOTTOM OF DECK
        public Marshal marshalInstance;
        
        public PlayedPile playedPileInstance;
        
        //  CONVENTION FOR DECK: POSITION DECK.SIZE() IS TOP OF DECK, POSITION 0 IS
        //  BOTTOM OF DECK
        public TrainUnit[] trainRoof;
        
        public TrainUnit[] trainCabin;
        
        //  public TrainUnit[][] train; // change to 2 arrays
        //  public TrainUnit[] stagecoach; // change to arraylist
        public ArrayList<Bandit> bandits = new ArrayList<Bandit>();
        
        public HashMap<Bandit, User> banditmap = new HashMap<Bandit, User>();
        
        public static ColtMultiHandler handler;
        
        public ArrayList<Card> neutralBulletCard = new ArrayList<Card>();
        
        public int banditsPlayedThisTurn;
        
        public int roundIndex;
        
        public int banditIndex;
        
        public static void setHandler(ColtMultiHandler handle) {
            handler = handle;
            //  ISFSObject rtn = SFSObject.newInstance();
            //  handler.updateGameState(rtn);
        }
        
        //  SOME OF THESE FIELDS SHOULD BE AUTOMATICALLY INITIALIZED, NOT PASSED AS
        //  PARAMS
        //  this method should only be called from if-else block in chosenCharacter
        public void initializeGame() {
            //  set train-related attributes
            //  this.stagecoach = TrainUnit.createStagecoach();
            //  this.train = TrainUnit.createTrain(bandits.size());
            this.trainRoof = TrainUnit.createTrainRoof(this.getNumOfPlayers());
            this.trainCabin = TrainUnit.createTrainCabin(this.getNumOfPlayers());
            ArrayList<Bandit> bandits = this.getBandits();
            foreach (Bandit b in this.bandits) {
                //  initialize each bandit cards, purse
                b.createStartingCards();
                //  also the hand for bandits
                b.createBulletCards();
                b.createStartingPurse();
            }
            
            this.marshalInstance = Marshal.getInstance();
            //  initialize round cards, round attributes/create round constructor
            this.rounds = this.createRoundCards(this.getNumOfPlayers());
            Collections.shuffle(this.bandits);
            //  <- to decide who goes first, shuffle bandit list
            this.currentBandit = this.bandits.get(0);
            this.currentRound = this.rounds.get(0);
            this.setUpPositions(this.bandits);
            // 
            Marshal marshal = new Marshal();
            Money strongbox = new Money("STRONGBOX", 1000);
            // marshal.setMarshalPosition(this.trainCabin[this.getNumOfPlayers()]);
            // strongbox.setPosition(this.trainCabin[this.getNumOfPlayers()]);
            // 
            //  create netural bullet card
            Card NBullet1 = new BulletCard();
            Card NBullet2 = new BulletCard();
            Card NBullet3 = new BulletCard();
            Card NBullet4 = new BulletCard();
            Card NBullet5 = new BulletCard();
            Card NBullet6 = new BulletCard();
            Card NBullet7 = new BulletCard();
            Card NBullet8 = new BulletCard();
            Card NBullet9 = new BulletCard();
            Card NBullet10 = new BulletCard();
            Card NBullet11 = new BulletCard();
            Card NBullet12 = new BulletCard();
            Card NBullet13 = new BulletCard();
            this.neutralBulletCard.add(NBullet1);
            this.neutralBulletCard.add(NBullet2);
            this.neutralBulletCard.add(NBullet3);
            this.neutralBulletCard.add(NBullet4);
            this.neutralBulletCard.add(NBullet5);
            this.neutralBulletCard.add(NBullet6);
            this.neutralBulletCard.add(NBullet7);
            this.neutralBulletCard.add(NBullet8);
            this.neutralBulletCard.add(NBullet9);
            this.neutralBulletCard.add(NBullet10);
            this.neutralBulletCard.add(NBullet11);
            this.neutralBulletCard.add(NBullet12);
            this.neutralBulletCard.add(NBullet13);
            this.roundIndex = 0;
            //  this.currentRound = this.rounds.get(roundIndex);
            //  currentRound and currentRound.currentTurn must be initialized
            this.banditsPlayedThisTurn = 0;
            //this.gameStatus = GameStatus.SCHEMIN;
            this.strGameStatus = "SCHEMIN";
            this.currentBandit = this.bandits.get(0);
        }
        
        public void playTurn() {
            if ((this.strGameStatus == "SCHEMIN")) {
                this.promptDrawCardsOrPlayCard();
            }
            else if ((this.strGameStatus == "STEALIN")) {
                this.resolveAction(this.currentBandit.getToResolve());
            }
            
        }
        
        public void promptDrawCardsOrPlayCard() {
            // TODO
        }
        
        public void resolveAction(ActionCard toResolve) {
            if ((toResolve.getActionTypeAsString() == "CHANGEFLOOR")) {
                this.changeFloor();
            }
            else if ((toResolve.getActionTypeAsString() == "MARSHAL")) {
                ArrayList<TrainUnit> possibilities = this.calculateMoveMarshal();
                TrainUnit to = this.moveMarshalPrompt(possibilities);
                this.moveMarshal(to);
            }
            else if ((toResolve.getActionTypeAsString() == "MOVE")) {
                ArrayList<TrainUnit> possibilities = this.calculateMove();
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
            //  Remove card from bandit's hand
            this.currentBandit = c.getBelongsTo();
            this.currentBandit.removeHand(c);
            //  Prompt playing face down
            if (((this.currentBandit.getCharacter() == Character.GHOST) 
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
                Card toAdd = this.currentBandit.removeDeckAt(i);
                this.currentBandit.addHand(toAdd);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public void endOfTurn() {
            if ((this.strGameStatus == "SCHEMIN")) {
                string currentTurnType = this.currentRound.getCurrentTurn().getTurnTypeAsString()();
                if (((currentTurnType == "STANDARD") 
                            || (currentTurnType == "TUNNEL"))) {
                    this.banditIndex = ((this.banditIndex + 1) 
                                % this.bandits.size());
                    
                    this.banditsPlayedThisTurn++;
                    //  IF END OF TURN
                    if ((this.banditsPlayedThisTurn > this.bandits.size())) {
                        //  IF THERE ARE MORE TURNS IN THE ROUND
                        if ((this.currentRound.getNextTurn() != null)) {
                            //  IMPORTANT: getNextTurn() also SETS to next turn
                            this.currentBandit = this.bandits.get(this.banditIndex);
                        }
                        
                        //  IF THERE ARE NO MORE TURNS IN THE ROUND
                        this.banditsPlayedThisTurn = 0;
                        this.setGameStatus("STEALIN");
                    }
                    
                    //  IF NOT END OF TURN
                    this.currentBandit = this.bandits.get(this.banditIndex);
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
                    if ((this.roundIndex == this.rounds.size())) {
                        this.setGameStatus("COMPLETED");
                    }
                    else {
                        this.currentRound = this.rounds.get(this.roundIndex);
                        this.setGameStatus("SCHEMIN");
                        this.banditsPlayedThisTurn = 0;
                    }
                    
                }
                
            }
            
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
            return this.rounds.get(index);
        }
        
        public void addRound(Round a) {
            int size = this.rounds.size();
            this.rounds.add(a);
        }
        
        public void removeRound(Round a) {
            if (this.rounds.contains(a)) {
                this.rounds.remove(a);
            }
            
        }
        
        public bool roundsContains(Round a) {
            bool contains = this.rounds.contains(a);
            return contains;
        }
        
        public int sizeOfRounds() {
            int size = this.rounds.size();
            return size;
        }
        
        public ArrayList<Round> getRounds() {
            return this.rounds;
        }
        
        public ArrayList<Round> createRoundCards(int numOfPlayers) {
            ArrayList<Round> RoundCards = new ArrayList<Round>();
            if (((numOfPlayers == 2) 
                        || ((numOfPlayers == 3) 
                        || (numOfPlayers == 4)))) {
                Round r1 = new Round("AngryMarshal");
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("TUNNEL"));
                r1.addTurn(new Turn("SWITCHING"));
                RoundCards.add(r1);
                Round r2 = new Round("SwivelArm");
                r2.addTurn(new Turn("STANDARD"));
                r2.addTurn(new Turn("TUNNEL"));
                r2.addTurn(new Turn("STANDARD"));
                r2.addTurn(new Turn("STANDARD"));
                RoundCards.add(r2);
                Round r3 = new Round("Braking");
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                RoundCards.add(r3);
                Round r4 = new Round("TakeItAll");
                r4.addTurn(new Turn("STANDARD"));
                r4.addTurn(new Turn("TUNNEL"));
                r4.addTurn(new Turn("SPEEDINGUP"));
                r4.addTurn(new Turn("STANDARD"));
                RoundCards.add(r4);
                Round r5 = new Round("PassengersRebellion");
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("TUNNEL"));
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("STANDARD"));
                RoundCards.add(r5);
                Round r6 = new Round("SIX");
                r6.addTurn(new Turn("STANDARD"));
                r6.addTurn(new Turn("SPEEDINGUP"));
                r6.addTurn(new Turn("STANDARD"));
                RoundCards.add(r6);
                Round r7 = new Round("SEVEN");
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                r7.addTurn(new Turn("STANDARD"));
                RoundCards.add(r7);
            }
            else if (((numOfPlayers == 5) 
                        || (numOfPlayers == 6))) {
                Round r1 = new Round("AngryMarshal");
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("STANDARD"));
                r1.addTurn(new Turn("SWITCHING"));
                RoundCards.add(r1);
                Round r2 = new Round("SwivelArm");
                r2.addTurn(new Turn("STANDARD"));
                r2.addTurn(new Turn("TUNNEL"));
                r2.addTurn(new Turn("STANDARD"));
                RoundCards.add(r2);
                Round r3 = new Round("Braking");
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("TUNNEL"));
                r3.addTurn(new Turn("STANDARD"));
                r3.addTurn(new Turn("STANDARD"));
                RoundCards.add(r3);
                Round r4 = new Round("TakeItAll");
                r4.addTurn(new Turn("STANDARD"));
                r4.addTurn(new Turn("SPEEDINGUP"));
                r4.addTurn(new Turn("SWITCHING"));
                RoundCards.add(r4);
                Round r5 = new Round("PassengersRebellion");
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("TUNNEL"));
                r5.addTurn(new Turn("STANDARD"));
                r5.addTurn(new Turn("SWITCHING"));
                RoundCards.add(r5);
                Round r6 = new Round("SIX");
                r6.addTurn(new Turn("STANDARD"));
                r6.addTurn(new Turn("SPEEDINGUP"));
                RoundCards.add(r6);
                Round r7 = new Round("SEVEN");
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                r7.addTurn(new Turn("STANDARD"));
                r7.addTurn(new Turn("TUNNEL"));
                RoundCards.add(r7);
            }
            else {
                return null;
            }
            
            Collections.shuffle(RoundCards);
            return RoundCards;
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
            if ((this.bandits.size() >= index)) {
                this.bandits.remove(index);
            }
            
        }
        
        public Bandit getBanditsAt(int index) {
            if ((this.bandits.size() >= index)) {
                return this.bandits.get(index);
            }
            
            return null;
        }
        
        public void addBandit(Bandit a) {
            this.bandits.add(a);
        }
        
        public void removeBandits(Bandit a) {
            if (this.bandits.contains(a)) {
                this.bandits.remove(a);
            }
            
        }
        
        public bool containsBandits(Bandit a) {
            bool contains = this.bandits.contains(a);
            return contains;
        }
        
        public int sizeOfBandits() {
            int size = this.bandits.size();
            return size;
        }
        
        public ArrayList<Bandit> getBandits() {
            ArrayList<Bandit> b = ((ArrayList<Bandit>)(this.bandits.clone()));
            return b;
        }
        
        bool allPlayersChosen(int numPlayers) {
            //  TO DO
            if ((numPlayers == this.bandits.size())) {
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
        
        public void setUpPositions(ArrayList<Bandit> b) {
            int numOfBandit = b.size();
            if ((numOfBandit == 2)) {
                b.get(0).setPosition(this.trainCabin[0]);
                b.get(1).setPosition(this.trainCabin[1]);
            }
            else if ((numOfBandit == 3)) {
                b.get(0).setPosition(this.trainCabin[0]);
                b.get(1).setPosition(this.trainCabin[1]);
                b.get(2).setPosition(this.trainCabin[0]);
            }
            else if ((numOfBandit == 4)) {
                b.get(0).setPosition(this.trainCabin[0]);
                b.get(1).setPosition(this.trainCabin[1]);
                b.get(2).setPosition(this.trainCabin[0]);
                b.get(3).setPosition(this.trainCabin[1]);
            }
            else if ((numOfBandit == 5)) {
                b.get(0).setPosition(this.trainCabin[0]);
                b.get(1).setPosition(this.trainCabin[1]);
                b.get(2).setPosition(this.trainCabin[2]);
                b.get(3).setPosition(this.trainCabin[0]);
                b.get(4).setPosition(this.trainCabin[1]);
            }
            else if ((numOfBandit == 6)) {
                b.get(0).setPosition(this.trainCabin[0]);
                b.get(1).setPosition(this.trainCabin[1]);
                b.get(2).setPosition(this.trainCabin[2]);
                b.get(3).setPosition(this.trainCabin[0]);
                b.get(4).setPosition(this.trainCabin[1]);
                b.get(4).setPosition(this.trainCabin[2]);
            }
            else {
                return;
            }
            
        }
        
        //  void chosenCharacter(int playerId, Character c) {
        public void chosenCharacter(User player, Character c, int numPlayers) {
            Bandit newBandit = new Bandit(c);
            this.bandits.add(newBandit);
            this.banditmap.put(newBandit, player);
            //  TO DO.
            //  Here to associate playerId with newBandit.
            bool ready = this.allPlayersChosen(numPlayers);
            if (!ready) {
                Console.WriteLine("Not all players are ready!");
            }
            else {
                this.initializeGame();
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
        
        public Loot RobPrompt(Bandit b, HashSet<Loot> l) {
            //  TO DO
            //  ask b to choose loot from l
            return l.iterator().next();
        }
        
        public void Rob() {
            if (!this.currentBandit.getPosition().lootHere.isEmpty()) {
                Loot l = this.RobPrompt(this.currentBandit, this.currentBandit.getPosition().lootHere);
                this.currentBandit.addLoot(l);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public Bandit RoofShootPrompt(Bandit b, ArrayList<Bandit> ab) {
            //  TO DO
            //  ask b to choose one target from ab
            return ab.get(0);
        }
        
        public Bandit CarShootPrompt(Bandit b, ArrayList<Bandit> ab) {
            //  TO DO
            //  ask b to choose one target from ab
            return ab.get(0);
        }
        
        public void shoot() {
            ArrayList<Bandit> roofShootTarget = new ArrayList<Bandit>();
            ArrayList<Bandit> carShootTarget = new ArrayList<Bandit>();
            if ((this.currentBandit.getPosition().carFloor == CarFloor.ROOF)) {
                foreach (Bandit b in this.bandits) {
                    if ((b != this.currentBandit)) {
                        if ((b.getPosition().carFloor == CarFloor.ROOF)) {
                            roofShootTarget.add(b);
                        }
                        
                    }
                    
                }
                
                if ((roofShootTarget.get(0) != null)) {
                    //  ask the current player to choose which bandit to shoot
                    Bandit b = this.RoofShootPrompt(this.currentBandit, roofShootTarget);
                    BulletCard bc = this.currentBandit.bullets.remove(0);
                    if ((bc != null)) {
                        b.addDiscardPile(bc);
                    }
                    
                }
                
            }
            else {
                foreach (Bandit bl in this.currentBandit.getPosition().getLeft().banditsHere) {
                    carShootTarget.add(bl);
                }
                
                foreach (Bandit br in this.currentBandit.getPosition().getRight().banditsHere) {
                    carShootTarget.add(br);
                }
                
                if ((carShootTarget.get(0) != null)) {
                    //  ask the current player to choose which bandit to shoot
                    Bandit b = this.CarShootPrompt(this.currentBandit, carShootTarget);
                    BulletCard bc = this.currentBandit.bullets.remove(0);
                    if ((bc != null)) {
                        b.addDiscardPile(bc);
                    }
                    
                }
                
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public Bandit punchBanditPrompt(Bandit b, ArrayList<Bandit> ab) {
            //  TO DO
            //  ask b to choose one target from ab
            return ab.get(0);
        }
        
        public Loot punchLootPrompt(Bandit b, Bandit b2) {
            //  TO DO
            //  ask b to choose one loot from b2
            return b2.loot.get(0);
        }
        
        public TrainUnit punchPositionPrompt(Bandit b, Bandit b2) {
            //  TO DO
            //  ask b to choose one position that b2 can be punched to
            return b2.getPosition().left;
        }
        
        public void punch() {
            ArrayList<Bandit> otherBandits = new ArrayList<Bandit>();
            foreach (Bandit b in this.currentBandit.getPosition().banditsHere) {
                if ((b != this.currentBandit)) {
                    otherBandits.add(b);
                }
                
            }
            
            if ((otherBandits.get(0) != null)) {
                //  ask the player to choose target
                Bandit target = this.punchBanditPrompt(this.currentBandit, otherBandits);
                if (!target.getLoot().isEmpty()) {
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
        
        public static ArrayList<TrainUnit> moveAlgorithm(Bandit b) {
            // use currentBandit instead of parameter //void method
            ArrayList<TrainUnit> possibleMoving = new ArrayList<TrainUnit>();
            TrainUnit currentPosition = b.getPosition();
            if ((currentPosition.getLeft() != null)) {
                possibleMoving.add(currentPosition.getLeft());
            }
            
            if ((currentPosition.getRight() != null)) {
                possibleMoving.add(currentPosition.getLeft());
            }
            
            if ((currentPosition.carFloor == CarFloor.ROOF)) {
                if ((currentPosition.getLeft().getLeft() != null)) {
                    possibleMoving.add(currentPosition.getLeft());
                }
                
                if ((currentPosition.getRight().getRight() != null)) {
                    possibleMoving.add(currentPosition.getLeft());
                }
                
                if ((currentPosition.getLeft().getLeft().getLeft() != null)) {
                    possibleMoving.add(currentPosition.getLeft());
                }
                
                if ((currentPosition.getRight().getRight().getRight() != null)) {
                    possibleMoving.add(currentPosition.getLeft());
                }
                
            }
            
            return possibleMoving;
            // call promptMoves(possibleMoving)
        }
        
        public ArrayList<TrainUnit> calculateMove() {
            return new ArrayList<TrainUnit>();
        }
        
        public TrainUnit movePrompt(ArrayList<TrainUnit> possibilities) {
            // TODO
            return new TrainUnit();
        }
        
        public void move(TrainUnit targetPosition) {
            TrainUnit currentPosition = this.currentBandit.getPosition();
            this.currentBandit.setPosition(targetPosition);
            if (targetPosition.isMarshalHere) {
                this.currentBandit.addDiscardPile(this.neutralBulletCard.remove(0));
                this.currentBandit.setPosition(currentPosition);
            }
            
            this.endOfTurn();
            // might have to put this in an if else block for cases like SpeedingUp/Whiskey
        }
        
        public ArrayList<TrainUnit> calculateMoveMarshal() {
            // TODO
            return new ArrayList<TrainUnit>();
        }
        
        public TrainUnit moveMarshalPrompt(ArrayList<TrainUnit> possibilities) {
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