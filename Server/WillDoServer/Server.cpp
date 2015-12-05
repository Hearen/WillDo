/*
Hearen 2015-04-23 12:08
trying to use the functions of a class, new this class inside - understand the relationship between main and child thread;
child thread can access all the content but if you intend to use its own values - new it in its own space;
*/
#include"Server.h"


int main()
{
	init();
}

///this simple function is used to start the server socket and accept the
///remote the connection and then start a new thread to handle the requests from clients;
int init()
{

	WSADATA wsd;
	SOCKET sServer;
	SOCKET sClient;
	int retVal;
	char buf[BUF_SIZE];

	//initialize the environment;
	if (WSAStartup(MAKEWORD(2, 2), &wsd) != 0)
	{
		printf("WSAStartup failed!\n");
		return 1;
	}
	//initialize the server socket;
	sServer = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (INVALID_SOCKET == sServer)
	{
		printf("socket failed!\n");
		WSACleanup();
		return -1;
	}
	//initialize the basic attributes for the server socket;
	SOCKADDR_IN addrServ;
	addrServ.sin_family = AF_INET;
	addrServ.sin_port = htons(8888);
	addrServ.sin_addr.S_un.S_addr = htonl(INADDR_ANY); //bind to a random ip
	//addrServ.sin_addr.S_un.S_addr = inet_addr(IP); //bind to a predefined ip

	//binding to the local ip and certain port;
	retVal = bind(sServer, (const struct sockaddr*)&addrServ, sizeof(SOCKADDR_IN));
	if (SOCKET_ERROR == retVal)
	{
		printf("bind failed!\n");
		closesocket(sServer);
		WSACleanup();
		return -1;
	}

	//start listenning;
	retVal = listen(sServer, 1);
	if (SOCKET_ERROR == retVal)
	{
		printf("listen failed!\n");
		closesocket(sServer);
		WSACleanup();
		return -1;
	}

	//set non-blocking mode;
	int iMode = 1;
	retVal = ioctlsocket(sServer, FIONBIO, (u_long FAR*) &iMode);
	if (retVal == SOCKET_ERROR)
	{
		printf("ioctlsocket failed!\n");
		WSACleanup();
		return -1;
	}

	//start accepting;
	printf("WillDo Server start working now...\n");
	sockaddr_in addrClient;
	int addrClientlen = sizeof(addrClient);
	while (true)
	{
		SOCKET sClient = accept(sServer, (sockaddr FAR*)&addrClient, &addrClientlen);
		if (INVALID_SOCKET == sClient)
		{
			int err = WSAGetLastError();
			if (err == WSAEWOULDBLOCK)
			{
				Sleep(5);
				continue;
			}
			else
			{
				printf("accept failed!\n");
				closesocket(sServer);
				WSACleanup();
				return -1;
			}
		}
		SYSTEMTIME st = { 0 };
		GetLocalTime(&st);
		char timeString[128];
		sprintf_s(timeString, sizeof(timeString), "%d-%02d-%02d %02d:%02d:%02d",
			st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute,st.wSecond);
		cout << "Accepted Client IP: " << inet_ntoa(addrClient.sin_addr) << " PORT: " << addrClient.sin_port << "\tTime: " <<timeString<< endl;
		//once connected, create a thread to handle the rest of it;
		DWORD dwThreadId;
		//DWORD WINAPI AnswerThread(LPVOID lparam);

		CreateThread(NULL, NULL, AnswerThread, (LPVOID)sClient, 0, &dwThreadId);
	}
	system("pause");
}

