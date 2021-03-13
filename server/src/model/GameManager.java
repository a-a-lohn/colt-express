package model;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.extensions.SFSExtension;
import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

import main.ColtMultiHandler;
import model.Bandit;
import model.TrainUnit;

import com.smartfoxserver.v2.annotations.Instantiation;
import com.smartfoxserver.v2.annotations.Instantiation.InstantiationMode;
import com.smartfoxserver.v2.annotations.MultiHandler;

//@Instantiation(InstantiationMode.SINGLE_INSTANCE)
//@MultiHandler
public class GameManager /* extends BaseClientRequestHandler */ implements SerializableSFSType {

	transient public static GameManager singleton;
	transient public GameStatus gameStatus = GameStatus.SETUP;
	public String strGameStatus = "SETUP";
	public Round currentRound;
	public Bandit currentBandit;
	public ArrayList<Round> rounds = new ArrayList<Round>(); // CONVENTION FOR DECK: POSITION DECK.SIZE() IS TOP OF
																// DECK, POSITION 0 IS BOTTOM OF DECK
	public Marshal marshalInstance;
	public PlayedPile playedPileInstance; // CONVENTION FOR DECK: POSITION DECK.SIZE() IS TOP OF DECK, POSITION 0 IS
											// BOTTOM OF DECK
	public TrainUnit[] trainRoof;
	public TrainUnit[] trainCabin;
	public TrainUnit[][] train; 
	public TrainUnit[] stagecoach;
	public ArrayList<Bandit> bandits = new ArrayList<Bandit>();
	transient public HashMap<Bandit, User> banditmap = new HashMap<Bandit, User>();
	public static ColtMultiHandler handler;
	public ArrayList<Card> neutralBulletCard = new ArrayList<Card>();
	public int banditsPlayedThisTurn;
	public int roundIndex;
	public int banditIndex;


	public static void setHandler(ColtMultiHandler handle) {
		handler = handle;
		// ISFSObject rtn = SFSObject.newInstance();
		// handler.updateGameState(rtn);
	}

	/*
	 * @Override public void handleClientRequest(User sender, ISFSObject params) {
	 * //String command = params.getUtfString(SFSExtension.MULTIHANDLER_REQUEST_ID);
	 * 
	 * ISFSObject gameState = SFSObject.newInstance();
	 * 
	 * // if(truecommand.equals("test")) { bandits.add(b); // //ISFSArray
	 * banditsArray = SFSArray.newInstance(); // //gameState.putClass("bandits",
	 * bandits); // } gameState.putUtfString("testStr", "someData"); ColtExtension
	 * parentExt = (ColtExtension)getParentExtension(); Zone zone =
	 * parentExt.getParentZone(); //parentExt.send("updateGameState", gameState,
	 * (List<User>) zone.getUserList()); parentExt.send("updateGameState",
	 * gameState, sender);
	 * 
	 * }
	 */

	// SOME OF THESE FIELDS SHOULD BE AUTOMATICALLY INITIALIZED, NOT PASSED AS
	// PARAMS

	/*
	 * private GameManager(ArrayList<Bandit> bandits, Bandit currentBandit,
	 * ArrayList<Round> rounds, Round currentRound, GameStatus status, TrainUnit[][]
	 * trainUnits) { =======
	 * 
	 * /**
	 * 
	 * @param bandits ALL BANDITS MUST BE CREATED BEFORE THIS METHOD CAN BE CALLED
	 * 
	 * @param currentBandit
	 * 
	 * @param rounds
	 * 
	 * @param currentRound
	 * 
	 * @param status
	 */
	/*
	 * private GameManager(ArrayList<Bandit> bandits, Bandit currentBandit,
	 * ArrayList<Round> rounds, Round currentRound, GameStatus status) {
	 * 
	 * this.bandits = bandits; this.train =
	 * TrainUnit.createTrain(this.bandits.size()); this.stagecoach =
	 * TrainUnit.createStagecoach();
	 * 
	 * this.currentBandit = currentBandit; this.rounds = rounds; this.currentRound =
	 * currentRound; this.gameStatus = status; this.marshalInstance =
	 * Marshal.createMarshal(); this.playedPileInstance = PlayedPile.getInstance();
	 * 
	 * }
	 */

