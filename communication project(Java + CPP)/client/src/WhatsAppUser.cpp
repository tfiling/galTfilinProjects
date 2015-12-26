
#include "WhatsAppUser.h"
#include "TcpClient.h"
#include "ResponseMessage.h"
#include <vector>
#include <boost/algorithm/string.hpp>
#include <boost/thread.hpp>

using namespace std;
using namespace boost;

WhatsAppUser::WhatsAppUser(string serverAddress, int port): loggedIn(false){
	_client = new TcpClient(serverAddress, port);
}

void WhatsAppUser::checkNewMessages(){
	cout << "listening to messages" << endl;
	while(loggedIn){
		string messageToSend = "GET /queue.jsp HTTP/1.1\nCookie: " + _cookie + "\n\n$\n";//check for new messages
		string receivedMessage = "";

		locker.lock();
		_client->send_data(messageToSend);
		receivedMessage = _client->receive();
		if(receivedMessage != "recv failed"){
			ResponseMessage responseMessage(parseMessage(receivedMessage));
			string body = responseMessage.getBody();
			boost::trim(body);
			if(body != "No new messages"){
				cout << endl << "Received new message" << endl;
				cout << responseMessage.getBody()<< endl;
				if (body == "server is closed, goodbye")
				{
					exit(EXIT_SUCCESS);
				}
			}

		}
		locker.unlock();

		boost::posix_time::milliseconds myTime(1000);
		boost::this_thread::sleep(myTime);
	}
}

void WhatsAppUser::connect(){
	if(_client->connectToServer()){
		string status = "";
		while(status != "exit"){
			status = sendRequest();
		}
	}
	else{
		cout << "unable to connect to the server" << endl;
	}

}

string WhatsAppUser::sendRequest(){
	string request;
	cout<< "pleas insert your command\n";
	cout<< "Login [Username] [Phone]\n"
			"Send User [Phone] [Message]\n"
			"Send Group [Group Name] [Message]\n"
			"List Users\n"
			"List Groups\n"
			"List Group [Group Name]\n"
			"Add [Group Name] [Phone]\n"
			"Remove [Group Name] [Phone]\n"
			"CreateGroup [GroupName] [user1 phone],[user2 phone],…\n"
			"Logout\n"
			"Exit\n";
	getline(cin, request);
	cin.clear();
	return sendRequest(request);
}

string WhatsAppUser::logout(){
	string messageToSend = "GET /logout.jsp HTTP/1.1\nCookie: " + _cookie + "\n\n$\n";
	return messageToSend;
}
string WhatsAppUser::sendUserMessage(string input){//Send User [Phone] [Message]
	string phoneNumber, message;
	input = input.substr(10, string::npos);
	phoneNumber = input.substr(0, input.find(" "));
	message = input.substr(input.find(" ") + 1, string::npos);
	return ("POST /send.jsp HTTP/1.1\nCookie: " + _cookie + "\n\nType=Direct&Target=" + phoneNumber + "&Content=" + message + "\n$\n");

}
string WhatsAppUser::sendGroupMessage(string input){//Send Group [Group Name] [Message]
	input = input.substr(11, string::npos);
	string groupName = input.substr(0, input.find(" "));
	string message = input.substr(input.find(" ") + 1, string::npos);
	return ("POST /send.jsp HTTP/1.1\nCookie: " + _cookie + "\n\nType=Group&Target=" + groupName + "&Content=" + message + "\n$\n");
}
string WhatsAppUser::listAllUsers(){//List Users
	return "POST /list.jsp HTTP/1.1\nCookie: " + _cookie +"\n\nList=Users\n$\n";
}
string WhatsAppUser::listAllGroups(){//List Groups
	return "POST /list.jsp HTTP/1.1\nCookie: " + _cookie +"\n\nList=Groups\n$\n";
}
string WhatsAppUser::listGroup(string input){//List Group [Group Name]
	input = input.substr(11, string::npos);
	return  "POST /list.jsp HTTP/1.1\nCookie: " + _cookie +"\n\nList=Group&Group_Name=" + input +"\n$\n";
}
string WhatsAppUser::addUserToGroup(string input){//Add [Group Name] [Phone]
	input = input.substr(4, string::npos);
	string groupName = input.substr(0, input.find(" "));
	string phoneNumber = input.substr(input.find(" ") + 1, string::npos);
	return "POST /add_user.jsp HTTP/1.1\nCookie: " + _cookie +"\n\nTarget=" + groupName + "&User=" + phoneNumber +"\n$\n";
}
string WhatsAppUser::removeUserFromGroup(string input){//Remove [Group Name] [Phone]
	input = input.substr(7, string::npos);
	string groupName = input.substr(0, input.find(" "));
	string phoneNumber = input.substr(input.find(" ") + 1, string::npos);
	return "POST /remove_user.jsp HTTP/1.1\nCookie: " + _cookie +"\n\nTarget=" + groupName + "&User=" + phoneNumber +"\n$\n";
}

