package main;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

import model.Bandit;
import model.Character;
import model.GameManager;

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
	
	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		
		if(gm != null) {
			GameManager.setHandler(this);
		}
		
		String command = params.getUtfString(SFSExtension.MULTIHANDLER_REQUEST_ID);
		ISFSObject rtn = SFSObject.newInstance();
		
		if(command.equals("enterChooseCharacterScene")) {
			 handleEnterChooseCharacterScene(sender, params, rtn);
		}
		else if(command.equals("chosenCharacter")) {
			handleChosenCharacter(sender, params, rtn);
		} else if(command.equals("newGameState")) {
			handleNewGameState(params, rtn);
		}
		
		//ISFSArray banditsArray = SFSArray.newInstance();
		//banditsArray.addClass(b);
		//gameState.putSFSArray("banditsArr", banditsArray);

		//String received = params.getUtfString("sentData");
		//gameState.putUtfString("testStr", "evenbetterData");
		
	}
	
	public void handleNewGameState(ISFSObject params, ISFSObject rtn) {
		gm = (GameManager) params.getClass("gm");
		System.out.println("received game state");
		System.out.println(gm.bandits.get(0).position.carTypeAsString);
		updateGameState(rtn);
	}
	
	public void updateGameState(ISFSObject rtn) {
		rtn.putClass("gm", gm);
		sendToAllUsers(rtn, "updateGameState");
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
		
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		int numPlayers = zone.getUserList().size();
		
		String chosen = params.getUtfString("chosenCharacter");
		Character character = null;
		try {
			character = Character.valueOf(chosen);
		} catch(IllegalArgumentException e) {
			//TODO: handle error
		}
		gm.chosenCharacter(sender, character, numPlayers);
		
		int numChosen = gm.getBandits().size();
		System.out.println("Num players: " + numPlayers + " numChosen: " + numChosen);
		if(numChosen == numPlayers) {
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
		
		sendToAllUsers(rtn, "remainingCharacters");
	}
	
	
	private void sendToAllUsers(ISFSObject rtn, String response) {
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		parentExt.send(response, rtn, (List<User>) zone.getUserList());
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
			System.out.println("Chosen: "+ b.getCharacter().toString());
			chosenCharacters.add(b.getCharacter().toString());
		}
		for (int i = 0; i < allCharacters.size(); i++) {
			if (!chosenCharacters.contains(allCharacters.get(i))) {
				characterStrings.add(allCharacters.get(i));
				System.out.println("Added: " + allCharacters.get(i));
			}
		}
		return characterStrings;
	}
	
}
