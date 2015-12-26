package protocol_whatsapp;

import protocol.ServerProtocol;
import protocol.ServerProtocolFactory;
import protocol_http.DataBase;

public class WhatsAppProtocolFactory implements ServerProtocolFactory {
	
	private DataBase db;

	public WhatsAppProtocolFactory()
	{
		this.db = new DataBase();
	}
	
	@Override
	public ServerProtocol create() {
		return new WhatsAppProtocol(this.db);
	}

}
