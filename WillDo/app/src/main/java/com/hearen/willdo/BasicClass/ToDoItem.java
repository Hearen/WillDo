package com.hearen.willdo.BasicClass;

import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.UUID;
/**
 * Created by Hearen on 2015/5/25 0025.
 */
public class ToDoItem {
    private static final String TAG = "ToDoItem";

    private static final String JSON_ID = "ID";
    private static final String JSON_TITLE = "Title";
    private static  final String JSON_SOLVED = "IsComplete";
    private static final String JSON_DUE = "Due";
    private static final String JSON_LOCATION = "Location";
    private static final String JSON_DETAIL = "Comment";
    private static final String JSON_CREATE = "Created";
    private static final String JSON_PRIORITY = "Priority";
    private static final String JSON_COLOR = "Color";
    private static final String JSON_PROGRESS = "Progress";
    private static final String JSON_ISDELETED = "IsDeleted";
    private static final String JSON_LAST_MODIFIED = "LastModified";
    private static final String JSON_CATEGORY = "Category";

    ///try to use the same way, consistent with Windows Client;//ToDo
    private int mId;///this won't work, working with the Windows Client which use stored max_item_id to identify the unique item_id;
    private String mTitle;
    private Date mDue;
    private boolean mResolved;
    private String mLocation;
    private String mCategory;
    private String mDetail;
    private Date mCreate;
    private int mPriority;
    private String mColor;
    private int mProgress;
    private boolean mDeleted;
    private Date mLastModified;

    public ToDoItem()
    {
        mId = User.getUNID();
        mTitle = "to-do item";
        mDetail = "";

        mPriority = 5;
        mProgress = 0;
        mColor = "Black";
        mResolved = false;
        mDeleted = false;
        mCategory = "";
        mCreate = new Date();
        mDue = new Date();
        mLastModified = new Date();
        mLocation = "";
    }

    public ToDoItem(JSONObject json)
    {
        try
        {
            mId = json.getInt(JSON_ID);
            mTitle = json.getString(JSON_TITLE);
            mDetail = json.getString(JSON_DETAIL);
            mPriority = json.getInt(JSON_PRIORITY);
            mProgress = json.getInt(JSON_PROGRESS);
            mColor = json.getString(JSON_COLOR);
            mResolved = json.getBoolean(JSON_SOLVED);
            mCategory = json.getString(JSON_CATEGORY);
            mLocation = json.getString(JSON_LOCATION);
            mDue = new Date(json.getLong(JSON_DUE));
            mCreate = new Date(json.getLong(JSON_CREATE));
            mLastModified = new Date(json.getString(JSON_LAST_MODIFIED));
        }catch (JSONException e)
        {
            Log.e(TAG, "error trying to parse json string to initialize ToDoItem");
        }

    }

    public JSONObject toJSON()
    {
        try
        {
            JSONObject json = new JSONObject();
            json.put(JSON_ID, mId);
            json.put(JSON_TITLE, mTitle);
            json.put(JSON_DETAIL, mDetail);
            json.put(JSON_PRIORITY, mPriority);
            json.put(JSON_PROGRESS, mProgress);
            json.put(JSON_COLOR, mColor);
            json.put(JSON_SOLVED, mResolved);
            json.put(JSON_CATEGORY, mCategory);
            json.put(JSON_CREATE, mCreate.getTime());
            json.put(JSON_DUE, mDue.getTime());
            json.put(JSON_LAST_MODIFIED, mLastModified.getTime());
            json.put(JSON_LOCATION, mLocation);
            return json;
        }catch (JSONException e)
        {
            Log.e(TAG, "get the json string from a ToDoItem");
        }
        return null;
    }


    public int getId() {
        return mId;
    }

    public void setId(int id) {
        mId = id;
    }

    public String getTitle() {
        return mTitle;
    }

    public void setTitle(String title) {
        mTitle = title;
    }

    public Date getDueDate() {
        return mDue;
    }

    public void setDueDate(String dateString) {
        mDue = stringConvertToDate(dateString);
    }

    public boolean isResolved() {
        return mResolved;
    }

    public void setResolved(boolean resolved) {
        mResolved = resolved;
    }

    public String getLocation() {
        return mLocation;
    }

    public void setLocation(String location) {
        mLocation = location;
    }

    public String getDetail() {
        return mDetail;
    }

    public void setDetail(String detail) {
        mDetail = detail;
    }

    public Date getCreate() {
        return mCreate;
    }

    public void setCreate(String createTimeString) {
        mCreate = stringConvertToDate(createTimeString);
    }

    private Date stringConvertToDate(String dateString)
    {
        try
        {
            DateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
            Date myDate = dateFormat.parse("2010-09-13 22:36:01");
            return myDate;
        }catch (ParseException e)
        {
            Log.e(TAG, "date parsing failed!");
        }
        return null;
    }

    public int getmPriority() {
        return mPriority;
    }

    public void setmPriority(int mPriority) {
        this.mPriority = mPriority;
    }

    public String getmColor() {
        return mColor;
    }

    public void setmColor(String mColor) {
        this.mColor = mColor;
    }

    public int getmProgress() {
        return mProgress;
    }

    public void setmProgress(int mProgress) {
        this.mProgress = mProgress;
    }

    public boolean ismDeleted() {
        return mDeleted;
    }

    public void setmDeleted(boolean mDeleted) {
        this.mDeleted = mDeleted;
    }

    public Date getmLastModified() {
        return mLastModified;
    }

    public void setmLastModified(Date mLastModified) {
        this.mLastModified = mLastModified;
    }
}

