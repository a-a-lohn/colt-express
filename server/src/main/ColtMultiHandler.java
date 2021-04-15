package main;

import java.util.ArrayList;

import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.stream.Collectors;

import javax.management.MBeanServerInvocationHandler;

import model.Bandit;
import model.Character;
import model.GameManager;
import model.Horse;
import model.TrainUnit;

import com.smartfoxserver.v2.annotations.Instantiation;
import com.smartfoxserver.v2.annotations.MultiHandler;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.extensions.SFSExtension;
import com.smartfoxserver.v2.annotations.Instantiation.InstantiationMode;

@Instantiation(InstantiationMode.SINGLE_INSTANCE)
@MultiHandler
public class ColtMultiHandler extends BaseClientRequestHandler {
	
	GameManager gm;
	HashMap<String, GameManager> saveGames = new HashMap<String, GameManager>();
	boolean savedCurrentGameAtLeastOnce = false;
	String currentSaveGameId;
	int positionTurns = 0;
	//int trainIndex = 1;
	private static List<String> chosenCharactersSavedGame = new ArrayList<String>();
	private static ArrayList<Bandit> orderedBandits = new ArrayList<Bandit>();
	
	boolean gameOver = false;
	
	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		
		
		String command = params.getUtfString(SFSExtension.MULTIHANDLER_REQUEST_ID);
		ISFSObject rtn = SFSObject.newInstance();
		
