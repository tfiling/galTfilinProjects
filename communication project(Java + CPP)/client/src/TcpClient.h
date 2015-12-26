
#ifndef TCPCLIENT_H_
#define TCPCLIENT_H_
#include <arpa/inet.h> //inet_address
#include <iostream>
#include <string>
using namespace std;
class TcpClient
{
private:
    string _address;
    int _port;
    int _socket;
	struct sockaddr_in _server;

public:
    TcpClient(string address, int port);
    bool connectToServer();
    bool send_data(string data);
	static const int READ_SIZE = 512;
    string receive();
	bool checkContainsDelimiter(char input[]);
};
#endif
