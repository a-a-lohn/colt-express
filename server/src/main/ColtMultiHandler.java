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
	//ArrayList<Bandit> bandits = gm.bandits;
	//Bandit b = new Bandit(Character.CHEYENNE);
	
	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		
		String command = params.getUtfString(SFSExtension.MULTIHANDLER_REQUEST_ID);
		ISFSObject rtn = SFSObject.newInstance();
		
		if(command.equals("enterChooseCharacterScene")) {
			 handleEnterChooseCharacterScene(sender, params, rtn);
		}
		else if(command.equals("chosenCharacter")) {
			handleChosenCharacter(sender, params, rtn);
		}
		
		//ISFSArray banditsArray = SFSArray.newInstance();
		//banditsArray.addClass(b);
		//gameState.putSFSArray("banditsArr", banditsArray);

		//String received = params.getUtfString("sentData");
		//gameState.putUtfString("testStr", "evenbetterData");
		
	}
	
	
	
	private void updateGameState(ISFSObject rtn) {
		rtn.putClass("gm", gm);
		sendToAllUsers(rtn, "updateGameState");
	}	
	
	private void handleEnterChooseCharacterScene(User sender, ISFSObject params, ISFSObject rtn) {
		gm = GameManager.getInstance();
		ISFSArray characters = SFSArray.newInstance();
		List<String> characterStrings = Arrays.asList("GHOST", "DOC", "TUCO", "CHEYENNE", "BELLE", "DJANGO");
		rtn.putSFSArray("characterList", characters);
		List<Bandit> chosenBandits = gm.getBandits();
		List<String> chosenCharacters = new ArrayList<String>();
		for (Bandit b : chosenBandits) {
			chosenCharacters.add(b.getCharacter().toString());
		}
		for (String c : chosenCharacters) {
			characterStrings.remove(c);
		}
		for (String c : characterStrings) {
			characters.addUtfString(c);
		}
		//rtn.putUtfStringArray("characterList", Arrays.asList(Character.values().toString()));//remainingCharacters());
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
		if(numChosen == numPlayers) {
			rtn.putUtfStringArray("characterList", new ArrayList<String>());
		} else {
			//rtn.putUtfStringArray("characterList", remainingCharacters());
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
	
	/*private List<String> remainingCharacters() {
		List<Bandit> chosenBandits = gm.getBandits();
		List<String> chosenCharacters;
		for (Bandit b : chosenBandits) {
			chosenCharacters.add(b.getCharacter().toString());
		}
		List<String> charactersLeft = Arrays.asList(Character.values().toString());
		return charactersLeft.stream().filter(c -> !chosenCharacters.contains(c)).collect(Collectors.toList());
	}*/
	
}
