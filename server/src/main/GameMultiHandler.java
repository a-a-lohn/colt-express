package main;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;


import com.smartfoxserver.v2.annotations.Instantiation;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.extensions.SFSExtension;

import model.*;
import model.Character;

import com.smartfoxserver.v2.annotations.Instantiation.InstantiationMode;


public class GameMultiHandler extends BaseClientRequestHandler
{
	public void handleClientRequest(User sender, ISFSObject params) {
		String command = params.getUtfString((SFSExtension.MULTIHANDLER_REQUEST_ID));
		
		if (command.equals("chooseBandit")) {
			handleChooseBandit(sender, params);
		}
		else if (command.equals("begin")) {
			handleBegin();
		}
		else if (command.equals("")) {
			trace("");
		}
	}
	
	public void handleBegin() {
		//assigning null variable to indicate bandits have not been chosen
		GameExtension parentExt = (GameExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		List<User> users = (List<User>)zone.getUserList();
		for (User user: users) {
			UserVariable assignedBandit = new SFSUserVariable("bandit", null);
			getApi().setUserVariables(user, Arrays.asList(assignedBandit));
		}
		GameManager gm = GameManager.getInstance();
		updateGameState(gm);
	}
	
	//"bandit" -> Bandit string that player chooses
	public void handleChooseBandit(User sender, ISFSObject params) {
		//execute choose bandit logic
		String strBandit = (String)params.getText("bandit");
		//create bandit from string info
		Character character = null;
		try {
			character = Character.valueOf(strBandit);
		} catch(IllegalArgumentException e) {
			//TODO: handle error
		}
		Bandit b = new Bandit(character);
		//assigning bandit to user variable
		List<UserVariable> uv = new ArrayList<UserVariable>();
		UserVariable assignedBandit = new SFSUserVariable("bandit", b);
		uv.add(assignedBandit);
		getApi().setUserVariables(sender, uv);
		//updating gamemanager bandit list
		GameManager gm = GameManager.getInstance();
		gm.bandits.add(b);
		//get all users and check if they are assigned
		GameExtension parentExt = (GameExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		List<User> users = (List<User>)zone.getUserList();
		boolean done = true;
		ISFSArray chosen = new SFSArray();
		for (User user: users) {
			Bandit a = (Bandit)user.getVariable("bandit");
			if (a == null) {
				done = false;
			}
			else {
				chosen.addUtfString(a.strBanditName);
			}
		}
		if (done==true) {
			initializeGame();
		}
		else {
			//if choosing bandit phase still ongoing then send this info back
			ISFSObject gameState = SFSObject.newInstance();
			gameState.putSFSArray("chosen", chosen);
			parentExt.send("chosenBandits", gameState, users);
		}
			
		
	}
	
	public void initializeGame() {
		//TODO: implement
		//method for creating all non-bandit game objects and sending them to client
	}
	
	public void updateGameState(GameManager gm) {
		//TODO: implement
	}
}