/*
*@author Hearen
*2015-3-31
*/

#pragma once
#include<Windows.h>

#include<WinSock.h>
#include<mysql.h>
#include<iostream>
#include<string>
#include"User.h"

#pragma comment(lib,"libmysql.lib") 

using namespace std;
const int BUFFER_SIZE = 1024;
class MyDB
{
private:
	char *host;
	char *user;
	char *pwd;
	char *db; //the database you need;
	unsigned int port; //server port;
	int user_id;//after login;
	MYSQL *mysql;//the handler of mysql;

	//initialize mysql;
	int init();

	string encode(string s);

	string decode(string s);

	//used to fetch categories and locations of the user frequently used;
	//column_name can location and category; precondition - user_id is known already;
	vector<string> fetch_elements(string column_name);

	//used to fetch all the items of the user: precondition - user_id is known already;
	vector<ToDoItem> fetch_items();

	///update all the titles, categoris and locations for the user - the user_id is known already;
	///delete them first and then insert all of the new ones;
	void update_elements(vector<string> v, string columnName);

	///quite similar to the update_elements but the element now is the complex one - to-do item;
	void update_items(vector<ToDoItem> itemsVector);

	///update the basic user profile;
	void update_user_info(User user);
public:
	//initialize mysql;
	MyDB() :host("localhost"), user("root"), pwd("520 Noodle"), db("willdo"), port(3306), user_id(0){ init(); }

	~MyDB(){ mysql_close(mysql); }

	//verify the user and set the user_id;
	//-2 error happened, -1 no such user, 1 wrong password, 0 success;
	//sete the user_id if there is a user;
	int verify_user(string name, string pwd);

	//according to the user name and password to get the whole data of the user;
	User retrieve_info(string name, string pwd);

	//add new user; just add another newie without any records;
	int add_user(string user_name, string pwd);

	//using the user to update the user in db;
	void update_user(User user);
};
