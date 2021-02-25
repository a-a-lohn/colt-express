package main;

import java.util.List;
import main.Bandit;
import main.Loot;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class SerialHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User player, ISFSObject params) {

		Bandit test = new Bandit();
		Loot l = new Loot();
		l.setIsWhiskey(false);
		test.setGetsAnotherAction(true);
		test.setPlayedThisTurn(false);
		test.addLoot(l);
		ISFSObject rtn = new SFSObject();
		rtn.putClass("class", test);
		MyExt parentExt = (MyExt)getParentExtension();
		// can choose who to send the result of the server code back, giving the command a name, passing the
		// object and specifying the recipient
		Zone zone = parentExt.getParentZone();
		parentExt.send("serial", rtn, (List<User>) zone.getUserList());
	}

}
