package protocol_http;

import java.util.ArrayList;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

public class User {

	private String name;
	private String phone;
	private LinkedBlockingQueue<String> unReadMsg;
	
	public User(String name, String phone){
		this.name=name;
		this.phone=phone;
		this.unReadMsg=new LinkedBlockingQueue<String>();
	}
	
	public String getName(){
		return this.name;
	}

	public String getPhone(){
		return this.phone;
	}
	
	public LinkedBlockingQueue<String> getUnReadMsg(){
		LinkedBlockingQueue<String> tmp = this.unReadMsg;
		this.unReadMsg= new LinkedBlockingQueue<String>();
		return tmp;
	}
	
	@Override
	public 	boolean equals(Object otherUser)
	{
		User other = (User)otherUser;
		return (this.name.equals(other.name) && this.phone.equals(other.phone));
	}

	public void sendMessage(String senderPhone, String content) {
		this.unReadMsg.add("From:" + senderPhone + System.lineSeparator() +
							"Msg:" + content + System.lineSeparator());
	}
}
