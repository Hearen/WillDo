package com.hearen.willdo.BasicClass;

import android.content.Context;
import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.lang.reflect.Array;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.UUID;
/**
 * Created by Hearen on 2015/5/25 0025.
 */
public class User {

    private static final String TAG = "User";

    private static final String FILENAME = "user.json";

    private static final int BASELINE = 10000;

    ///used as json tags;
    private static final String USER_ID = "user_id";
    private static final String USER_NAME = "user_name";
    private static final String PASSWORD = "password";
    private static final String TITLE_LIST = "titleList";
    private static final String LOCATION_LIST = "locationList";
    private static final String CATEGORY_LIST = "categoryList";
    private static final String TASKS_LIST = "allItemsList";

    private static User sUser;

    private int user_id;

    private String user_name;

    private String password;

    private List<String> titleList;

    private List<String> categoryList;

    private List<String> locationList;

    private TasksJsonSerializer mTasksJsonSerializer;

    private Context mAppContext;

    static private ArrayList<ToDoItem> mToDoItems;

    private User(Context appContext) {
        mAppContext = appContext;
        //mToDoItems = new ArrayList<ToDoItem>();
        mTasksJsonSerializer = new TasksJsonSerializer(mAppContext, FILENAME);
        try {
            mTasksJsonSerializer.loadTasks();
        } catch (Exception e) {
            mToDoItems = new ArrayList<ToDoItem>();
            Log.e(TAG, "Error loading tasks");
        }

        Log.e(TAG, "");
        ///for testing;
        if(mToDoItems == null)
            mToDoItems = new ArrayList<ToDoItem>();
        if (mToDoItems.size() == 0)
            for (int i = 0; i < 5; i++) {
                ToDoItem item = new ToDoItem();
                item.setResolved(i % 2 == 0);
                item.setTitle("to-do item # " + i);
                mToDoItems.add(item);
            }
    }

    public static User get(Context c) {
        if (sUser == null) {
            sUser = new User(c.getApplicationContext());
        }
        return sUser;
    }

    ///this should be further designed
    //ToDo
    public static User getExistedUser()
    {
        return sUser;
    }

    public boolean saveTasks()
    {
        try
        {
            mTasksJsonSerializer.saveTasks(mToDoItems);
            Log.d(TAG, "Tasks saved to json file");
            return true;
        }catch(Exception e)
        {///using toast or dialog to prompt the user of the failure;
            Log.e(TAG, "Error saving tasks");//ToDo!
            return false;
        }
    }

    public ArrayList<ToDoItem> getToDoItems()
    {
        return mToDoItems;
    }

    public void setToDoItems(ArrayList<ToDoItem> list)
    {
        mToDoItems = list;
    }
    ///get the to-do item by unique id in mToDoItems;
    public ToDoItem getToDoItem(int id)
    {
        for(ToDoItem item: mToDoItems)
        {
            if(item.getId() == id)
            {
                return item;
            }
        }
        return null;
    }


    public void addItem(ToDoItem item)
    {
        mToDoItems.add(item);
    }

    public void deleteItem(ToDoItem item)
    {
        mToDoItems.remove(item);
    }

    public int getUser_id() {
        return user_id;
    }

    public void setUser_id(int user_id) {
        this.user_id = user_id;
    }

    public String getUser_name() {
        return user_name;
    }

    public void setUser_name(String user_name) {
        this.user_name = user_name;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public List<String> getTitleList() {
        return titleList;
    }

    public void setTitleList(List<String> titleList) {
        this.titleList = titleList;
    }

    public List<String> getCategoryList() {
        return categoryList;
    }

    public void setCategoryList(List<String> categoryList) {
        this.categoryList = categoryList;
    }

    public List<String> getLocationList() {
        return locationList;
    }

    public void setLocationList(List<String> locationList) {
        this.locationList = locationList;
    }

    //get the json string according to the current info of the user
    public String getJsonString()
    {
        JSONObject root = new JSONObject();
        try
        {
            root.put(USER_ID, user_id);
            root.put(USER_NAME, user_name);
            root.put(PASSWORD, password);

            JSONArray titleArray = new JSONArray();
            //TODO can be edited further to add more features for the application
            root.put(TITLE_LIST, titleArray);

            JSONArray locationArray = new JSONArray();
            //TODO can be edited further to add more features for the application
            root.put(LOCATION_LIST, locationArray);

            JSONArray categoryArray = new JSONArray();
            //TODO can be edited further to add more features for the application
            root.put(CATEGORY_LIST, categoryArray);

            JSONArray taskArray = new JSONArray();
            for(ToDoItem item : mToDoItems)
            {
                taskArray.put(item.toJSON());
            }
            root.put(TASKS_LIST, taskArray);
        }
        catch (JSONException e)
        {
            Log.e(TAG, "getJsonString error!");
        }

        return root.toString();
    }

    ///initialize the User by a json string;
    public boolean initFromJsonString(String jsonString)
    {
        try
        {
            JSONObject root = (JSONObject)new JSONTokener(jsonString.toString()).nextValue();

            user_id = root.getInt(USER_ID);

            user_name = root.getString(USER_NAME);

            password = root.getString(PASSWORD);

            JSONArray titleArray = (JSONArray)root.getJSONArray(TITLE_LIST);
            for(int i = 0; i < titleArray.length(); i++)
                titleList.add(titleArray.get(i).toString());


            JSONArray categoryArray = (JSONArray)root.getJSONArray(CATEGORY_LIST);
            for(int i = 0; i < categoryArray.length(); i++)
                categoryList.add(categoryArray.get(i).toString());

            JSONArray locationArray = (JSONArray)root.getJSONArray(LOCATION_LIST);
            for(int i = 0; i < locationArray.length(); i++)
                locationList.add(locationArray.get(i).toString());

            JSONArray taskArray = (JSONArray) root.getJSONArray(TASKS_LIST);
            mToDoItems.clear();
            for(int i = 0; i < taskArray.length(); i++)
                mToDoItems.add(new ToDoItem(taskArray.getJSONObject(i)));
        }catch(JSONException e)
        {
            Log.e(TAG, "initialize user from json string failed!");
            return false;
        }
        return true;
    }

    public static int getUNID()
    {
        int i = 0, id = 0;
        while(true)
        {
            id = (int)(Math.random()*Integer.MAX_VALUE + BASELINE);
            for(i = 0; i < mToDoItems.size(); i++)
                if(mToDoItems.get(i).getId() == id)
                    break;
            if(i == mToDoItems.size() - 1)///till now there is no id exists in mToDoItems
                return id;
        }
    }
}

