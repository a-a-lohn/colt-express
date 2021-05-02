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
	
	//fired
	public boolean getFired() {
		return this.fired;
	}
	public void setFired(boolean b) {
		this.fired = b;
	}
	
	//belongsToAsString
    public String getBelongsToAsString() {
        return this.belongsToAsString;
    }

    public void setBelongsToAsString(String belongsTo) {
        this.belongsToAsString = belongsTo;
    }
}
