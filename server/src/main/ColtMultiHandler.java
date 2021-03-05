package main;

import java.util.ArrayList;
import java.util.List;

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
	
	GameManager gm = new GameManager();
	//ArrayList<Bandit> bandits = gm.bandits;
	//Bandit b = new Bandit(Character.CHEYENNE);
	
	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		String command = params.getUtfString(SFSExtension.MULTIHANDLER_REQUEST_ID);
		
		ISFSObject gameState = SFSObject.newInstance();
		if(command.equals("chosenCharacter")) {
			gameState = handleChooseCharacter(sender, params, gameState);
		}
		
		//ISFSArray banditsArray = SFSArray.newInstance();
		//banditsArray.addClass(b);
		//gameState.putSFSArray("banditsArr", banditsArray);

		gameState.putClass("gm", gm);

		//String received = params.getUtfString("sentData");
		//gameState.putUtfString("testStr", "evenbetterData");
		
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		parentExt.send("updateGameState", gameState, (List<User>) zone.getUserList());
		
	}
	
	private ISFSObject handleChooseCharacter(User sender, ISFSObject params, ISFSObject rtn) {
		int size = gm.getBandits().size();
		if(size == 1) {
			rtn.putUtfString("testStr", "Already called!");
		} else {
			String chosen = params.getUtfString("character");
			Character character = null;
			try {
				character = Character.valueOf(chosen);
			} catch(IllegalArgumentException e) {
				//TODO: handle error
			}
			ColtExtension parentExt = (ColtExtension)getParentExtension();
			Zone zone = parentExt.getParentZone();
			int numPlayers = zone.getUserList().size();
			
			gm.chosenCharacter(sender, character, numPlayers);
		}
		return rtn;
	}
	
}