	// this method should only be called from if-else block in chosenCharacter
	public void initializeGame() {
		// set train-related attributes
		// this.stagecoach = TrainUnit.createStagecoach();
		// this.train = TrainUnit.createTrain(bandits.size());
		this.trainRoof = TrainUnit.createTrainRoof(this.getNumOfPlayers());
		this.trainCabin = TrainUnit.createTrainCabin(this.getNumOfPlayers());
		ArrayList<Bandit> bandits = this.getBandits();
		for (Bandit b : bandits) {
			// initialize each bandit cards, purse
			b.createStartingCards(); // also the hand for bandits
			b.createBulletCards();
			b.createStartingPurse();
		}
		this.marshalInstance = Marshal.getInstance();
		// initialize round cards, round attributes/create round constructor
		this.rounds = this.createRoundCards(this.getNumOfPlayers());
		Collections.shuffle(this.bandits); // <- to decide who goes first, shuffle bandit list
		this.currentBandit = this.bandits.get(0);
		this.currentRound = this.rounds.get(0);
		this.setUpPositions(this.bandits);
		//
		Marshal marshal = new Marshal();
		Money strongbox = new Money(MoneyType.STRONGBOX, 1000);
		//marshal.setMarshalPosition(this.trainCabin[this.getNumOfPlayers()]);
		//strongbox.setPosition(this.trainCabin[this.getNumOfPlayers()]);
		//
		// create netural bullet card
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
		// this.currentRound = this.rounds.get(roundIndex);
		// currentRound and currentRound.currentTurn must be initialized
		this.banditsPlayedThisTurn = 0;
		this.gameStatus = GameStatus.SCHEMIN;
		this.strGameStatus = "SCHEMIN";
		this.currentBandit = this.bandits.get(0);
	}

	public void playTurn() {
		if(/*currentBandit == me &&*/ this.gameStatus == GameStatus.SCHEMIN) {
			promptDrawCardsOrPlayCard();
		}
		else if(/*currentBandit == me &&*/ this.gameStatus == GameStatus.STEALIN) {
			resolveAction(currentBandit.getToResolve());
		}
		
	}
	
	public void promptDrawCardsOrPlayCard(){
		//TODO
		/*
		 * if(click draw cards){
		 * 	drawCards(3);
		 * }
		 * else if(click ActionCard c){
		 * 	playCard(c);
		 * }
		 */
	}
	
	public void resolveAction(ActionCard toResolve) {
		if(toResolve.getActionType() == ActionType.CHANGEFLOOR) {
			changeFloor();
		}
		else if(toResolve.getActionType() == ActionType.MARSHAL) {
			calculateMoveMarshal();
		}
		else if(toResolve.getActionType() == ActionType.MOVE) {
			calculateMove();
		}
		else if(toResolve.getActionType() == ActionType.PUNCH) {
			calculatePunch();
		}
		else if(toResolve.getActionType() == ActionType.ROB) {
			calculateRob();
		}
		else if(toResolve.getActionType() == ActionType.SHOOT) {
			calculateShoot();
		}
		
	}
	
