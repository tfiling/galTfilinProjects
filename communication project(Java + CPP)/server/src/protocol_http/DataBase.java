package protocol_http;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.LinkedBlockingQueue;

public class DataBase {

	private	ConcurrentHashMap<String, User> userByCookie;
	private	ConcurrentHashMap<String, User> userByPhone; //for URI: add_user, remove_user
	private ConcurrentHashMap<String, GroupUsers> groupByGroupName;

	public DataBase(){
		this.userByCookie=new ConcurrentHashMap<String, User>();
		this.userByPhone=new ConcurrentHashMap<String, User>();
		this.groupByGroupName=new ConcurrentHashMap<String, GroupUsers>();
	}
	
	public String[] login(String userName, String phone)
	{
		if (userName.length() == 0 || phone.length() == 0)
		{
			String[] result = {"ERROR 765: Cannot login, missing parameters"};
			return result;
		}
		User newUser = new User(userName, phone);
		String cookie = this.generateCookie(userName, phone);
		this.userByCookie.put(cookie, newUser);
		this.userByPhone.put(phone, newUser);
		String[] result = {"Welcome " + userName + "@" + phone, cookie}; 
		return result;
	}

	//make sure to update in all of the data structures
	public String logout(String cookie) {
		User user = this.userByCookie.remove(extractCookieContent((cookie)));
		this.userByPhone.remove(user.getPhone());
		Collection<GroupUsers> groups = this.groupByGroupName.values();
		for (GroupUsers group : groups)
		{
			group.removeUserFromGroup(user);
		}
		return "Goodbye";
		
	}

	public String getUsersList() {

		//the map have to be != null because at least the user who asked exist in the list
		StringBuilder res=new StringBuilder();
		Iterator<User> userIt = (this.userByCookie.values()).iterator();
		while (userIt.hasNext())
		{
			User user = userIt.next();
			res.append( user.getName()+"@"+user.getPhone() );
			res.append(System.lineSeparator());
		}
		while (res.charAt(res.length() - 1)  ==  ' ')
		{
			res.deleteCharAt(res.length() - 1);
		}
		return res.toString(); 
	}

	public String getGroupsList() {

		StringBuilder res=new StringBuilder();
		Iterator<GroupUsers> groupsIt = (this.groupByGroupName.values()).iterator();
		while (groupsIt.hasNext())
		{
			GroupUsers group = groupsIt.next();
			res.append(group.getgroupName()+":" );
			ArrayList<User> currentGroupUsers = group.getUsersList();
			for (int i = 0; i < currentGroupUsers.size(); i++)
			{
				res.append(currentGroupUsers.get(i).getPhone() + ",");
			}
			res.deleteCharAt(res.length() - 1);//removes the additional unwanted , at the end of the group's users list
			res.append(System.lineSeparator());
		}		
		return res.toString(); 
	}

	public String getGroupList(String groupName) {
		StringBuilder res=new StringBuilder();
		protocol_http.GroupUsers reqGroup=this.groupByGroupName.get(groupName);
		if (reqGroup == null)
		{
			return "Error 274: requested group does not exists";
		}
		ArrayList<User> usersList=reqGroup.getUsersList();
		
		for(int i=0; i<usersList.size(); i++){
			res.append( usersList.get(i) .getPhone() + " , ");
		}
		res.deleteCharAt(res.length() - 1);
		res.deleteCharAt(res.length() - 1);//removes the additional unwanted , at the end of the group's users list
		return res.toString();
		
	}

	public String createGroup(String groupName, String[] groupMembers) {

		if (groupNameExist(groupName)){
			return "ERROR 511: Group Name Already Taken";
		}
		else{
			GroupUsers newGroup = new GroupUsers(groupName);
			for (String s : groupMembers)
			{
				User addedUser = this.userByPhone.get(s);
				if (addedUser == null)
				{
					return "ERROR 929: Unknown User " + s;
				}
				newGroup.addUserToGroup(addedUser);
			}
			this.groupByGroupName.put(groupName, newGroup);

		}
		return "Group " + groupName + " Created";
	}

	private boolean groupNameExist(String groupName) {
		return this.groupByGroupName.containsKey(groupName);
		
	}

	public String sendDirectMessage(String target, String content, String senderCookie) {
		User sender = this.userByCookie.get(extractCookieContent((senderCookie)));
		User reciever = this.userByPhone.get(target);
		if (reciever == null)
		{
			return "ERROR 771: Target Does not Exist";
		}
		reciever.sendMessage(sender.getPhone(), content);
		return "message sent";
	}

	public String sendGroupMessage(String target, String content, String senderCookie) {
		User sender = this.userByCookie.get(extractCookieContent((senderCookie)));
		GroupUsers group = this.groupByGroupName.get(target);
		if (group == null)
		{
			return "ERROR 771: Target Does not Exist";
		}
		ArrayList<User> groupUsers = group.getUsersList();
		for (User user : groupUsers)
		{
			user.sendMessage(sender.getPhone(), content);
		}
		return "message sent";
	}

	public String addUserToGroup(String groupName, String newMemberPhone, String adderCookie) {
		GroupUsers group = this.groupByGroupName.get(groupName);
		if (group == null)
		{
			return "ERROR 770: Target Does not Exist, couldnt find the specified goup";
		}
		User adderUser = this.userByCookie.get(extractCookieContent((adderCookie)));
		if (!group.contains(adderUser))
		{
			return "ERROR 669: Permission denied, you are not a member in this group";
		}
		User addedUser = this.userByPhone.get(newMemberPhone);
		if (addedUser == null)
		{
			return "ERROR 770: Target Does not Exist, could not find the specified user";
		}
		if (group.contains(addedUser))
		{
			return "ERROR 142: Cannot add user, user already in group";
		}
		group.addUserToGroup(addedUser);
		return newMemberPhone + " added to " + groupName;
	}

	public String removeUserFromGroup(String target, String removedUserPhone, String removingUserCookie) {
		GroupUsers group = this.groupByGroupName.get(target);
		if (group == null)
		{
			return "ERROR 769: Target does not exist, could not find this group";
		}
		User remover = this.userByCookie.get(extractCookieContent((removingUserCookie)));
		if (!group.contains(remover))
		{
			return "ERROR 668: Permission denied, you are not a member of this group";
		}
		User removedUser = this.userByPhone.get(removedUserPhone);
		if (!group.contains(removedUser))
		{
			return "ERROR 769: Target does not exist, specified user does not belong to this group";
		}
		group.removeUserFromGroup(removedUser);
		return removedUserPhone + " removed from " + target;
	}

	public String getMessages(String cookie) {
		User user = this.userByCookie.get(extractCookieContent((cookie)));
		LinkedBlockingQueue<String> messages = user.getUnReadMsg();
		StringBuffer result = new StringBuffer();
		if (messages.size() == 0)
		{
			result.append("No new messages");
		}
		else
		{
			result.append(messages.remove());
			while (!messages.isEmpty())
			{
				result.append(System.lineSeparator());
				result.append(messages.remove());
			}
		}
		return result.toString();
	}

	public boolean checkCookie(String cookie) {
		User user = this.userByCookie.get(extractCookieContent((cookie)));
		return !(user == null);
	}
	
	private String generateCookie(String userName, String phone)
	{
		return userName+"%"+phone;
	}
	
	private String extractCookieContent(String cookie)
	{
		return cookie.replace("user_auth=", "");
	}
	
	
}
