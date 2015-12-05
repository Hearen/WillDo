/*
*@author Hearen
*2015-3-31
*/

#include"MyDB.h"


//used to initialize the connection with mysql;
int MyDB::init()
{
	mysql = mysql_init(nullptr);
	mysql_real_connect(mysql, host, user, pwd, db, 0, nullptr, 0);
	if (mysql == nullptr)
	{
		cout << "connection failed" << endl;
		return -1;
	}
	mysql_query(mysql, "SET NAMES GBK"); //ensure the encoding will work right in console;
	return 0;
}

//the encoding and decoding process is all enclosed inside;
//given the user name and its password, return the result:
//-2 error happened, -1 no such user, 1 wrong password, 0 success;
//sete the user_id if there is a user;
int MyDB::verify_user(string name, string pwd)
{

	string query_string = "select pwd, user_id, user_name from users where user_name = \"" + name + "\";";
	mysql_query(mysql, query_string.c_str());

	//cout << query_string << endl;

	MYSQL_RES *resultset = mysql_store_result(mysql);
	if (resultset == nullptr)
		return -2;
	if (mysql_num_rows(resultset) == 0)//no rows returned;
		return -1;
	MYSQL_ROW row = mysql_fetch_row(resultset);//Fetches the next row from the result set
	if (row == nullptr)//returns NULL when there are no more rows to retrieve or if an error occurred.
		return -2;

	string pwd0 = row[0];//there must be just one row and only one field;
	this->user_id = atoi(row[1]);
	if (strcmp(name.c_str(), row[2]))//mysql ignore uppercase, so we have to check for ourselves;
		return -1;//do not exist;
	mysql_free_result(resultset);//after this operation, the row will not be useable;
	if (encode(pwd) == pwd0)//decoding and encoding should be used here;
		return 0;
	else
		return 1;
}

//if there is no element, the vector returned will be empty;
vector<string> MyDB::fetch_elements(string column_name)
{
	//withdraw all categories;
	MYSQL_RES *resultset;
	vector<string> elements;
	char query_string[BUFFER_SIZE];
	//using sprintf to convert integer to string;
	sprintf_s(query_string, sizeof(query_string), "select %s from used_%s where user_id = %d;",
		column_name.c_str(), column_name.c_str(), this->user_id);//using c_str instead of just a string object;

	cout << query_string << endl;

	mysql_query(mysql, query_string);
	resultset = mysql_store_result(mysql);
	if (resultset == nullptr)
		return elements;
	//int row_count = (int)mysql_num_rows(resultset);
	MYSQL_ROW row = mysql_fetch_row(resultset);
	while (row != nullptr)
	{
		elements.push_back(row[0]);
		row = mysql_fetch_row(resultset);
	}
	mysql_free_result(resultset);
	return elements;
}


//used to fetch all the items of the user: precondition - user_id is known already;
vector<ToDoItem> MyDB::fetch_items()
{
	//withdraw all categories;
	MYSQL_RES *resultset;
	vector<ToDoItem> items;
	char query_string[BUFFER_SIZE];
	//using sprintf to convert integer to string;
	sprintf_s(query_string, sizeof(query_string), "select * from to_do_items where user_id = %d;", this->user_id);
	mysql_query(mysql, query_string);
	resultset = mysql_store_result(mysql);
	if (resultset == nullptr)
		return items;

	//just for testing;
	//int row_count = mysql_num_rows(resultset);
	int field_count = mysql_num_fields(resultset);


	cout << "field count: " << field_count << endl;
	//test the fields attributes;
	MYSQL_FIELD *field;
	while ((field = mysql_fetch_field(resultset)))
	{
		cout << field->name << endl;
	}


	//fetching all the data from resultset and insert them into items - vector<ToDoItem>;
	MYSQL_ROW row = mysql_fetch_row(resultset);

	while (row != nullptr)
	{
		//new a bran-new to-do item and set its attributes;
		ToDoItem item;
		item.set_item_id(atoi(row[1]));
		item.set_user_id(atoi(row[2]));
		item.set_category(row[3]);
		item.set_subject(row[4]);
		item.set_priority(atoi(row[5]));
		item.set_created_time(row[6]);
		item.set_due_time(row[7]);
		item.set_last_modified(row[8]);
		item.set_isCompleted(row[9] == "0" ? false : true);
		item.set_isDeleted(row[10] == "0" ? false : true);
		item.set_progress(atoi(row[11]));
		item.set_location(row[12]);
		item.set_detail(row[13]);

		//push_back the item into items;
		items.push_back(item);
		row = mysql_fetch_row(resultset);
	}

	mysql_free_result(resultset);
	return items;
}

