package tokenizer_http;

import java.util.Map;

import tokenizer.Message;

public class ResponseMessage implements Message {
	public static final String OK = "200";
	public static final String FORBIDDEN = "403";
	public static final String NOT_FOUND = "404";
	public static final String METHOD_NOT_ALLOWED = "405";
	public static final String IM_A_TEAPOT = "418";
	
	private String version;
	private String statusCode;
	private Map<String, String> headers;
	private String body;
	
	public ResponseMessage(String version, String statusCode, Map<String, String> headers, String body)
	{
		this.version = version;
		this.statusCode = statusCode;
		this.headers = headers;
		this.body = body;
	}
	
	@Override
	public String toString()
	{
		StringBuilder str=new StringBuilder();
		str.append(version+" "+statusCode);
		str.append(System.lineSeparator());
		for(String header : headers.keySet()){
			str.append(header + ": " + this.headers.get(header));
			str.append(System.lineSeparator());
		}
		str.append(System.lineSeparator());
		str.append(body);
		str.append(System.lineSeparator());
		str.append("$");
		
		return str.toString();
		
	}

	@Override
	public String getMessageBody() {
		return body;
	}

	@Override
	public String getCookie() {
		return headers.get("cookie");
	}
	
	public int getStatusCode()
	{
		return Integer.parseInt(this.statusCode);
	}
	

}
