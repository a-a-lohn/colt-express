package main;

import java.util.List;

import com.smartfoxserver.v2.annotations.Instantiation;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.annotations.Instantiation.InstantiationMode;

@Instantiation(InstantiationMode.SINGLE_INSTANCE)
//@MultiHandler
public class ColtMultiHandler extends BaseClientRequestHandler {
	
	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		//String command = params.getUtfString(SFSExtension.MULTIHANDLER_REQUEST_ID);
		
		ISFSObject gameState = SFSObject.newInstance();
		
//		if(true/*command.equals("test")*/) {
//			bandits.add(b);
//			//ISFSArray banditsArray = SFSArray.newInstance();
//			gameState.putClass("bandits", bandits);
//		}
		//String received = params.getUtfString("sentData");
		gameState.putUtfString("testStr", "someData");
		
		ColtExtension parentExt = (ColtExtension)getParentExtension();
		Zone zone = parentExt.getParentZone();
		//parentExt.send("updateGameState", gameState, (List<User>) zone.getUserList());
		parentExt.send("updateGameState", gameState, sender);
		
	}
}
