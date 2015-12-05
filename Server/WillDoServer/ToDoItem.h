/*
*@author Hearen
*2015-3-31
*/

#pragma once
#include<string>
using namespace std;
/**
*
**/
class ToDoItem
{
private:

	int item_id;//unique id in client;
	int user_id;//a method to find its owner;
	int priority;
	int progress;


	bool isCompleted;
	bool isDeleted;
	string category;
	string subject;
	string color;
	string created_time;
	string due_time;
	string last_modified;
	string location;
	string detail;
public:
	//default initialization;
	ToDoItem();

	int get_item_id() { return item_id; }
	int get_user_id() { return user_id; }
	int get_priority() { return priority; }
	int get_progress() { return progress; }

	int get_isCompleted() { return isCompleted ? 1 : 0; }
	int get_isDeleted() { return isDeleted ? 1 : 0; }
	string get_color() { return color; }
	string get_category() { return category; }
	string get_subject() { return subject; }
	string get_created_time() { return created_time; }
	string get_due_time() { return due_time; }
	string get_last_modified() { return last_modified; }
	string get_location() { return location; }
	string get_detail() { return detail; }

	//set the basic attributes;
	void set_item_id(int item_id) { this->item_id = item_id; }
	void set_user_id(int user_id) { this->user_id = user_id; }
	void set_subject(string subject) { this->subject = subject; }
	void set_priority(int priority) { this->priority = priority; }
	void set_progress(int progress) { this->progress = progress; }
	void set_detail(string detail) { this->detail = detail; }
	void set_color(string color) { this->color = color; }
	void set_isCompleted(bool isCompleted) { this->isCompleted = isCompleted; }
	void set_isDeleted(bool isDeleted) { this->isDeleted = isDeleted; }
	void set_category(string category) { this->category = category; }

	void set_created_time(string created_time) { this->created_time = created_time; }
	void set_due_time(string due_time) { this->due_time = due_time; }
	void set_last_modified(string last_modified) { this->last_modified = last_modified; }
	void set_location(string location) { this->location = location; }


	//get the basic attributes;

};

//default initialization;
ToDoItem::ToDoItem()
{
	item_id = 0;
	user_id = 0;
	priority = 0;
	progress = 0;
	isCompleted = false;
}
