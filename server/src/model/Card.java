package model;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

public abstract class Card implements SerializableSFSType {
	
	public String belongsToAsString;
	
	//--EMPTY CONSTRUCTOR FOR SERIALIZATION--
	public Card() { }
	
    public String getBelongsTo() {
        return this.belongsToAsString;
    }

    public void setBelongsTo(String belongsTo) {
        this.belongsToAsString = belongsTo;
    }
}
