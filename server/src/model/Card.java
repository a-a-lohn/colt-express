package model;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

public abstract class Card implements SerializableSFSType {
	
	public Bandit belongsTo;
	
	//--EMPTY CONSTRUCTOR FOR SERIALIZATION--
	public Card() { }
	
    public Bandit getBelongsTo() {
        return this.belongsTo;
    }

    public void setBelongsTo(Bandit belongsTo) {
        this.belongsTo = belongsTo;
    }
}
