package tokenizer_http;

import java.util.Map;

import tokenizer.Message;

public abstract class RequestMessage implements Message<String>  {
	protected String uri;
	protected String version;
	protected Map<String, String> headers;
	protected String body;
	
	public static final String NO_COOKIE = "$";
	
	/**
	 * checks if there are no empty fields
	 * @return true if true
	 */
	public abstract boolean checkLegalMessage();
	
	public RequestMessage(String uri, String version, Map<String, String> headers, String body) 
	{
		this.uri = uri;
		this.version = version;
		this.headers = headers;
		this.body = body;
	}
	
	/**
	 * checks if this is a message indicating the end of communication
	 * @return true if this is an end communication message
	 */
	public boolean checkEndMessage()
	{
		if (this.uri.equals("/disconnect.jsp"))
			return true;
		else
			return false;
	}
	
	@Override
	public String getCookie() {
		String cookie = this.headers.get("Cookie");
		if (cookie == null)
		{
			return RequestMessage.NO_COOKIE;
		}
		else return cookie;
	}
	
	@Override
	public String getMessageBody() {
		return this.body;
	}
	
	public String getUri()
	{
		return this.uri;
	}
	
}