	/**
	 * @param c Card will be moved from bandit's hand to played pile and it's effect
	 *          will be resolved
	 *
	 */
	public void playCard(ActionCard c) {

		// Remove card from bandit's hand
		this.currentBandit = c.getBelongsTo();
		this.currentBandit.removeHand(c);

		// Prompt playing face down
		if (currentBandit.getCharacter() == Character.GHOST && this.currentRound.getTurnCounter() == 0) {
			promptFaceUpOrFaceDown(c);
		} else if (this.currentRound.getCurrentTurn().getTurnType() == TurnType.TUNNEL) {
			// this.currentRound.getCurrentTurn().getTurnTypeAsString().equals("TUNNEL")
			c.setFaceDown(true);
		}

		// Assign card to played pile
		PlayedPile pile = PlayedPile.getInstance();
		pile.addPlayedCards(c);
		// TODO: graphical response
		
		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
		
	}

	public void promptFaceUpOrFaceDown(ActionCard c) {
		//TODO
		/*
		 * if(click face down choice){
		 * 	c.setFaceDown(true);
		 * 	PlayedPile.getInstance().addPlayedCards(c);
		 * 	TODO: graphical response
		 * 	endOfTurn();
		 * }
		 * else if(click face up up choice){
		 * 	PlayedPile.getInstance().addPlayedCards(c);
		 *  TODO: graphical response
		 *  endOfTurn();
		 */
	}
	
	/**
	 * 
	 * @param cardsToDraw the number of cards to add to the Bandit's hand from the
	 *                    top of their deck
	 * @pre currentBandit must be set correctly
	 */
	public void drawCards(int cardsToDraw) {
		for (int i = currentBandit.sizeOfDeck() - 1; i > currentBandit.sizeOfDeck() - cardsToDraw - 1; i--) {
			Card toAdd = currentBandit.removeDeckAt(i);
			currentBandit.addHand(toAdd);
		}
		
		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
	}	
	
	public void endOfTurn() {
		if (this.gameStatus == GameStatus.SCHEMIN) {

			TurnType currentTurnType = this.currentRound.getCurrentTurn().getTurnType();

			if (currentTurnType == TurnType.STANDARD || currentTurnType == TurnType.TUNNEL) {
				banditIndex = (banditIndex + 1) % this.bandits.size();
				banditsPlayedThisTurn++;
				// IF END OF TURN
				if (banditsPlayedThisTurn == this.bandits.size()) {
					// IF THERE ARE MORE TURNS IN THE ROUND
					if (this.currentRound.hasNextTurn() == true) {
						this.currentRound.setNextTurn();
						this.currentBandit = this.bandits.get(banditIndex);
						this.banditIndex++;
					}
					// IF THERE ARE NO MORE TURNS IN THE ROUND
					else {
						banditsPlayedThisTurn = 0;
						this.setGameStatus(GameStatus.STEALIN);
					}
				}
				// IF NOT END OF TURN
				else {
					this.currentBandit = this.bandits.get(banditIndex);
				}
			}

			else if (currentTurnType == TurnType.SPEEDINGUP) {
				if(currentBandit.consecutiveTurnCounter == 0) {
					currentBandit.setConsecutiveTurnCounter(1);
					promptDrawCardsOrPlayCard();
				}
				else if(currentBandit.consecutiveTurnCounter == 1) {
					banditIndex = (banditIndex + 1) % this.bandits.size();
					banditsPlayedThisTurn++;
					// IF END OF TURN
					if(banditsPlayedThisTurn == this.bandits.size()) {
						// IF THERE ARE MORE TURNS IN THE ROUND
						if(this.currentRound.hasNextTurn() == true) {
							this.currentRound.setNextTurn();
							this.currentBandit = this.bandits.get(banditIndex);
							this.banditIndex++;
						}
						// IF THERE ARE NO MORE TURNS IN THE ROUND
						else {
							banditsPlayedThisTurn = 0;
							this.setGameStatus(GameStatus.STEALIN);
						}
					}
					// IF NOT END OF TURN
					else {
						this.currentBandit = this.bandits.get(banditIndex);
					}
				}
			}

			else if (currentTurnType == TurnType.SWITCHING) {
				banditIndex = (banditIndex - 1 + this.bandits.size()) % this.bandits.size();
				banditsPlayedThisTurn++;
				// IF END OF TURN
				if (banditsPlayedThisTurn == this.bandits.size()) {
					// IF THERE ARE MORE TURNS IN THE ROUND
					if (this.currentRound.hasNextTurn() == true) {
						this.currentRound.setNextTurn();
						this.currentBandit = this.bandits.get(banditIndex);
						this.banditIndex++;
					}
					// IF THERE ARE NO MORE TURNS IN THE ROUND
					else {
						banditsPlayedThisTurn = 0;
						this.setGameStatus(GameStatus.STEALIN);
					}
				}
				// IF NOT END OF TURN
				else {
					this.currentBandit = this.bandits.get(banditIndex);
				}
			}
		} 
		else if (this.gameStatus == GameStatus.STEALIN) { 
			Card toResolve = this.playedPileInstance.takeTopCard();
			if (toResolve != null) {
				currentBandit = toResolve.getBelongsTo();
			} else { // played pile is empty
				roundIndex++;
				if (roundIndex == this.rounds.size()) {
					this.setGameStatus(GameStatus.COMPLETED);
				} else {
					this.currentRound = this.rounds.get(roundIndex);
					this.setGameStatus(GameStatus.SCHEMIN);
					this.banditsPlayedThisTurn = 0;
				}
			}
		}
	}

