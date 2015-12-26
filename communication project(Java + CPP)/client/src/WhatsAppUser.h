#ifndef WHATSAPPUSER_H_
#define WHATSAPPUSER_H_
#include "TcpClient.h"
#include "ResponseMessage.h"
#include <iostream>
#include <string>
#include <boost/thread/mutex.hpp>
#include <boost/thread/thread.hpp>
using namespace std;

class WhatsAppUser {
public:
	WhatsAppUser(string serverAddrss, int port);
	void connect();
private:
	string login(string input);
	string logout();
	string sendRequest();
	string sendRequest(string input);
	string sendUserMessage(string input);
	string sendGroupMessage(string input);
	string listAllUsers();
	string listAllGroups();
	string listGroup(string input);
	string addUserToGroup(string input);
	string removeUserFromGroup(string input);
	string createGroup(string input);
	void checkNewMessages();
	ResponseMessage parseMessage(string messageToParse);

	TcpClient *_client;
	string _cookie;
	bool loggedIn;
	boost::mutex locker;
};

#endif
