#include "WhatsAppUser.h"
#include <iostream>
#include <stdlib.h>     // atoi

using namespace std;
int main(int argc , char *argv[]){
	if (argc == 3)
	{
		cout << "connecting to" << argv[1] << "via port" << argv[2] << endl;
		WhatsAppUser *user = new WhatsAppUser(argv[1], atoi(argv[2]));
		user->connect();
	}
	else
	{
		cout << "missing arguments" << endl;
	}


	return 0;

}

