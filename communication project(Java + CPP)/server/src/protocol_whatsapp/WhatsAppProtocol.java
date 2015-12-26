package protocol_whatsapp;

import protocol.ServerProtocol;
import protocol_http.DataBase;
import protocol_http.HttpServerProtocol;
import tokenizer.Message;
import tokenizer_http.ResponseMessage;

public class WhatsAppProtocol implements ServerProtocol<Message> {
	
	private DataBase db;
	private HttpServerProtocol httpProtocol;
	
	
	public WhatsAppProtocol(DataBase db)
	{
		this.db = db;
		this.httpProtocol = new HttpServerProtocol(db);
	}

	@Override
	public Message processMessage(Message msg) {
		Message result = this.httpProtocol.processMessage(msg);
		return result;
	}

	@Override
	public boolean isEnd(Message msg) {
		return this.httpProtocol.isEnd(msg);
	}

}
