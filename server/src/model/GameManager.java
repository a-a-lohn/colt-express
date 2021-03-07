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
public class GameManager /*extends BaseClientRequestHandler */implements SerializableSFSType {

	transient public static GameManager singleton;
	transient public GameStatus gameStatus;
	public String strGameStatus;
	public Round currentRound;
	public Bandit currentBandit;
	public ArrayList<Round> rounds = new ArrayList<Round>();
	public Marshal marshalInstance;
	public PlayedPile playedPileInstance;
	public TrainUnit[][] train;
	public TrainUnit[] stagecoach;
	public ArrayList<Bandit> bandits = new ArrayList<Bandit>();
	transient public HashMap<Bandit, User> banditmap = new HashMap<Bandit, User>();
	public static ColtMultiHandler handler;
	

	public static void setHandler(ColtMultiHandler handle) {
		handler = handle;
		//ISFSObject rtn = SFSObject.newInstance();
		//handler.updateGameState(rtn);
	}
		
	/*@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		//String command = params.getUtfString(SFSExtension.MULTIHANDLER_REQUEST_ID);
		
		ISFSObject gameState = SFSObject.newInstance();
		
//		if(truecommand.equals("test")) {
			bandits.add(b);
//			//ISFSArray banditsArray = SFSArray.newInstance();
//			//gameState.putClass("bandits", bandits);
//		}
		gameState.putUtfString("testStr", "someData");
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		//parentExt.send("updateGameState", gameState, (List<User>) zone.getUserList());
		parentExt.send("updateGameState", gameState, sender);
		
	}*/
	
	// SOME OF THESE FIELDS SHOULD BE AUTOMATICALLY INITIALIZED, NOT PASSED AS PARAMS

	/*private GameManager(ArrayList<Bandit> bandits, Bandit currentBandit, ArrayList<Round> rounds, Round currentRound,
			GameStatus status, TrainUnit[][] trainUnits) {
=======
	
	/**
	 * 
	 * @param bandits
	 *           ALL BANDITS MUST BE CREATED BEFORE THIS METHOD CAN BE CALLED 
	 * @param currentBandit
	 * @param rounds
	 * @param currentRound
	 * @param status
	 */
	/*private GameManager(ArrayList<Bandit> bandits, Bandit currentBandit, ArrayList<Round> rounds, Round currentRound,
			GameStatus status) {
		
		this.bandits = bandits;
		this.train = TrainUnit.createTrain(this.bandits.size());
		this.stagecoach = TrainUnit.createStagecoach();
		
		this.currentBandit = currentBandit;
		this.rounds = rounds;
		this.currentRound = currentRound;
		this.gameStatus = status;
		this.marshalInstance = Marshal.createMarshal();
		this.playedPileInstance = PlayedPile.getInstance();

	}*/


	//this method should only be called from if-else block in chosenCharacter
	public void initializeGame() {
		//set train-related attributes
		this.stagecoach = TrainUnit.createStagecoach();
		//this.train = TrainUnit.createTrain(bandits.size());
		ArrayList<Bandit> bandits = this.getBandits();
		for (Bandit b: bandits) {
			//initialize each bandit cards, purse
			b.createStartingCards();
			b.createHand();
			b.createBulletCards();
			b.createStartingPurse();
			//TODO: place bandits
		}
		this.marshalInstance = Marshal.getInstance();
		//TODO: initialize round cards, round attributes/create round constructor
		Round current = new Round();
		Collections.shuffle(this.bandits); //<- to decide who goes first, shuffle bandit list
		this.currentBandit = this.bandits.get(0);
		this.rounds.add(current);
		this.currentRound = current;
		this.setUpPositions(this.bandits);
		//
		Marshal marshal = new Marshal();
		Money strongbox = new Money(MoneyType.STRONGBOX, 1000);
		marshal.setMarshalPosition(this.train[1][this.bandits.size()]);
		strongbox.setPosition(this.train[1][this.bandits.size()]);
		//
		// TODO: create netural bullet card
	}
	
	public void endOfTurn() {
		int index = this.bandits.indexOf(this.currentBandit);
		index++;
		if (index<this.bandits.size()) {
			this.currentBandit = this.bandits.get(index);
		}
		else {
			//TODO: deal with end of round 
		}
	}
	
	public GameManager() {};

