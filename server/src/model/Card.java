package model;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

public abstract class Card implements SerializableSFSType {
	
	public String belongsTo;
	
	//--EMPTY CONSTRUCTOR FOR SERIALIZATION--
	public Card() { }
	
    public String getBelongsTo() {
        return this.belongsTo;
    }

    public void setBelongsTo(String belongsTo) {
        this.belongsTo = belongsTo;
    }
}
