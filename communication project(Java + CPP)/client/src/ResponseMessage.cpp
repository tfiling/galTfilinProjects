#include "ResponseMessage.h"

using namespace std;

ResponseMessage::ResponseMessage(string httpVersion, string code): _httpVersion(httpVersion), _code(code), _body(""){


}

void ResponseMessage::addHeader(string headerName, string headerValue){
	_headersMap.insert(pair <string, string>(headerName, headerValue));
	string stringHeader = headerName + ":" + headerValue;

}

void ResponseMessage::addBody(string body){
	_body += body;
	_body += "\n";
}
string ResponseMessage::getCode(){
	return this->_code;
}
string ResponseMessage::getHeaderValue(string header){
	return this->_headersMap.at(header);
}
string ResponseMessage::getBody(){
	return _body;
}

string ResponseMessage::toString(){
	string str = "\t--Message--\nCode: " + _code + "\n";
	str += 	_body;
	return str;
}
