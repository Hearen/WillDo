package com.hearen.willdo.BasicClass;

import android.content.Context;
import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.BufferedReader;
import java.io.Console;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.Writer;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Hearen on 2015/4/14 014.
 */
public class TasksJsonSerializer{
    static private final String TAG = "TasksJsonSerializer";

    private Context mContext;
    private String mFileName;

    public TasksJsonSerializer(Context c, String f)
    {
        mContext = c;
        mFileName = f;
    }

    //save the User info to the local file in json format
    public void saveTasks(ArrayList<ToDoItem> toDoItems) throws JSONException, IOException
    {
        Writer writer = null;
        try
        {
            OutputStream out = mContext.openFileOutput(mFileName, Context.MODE_PRIVATE);
            writer = new OutputStreamWriter(out);
            writer.write(User.get(mContext).getJsonString());
        }
        finally {
            if(writer != null)
                writer.close();
        }
    }

    ///load the info from local file to initialize the User
    public void loadTasks() throws IOException, JSONException
    {
        BufferedReader reader = null;
        try
        {
            InputStream in = mContext.openFileInput(mFileName);
            reader = new BufferedReader(new InputStreamReader(in));

            StringBuilder jsonStringBuilder = new StringBuilder();
            String line = null;
            while((line = reader.readLine()) != null)
            {
                jsonStringBuilder.append(line);
            }

            User.get(mContext).initFromJsonString(jsonStringBuilder.toString());
        }
        finally {
            if(reader != null)
                reader.close();
        }
    }

    //parse the json string to the user info and save the info locally
    private void parseAndSaveJson(String json)
    {
        try
        {
            JSONTokener jsonParser = new JSONTokener(json);
            JSONObject rootObject = (JSONObject)jsonParser.nextValue();
            User user = User.get(this.mContext);
            user.setUser_id(rootObject.getInt("user_id"));
            user.setUser_name(rootObject.getString("user_name"));
            user.setPassword(rootObject.getString("password"));

            ArrayList<ToDoItem> todoList = new ArrayList<ToDoItem>();
            JSONArray todoItemsArray = rootObject.getJSONArray("allItemsList");
            for(int i = 0; i < todoItemsArray.length(); i++)
            {
                ToDoItem todoItem = new ToDoItem();
                JSONObject jsonObject = (JSONObject)todoItemsArray.get(i);
                todoItem.setId(jsonObject.getInt("ID"));//ToDo
                todoItem.setTitle(jsonObject.getString("Title"));
                todoItem.setDueDate(jsonObject.getString("Due"));
                todoItem.setCreate(jsonObject.getString("Created"));
                todoItem.setDetail(jsonObject.getString("Comment"));
                todoItem.setLocation(jsonObject.getString("Location"));
                todoItem.setResolved(jsonObject.getBoolean("IsCompleted"));
                todoList.add(todoItem);
            }

            //there are titleList, locationList, categoryList which can also be parsed here from server

            user.setToDoItems(todoList);
            saveTasks(todoList);
        }catch (Exception e)
        {
            Log.e(TAG, "JSON parsing failed!");
        }
    }
}
