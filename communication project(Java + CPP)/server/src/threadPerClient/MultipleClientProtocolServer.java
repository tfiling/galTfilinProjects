package threadPerClient;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.SocketTimeoutException;
import java.util.concurrent.atomic.AtomicBoolean;

import protocol.ServerProtocolFactory;
import protocol_whatsapp.WhatsAppProtocolFactory;
import tokenizer.TokenizerFactory;
import tokenizer_whatsaap.WhatsAppTokenizerFactory;

public class MultipleClientProtocolServer<T> implements Runnable {
	private ServerSocket serverSocket;
	private int listenPort;
	private ServerProtocolFactory<T> _protocolFactory;
	private TokenizerFactory<T> _tokenizerFactory;


	public MultipleClientProtocolServer(int port, ServerProtocolFactory<T> protocolFactory, TokenizerFactory<T> tokenizerFactory)
	{
		serverSocket = null;
		listenPort = port;
		_protocolFactory = protocolFactory;
		_tokenizerFactory = tokenizerFactory;
	}

	public void run()
	{
		BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
		AtomicBoolean serverAlive = new AtomicBoolean(true);
		try {
			serverSocket = new ServerSocket(listenPort);
			serverSocket.setSoTimeout(5000);
			System.out.println("Listening..." + serverSocket.getLocalPort());
		}
		catch (IOException e) {
			System.out.println("Cannot listen on port " + listenPort);
		}

		while (serverAlive.get())
		{
			try {
				ConnectionHandler newConnection = new ConnectionHandler(serverSocket.accept(), _protocolFactory.create(),_tokenizerFactory.create(), serverAlive);
				new Thread(newConnection).start();
			}
			catch (SocketTimeoutException e)
			{
				try{
					if(System.in.available() > 0)
					{
						String s = br.readLine(); 
						if(s.equals("exit"))
						{
							serverAlive.set(false);
						}
					}
				}catch (IOException e2)
				{
					e.printStackTrace();
				}
			}
			catch (IOException e)
			{
				System.out.println("Failed to accept on port " + listenPort);
			}
		}
		try {
			close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}


	// Closes the connection
	public void close() throws IOException
	{
		serverSocket.close();
	}

	public static void main(String[] args) throws IOException
	{
		// Get port
						
		int port = Integer.decode(args[0]).intValue();
		
			
		//MultipleClientProtocolServer server = new MultipleClientProtocolServer(port, new HttpProtocolFactory(), new HttpTokenizerFactory());
		MultipleClientProtocolServer server = new MultipleClientProtocolServer(port, new WhatsAppProtocolFactory(), new WhatsAppTokenizerFactory());
		Thread serverThread = new Thread(server);//the thread is operating the above run method
		serverThread.start();
		try {
			serverThread.join();
		}
		catch (InterruptedException e)
		{
			System.out.println("Server stopped");
		}



	}
}

