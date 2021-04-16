package model;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;

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
	public int trainIndex;															// DECK, POSITION 0 IS BOTTOM OF DECK
	public Marshal marshalInstance;
	public PlayedPile playedPileInstance; // CONVENTION FOR DECK: POSITION DECK.SIZE() IS TOP OF DECK, POSITION 0 IS
											// BOTTOM OF DECK
	public ArrayList<TrainUnit> trainRoof;
	public ArrayList<TrainUnit> trainCabin;
	public int trainLength;
	public ArrayList<TrainUnit> stagecoach;
	public ArrayList<Horse> horses = new ArrayList<Horse>();
	public ArrayList<Bandit> bandits = new ArrayList<Bandit>();
	transient public HashMap<User, Bandit> banditmap = new HashMap<User, Bandit>();
	public Map<String, TrainUnit> banditPositions = new HashMap<String, TrainUnit>();
	public ArrayList<Card> neutralBulletCard = new ArrayList<Card>();
	public int banditsPlayedThisTurn;
	public int roundIndex;
	public int banditIndex;
	transient public boolean positionsChosen = false;

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
	
	/*public static void main(String[]args) {
		Bandit b1 = new Bandit(Character.DJANGO);
		Bandit b2 = new Bandit(Character.BELLE);
		Bandit b3 = new Bandit(Character.CHEYENNE);
		GameManager.getInstance().bandits.add(b1);
		GameManager.getInstance().bandits.add(b2);
		GameManager.getInstance().bandits.add(b3);
		GameManager.getInstance().currentBandit = GameManager.getInstance().bandits.get(0);
		GameManager.getInstance().initializeGame();
		GameManager gm = GameManager.getInstance();
		System.out.println();
	}*/
	
	/**
	 * --INITIALIZING THE GAME--
	 * 1. Create locomotive, stagecoach and 1 train car for each bandit
	 * 2. Give each bandit a $250 purse, 11 action cards, and 6 bullet cards
	 * 3. Create loot for each train cabin
	 * 4. Place marshal and strongbox in locomotive
	 * 5. Place shotgun and strongbox on roof of stagecoach
	 * 6. Create number of bandits minus 1 hostages
	 * 7. Create 16 neutral bullet cards
	 * 8. Create 4 round cards and 1 train station card
	 * 9. Create a horse for each bandit
	 * 10. Game Status set to Schemin, indices initialized
	 */
	public void initializeGame() {
		System.out.println("Initializing the game now!");
		// 1. Create locomotive and 1 train car for each bandit
		this.trainLength = this.getNumOfPlayers() + 1;
		this.trainIndex = this.trainLength-1;
		System.out.println("trainlength: " + trainLength);
		this.trainRoof = TrainUnit.createTrainRoof();
		this.trainCabin = TrainUnit.createTrainCabin();
		this.stagecoach = TrainUnit.createStagecoach();
		
		// 2. Give each bandit a $250 purse, 11 action cards, and 6 bullet cards
		ArrayList<Bandit> bandits = this.getBandits();
		// initialize each bandit cards, purse
		for (Bandit b : bandits) {
			b.createStartingPurse();
			b.createStartingCards();
			b.createBulletCards();
		}

		// 3. Create loot for each train cabin (CAR1 3 PURSE, CAR2 3 GEM, CAR3 1 PURSE 1 GEM, CAR4 3 PURSE 1 GEM, CAR5 4 PURSE 1 GEM, CAR6 1 PURSE)
		Loot.createLoot();
		
		// 4. Place marshal and strongbox in locomotive
		this.marshalInstance = Marshal.getInstance();
		this.trainCabin.get(0).setIsMarshalHere(true);
		Money strongbox = new Money(MoneyType.STRONGBOX, 1000);
		this.trainCabin.get(0).addLoot(strongbox);
		
		// 5. Place shotgun and strongbox on roof of stagecoach
		//TODO

		// 6. Create number of bandits minus 1 hostages
		//TODO
		
		// 7. Create 16 neutral bullet cards
		for(int i=0; i<16; i++) {
			Card neutralBullet = new BulletCard("NEUTRAL");
			neutralBulletCard.add(neutralBullet);
		}
		
		// 8. Create 4 round cards and 1 train station card
		this.rounds = this.createRoundCards(this.getNumOfPlayers());
		this.currentRound = this.rounds.get(0);
		
		// 9. Create a horse for each bandit
		this.horses = Horse.createHorses();
		
		// --FOR TESTING--
		//this.setUpPositions(this.bandits);
		System.out.println("bpos size: " + banditPositions.size());
		// --END--
		
		// 10. Game Status set to Schemin, indices initialized
		this.banditIndex = 0;
		this.roundIndex = 0;
		this.currentRound = rounds.get(roundIndex);
		this.banditsPlayedThisTurn = 0;
		this.gameStatus = GameStatus.SCHEMIN;
		this.strGameStatus = "SCHEMIN";

		// Horse Attack
		//this.horseAttack();
		
	}

	public void horseAttack() {
		int turns = 1; // start with the first turn

		ArrayList<Bandit> banditList = new ArrayList<Bandit>();
		// copy bandits list
		for (Bandit b : this.bandits) {
			banditList.add(b);
		}

		int trainUnitLength = this.trainLength;

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
		for (int i = turns; i < this.trainLength; i++) {
			target.add(this.getTrainCabinAt(this.trainLength - turns));
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
		if (numOfPlayers >= 2 && numOfPlayers <= 6) {
			Round r1 = new Round(RoundType.AngryMarshal);
			RoundCards.add(r1);
			Round r2 = new Round(RoundType.SwivelArm);
			RoundCards.add(r2);
			Round r3 = new Round(RoundType.Braking);
			RoundCards.add(r3);
			Round r4 = new Round(RoundType.TakeItAll);
			RoundCards.add(r4);
			Round r5 = new Round(RoundType.PassengersRebellion);
			RoundCards.add(r5);
			Round r6 = new Round(RoundType.Bridge);
			RoundCards.add(r6);
			Round r7 = new Round(RoundType.Cave);
			RoundCards.add(r7);
		} 
		else {
			return null;
		}

		Collections.shuffle(RoundCards);
		RoundCards.remove(0);
		RoundCards.remove(0);
		RoundCards.remove(0);
		
		//Round r8 = new Round(RoundType.PickPocketing);
		Round r9 = new Round(RoundType.MarshalsRevenge);
		//Round r10 = new Round(RoundType.HostageConductor);
		RoundCards.add(r9);
		
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
		if (index < this.trainLength) {
			 return this.trainCabin.get(index);
		} 
		else {
			 return null;
		}
	 }
	 
	 public TrainUnit getTrainRoofAt(int index) {
		 if(index >= this.trainLength) {
			 return this.trainRoof.get(index);
		 }
		 else {
			 return null;
		 }
	 }
	 
	 public int sizeOfTrainUnits() {
		 return this.trainLength;
	 }
	 
	 public ArrayList<TrainUnit> getTrainCabin() {
		 return this.trainCabin;
	 }
	 
	 public ArrayList<TrainUnit> getTrainRoof(){
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

	public void setUpPositions(ArrayList<Bandit> b) { //<-- fix this
		int numOfBandit = b.size();
		int tracker=0;
		for (int i=0; i<numOfBandit; i++) {
			Bandit tbp = this.bandits.get(i);
			TrainUnit tu = this.trainCabin.get(tracker + numOfBandit - 1);
			this.banditPositions.put(tbp.characterAsString, tu);
			tracker = (tracker + 1) % 2;
		}
	}

	// void chosenCharacter(int playerId, Character c) {
	public void chosenCharacter(User player, Character c, int numPlayers) {
		System.out.println("received " + c.toString());
		Bandit newBandit = new Bandit(c);
		this.bandits.add(newBandit);
		this.banditmap.put(player, newBandit);
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
