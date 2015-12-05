/*
Hearen 2015-04-22
*/
#pragma once

#include<WINSOCK2.H>
#include<iostream>
#include<stdlib.h>
#include<string>
#include<sstream>
#include<pthread.h>
#include "time.h"
#include<vector>
#include"MyDB.h"
#include "json.h"

//#pragma comment(lib,"jsoncpp.lib")
#pragma comment(lib,"WS2_32.lib")



#define PORT 8888
#define IP "192.168.8.88"
#define BUF_SIZE 1024 * 100 //this value cannot be too huge;



class Server
{
public:
	//in thie function, we handle the conflicting items;
	//after which storing and returning back the updated items to database and client;
	User getConsistentUser(User userFromClient);///the rule here: the more the better;


	///according to the json string, parse all the values of a user and return the user;
	User parseForWindowsClient(string jsonString);

	///when the item is in the vector according to the unique item_id, return index in vector otherwise return -1;
	///used in getConsistentUser;
	int itemIndexOfVecotr(ToDoItem item, vector<ToDoItem> itemsVector);

	///return the index of the string in sVector otherwise return -1;
	///used in getConsistentUser - using template to merge it with itemIndexOfVector;
	int stringIndexOfVector(string s, vector<string> sVector);

	string getJsonStringFromUser(User user);

	string getStringFromInt(int a);
};


int init();

DWORD WINAPI AnswerThread(LPVOID lparam);
