package tokenizer_http;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.HashMap;

import tokenizer.Tokenizer;

public class HttpTokenizer implements Tokenizer<RequestMessage> {
	public static final char DELIMITER = '$';
	private BufferedReader br;
	private boolean closed;
	
	
	public HttpTokenizer()
	{
		this.br = null;
		this.closed = false;
	}

	@Override
	public RequestMessage nextMessage() {
		String input;
		StringBuilder body = new StringBuilder();
		RequestMessage result = null;
		try {
		/******************reads first line*******************************************/
			input = this.br.readLine();
			String[] firstLine = input.split(" ");
			if (firstLine.length != 3)
			{
				throw new IllegalArgumentException();
			}
			String type = firstLine[0];
			String uri = firstLine[1];
			String version = firstLine[2];
		/******************reads second line with the headers*********************************/
			input = br.readLine();
			HashMap<String, String> headers = new HashMap<String, String>();
			String[] headerLine;
			while (!(input.trim()).isEmpty())
			{//reads the headers until the empty line that separates it from the message body
				headerLine = input.split(": ");
				if (headerLine.length != 2)
				{
					throw new IllegalArgumentException();
				}
				headers.put(headerLine[0], headerLine[1]);
				input = br.readLine();
			}
		/********************reads the message body*******************************************/
			while ((input = this.br.readLine()) != null && input.indexOf(DELIMITER) == -1)
			{
				body.append(input);
			}
			
			if (uri.equals("/logout.jsp") || uri.equals("/queue.jsp") || uri.equals("/disconnect.jsp"))
			{//this is a get request message
				result = new GetRequestMessage(uri, version, headers, body.toString());
			}
			else if (uri.equals("/login.jsp") || uri.equals("/list.jsp") ||
					 uri.equals("/create_group.jsp") || uri.equals("/send.jsp") ||
					 uri.equals("/add_user.jsp") || uri.equals("/remove_user.jsp"))
			{//this is a post request message 
				result = new PostRequestMessage(uri, version, headers, body.toString());
			}
			else
			{
				result = new PostRequestMessage(uri, version, headers, body.toString());//Didn't recognize the URI
			}
		} 
		catch (IOException | NullPointerException | IllegalArgumentException e) {
			this.closed = true;
			e.printStackTrace();
		}

		return result;
	}

	@Override
	public boolean isAlive() {
		return !this.closed;
	}

	@Override
	public void addInputStream(InputStreamReader inputStreamReader) {
		this.br = new BufferedReader(inputStreamReader);
	}

}