	public static GameManager getInstance() {
		GameManager gm = null;
		if (singleton == null) {
			gm = new GameManager();/*new ArrayList<Bandit>(), null, new ArrayList<Round>(), null, GameStatus.SETUP,
					TrainUnit.createTrain(0));*/
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
	/*public void addTrainUnitsAt(int index, TrainUnit a) {
		boolean contains = this.trainUnits.contains(a);
		if (contains) {
			return;
		}
		trainUnits.add(index, a);
	}

	public void removeTrainUnitsAt(int index) {
		if (this.trainUnits.size >= index) {
			this.trainUnits.remove(index);
		}
	}

	public TrainUnit getTrainUnitsAt(int index) {
		if (this.trainUnits.size >= index) {
			return this.trainUnits.get(index);
		}
	}

	public void addTrainUnit(TrainUnit a) {
		this.trainUnits.add(a);
	}

	public void removeTrainUnits(TrainUnit a) {
		if (this.trainUnits.contains(a)) {
			this.trainUnits.remove(a);
		}
	}

	public boolean trainUnitsContain(TrainUnit a) {
		boolean contains = trainUnits.contains(a);
		return contains;
	}

	public int sizeOfTrainUnits() {
		int size = this.trainUnits.size();
		return size;
	}

	public ArrayList<TrainUnit> getTrainUnits() {
		return this.trainUnits;
	}*/

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
		if(numPlayers == bandits.size()) {
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
			b.get(0).setPosition(this.train[1][0]);
			b.get(1).setPosition(this.train[1][1]);
		} else if (numOfBandit == 3) {
			b.get(0).setPosition(this.train[1][0]);
			b.get(1).setPosition(this.train[1][1]);
			b.get(2).setPosition(this.train[1][0]);
		} else if (numOfBandit == 4) {
			b.get(0).setPosition(this.train[1][0]);
			b.get(1).setPosition(this.train[1][1]);
			b.get(2).setPosition(this.train[1][0]);
			b.get(3).setPosition(this.train[1][1]);
		} else if (numOfBandit == 5) {
			b.get(0).setPosition(this.train[1][0]);
			b.get(1).setPosition(this.train[1][1]);
			b.get(2).setPosition(this.train[1][2]);
			b.get(3).setPosition(this.train[1][0]);
			b.get(4).setPosition(this.train[1][1]);
		} else if (numOfBandit == 6) {
			b.get(0).setPosition(this.train[1][0]);
			b.get(1).setPosition(this.train[1][1]);
			b.get(2).setPosition(this.train[1][2]);
			b.get(3).setPosition(this.train[1][0]);
			b.get(4).setPosition(this.train[1][1]);
			b.get(5).setPosition(this.train[1][2]);
		} else {return;}
	}
	
	//void chosenCharacter(int playerId, Character c) {
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
			//this.setCurrentRound(this.rounds.get(0));
			// set waiting for input to be true;
	}
	
	
	/**
	 * --EXECUTE ACTIONS--
	 * BEFORE CALLING ANY OF THESE METHODS, CURRENT BANDIT MUST BE ASSIGNED CORRECTLY
	 * All actions will be called from POV of this.currentBandit
	 */
	
	// SEND PROMPT
	// RECEIVE RESPONSE
	
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

	}
	
	public Bandit RoofShootPrompt(Bandit b, ArrayList<Bandit> ab) {
		// TO DO
		// ask b to choose one target from ab
		return ab.get(0);
	}
	
	public Bandit CarShootPrompt(Bandit b, ArrayList<Bandit> ab) {
		// TO DO
		// ask b to choose one target from ab
		return ab.get(0);
	}
	
	public void shoot() {

		ArrayList<Bandit> roofShootTarget = new ArrayList<Bandit>();
		ArrayList<Bandit> carShootTarget = new ArrayList<Bandit>();
		if (this.currentBandit.getPosition().carFloor == CarFloor.ROOF) {
			for (Bandit b : this.bandits) {
				if (b != this.currentBandit) {
					if (b.getPosition().carFloor == CarFloor.ROOF) {
						roofShootTarget.add(b);
					}
				}
			}

			if (roofShootTarget.get(0) != null) {
				// ask the current player to choose which bandit to shoot
				Bandit b = this.RoofShootPrompt(this.currentBandit, roofShootTarget);

				BulletCard bc = this.currentBandit.bullets.remove(0);
				if (bc != null) {
					b.addDiscardPile(bc);
				}
			}

		} else {
			for (Bandit bl : this.currentBandit.getPosition().getLeft().banditsHere) {
				carShootTarget.add(bl);
			}
			for (Bandit br : this.currentBandit.getPosition().getRight().banditsHere) {
				carShootTarget.add(br);
			}
			if (carShootTarget.get(0) != null) {
				// ask the current player to choose which bandit to shoot
				Bandit b = this.CarShootPrompt(this.currentBandit, carShootTarget);

				BulletCard bc = this.currentBandit.bullets.remove(0);
				if (bc != null) {
					b.addDiscardPile(bc);
				}
			}
		}

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

	}
	
	public void changeFloor() {
		TrainUnit currentPosition = this.currentBandit.getPosition();
		if(currentPosition.getAbove() == null && currentPosition.getBelow() != null) {
			currentPosition.removeBandit(currentBandit);
			currentPosition.getBelow().addBandit(currentBandit);
			currentBandit.setPosition(currentPosition.getBelow());
		}
		else if(currentPosition.getBelow() == null && currentPosition.getAbove() != null) {
			currentPosition.removeBandit(currentBandit);
			currentPosition.getAbove().addBandit(currentBandit);
			currentBandit.setPosition(currentPosition.getAbove());
		}
		//TODO FRONT END RESPONSE/SEND TO CLIENTS
	}
	
	public void move() {
		//TODO SEND PROMPT
		//TODO RECEIVE RESPONSE
	}
	
	public void moveMarshal() {
		//TODO SEND PROMPT
		//TODO RECEIVE RESPONSE
	}

	/**
     * @param c
     *           Card will be moved from bandit's hand to played pile and it's effect will be resolved
     *
     */
	public void playActionCard(ActionCard c){

	    //Remove card from bandit's hand
        this.currentBandit = c.getBelongsTo();
        this.currentBandit.removeHand(c);

        //Prompt playing face down
        if(currentBandit.getCharacter() == Character.GHOST && this.currentRound.getTurnCounter() == 0){
            //TODO: prompt choice;
        	//TODO: receive choice;
        }
        else if(this.currentRound.getCurrentTurn().getTurnType() == TurnType.TUNNEL) {
        	c.setFaceDown(true);
        }

        //Assign card to played pile
        PlayedPile pile = PlayedPile.getInstance();
        pile.addPlayedCards(c);
        //TODO: graphical response

    }

}
