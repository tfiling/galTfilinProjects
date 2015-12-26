package protocol_http;

import java.awt.List;
import java.util.ArrayList;
import java.util.Vector;

public class GroupUsers {

	private String groupName;
	private ArrayList<User> listOfUsers;
	
	public GroupUsers(String groupName) {
		this.groupName=groupName;
		this.listOfUsers=new ArrayList<User>();
	}

	public String getgroupName(){
		return this.groupName;
	}
	
	public ArrayList<User> getUsersList()
	{
		ArrayList<User> result = new ArrayList<User>(this.listOfUsers); 
		return result;
	}

	public void addUserToGroup(User user){
		this.listOfUsers.add(user);
	}

	public void removeUserFromGroup(User user){
		this.listOfUsers.remove(user);
	}

	public boolean contains(User user) {
		return this.listOfUsers.contains(user);
	}
	
	public int getGroupSize()
	{
		return this.listOfUsers.size();
	}
	
	public String getUserList(){
		ArrayList<User> clonedUserList = (ArrayList<User>) this.listOfUsers.clone();
		StringBuilder result = new StringBuilder();
		for (int i = 0; i < clonedUserList.size() - 1; i++)
		{
			result.append(clonedUserList.get(i).getPhone());
			result.append(",");
		}
		result.append(clonedUserList.get(clonedUserList.size() - 1));
		return result.toString();
	}

}