//according to the user name and password to get the whole data of the user;
User MyDB::retrieve_info(string name, string pwd)
{
	User user;
	MYSQL_RES *resultset = nullptr;
	if (verify_user(name, pwd) != 0)
		return User();

	//retriving categories;
	vector<string> categories = fetch_elements("category");
	//retrieving locations;
	vector<string> locations = fetch_elements("location");

	//retrieving items;
	vector<ToDoItem> items = fetch_items();

	user.set_categories(categories);
	user.set_locations(locations);
	user.set_items(items);
	return user;
}

void MyDB::update_elements(vector<string> v, string column_name)
{
	char query_string[BUFFER_SIZE];
	//using sprintf to convert integer to string;
	sprintf_s(query_string, sizeof(query_string), "delete from used_%s where user_id = %d;",
		column_name.c_str(), column_name.c_str(), this->user_id);//using c_str instead of just a string object;
	mysql_query(mysql, query_string);
	//cout << query_string << endl;

	//insert all the values in v to the selected table;
	for (int i = 0; i < v.size(); i++)
	{
		sprintf_s(query_string, sizeof(query_string), "insert into used_%s (user_id, %s) values(%d, %s);",
			column_name.c_str(), column_name.c_str(), this->user_id, v[i].c_str());
		mysql_query(mysql, query_string);
	}
}

void MyDB::update_items(vector<ToDoItem> itemsVector)
{
	char query_string[BUFFER_SIZE];
	//using sprintf to convert integer to string;
	///delete all the items of user_id;
	sprintf_s(query_string, sizeof(query_string), "delete from to_do_items where user_id = %d;", this->user_id);
	mysql_query(mysql, query_string);
	//cout << query_string << endl;

	//insert all the values in v to the selected table;
	ToDoItem item;
	for (int i = 0; i < itemsVector.size(); i++)
	{
		item = itemsVector[i];
		sprintf_s(query_string, sizeof(query_string), "insert into to_do_items (item_id, user_id, category,"
			"subject, priority, created_time, due_time, last_modified_time, is_completed, is_deleted, progress, location, details)"
			" values(%d, %d, %s, %s, %d, %s, %s, %s, %d, %d, %d, %s, %s);",
			item.get_item_id(), this->user_id, item.get_category(), item.get_subject(), item.get_priority(), item.get_created_time(),
			item.get_due_time(), item.get_last_modified(), item.get_isCompleted(), item.get_isDeleted(), item.get_progress(), item.get_location(), item.get_detail());
		mysql_query(mysql, query_string);
	}
}

void MyDB::update_user_info(User user)
{
	char query_string[BUFFER_SIZE];
	//using sprintf to convert integer to string;
	///delete all the items of user_id;
	sprintf_s(query_string, sizeof(query_string), "update users set user_name = %s, pwd = %s where user_id = %d;",
		user.get_user_name().c_str(), user.get_password().c_str(), this->user_id);
	mysql_query(mysql, query_string);
}

///update all the info of the user_id in database;
void MyDB::update_user(User user)
{
	update_user_info(user);
	update_elements(user.getCategories(), "category");
	update_elements(user.getTitles(), "title");
	update_elements(user.getLocations(), "location");
	update_items(user.getItems());
}

//currently, this is just forward and needless to transfer the encoded string;
//using modulus to shuffle given string; besides to make it more complicated, insert characters among them;
string MyDB::encode(string s)
{
	string result_string;
	//shuffle; 
	int index = 3; //used to track the next character used to generate a new one for the current;
	for (int i = 0; i < s.length(); i++, index++)
	{
		result_string += char((s[i] + s[index % s.length()]) / 2);
	}

	//insert another character into result_string;
	string final_string;
	for (int i = 0; i < result_string.length(); i++)
	{
		final_string += result_string[i];
		final_string += char((result_string[i] + s[index % s.length()]) / 2);
	}
	return final_string;
}

//add new user; just add another newie without any records;
//-1 already existed, -2 error; 0 success;
int MyDB::add_user(string user_name, string pwd)
{
	int user_existed = verify_user(user_name, encode(pwd));
	if (user_existed == -2)
		return -2;
	if (user_existed != -1)
		return -1;
	string query_string = "insert into users(user_name, pwd) values(\"" + user_name + "\", \"" + encode(pwd) + "\");";

	//just for testing;
	cout << query_string << endl;

	mysql_query(mysql, query_string.c_str());
	mysql_refresh(mysql, REFRESH_TABLES);
	cout << (long)mysql_affected_rows(mysql) << endl;
	if (mysql_affected_rows(mysql))
		return 0;
	return -2;
}