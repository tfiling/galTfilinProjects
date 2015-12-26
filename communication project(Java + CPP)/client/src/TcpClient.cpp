#include "TcpClient.h"
#include <iterator>
#include<iostream>    //cout
#include<string>  //string
#include<string.h>    //strlen
#include<stdio.h> //printf
#include<sys/socket.h> //socket
#include<sys/types.h>
#include<netinet/in.h>


using namespace std;
TcpClient::TcpClient(string address, int port):_address(address), _port(port), _socket(-1){}

/**
    Connect to a host on a certain port number
*/
bool TcpClient::connectToServer(){
    //create socket if it is not already created
    if(_socket == -1)
    {
        //Create socket
        _socket = socket(AF_INET , SOCK_STREAM , IPPROTO_TCP);
        if (_socket == -1)
        {
            perror("Could not create socket");
			return false;
        }
        cout<<"Socket created " <<endl;
    }

    _server.sin_addr.s_addr = inet_addr(_address.c_str() );
    _server.sin_family = AF_INET;
    _server.sin_port = htons(_port);
    //Connect to remote server
    cout << "trying to connect" << endl;
    if(connect(_socket , (struct sockaddr *)&_server , sizeof(_server)) < 0){
        perror("connect failed. Error");
        return false;
    }
    cout<<"Connected\n";
    return true;
}

/**
    Send data to the connected host
*/
bool TcpClient::send_data(string data)
{
    if( send(_socket , data.c_str() , strlen( data.c_str() ) , 0) < 0)
    {
        perror("Send failed : ");
        return false;
    }
    return true;
}

/**
    Receive data from the connected host
*/
string TcpClient::receive()
{

    string reply;
	char buffer[READ_SIZE];
	int recvResult;
    //Receive a reply from the server
	do
	{
		if ((recvResult = recv(_socket, buffer, READ_SIZE, 0))<0){
			puts("recv failed");
		}
		reply += buffer;
	} while (checkContainsDelimiter(buffer) == false);//reads until the delimiter(new line + $
	return reply;

}

bool TcpClient::checkContainsDelimiter(char input[])
{
	for (int i = 0; i < READ_SIZE - 1; i++)
	{
		if (input[i] == '\n' & input[i + 1] == '$')
			return true;
	}
	return false;
}