string WhatsAppUser::login(string input){//Login [Username] [Phone]
	input = input.substr(6, string::npos);
	string userName = input.substr(0, input.find(" "));
	string phone = input.substr(input.find(" ") + 1, string::npos);
	return "POST /login.jsp HTTP/1.1\n\nUserName=" + userName + "&Phone=" + phone+"\n$\n";
}
string WhatsAppUser::createGroup(string input){//CreateGroup [GroupName] [user1 phone],[user2 phone],...
	input = input.substr(12, string::npos);
	string groupName = input.substr(0, input.find(" "));
	input = input.substr(input.find(" ") + 1, string::npos);
	string users = input.substr(input.find(" ") + 1, string::npos);
	return "POST /create_group.jsp HTTP/1.1\nCookie: " + _cookie + "\n\nGroupName=" + groupName + "&Users=" + users +"\n$\n";
}

string WhatsAppUser::sendRequest(string input){
	string checkedString;
	checkedString = input.substr(0,5);
	bool commandDetected = false;
	string dataToSend = "";
	if (!commandDetected && checkedString == "Login")
	{
		dataToSend = login(input);
		commandDetected = true;
	}

	checkedString = input.substr(0,6);
	if (!commandDetected && checkedString == "Logout")
	{
		dataToSend = logout();
		commandDetected = true;
	}

	checkedString = input.substr(0, 10);
	if (!commandDetected && checkedString == "List Users")
	{
		dataToSend = listAllUsers();
		commandDetected = true;
	}

	checkedString = input.substr(0, 11);
	if (!commandDetected && checkedString == "List Groups")
	{
		dataToSend = listAllGroups();
		commandDetected = true;
	}

	checkedString = input.substr(0, 10);
	if (!commandDetected && checkedString == "List Group")
	{
		dataToSend = listGroup(input);
		commandDetected = true;
	}

	checkedString = input.substr(0, 10);
	if (!commandDetected && checkedString == "Send Group")
	{
		dataToSend = sendGroupMessage(input);
		commandDetected = true;
	}

	checkedString = input.substr(0, 9);
	if (!commandDetected && checkedString == "Send User")
	{
		dataToSend = sendUserMessage(input);
		commandDetected = true;
	}

	checkedString = input.substr(0, 3);
	if (!commandDetected && checkedString == "Add")
	{
		dataToSend = addUserToGroup(input);
		commandDetected = true;
	}

	checkedString = input.substr(0, 6);
	if (!commandDetected && checkedString == "Remove")
	{
		dataToSend = removeUserFromGroup(input);
		commandDetected = true;
	}

	checkedString = input.substr(0, 11);
	if (!commandDetected && checkedString == "CreateGroup")
	{
		dataToSend = createGroup(input);
		commandDetected = true;
	}

	checkedString = input.substr(0, 4);
	if (!commandDetected && checkedString == "Exit")
	{
		dataToSend = "GET /disconnect.jsp HTTP/1.1\n\n\n\n$\n";
		_client->send_data(dataToSend);// send msg to the server
		return "exit";
	}

	if (!commandDetected){
		cout<<"syntax ERROR !\n\n";
				return "error";
	}
	else{
		locker.lock();

		_client->send_data(dataToSend);// send msg to the server

		string responseString = "";
		while(responseString == ""){
			responseString = _client->receive();// wait till gets response from the server
		}
		ResponseMessage responseMessage = parseMessage(responseString);
		cout << responseMessage.toString() << endl;

		locker.unlock();

		if (responseMessage.getBody() == "server is closed, goodbye")
		{
			exit(EXIT_SUCCESS);
		}

		if(responseMessage.getCode() == "200"){
			//Logged in:
			if (input.substr(0, 5) == "Login"){
				_cookie = responseMessage.getHeaderValue("Set-Cookie");
				loggedIn = true;
				boost::thread listen(&WhatsAppUser::checkNewMessages, this);
				listen.detach();
			}
			if (input.substr(0, 6) == "Logout"){
				cout<< "Disconnected from server."<< endl;
				loggedIn = false;
			}
		}

		return "done";
	}
}


ResponseMessage WhatsAppUser::parseMessage(string messageToParse){
	messageToParse = messageToParse.substr(0, messageToParse.find('$') - 1);
	vector<string> messageByLines;
	split(messageByLines, messageToParse, is_any_of("\n"));

	size_t spaceLocation = messageByLines[0].find(' ');
	string httpVersion = messageByLines[0].substr(0, spaceLocation);
	string code = messageByLines[0].substr(spaceLocation + 1);
	ResponseMessage *response = new ResponseMessage(httpVersion, code);
	int i = 1;
	size_t seperator = (messageByLines[i].find(':'));
	while (seperator != string::npos & i < messageByLines.size())
	{
		string Header = messageByLines[i].substr(0, seperator);
		string HeaderValue = messageByLines[i].substr(seperator + 1);
		boost::trim(Header);
		boost::trim(HeaderValue);
		response->addHeader(Header, HeaderValue);
		i++;
		seperator = (messageByLines[i].find(':'));
	}
	i++;//skips the empty line

	while (i < messageByLines.size())
	{//inserts the remaining content to the message body
		response->addBody(messageByLines[i]);
		i++;
	}
	return *response;
}

