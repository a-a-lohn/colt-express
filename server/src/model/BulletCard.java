package model;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

public class BulletCard extends Card implements SerializableSFSType {

	public BulletCard() {
		/**
		 * if fired == false then the bullet has not been fired ie. it is in the revolver of the bandit it belongs to
		 * else if fired == true then the bullet has been fired ie. it is in the card pile/hand of the bandit it belongs to
		 */
		boolean fired;
	}
}
