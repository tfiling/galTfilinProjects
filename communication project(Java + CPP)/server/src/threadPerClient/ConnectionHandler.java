package threadPerClient;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.HashMap;
import java.util.concurrent.atomic.AtomicBoolean;

import protocol.ServerProtocol;
import tokenizer.Tokenizer;
import tokenizer_http.RequestMessage;
import tokenizer_http.ResponseMessage;

public class ConnectionHandler<T> implements Runnable {

	private BufferedReader in;
	private PrintWriter out;
	private Socket clientSocket;
	private ServerProtocol<T> protocol;
	private Tokenizer<T> tokenizer;
	private AtomicBoolean serverAlive;

	public ConnectionHandler(Socket acceptedSocket, ServerProtocol<T> p, Tokenizer<T> t, AtomicBoolean serverAlive)
	{
		in = null;
		out = null;
		clientSocket = acceptedSocket;
		protocol = p;
		tokenizer = t;
		this.serverAlive = serverAlive;
		System.out.println("Accepted connection from client!");
		System.out.println("The client is from: " + acceptedSocket.getInetAddress() + ":" + acceptedSocket.getPort());
	}

	public void run()
	{

		T msg;

		try {
			initialize();
		}
		catch (IOException e) {
			System.out.println("Error in initializing I/O");
		}

		try {
			process();
		} 
		catch (IOException e) {
			System.out.println("Error in I/O");
		} 

		System.out.println("Connection closed - bye bye...");
		close();

	}

	public void process() throws IOException
	{
		T msg;

		while ((msg = tokenizer.nextMessage()) != null)
		{
			T response = protocol.processMessage(msg);
			if (response != null && !response.toString().contains("No new messages"))
			{
				System.out.println(response);	
			}
			if (response != null)
			{
				String s = response.toString();
				out.println(s);
			}

			if (protocol.isEnd(msg))
			{
				break;
			}
			else if (!this.serverAlive.get())
			{
				ResponseMessage endConnectionMessage = new ResponseMessage("HTTP/1.1", "404", new HashMap<String, String>(), "server is closed, goodbye");
				out.println(endConnectionMessage);
				break;
			}

		}
	}

	// Starts listening
	public void initialize() throws IOException
	{
		// Initialize I/O
		in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream(),"UTF-8"));
		out = new PrintWriter(new OutputStreamWriter(clientSocket.getOutputStream(),"UTF-8"), true);
		tokenizer.addInputStream(new InputStreamReader(clientSocket.getInputStream(),"UTF-8"));
		System.out.println("I/O initialized");
	}

	// Closes the connection
	public void close()
	{
		try {
			if (tokenizer.isAlive())//Handle this in tokenizer
			{
				in.close();
			}
			if (out != null)
			{
				out.close();
			}

			clientSocket.close();
		}
		catch (IOException e)
		{
			System.out.println("Exception in closing I/O");
		}
	}

}