///the accepted socket is its parameter, use this socket to receive and send data;
///creating an object of class Server to handle the data process - parse, update and serialize;
///you cannot just create a object of Server outside and pass it into this function
///which will overwrite the content of the server and affect other threads;
DWORD WINAPI AnswerThread(LPVOID lparam)
{
	char buf[BUF_SIZE];
	int retVal;

	///get the accepted sClient from main thread;
	SOCKET sClient = (SOCKET)lparam;

	///if we want to use the functions of the Server, we have to new an object of its class inside the thread function;
	///the child thread will have its own space, we cannot use the parent's space to share an object of class Server;
	Server server = Server();

	//used to get the current time;
	SYSTEMTIME systemTime;
	GetLocalTime(&systemTime);

	string stringReceived = "";//to contain the complete content - a json string;

	//make sure that only the data received the first time will handle the '{' to get the header length;
	bool isJustConnected = true;
	//to ensure the completeness of the transfer process, using a length string before the json string
	//so before we actually handle the json string, we need to get the total length of the json string
	//by the header length in the front of the receiving string just before the first '{' after which 
	//we will handle all the left string as json string;

	
	int receivedLength = 0;

	while (true)
	{
		//receive data from sClient;
		if ((retVal = recv(sClient, buf, sizeof(buf), 0)) == SOCKET_ERROR)
		{
			if (SOCKET_ERROR == retVal)
			{
				int err = WSAGetLastError();
				if (err == WSAEWOULDBLOCK)
				{
					Sleep(100);
					continue;
				}
				else if (err == WSAETIMEDOUT || err == WSAENETDOWN)
				{
					cout << "recv failed " << endl;
					closesocket(sClient);
					return -1;
				}
			}
			if (retVal <= 0)
				return -1;
		}
		buf[retVal] = '\0';//using the byte array to form a string;

		stringReceived.append(buf);
		if (isJustConnected)///only the first '{' will be crucial;
		{
			int startPos = 0;//used to index the first '{' to get the right header length;

			///json string of an object will be started with a left brace bracket;
			///to find the header added length, we need to find the position of the first '{';
			startPos = stringReceived.find_first_of("{");
			if (startPos > 0 && ((receivedLength = atoi(stringReceived.substr(0, startPos).c_str())) != 0))
			{
				//cout << stringReceived.substr(0, startPos).c_str() << endl;
				isJustConnected = false;///only handle the data received the first time;
				stringReceived = stringReceived.substr(startPos);///substract the length header;
			}
			else//error;
				return -1;
		}
		if (int(stringReceived.length()) == receivedLength)
			break;
	}

	///according to the json string, get the user from client;
	User user = server.parseForWindowsClient(stringReceived);

	///according to the client user, withdraw all its data from database and handle the conflicting items;
	User user2 = server.getConsistentUser(user);

	MyDB myDB;

	///using the updated user info to update the database;
	myDB.update_user(user2);

	cout << "database updated for " << user.get_user_name() << endl;

	///get the json string via the updated user;
	string stringForSend = server.getJsonStringFromUser(user2);

	///add the total length before the json string;
	stringForSend = server.getStringFromInt(stringForSend.length()) + stringForSend;

	while (true)
	{
		///using the consistent data to form a formatted data which the client will be able to handle
		///using TCP protocol;
		retVal = send(sClient, stringForSend.c_str(), stringForSend.length(), 0);
		if (SOCKET_ERROR == retVal)
		{
			int err = WSAGetLastError();
			if (err == WSAEWOULDBLOCK)
			{
				Sleep(100);
				continue;
			}
			else
			{
				cout << "send failed" << endl;
				closesocket(sClient);
				return -1;
			}
		}
		break;
	}
	closesocket(sClient);
}

string Server::getStringFromInt(int a)
{
	stringstream ss;
	ss << a;
	return ss.str();
}

///according to the json string, parse all the values of a user and return the user;
User Server::parseForWindowsClient(string jsonString)
{
	User user;
	
	vector<string> titleVector;
	vector<string> locationVector;
	vector<string> categoryVector;
	vector<ToDoItem> itemsVector;
	Json::Reader reader;
	Json::Value root;
	reader.parse(jsonString, root);

	user.set_id(root["user_id"].asInt());
	user.set_name(root["user_name"].asString());
	user.set_password(root["password"].asString());

	///get the most frequenctly used titles, locations and categories;
	Json::Value titlesValue = root["titleList"];
	for (int i = 0; i < titlesValue.size(); i++)
	{
		titleVector.push_back(titlesValue[i].asString());
	}

	Json::Value locationValue = root["locationList"];
	for (int i = 0; i < locationValue.size(); i++)
	{
		locationVector.push_back(locationValue[i].asString());
	}

	Json::Value categoryValue = root["categoryList"];
	for (int i = 0; i < categoryValue.size(); i++)
	{
		categoryVector.push_back(categoryValue[i].asString());
	}

	///get all the to-do items in allItemsList;
	Json::Value items = root["allItemsList"];
	for (int i = 0; i < items.size(); i++)
	{
		ToDoItem item;
		item.set_item_id(items[i]["ID"].asInt());
		item.set_subject(items[i]["Title"].asString());
		item.set_detail(items[i]["Comment"].asString());
		item.set_priority(items[i]["Priority"].asInt());
		item.set_progress(items[i]["Progress"].asInt());
		item.set_color(items[i]["Color"].asString());
		item.set_isCompleted(items[i]["IsComplete"].asBool());
		item.set_isDeleted(items[i]["IsDeleted"].asBool());
		item.set_category(items[i]["Category"].asString());
		item.set_created_time(items[i]["Created"].asString());
		item.set_due_time(items[i]["Due"].asString());
		item.set_last_modified(items[i]["LastModified"].asString());
		item.set_location(items[i]["Location"].asString());
		itemsVector.push_back(item);
	}
	user.set_titles(titleVector);
	user.set_categories(categoryVector);
	user.set_locations(locationVector);
	user.set_items(itemsVector);

	
	return user;
}

