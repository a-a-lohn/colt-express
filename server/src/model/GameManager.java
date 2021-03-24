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
	public ArrayList<TrainUnit> trainRoof;
	public ArrayList<TrainUnit> trainCabin;
	public ArrayList<TrainUnit> stagecoach;
	public ArrayList<Horse> horses = new ArrayList<Horse>();
	public ArrayList<Bandit> bandits = new ArrayList<Bandit>();
	transient public HashMap<Bandit, User> banditmap = new HashMap<Bandit, User>();
	public HashMap<Bandit, TrainUnit> banditPositions = new HashMap<Bandit, TrainUnit>();
	public ArrayList<Card> neutralBulletCard = new ArrayList<Card>();
	public int banditsPlayedThisTurn;
	public int roundIndex;
	public int banditIndex;

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
		System.out.println("Initializing the game now!");
		// set train-related attributes
		TrainUnit.trainLength = this.getNumOfPlayers() + 1;
		this.trainRoof = TrainUnit.createTrainRoof();
		this.trainCabin = TrainUnit.createTrainCabin();
		this.stagecoach = TrainUnit.createStagecoach();
		ArrayList<Bandit> bandits = this.getBandits();
		// initialize each bandit cards, purse
		for (Bandit b : bandits) {
			b.createStartingCards();
			b.createHand();
			b.createBulletCards();
			b.createStartingPurse();
		}

		//this.horses = Horse.createHorses();
		
		// Horse Attack
		this.horseAttack();

		this.marshalInstance = Marshal.getInstance();
		// initialize round cards, round attributes/create round constructor
		this.rounds = this.createRoundCards(this.getNumOfPlayers());
		Collections.shuffle(this.bandits); // <- to decide who goes first, shuffle bandit list
		this.currentBandit = this.bandits.get(0);
		this.currentRound = this.rounds.get(0);
		this.setUpPositions(this.bandits);
		Money strongbox = new Money(MoneyType.STRONGBOX, 1000);
		this.trainCabin.get(0).setIsMarshalHere(true);
		this.trainCabin.get(0).addLoot(strongbox);
		// create neutral bullet card
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
		this.banditsPlayedThisTurn = 0;
		this.gameStatus = GameStatus.SCHEMIN;
		this.strGameStatus = "SCHEMIN";
		this.currentBandit = this.bandits.get(0);

	}

	public void horseAttack() {
		int turns = 1; // start with the first turn

		ArrayList<Bandit> banditList = new ArrayList<Bandit>();
		// copy bandits list
		for (Bandit b : this.bandits) {
			banditList.add(b);
		}

		int trainUnitLength = TrainUnit.trainLength;

		while (banditList.size() > 1) {
			TrainUnit currentTrain = this.getTrainCabinAt(trainUnitLength - turns);
			if (currentTrain == this.getTrainCabinAt(1)) {
				break;
			} else {
				for (Bandit b : banditList) {
					boolean continueToRide = promptHorseAttack1(b, turns);
					if (!continueToRide) {
						banditList.remove(b);
						b.setPosition(currentTrain);
						Horse newHorse = new Horse(null); // not belong to any bandit
						newHorse.setAdjacentTo(currentTrain); // set adjacent to current train
						this.horses.add(newHorse);
						banditList.remove(b);
					}
				}
				turns++;
			}
		}

		// case 1, banditList size > 1
		if (banditList.size() > 1) {
			for (Bandit b : banditList) {
				b.setPosition(this.getTrainCabinAt(1));
				Horse newHorse = new Horse(null); // not belong to any bandit
				newHorse.setAdjacentTo(this.getTrainCabinAt(1)); // set adjacent to current train
				this.horses.add(newHorse);
			}
		}

		// case 2, banditList size == 1
		if (banditList.size() == 1) {
			calculateHorseAttack(banditList.get(0), turns);
		}

	}

	public void calculateHorseAttack(Bandit bandit, int turns) {
		ArrayList<TrainUnit> target = new ArrayList<TrainUnit>();
		for (int i = turns; i < TrainUnit.trainLength; i++) {
			target.add(this.getTrainCabinAt(TrainUnit.trainLength - turns));
		}
		TrainUnit currentTrain = promptHorseAttack2(bandit, target);
		bandit.setPosition(currentTrain);
		Horse newHorse = new Horse(null); // not belong to any bandit
		newHorse.setAdjacentTo(currentTrain); // set adjacent to current train
		this.horses.add(newHorse);
	}
	
	public TrainUnit promptHorseAttack2(Bandit b, ArrayList<TrainUnit> target) {
		// TO DO : ask the client to choose one place to start
		return target.get(0);
	}
	
	public boolean promptHorseAttack1(Bandit b, int turns) {
		// TO DO : ask the client to stay at current position or ride horse to move on
		return true;
	}

	/**
	 * @param c Card will be moved from bandit's hand to played pile and it's effect
	 *          will be resolved
	 *
	 */

	public GameManager() {
	}

	public static GameManager getInstance() {
		// GameManager gm = null;
		if (singleton == null) {
			singleton = new GameManager();/*
											 * new ArrayList<Bandit>(), null, new ArrayList<Round>(), null,
											 * GameStatus.SETUP, TrainUnit.createTrain(0));
											 */
		}
		return singleton;
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
	public TrainUnit getTrainCabinAt(int index) {
		if (index >= this.trainCabin.size()) {
			return this.trainCabin.get(index);
		} else {
			return null;
		}
	}

	public TrainUnit getTrainRoofAt(int index) {
		if (index >= this.trainRoof.size()) {
			return this.trainRoof.get(index);
		} else {
			return null;
		}
	}

	public int sizeOfTrainUnits() {
		int size = this.trainCabin.size();
		return size;
	}

	public ArrayList<TrainUnit> getTrainCabin() {
		return this.trainCabin;
	}

	public ArrayList<TrainUnit> getTrainRoof() {
		return this.trainRoof;
	}

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
		return getBandits().size();
	}

	public void setUpPositions(ArrayList<Bandit> b) { // <-- fix this
		int numOfBandit = b.size();
		int tracker = 0;
		for (int i = 0; i < numOfBandit; i++) {
			Bandit tbp = this.bandits.get(i);
			TrainUnit tu = this.trainCabin.get(tracker + numOfBandit - 1);
			this.banditPositions.put(tbp, tu);
			tracker = (tracker + 1) % 2;
		}
	}

	// void chosenCharacter(int playerId, Character c) {
	public void chosenCharacter(User player, Character c, int numPlayers) {
		System.out.println("received " + c.toString());
		Bandit newBandit = new Bandit(c);
		this.bandits.add(newBandit);
		this.banditmap.put(newBandit, player);
		// TO DO.
		// Here to associate playerId with newBandit.
		boolean ready = this.allPlayersChosen(numPlayers);
		if (!ready) {
			System.out.println("Not all players are ready!");
		} else {
			System.out.println("initializing game");
			this.initializeGame();
			this.setGameStatus(GameStatus.SCHEMIN);
		}
		// this.setCurrentRound(this.rounds.get(0));
		// set waiting for input to be true;

	}
}

/**
 * --EXECUTE ACTIONS-- BEFORE CALLING ANY OF THESE METHODS, CURRENT BANDIT MUST
 * BE ASSIGNED CORRECTLY All actions will be called from POV of
 * this.currentBandit
 */
