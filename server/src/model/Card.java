package model;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

public abstract class Card implements SerializableSFSType {
	
	public String belongsToAsString;
	
	//--EMPTY CONSTRUCTOR FOR SERIALIZATION--
	public Card() { }
	
    public String getBelongsToAsString() {
        return this.belongsToAsString;
    }

    public void setBelongsToAsString(String belongsTo) {
        this.belongsToAsString = belongsTo;
    }
}