string Server::getJsonStringFromUser(User user)
{
	
	Json::FastWriter fastWriter;
	Json::Value root;
	root["user_id"] = user.get_user_id();
	root["user_name"] = user.get_user_name();
	root["password"] = user.get_password();

	Json::Value titleList;
	for (int i = 0; i < user.getTitles().size(); i++)
		titleList.append(user.getTitles()[i]);///an array;
	root["titleList"] = titleList;//a value;

	Json::Value locationList;
	for (int i = 0; i < user.getLocations().size(); i++)
		locationList.append(user.getLocations()[i]);
	root["locationList"] = locationList;

	Json::Value categoryList;
	for (int i = 0; i < user.getCategories().size(); i++)
		categoryList.append(user.getCategories()[i]);
	root["categoryList"] = categoryList;

	Json::Value allItemsList;
	vector<ToDoItem> itemsVector = user.getItems();
	for (int i = 0; i < itemsVector.size(); i++)
	{
		Json::Value itemValue;
		itemValue["ID"] = itemsVector[i].get_item_id();
		itemValue["Title"] = itemsVector[i].get_subject();
		itemValue["Comment"] = itemsVector[i].get_detail();
		itemValue["Color"] = itemsVector[i].get_color();
		itemValue["Priority"] = itemsVector[i].get_priority();
		itemValue["Progress"] = itemsVector[i].get_progress();
		itemValue["IsComplete"] = itemsVector[i].get_isCompleted();
		itemValue["IsDeleted"] = itemsVector[i].get_isDeleted();
		itemValue["Category"] = itemsVector[i].get_category();
		itemValue["Location"] = itemsVector[i].get_location();
		itemValue["Created"] = itemsVector[i].get_created_time();
		itemValue["Due"] = itemsVector[i].get_due_time();
		itemValue["LastModified"] = itemsVector[i].get_last_modified();

		allItemsList.append(itemValue);
	}
	root["allItemsList"] = allItemsList;
	
	return fastWriter.write(root);
	
	return "";
}

//in thie function, we handle the conflicting items;
//after which storing and returning back the updated items to database and client;
User Server::getConsistentUser(User userFromClient)///the rule here: the more the better;
{//once two items are the same, select the last modified;
	MyDB myDB = MyDB();
	User userFromDB = myDB.retrieve_info("Hearen", "Noodle");
	User user2;

	///set the basic consistent values for user2;
	user2.set_id(userFromDB.get_user_id());
	user2.set_password(userFromDB.get_password());
	user2.set_name(userFromDB.get_user_name());

	vector<string> titles = userFromClient.getTitles();
	vector<string> categories = userFromClient.getCategories();
	vector<string> locations = userFromClient.getLocations();

	//reset the most frequently used titles, locations and categories;
	//the more the better;
	for (int i = 0; i < userFromDB.getTitles().size(); i++)
	{
		if (stringIndexOfVector(userFromDB.getTitles()[i], titles) == -1)///only add different ones;
			titles.push_back(userFromDB.getTitles()[i]);
	}
	user2.set_titles(titles);


	for (int i = 0; i < userFromDB.getCategories().size(); i++)
	{
		if (stringIndexOfVector(userFromDB.getCategories()[i], categories) == -1)///only add different ones;
			categories.push_back(userFromDB.getCategories()[i]);
	}
	user2.set_categories(categories);


	for (int i = 0; i < userFromDB.getLocations().size(); i++)
	{
		if (stringIndexOfVector(userFromDB.getLocations()[i], locations) == -1)///only add different ones;
			locations.push_back(userFromDB.getLocations()[i]);
	}
	user2.set_locations(locations);
	//update the to-do items according to the DB and clients;
	vector<ToDoItem> itemsFromClient = userFromClient.getItems();
	vector<ToDoItem> itemsFromDB = userFromDB.getItems();
	vector<ToDoItem> itemsVector;

	//copy all the items from database to itemsVector;
	for each (ToDoItem item in itemsFromDB)
	{
		itemsVector.push_back(item);
	}

	//using last modified attribute to determine the final version of itemsVector;
	for (int i = 0; i < itemsFromClient.size(); i++)
	{
		int index = itemIndexOfVecotr(itemsFromClient[i], itemsVector);
		if (index != -1)///there is a same unique id in itemsVector;
		{
			//using string to compare two date;
			///the item in itemsVector is modified before that in itemsFromClient;
			if (itemsVector[index].get_last_modified().compare(itemsFromClient[i].get_last_modified()) < 0)
				itemsVector[index] = itemsFromClient[i];
		}
		else///add the extra to-do item in itemsVector - the more the better;
		{
			itemsVector.push_back(itemsFromClient[i]);
		}
	}//the more the better; get all the latest to-do items;

	user2.set_items(itemsVector);
	return user2;
}


///when the item is in the vector according to the unique item_id, return index in vector otherwise return -1;
///used in getConsistentUser;
int Server::itemIndexOfVecotr(ToDoItem item, vector<ToDoItem> itemsVector)
{
	for (int i = 0; i < itemsVector.size(); i++)
	{
		if (itemsVector[i].get_item_id() == item.get_item_id())
			return i;
	}
	return -1;
}

int Server::stringIndexOfVector(string s, vector<string> sVector)
{
	for (int i = 0; i < sVector.size(); i++)
	{
		if (s == sVector[i])
			return i;
	}
	return -1;
}