	public GameManager() {}

	public static GameManager getInstance() {
		GameManager gm = null;
		if (singleton == null) {
			gm = new GameManager();/*
									 * new ArrayList<Bandit>(), null, new ArrayList<Round>(), null,
									 * GameStatus.SETUP, TrainUnit.createTrain(0));
									 */
		} else {
			gm = singleton;
		}
		return gm;
	}

	/**
	 * --ROUND METHODS--
	 */
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
		int size = rounds.size();
		/*
		 * if (size == maximum) { return false; }
		 */
		this.rounds.add(a);
	}

	public void removeRound(Round a) {
		if (this.rounds.contains(a)) {
			this.rounds.remove(a);
		}
	}

	public boolean roundsContains(Round a) {
		boolean contains = rounds.contains(a);
		return contains;
	}

	public int sizeOfRounds() {
		int size = rounds.size();
		return size;
	}

	public ArrayList<Round> getRounds() {
		return this.rounds;
	}

	public ArrayList<Round> createRoundCards(int numOfPlayers) {
		ArrayList<Round> RoundCards = new ArrayList<Round>();
		if (numOfPlayers == 2 || numOfPlayers == 3 || numOfPlayers == 4) {
			Round r1 = new Round(RoundType.AngryMarshal);
			r1.addTurn(new Turn(TurnType.STANDARD));
			r1.addTurn(new Turn(TurnType.STANDARD));
			r1.addTurn(new Turn(TurnType.TUNNEL));
			r1.addTurn(new Turn(TurnType.SWITCHING));
			RoundCards.add(r1);

			Round r2 = new Round(RoundType.SwivelArm);
			r2.addTurn(new Turn(TurnType.STANDARD));
			r2.addTurn(new Turn(TurnType.TUNNEL));
			r2.addTurn(new Turn(TurnType.STANDARD));
			r2.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r2);

			Round r3 = new Round(RoundType.Braking);
			r3.addTurn(new Turn(TurnType.STANDARD));
			r3.addTurn(new Turn(TurnType.STANDARD));
			r3.addTurn(new Turn(TurnType.STANDARD));
			r3.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r3);

			Round r4 = new Round(RoundType.TakeItAll);
			r4.addTurn(new Turn(TurnType.STANDARD));
			r4.addTurn(new Turn(TurnType.TUNNEL));
			r4.addTurn(new Turn(TurnType.SPEEDINGUP));
			r4.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r4);

			Round r5 = new Round(RoundType.PassengersRebellion);
			r5.addTurn(new Turn(TurnType.STANDARD));
			r5.addTurn(new Turn(TurnType.STANDARD));
			r5.addTurn(new Turn(TurnType.TUNNEL));
			r5.addTurn(new Turn(TurnType.STANDARD));
			r5.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r5);

			Round r6 = new Round(RoundType.SIX);
			r6.addTurn(new Turn(TurnType.STANDARD));
			r6.addTurn(new Turn(TurnType.SPEEDINGUP));
			r6.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r6);

			Round r7 = new Round(RoundType.SEVEN);
			r7.addTurn(new Turn(TurnType.STANDARD));
			r7.addTurn(new Turn(TurnType.TUNNEL));
			r7.addTurn(new Turn(TurnType.STANDARD));
			r7.addTurn(new Turn(TurnType.TUNNEL));
			r7.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r7);

		} else if (numOfPlayers == 5 || numOfPlayers == 6) {
			Round r1 = new Round(RoundType.AngryMarshal);
			r1.addTurn(new Turn(TurnType.STANDARD));
			r1.addTurn(new Turn(TurnType.STANDARD));
			r1.addTurn(new Turn(TurnType.SWITCHING));
			RoundCards.add(r1);

			Round r2 = new Round(RoundType.SwivelArm);
			r2.addTurn(new Turn(TurnType.STANDARD));
			r2.addTurn(new Turn(TurnType.TUNNEL));
			r2.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r2);

			Round r3 = new Round(RoundType.Braking);
			r3.addTurn(new Turn(TurnType.STANDARD));
			r3.addTurn(new Turn(TurnType.TUNNEL));
			r3.addTurn(new Turn(TurnType.STANDARD));
			r3.addTurn(new Turn(TurnType.STANDARD));
			RoundCards.add(r3);

			Round r4 = new Round(RoundType.TakeItAll);
			r4.addTurn(new Turn(TurnType.STANDARD));
			r4.addTurn(new Turn(TurnType.SPEEDINGUP));
			r4.addTurn(new Turn(TurnType.SWITCHING));
			RoundCards.add(r4);

			Round r5 = new Round(RoundType.PassengersRebellion);
			r5.addTurn(new Turn(TurnType.STANDARD));
			r5.addTurn(new Turn(TurnType.TUNNEL));
			r5.addTurn(new Turn(TurnType.STANDARD));
			r5.addTurn(new Turn(TurnType.SWITCHING));
			RoundCards.add(r5);

			Round r6 = new Round(RoundType.SIX);
			r6.addTurn(new Turn(TurnType.STANDARD));
			r6.addTurn(new Turn(TurnType.SPEEDINGUP));
			RoundCards.add(r6);

			Round r7 = new Round(RoundType.SEVEN);
			r7.addTurn(new Turn(TurnType.STANDARD));
			r7.addTurn(new Turn(TurnType.TUNNEL));
			r7.addTurn(new Turn(TurnType.STANDARD));
			r7.addTurn(new Turn(TurnType.TUNNEL));
			RoundCards.add(r7);

		} else {
			return null;
		}
		;

		Collections.shuffle(RoundCards);

		return RoundCards;
	}

	/**
	 * --GAME STATUS METHODS--
	 */
	public void setGameStatus(GameStatus newStatus) {
		this.gameStatus = newStatus;
		this.strGameStatus = gameStatus.toString();
	}

	public GameStatus getGameStatus() {
		return this.gameStatus;
	}

	/**
	 * --TRAIN UNIT METHODS--
	 */
	/*
	 * public void addTrainUnitsAt(int index, TrainUnit a) { boolean contains =
	 * this.trainUnits.contains(a); if (contains) { return; } trainUnits.add(index,
	 * a); }
	 * 
	 * public void removeTrainUnitsAt(int index) { if (this.trainUnits.size >=
	 * index) { this.trainUnits.remove(index); } }
	 * 
	 * public TrainUnit getTrainUnitsAt(int index) { if (this.trainUnits.size >=
	 * index) { return this.trainUnits.get(index); } }
	 * 
	 * public void addTrainUnit(TrainUnit a) { this.trainUnits.add(a); }
	 * 
	 * public void removeTrainUnits(TrainUnit a) { if (this.trainUnits.contains(a))
	 * { this.trainUnits.remove(a); } }
	 * 
	 * public boolean trainUnitsContain(TrainUnit a) { boolean contains =
	 * trainUnits.contains(a); return contains; }
	 * 
	 * public int sizeOfTrainUnits() { int size = this.trainUnits.size(); return
	 * size; }
	 * 
	 * public ArrayList<TrainUnit> getTrainUnits() { return this.trainUnits; }
	 */

	/**
	 * --BANDIT METHODS--
	 */
	public Bandit getCurrentBandit() {
		return this.currentBandit;
	}

	public void setCurrentBandit(Bandit newObject) {
		this.currentBandit = newObject;
	}

	/*
	 * boolean addBanditsAt(int index, Bandit a) { int size = bandits.size(); if
	 * (size == maximum) { return false; } bandits.add(index, a); return true; }
	 */

	public void removeBanditsAt(int index) {
		if (this.bandits.size() >= index) {
			this.bandits.remove(index);
		}
	}

	public Bandit getBanditsAt(int index) {
		if (this.bandits.size() >= index) {
			return this.bandits.get(index);
		}
		return null;
	}

	public void addBandit(Bandit a) {
		bandits.add(a);
	}

	public void removeBandits(Bandit a) {
		if (this.bandits.contains(a)) {
			this.bandits.remove(a);
		}
	}

	public boolean containsBandits(Bandit a) {
		boolean contains = this.bandits.contains(a);
		return contains;
	}

	public int sizeOfBandits() {
		int size = this.bandits.size();
		return size;
	}

	public ArrayList<Bandit> getBandits() {
		ArrayList<Bandit> b = (ArrayList<Bandit>) this.bandits.clone();
		return b;
	}

	/**
	 * --GAME MANAGER METHODS--
	 */

	boolean allPlayersChosen(int numPlayers) {
		// TO DO
		if (numPlayers == bandits.size()) {
			return true;
		}
		// for all players, return if they are all associated with a bandit or not.
		return false;
	}

	int getNumOfPlayers() {
		// TO DO
		// Here to get number of players
		return 3;
	}

	public void setUpPositions(ArrayList<Bandit> b) {
		int numOfBandit = b.size();
		if (numOfBandit == 2) {
			b.get(0).setPosition(this.trainCabin[0]);
			b.get(1).setPosition(this.trainCabin[1]);
		} else if (numOfBandit == 3) {
			b.get(0).setPosition(this.trainCabin[0]);
			b.get(1).setPosition(this.trainCabin[1]);
			b.get(2).setPosition(this.trainCabin[0]);
		} else if (numOfBandit == 4) {
			b.get(0).setPosition(this.trainCabin[0]);
			b.get(1).setPosition(this.trainCabin[1]);
			b.get(2).setPosition(this.trainCabin[0]);
			b.get(3).setPosition(this.trainCabin[1]);
		} else if (numOfBandit == 5) {
			b.get(0).setPosition(this.trainCabin[0]);
			b.get(1).setPosition(this.trainCabin[1]);
			b.get(2).setPosition(this.trainCabin[2]);
			b.get(3).setPosition(this.trainCabin[0]);
			b.get(4).setPosition(this.trainCabin[1]);
		} else if (numOfBandit == 6) {
			b.get(0).setPosition(this.trainCabin[0]);
			b.get(1).setPosition(this.trainCabin[1]);
			b.get(2).setPosition(this.trainCabin[2]);
			b.get(3).setPosition(this.trainCabin[0]);
			b.get(4).setPosition(this.trainCabin[1]);
			b.get(4).setPosition(this.trainCabin[2]);
		} else {
			return;
		}
	}

	// void chosenCharacter(int playerId, Character c) {
	public void chosenCharacter(User player, Character c, int numPlayers) {
		Bandit newBandit = new Bandit(c);
		this.bandits.add(newBandit);
		this.banditmap.put(newBandit, player);
		// TO DO.
		// Here to associate playerId with newBandit.
		boolean ready = this.allPlayersChosen(numPlayers);
		if (!ready) {
			System.out.println("Not all players are ready!");
		} else {
			this.initializeGame();
			this.setGameStatus(GameStatus.SCHEMIN);
		}
		// this.setCurrentRound(this.rounds.get(0));
		// set waiting for input to be true;
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

	/**
	 * --EXECUTE ACTIONS-- BEFORE CALLING ANY OF THESE METHODS, CURRENT BANDIT MUST
	 * BE ASSIGNED CORRECTLY All actions will be called from POV of this.currentBandit
	 */


	//          --ROB--
	
	
	public ArrayList<Loot> calculateRob(){
		//TODO
		return new ArrayList<Loot>();
	}
	
	public Loot RobPrompt(Bandit b, HashSet<Loot> l) {
		// TO DO
		// ask b to choose loot from l
		return l.iterator().next();
	}

	public void Rob() {

		if (!this.currentBandit.getPosition().lootHere.isEmpty()) {
			Loot l = RobPrompt(this.currentBandit, this.currentBandit.getPosition().lootHere);
			this.currentBandit.addLoot(l);
		}
		
		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
	}

	
	//          --SHOOT--
	
	
	public Bandit calculateShoot() {
		//TODO REMEMBER BELLE AND TUCO CASES, REMEMBER ROOF AND CABIN CASES
		return new Bandit();
	}
	
	public Bandit shootPrompt(ArrayList<Bandit> possibilities) {
		// TODO
		return new Bandit();
	}

	public void shoot() {

		Bandit toShoot = new Bandit(); //TODO <- replace with the target chosen by shootPrompt
		if(!currentBandit.bulletsIsEmpty()) {
			toShoot.addDeck(currentBandit.removeTopBullet()); //TODO <- graphical response
		}
		
		if(currentBandit.getCharacter() == Character.DJANGO) {
			TrainUnit left = currentBandit.getPosition().getLeft();
			TrainUnit right = currentBandit.getPosition().getRight();
			
			//IF BANDIT IS TO DJANGO'S LEFT, PUSH BANDIT 1 CART TO DJANGO'S LEFT IF POSSIBLE
			if(left.containsBandit(toShoot)) {
				if(left.getLeft() != null) {
					toShoot.setPosition(left.getLeft()); //TODO <- graphical response
					left.removeBandit(toShoot);
					left.getLeft().addBandit(toShoot);
				}
			}
			//IF BANDIT IS TO DJANGO'S RIGHT, PUSH BANDIT 1 CART TO DJANGO'S RIGHT IF POSSIBLE
			else if(right.containsBandit(toShoot)) {
				if(right.getRight() != null) {
					toShoot.setPosition(right.getRight()); //TODO <- graphical response
					right.removeBandit(toShoot);
					right.getRight().addBandit(toShoot);
				}
				
			}
		}
		
		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
	}

	
	//          --PUNCH--
	
	
	public ArrayList<Bandit> calculatePunch(){
		//TODO
		return new ArrayList<Bandit>();
	}
	
	public Bandit punchBanditPrompt(Bandit b, ArrayList<Bandit> ab) {
		// TO DO
		// ask b to choose one target from ab
		return ab.get(0);
	}

	public Loot punchLootPrompt(Bandit b, Bandit b2) {
		// TO DO
		// ask b to choose one loot from b2
		return b2.loot.get(0);
	}

	public TrainUnit punchPositionPrompt(Bandit b, Bandit b2) {
		// TO DO
		// ask b to choose one position that b2 can be punched to
		return b2.getPosition().left;
	}

	public void punch() {
		ArrayList<Bandit> otherBandits = new ArrayList<Bandit>();
		for (Bandit b : this.currentBandit.getPosition().banditsHere) {
			if (b != this.currentBandit) {
				otherBandits.add(b);
			}
		}

		if (otherBandits.get(0) != null) {
			// ask the player to choose target
			Bandit target = this.punchBanditPrompt(this.currentBandit, otherBandits);
			if (!target.getLoot().isEmpty()) {
				// ask the player to choose 1 loot
				Loot l = this.punchLootPrompt(this.currentBandit, target);
				target.removeLoot(l);
				this.currentBandit.addLoot(l);
			}

			// ask where to punch to
			TrainUnit tu = this.punchPositionPrompt(this.currentBandit, target);
			target.setPosition(tu);

		}
		
		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
	}

	
	//          --CHANGE FLOOR--
	
	
	public void changeFloor() {
		TrainUnit currentPosition = this.currentBandit.getPosition();
		if (currentPosition.getAbove() == null && currentPosition.getBelow() != null) {
			currentPosition.removeBandit(currentBandit);
			currentPosition.getBelow().addBandit(currentBandit);
			currentBandit.setPosition(currentPosition.getBelow());
		} else if (currentPosition.getBelow() == null && currentPosition.getAbove() != null) {
			currentPosition.removeBandit(currentBandit);
			currentPosition.getAbove().addBandit(currentBandit);
			currentBandit.setPosition(currentPosition.getAbove());
		}

		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
	}

	
	//          --MOVE--
	
	
	public ArrayList<TrainUnit> calculateMove() { //use currentBandit instead of parameter //void method
		ArrayList<TrainUnit> possibleMoving = new ArrayList<TrainUnit>();
		TrainUnit currentPosition = this.currentBandit.getPosition();
		if (currentPosition.getLeft() != null) {
			possibleMoving.add(currentPosition.getLeft());
		}
		if (currentPosition.getRight() != null) {
			possibleMoving.add(currentPosition.getLeft());
		}
		if (currentPosition.carFloor == CarFloor.ROOF) {

			if (currentPosition.getLeft().getLeft() != null) {
				possibleMoving.add(currentPosition.getLeft());
			}
			if (currentPosition.getRight().getRight() != null) {
				possibleMoving.add(currentPosition.getLeft());
			}
			if (currentPosition.getLeft().getLeft().getLeft() != null) {
				possibleMoving.add(currentPosition.getLeft());
			}
			if (currentPosition.getRight().getRight().getRight() != null) {
				possibleMoving.add(currentPosition.getLeft());
			}

		}
		return possibleMoving; //call promptMoves(possibleMoving)
	}
	
	public TrainUnit movePrompt(ArrayList<TrainUnit> possibilities) {return new TrainUnit();} //PLACEHOLDER, WILL NOT BE IN GM
	
	public void move(TrainUnit targetPosition) {
		TrainUnit currentPosition = this.currentBandit.getPosition();
		this.currentBandit.setPosition(targetPosition);
		if (targetPosition.isMarshalHere) {
			this.currentBandit.addDiscardPile(this.neutralBulletCard.remove(0));
			this.currentBandit.setPosition(currentPosition);
		}

		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
	}

	
	//          --MOVE MARSHAL--
	
	
	public ArrayList<TrainUnit> calculateMoveMarshal(){
		//TODO
		return new ArrayList<TrainUnit>();
	}
	
	public TrainUnit moveMarshalPrompt(ArrayList<TrainUnit> possibilities) {return new TrainUnit();} //PLACEHOLDER, WILL NOT BE IN GM
	
	public void moveMarshal(TrainUnit targetPosition) {
		//TODO
		endOfTurn(); //might have to put this in an if else block for cases like SpeedingUp/Whiskey
	}



}
