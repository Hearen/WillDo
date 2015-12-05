/*
*@author Hearen
*2015-3-31
*/
#pragma once
#include<vector>
#include"ToDoItem.h"

class User
{
private:
	int user_id;
	string user_name;
	string pwd;

	//frequently used info;
	vector<string> used_titles;
	vector<string> used_locations;
	vector<string> used_categories;

	vector<ToDoItem> items_vector;//store all the to-do items;

public:
	//default constructor;
	User() :user_id(0){};

	//dispose;
	~User()
	{
		this->used_categories.clear();
		this->used_locations.clear();
		this->items_vector.clear();
		this->used_titles.clear();
	}

	int get_user_id()
	{
		return user_id;
	}

	string get_user_name()
	{
		return user_name;
	}

	string get_password()
	{
		return pwd;
	}

	vector<ToDoItem> getItems()
	{
		return items_vector;
	}

	vector<string> getTitles()
	{
		return used_titles;
	}

	vector<string> getCategories()
	{
		return used_categories;
	}

	vector<string> getLocations()
	{
		return used_locations;
	}

	void set_id(int id)
	{
		this->user_id = id;
	}

	void set_name(string name)
	{
		this->user_name = name;
	}

	void set_password(string pwd)
	{
		this->pwd = pwd;
	}
	//set attributes for the user;
	void set_titles(vector<string> titles)
	{
		this->used_titles = titles;
	}
	void set_locations(vector<string> &used_locations)
	{
		this->used_locations = used_locations;
	}

	void set_categories(vector<string> &used_categories)
	{
		this->used_categories = used_categories;
	}

	void set_items(vector<ToDoItem> &items)
	{
		this->items_vector = items;
	}
};