		switch(command) {
		case("enterChooseCharacterScene"): handleEnterChooseCharacterScene(sender, params, rtn); break;
		case("chosenCharacter"):handleChosenCharacter(sender, params, rtn); break;
		case("testSerial"): testSerial(sender, rtn); break;
		case("newGameState"): handleNewGameState(params,rtn); break;
		case("enterGameBoardScene"): handleEnterGameBoardScene(sender, params,rtn); break;
		case("nextAction"): handleNextAction(params,rtn); break;
		case("saveGameState"): handleSaveGameState(params,rtn); break;
		case("loadSavedGame"): handleLoadSavedGame(params,rtn); break;
		case("deleteSavedGame"): handleDeleteSavedGame(params,rtn); break;
		case("removeGame"): handleRemoveGame(params,rtn); break;
		case("gameOver"): gameOver = true; break;
		case("choosePosition"): handleChoosePosition(sender, params, rtn); break;
		case("testgame"): handleTestGame(sender, rtn); break;
		default: trace("invalid command passed to multihandler");
		}
		
	}
	
	public void handleLoadSavedGame(ISFSObject params, ISFSObject rtn) {
		String currentSaveGameId = params.getUtfString("savegameId");
		System.out.println("id: " + currentSaveGameId);
		System.out.println("currently saved Games: "+ saveGames.containsKey(currentSaveGameId));
		gm = saveGames.get(currentSaveGameId);
		GameManager.singleton = gm;
		savedCurrentGameAtLeastOnce = true;
		
		rtn.putUtfString("savegameID", currentSaveGameId);		
		sendToAllUsers(rtn, "currentSaveGameID");
		
	}
	
	public void handleDeleteSavedGame(ISFSObject params, ISFSObject rtn) {

		String currentSaveGameId = params.getUtfString("savegameId");
		System.out.println("currently saved Games to be deleted: "+ currentSaveGameId + " : "+ saveGames.containsKey(currentSaveGameId));
		boolean result = saveGames.remove(currentSaveGameId, saveGames.get(currentSaveGameId));
		System.out.println("successfully removed saved Games: "+ result);
		
	}
	
	public void handleSaveGameState(ISFSObject params, ISFSObject rtn) {
		System.out.println("saving game");
		savedCurrentGameAtLeastOnce = true;
		currentSaveGameId = params.getUtfString("savegameId");
		GameManager saveGame = (GameManager) params.getClass("gm");
		
		if(saveGames.containsKey(currentSaveGameId)) {
			saveGames.replace(currentSaveGameId, saveGame);
		}else {
			saveGames.put(currentSaveGameId, saveGame);
		}
	
		rtn.putUtfString("savegameID", currentSaveGameId);
		System.out.println("currently saved Games: "+ saveGames.containsKey(currentSaveGameId));
		
		sendToAllUsers(rtn, "currentSaveGameID");
		
	}
	
	public void handleRemoveGame(ISFSObject params, ISFSObject rtn) {
		System.out.println("removing game");
		if(savedCurrentGameAtLeastOnce && gameOver) {
			saveGames.remove(currentSaveGameId);
			// reset attributes
			gameOver = false;
		}
		savedCurrentGameAtLeastOnce = false;
		GameManager.singleton = null;
		chosenCharactersSavedGame.clear();
	}
	
	public void handleNextAction(ISFSObject params, ISFSObject rtn) {
		sendToAllUsers(params, "nextAction");
	}
	
	public void handleEnterGameBoardScene(User sender, ISFSObject params, ISFSObject rtn) {
		System.out.println("entering gb scene");
		gm = GameManager.getInstance();
//		ArrayList<Bandit> bandits = game.bandits;
//		assert(bandits.size() > 1);
//		
//		// for testing purposes - shouldn't be any current bandit at the start of game (horse attack)
		gm.currentBandit = new Bandit();
			//gm.currentBandit = gm.bandits.get(0);
		
		updateGameStateSenderOnly(sender, rtn);
	}
	
	public void handleNewGameState(ISFSObject params, ISFSObject rtn) {
		gm = (GameManager) params.getClass("gm");
		GameManager.singleton = gm;
		//GameManager.singleton = gm;
		System.out.println("received game state!");
		String log = (String) params.getUtfString("log");
		if(log != null) {
			System.out.println("Received log message: " + log);
			rtn.putUtfString("log", log);
		}
		//System.out.println(gm.bandits.get(0).position.carTypeAsString);
		updateGameState(rtn);
	}
	
	public void updateGameState(ISFSObject rtn) {
		rtn.putClass("gm", gm);
		System.out.println("Sending game state to all");
		//System.out.println("Current bandit: " + gm.currentBandit.characterAsString);
		sendToAllUsers(rtn, "updateGameState");
	}
	
	public void updateGameStateSenderOnly(User sender, ISFSObject rtn) {
		rtn.putClass("gm", gm);
		System.out.println("Sending game state to " + sender.getName());
		//System.out.println("Current bandit: " + gm.currentBandit.characterAsString);
		sendToSender(sender, rtn, "updateGameState");
	}	
	
	private void handleEnterChooseCharacterScene(User sender, ISFSObject params, ISFSObject rtn) {
		gm = GameManager.getInstance();
		ISFSArray characters = SFSArray.newInstance();
		rtn.putSFSArray("characterList", characters);
		
		List<String> characterStrings = remainingCharacters();
		
		for (String c : characterStrings) {
			characters.addUtfString(c);
		}
		sendToSender(sender, rtn, "remainingCharacters");
	}
	
	private void handleChosenCharacter(User sender, ISFSObject params, ISFSObject rtn) {
		gm =GameManager.getInstance();
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		int numPlayers = zone.getUserList().size();
		
		String chosen = params.getUtfString("chosenCharacter");
		Character character = null;
		try {
			character = Character.valueOf(chosen);
		} catch(IllegalArgumentException e) {
			System.out.println("Invalid character");
		}
		if(savedCurrentGameAtLeastOnce == true) {
			chosenCharactersSavedGame.add(chosen);
		}
		System.out.println("Calling chosenchar with " + character.toString());
		rtn.putUtfString("player", sender.getName());
		rtn.putUtfString("chosenCharacter", character.toString());
		if(!savedCurrentGameAtLeastOnce) {
			gm.chosenCharacter(sender, character, numPlayers);
		}
		int numChosen = gm.getBandits().size();
		System.out.println("Num players: " + numPlayers + " numChosen: " + numChosen);
		if((numChosen == numPlayers && !savedCurrentGameAtLeastOnce) | (chosenCharactersSavedGame.size() == numChosen)) {
			System.out.println("Sending back empty list");
			rtn.putUtfStringArray("characterList", new ArrayList<String>());
		} else {
			ISFSArray characters = SFSArray.newInstance();
			rtn.putSFSArray("characterList", characters);
			List<String> characterStrings = remainingCharacters();
			for (String c : characterStrings) {
				characters.addUtfString(c);
			}
		}
		System.out.println("First bandit: " + gm.bandits.get(0).characterAsString);
		sendToAllUsers(rtn, "remainingCharacters");
		
	}
	
	private void handleChoosePosition(User sender, ISFSObject params, ISFSObject rtn) {
		positionTurns += 1;
		gm = GameManager.getInstance();
		String ans = params.getUtfString("ans");
		if (ans.equals("y")) {
			Bandit curr = gm.banditmap.get(sender);
			TrainUnit tu = gm.trainCabin.get(gm.trainIndex);
			gm.banditPositions.put(curr.characterAsString, tu);
			orderedBandits.add(curr);
			for (Horse h: gm.horses) {
				if (h.riddenBy == curr)
					h.adjacentTo = tu;
			}
		}
		else if (ans.equals("n") && gm.trainIndex == 2) {
			Bandit curr = gm.banditmap.get(sender);
			TrainUnit tu = gm.trainCabin.get(1);
			gm.banditPositions.put(curr.characterAsString, tu);
			orderedBandits.add(curr);
			for (Horse h: gm.horses) {
				if (h.riddenBy == curr)
					h.adjacentTo = tu;
			}
		}
		if (gm.trainIndex == 1) {
			System.out.println("this should not be called");
			for (Bandit b: gm.bandits) {
				if (!gm.banditPositions.containsKey(b.characterAsString)) {
					gm.banditPositions.put(b.characterAsString, gm.trainCabin.get(1));
					orderedBandits.add(b);
				}
			}
		}
		int chosen = 0;
		TrainUnit current = gm.trainCabin.get(gm.trainIndex);
		TrainUnit last = gm.trainCabin.get(1);
		for (TrainUnit tu: gm.banditPositions.values()) {
			if (!tu.carTypeAsString.equals(current.carTypeAsString) && !tu.carTypeAsString.equals(last.carTypeAsString)) {
				chosen += 1;
			}
		}
		System.out.println("Num chosen: " + chosen);
		if (positionTurns == (gm.bandits.size()- chosen) && gm.bandits.size() > gm.banditPositions.size()) {
			if (gm.trainIndex > 0) {
			gm.trainIndex -= 1;
			}
			updateGameState(rtn);
			positionTurns = 0;
			System.out.println("first condition met");
		}
		else if (gm.bandits.size() == gm.banditPositions.size()) {
			System.out.println("second condition met");
			//note: currentBandit not being null is the trigger for game start
			if (gm.currentBandit.characterAsString != null) 
				return;
			ArrayList<Bandit> bd = new ArrayList<Bandit>();
//			Iterator it = gm.banditPositions.entrySet().iterator();
//			for(String s : orderedBandits) {
////				HashMap.Entry pair = (HashMap.Entry)it.next();
////				String name = (String)pair.getKey();
//				for (Bandit b: gm.bandits) {
//					if (b.characterAsString.equals(s))
//							bd.add(b);
//				}
//			}
			if(gm.bandits.size() == orderedBandits.size()) System.out.println("same size");
			gm.bandits = orderedBandits;
			gm.currentBandit = gm.bandits.get(0);
			orderedBandits = new ArrayList<Bandit>();
			updateGameState(rtn);
		}
	}
	
	
	private void sendToAllUsers(ISFSObject rtn, String response) {
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		System.out.println("before STAU");
		parentExt.send(response, rtn, (List<User>) zone.getUserList());
		System.out.println("after STAU");
	}
	
	private void sendToSender(User sender, ISFSObject rtn, String response) {
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		parentExt.send(response, rtn, sender);
	}
	
	private List<String> remainingCharacters() {
		
		List<String> allCharacters = Arrays.asList("GHOST", "DOC", "TUCO", "CHEYENNE", "BELLE", "DJANGO");
		List<String> characterStrings = new ArrayList<String>();
		List<Bandit> chosenBandits = gm.getBandits();
		List<String> chosenCharacters = new ArrayList<String>();
		
		for (Bandit b : chosenBandits) {
			System.out.println("Chosen: "+ b.characterAsString);
			chosenCharacters.add(b.characterAsString);
		}
		if(savedCurrentGameAtLeastOnce == false) {
			for (int i = 0; i < allCharacters.size(); i++) {
				if (!chosenCharacters.contains(allCharacters.get(i))) {
					characterStrings.add(allCharacters.get(i));
					System.out.println("Added: " + allCharacters.get(i));
				}
			}
		}else {
			
			for (String p: chosenCharactersSavedGame) {

			    if (chosenCharacters.contains(p)){
			    	chosenCharacters.remove(p);
			    }
			}
			for (String b: chosenCharacters) {
				characterStrings.add(b);
			}
		}
		
		return characterStrings;
	}
	
	private void testSerial(User sender, ISFSObject rtn) {
		GameManager.singleton = null;
		GameManager gm = GameManager.getInstance();
		Bandit b = new Bandit(Character.BELLE);
		Bandit c = new Bandit(Character.DJANGO);
		gm.bandits.add(b);
		gm.bandits.add(c);
		gm.initializeGame();
		rtn.putClass("gm", gm);
		sendToSender(sender, rtn, "testSerial");
	}
	
	
	private void handleTestGame(User sender, ISFSObject rtn) {
		GameManager gmTest = new GameManager();
		GameManager.singleton = gmTest;
		// note: the banditmap will only have one entry because it is being overwritten 3 times below,
		// but since it does not seem to be used this should be fine
		gmTest.chosenCharacter(sender, Character.BELLE, 3);
		gmTest.chosenCharacter(sender, Character.DOC, 3);
		gmTest.chosenCharacter(sender, Character.GHOST, 3);
		rtn.putClass("gm", gmTest);
		System.out.println("sending initialized test game");
		sendToSender(sender, rtn, "testgame");
		GameManager.singleton = null;
	}
	
}
