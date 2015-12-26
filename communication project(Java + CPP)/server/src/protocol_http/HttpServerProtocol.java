package protocol_http;

import java.util.HashMap;
import java.util.Map;

import protocol.ServerProtocol;
import tokenizer.Message;
import tokenizer_http.RequestMessage;
import tokenizer_http.ResponseMessage;

public class HttpServerProtocol implements ServerProtocol<Message<String>> {

	private DataBase db;
	private boolean connectionEnded;

	public HttpServerProtocol (DataBase db){
		this.db = db;
		this.connectionEnded = false;
	}

	@Override
	public Message<String> processMessage(Message<String> msg) {
		String uri = ((RequestMessage) msg).getUri();
		String messageBody = msg.getMessageBody();
		Message<String> response = null;
		
		if (uri.equals("/login.jsp"))
		{
			String[] variables = messageBody.split("=|&");	
			if (this.db.checkCookie(msg.getCookie()))
			{
				response = new ResponseMessage("HTTP/1.1", "403", new HashMap<String, String>(), "you already logged in!");
			}
			else if (variables.length == 4 && variables[0].equals("UserName") && variables[2].equals("Phone"))
			{
				String[] result = this.db.login(variables[1], variables[3]);
				if (result.length == 2)
				{
					HashMap<String, String> headers = new HashMap<String, String>();
					headers.put("Set-Cookie", "user_auth=" + result[1]);
					response = new ResponseMessage("HTTP/1.1", "200", headers, result[0]);

				}
				else
				{
					response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result[0]);

				}
			}
			else
			{
				response = new ResponseMessage("HTTP/1.1", "405", new HashMap<String, String>(), "ERROR 765: Cannot login, missing parameters");				
			}
			return response;
		}
		
		else if (this.db.checkCookie(msg.getCookie()))
		{
			switch(uri)
			{
			case "/logout.jsp":
			{
				String result = this.db.logout(msg.getCookie());
				response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result);
				return response;
			}

			case "/list.jsp":
			{
				String result;
				String[] variables = messageBody.split("=|&");
				if ((variables.length != 2 && variables.length != 4) || !(variables[0].equals("List")))
				{
					result = "ERROR 273: Missing Parameters";
				}
				else if (variables[1].equals("Users"))
				{
					result = this.db.getUsersList();
				}
				else if (variables[1].equals("Group") && variables[2].equals("Group_Name"))
				{
					result = this.db.getGroupList(variables[3]);
				}
				else if (variables[1].equals("Groups"))
				{
					result = this.db.getGroupsList();
				}
				else
				{
					result = "ERROR 273: Missing Parameters";
				}

				response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result);
				return response;
			}

			case "/create_group.jsp":
			{
				String result;
				String[] variables = messageBody.split("=|&");
				if (variables.length != 4 || !(variables[0].equals("GroupName")) || !(variables[2].equals("Users")) || 
						variables[1].length() == 0 || variables[3].length() == 0)
				{
					result = "ERROR 675: Cannot create group, missing parameters";
				}
				else
				{
					String[] groupMembers = variables[3].split(",");
					result = this.db.createGroup(variables[1], groupMembers);	
				}
				response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result);
				return response;
			}

			case "/send.jsp":
			{
				String[] variables = messageBody.split("=|&");
				String result;
				if (variables.length != 6)
				{
					result = "ERROR 711: Cannot send, missing parameters";
				}
				if (variables[1].equals("Group"))//we assume that the input is as described in description: type, target, content
				{
					result = this.db.sendGroupMessage(variables[3], variables[5], msg.getCookie());	
				}
				else if (variables[1].equals("Direct"))
				{
					result = this.db.sendDirectMessage(variables[3], variables[5], msg.getCookie());		
				}
				else
				{
					result = "ERROR 836: Invalid Type";
				}
				response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result);
				return response;
			}

			case "/add_user.jsp":
			{
				String[] variables = messageBody.split("=|&");
				String result;
				if (variables.length == 4 && variables[0].equals("Target") && variables[2].equals("User"))
				{
					result = this.db.addUserToGroup(variables[1], variables[3], msg.getCookie());
				}
				else if (variables.length == 4 && variables[0].equals("User") && variables[2].equals("target"))
				{
					result = this.db.addUserToGroup(variables[3], variables[1], msg.getCookie());
				}
				else
				{
					result = "ERROR 242: Cannot add user, missing parameters";
				}
				response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result);
				return response;
			}

			case "/remove_user.jsp":
			{
				String[] variables = messageBody.split("=|&");
				String result;
				if (variables.length == 4 && variables[0].equals("Target") && variables[2].equals("User"))
				{
					result = this.db.removeUserFromGroup(variables[1], variables[3], msg.getCookie());
				}
				else if (variables.length == 4 && variables[0].equals("User") && variables[2].equals("target"))
				{
					result = this.db.removeUserFromGroup(variables[3], variables[1], msg.getCookie());
				}
				else
				{
					result = "ERROR 336: Cannot remove, missing parameters";
				}
				response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result);
				return response;
			}

			case "/queue.jsp":
			{
				String result = this.db.getMessages(msg.getCookie());
				response = new ResponseMessage("HTTP/1.1", "200", new HashMap<String, String>(), result);
				break;
			}
			default:
			{
				response = new ResponseMessage("HTTP/1.1", "404", new HashMap<String, String>(), "could not recognize the requested uri");
				break;
			}
			}
			
		}
		else if (uri.equals("/disconnect.jsp"))
		{
			this.connectionEnded = true;
			response = null;//will make sure the connection handler will not send this message;
		}
		else
		{
			response = new ResponseMessage("HTTP/1.1", "403", new HashMap<String, String>(), "access denied, did you forget to login?");
		}
		return response;
	}

	@Override
	public boolean isEnd(Message msg) {
		return (this.connectionEnded || ((RequestMessage) msg).checkEndMessage() );
	}

}
