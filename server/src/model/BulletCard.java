package model;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

public class BulletCard extends Card implements SerializableSFSType {
	/**
	 * if fired == false then the bullet has not been fired ie. it is in the revolver of the bandit it belongs to
	 * else if fired == true then the bullet has been fired ie. it is in the card pile/hand of the bandit it belongs to
	 */
	public boolean fired;
	public String belongsToAsString;
	
	//--EMPTY CONSTRUCTOR FOR SERIALIZATION--
	public BulletCard() {}
	
	public BulletCard(String belongsTo) {
		this.belongsToAsString = belongsTo;
		this.fired = false;
	}
	
	public String getBelongsTo() {
        return this.belongsToAsString;
    }

    public void setBelongsTo(String belongsTo) {
        this.belongsToAsString = belongsTo;
    }
}
