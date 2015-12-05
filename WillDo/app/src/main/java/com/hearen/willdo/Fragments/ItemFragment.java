package com.hearen.willdo.Fragments;

import android.annotation.TargetApi;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.content.DialogInterface;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.NavUtils;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.Toast;

import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.UUID;
import com.hearen.willdo.BasicClass.*;
import com.hearen.willdo.R;

/**
 * Created by Hearen on 2015/4/13 013.
 */
public class ItemFragment extends Fragment {

    public static final String EXTRA_ITEM_ID = "com.example.hearen.willdo.item_id";

    private static final String DIALOG_DATE = "date";

    private static final int REQUEST_DATE = 0;

    private ToDoItem mToDoItem;
    private EditText mTitleField;
    private Button mDateButton;
    private EditText mDetailField;
    private CheckBox mSolvedCheckBox;

    private Button mDoneButton;
    private Button mLocationButton;

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);//ancestral navigation
        int itemId = (int)getArguments().getSerializable(EXTRA_ITEM_ID);
        mToDoItem = User.get(getActivity()).getToDoItem(itemId);
    }

    @TargetApi(11)
    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.menu_list, menu);
    }

    ///to accomplish the ancestral navigation event handler;
    @Override
    public boolean onOptionsItemSelected(MenuItem item)
    {
        switch (item.getItemId())
        {
            case android.R.id.home:///using finish() method may have same effect;
                checkTitle();
                return true;
            case R.id.delete_item:
                deleteItem();
                return true;
            default: return super.onOptionsItemSelected(item);
        }
    }

    ///using this method to encapsulate the current class to make more private and secure;
    public static ItemFragment newInstance(int itemId)
    {
        Bundle args = new Bundle();
        args.putSerializable(EXTRA_ITEM_ID, itemId);

        ItemFragment fragment = new ItemFragment();
        fragment.setArguments(args);

        return fragment;
    }


    ///this is the place where we create the UI;
    ///enable ancestral navigation;
    @TargetApi(11)
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
    {
        View v = inflater.inflate(R.layout.item_fragment, parent, false);///loading the configuration xml file;

        ///activate the ancestral navigation - hierarchical navigation
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
            if(NavUtils.getParentActivityName(getActivity()) != null)///it's more secure to check the existence of its parent activity;
                getActivity().getActionBar().setDisplayHomeAsUpEnabled(true);//we should add parent activity in manifest.xml also;

        mTitleField = (EditText)v.findViewById(R.id.item_title);
        mTitleField.setText(mToDoItem.getTitle());
        mTitleField.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                mToDoItem.setTitle(s.toString());
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });

        Date date = mToDoItem.getDueDate();
        final Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        mDateButton = (Button)v.findViewById(R.id.item_date);
        mDateButton.setText(mToDoItem.getDueDate().toString());
        mDateButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DatePickerDialog datePickerDialog = new DatePickerDialog(
                        getActivity(),
                        new DatePickerDialog.OnDateSetListener() {
                            @Override
                            public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                                Date tmpDate = new GregorianCalendar(year, monthOfYear, dayOfMonth).getTime();
                                mToDoItem.setDueDate(tmpDate.toString());//ToDo
                                mDateButton.setText(mToDoItem.getDueDate().toString());
                                /*Toast.makeText(getActivity(),
                                        String.valueOf(year) + "/" +
                                                String.valueOf(monthOfYear + 1) + "/" +
                                                String.valueOf(dayOfMonth),
                                        Toast.LENGTH_SHORT).show();*/
                            }
                        },
                        calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DAY_OF_MONTH));
                datePickerDialog.show();
            }
        });

        mLocationButton = (Button)v.findViewById(R.id.item_location);
        if(mToDoItem.getLocation().length() > 10)///this assertion is not good; but right now the basic address will have longer characters;
        {
            mLocationButton.setText(mToDoItem.getLocation());
            mLocationButton.setEnabled(false);
        }
        else
        {
            mLocationButton.setOnClickListener(new View.OnClickListener(){
                @Override
                public void onClick(View v) {
                    String location = Locator.getCurrentAddress(getActivity());
                    if(location == null)
                    {
                        Toast.makeText(getActivity(), "Make sure your GPS system or network connection work well.", Toast.LENGTH_LONG).show();
                        return;
                    }
                    mLocationButton.setText(location);
                    mToDoItem.setLocation(location);
                }
            });
        }

        //mDateButton.setEnabled(false);
        /*
        mDateButton.setOnClickListener(new View.OnClickListener(){
            public void onClick(View v)
            {
                FragmentManager fm = getActivity().getSupportFragmentManager();

                DatePickerFragment dialog = DatePickerFragment.newInstance(mToDoItem.getDate());
                dialog.setTargetFragment(ItemFragment.this,REQUEST_DATE);
                dialog.show(fm, DIALOG_DATE);
            }
        });
        */
        mDetailField = (EditText)v.findViewById(R.id.item_detail);
        mDetailField.setText(mToDoItem.getDetail());
        mDetailField.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                mToDoItem.setDetail(s.toString());
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });

        mSolvedCheckBox = (CheckBox)v.findViewById(R.id.item_solved);
        mSolvedCheckBox.setChecked(mToDoItem.isResolved());
        mSolvedCheckBox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                mToDoItem.setResolved(isChecked);
            }
        });

        mDoneButton = (Button)v.findViewById(R.id.done_button);
        mDoneButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                checkTitle();
            }
        });



        return v;
    }//end of onCreateView

    @Override
    public void onPause()
    {
        super.onPause();
        User.get(getActivity()).saveTasks();
    }

    ///deleting operation is handled here using alertDialog;
    private void deleteItem()
    {
        new AlertDialog.Builder(getActivity())
                .setTitle("Delete the current to-do task")
                .setMessage("Are you sure to delete current task")
                .setPositiveButton("Delete", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        if(NavUtils.getParentActivityName(getActivity()) != null)
                        {
                            NavUtils.navigateUpFromSameTask(getActivity());
                            User.get(getActivity()).deleteItem(mToDoItem);
                            Toast.makeText(getActivity(), "Task deleted", Toast.LENGTH_SHORT);
                        }
                    }
                })
                .setNegativeButton("Cancel", null)
                .create()
                .show();
    }

    ///when user tries to go back home or 'done' the current task but the title is empty;
    private void checkTitle()
    {
        if(mTitleField.getText().length() == 0) {
            new AlertDialog.Builder(getActivity())
                    .setTitle("Hint")
                    .setMessage("Stay to edit title or cancel to delete this task?")
                    .setPositiveButton("Edit", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            mTitleField.hasFocus();
                        }
                    })
                    .setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            if (NavUtils.getParentActivityName(getActivity()) != null) {
                                NavUtils.navigateUpFromSameTask(getActivity());
                                User.get(getActivity()).deleteItem(mToDoItem);
                            }
                        }
                    })
                    .create()
                    .show();
        }
        else
        {
            if (NavUtils.getParentActivityName(getActivity()) != null)
                NavUtils.navigateUpFromSameTask(getActivity());
        }
    }
    /*
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data)
    {
        if(resultCode != Activity.RESULT_OK) return;

        if(requestCode == REQUEST_DATE)
        {
            Date date = (Date)data.getSerializableExtra(DatePickerFragment.EXTRA_DATE);
            mToDoItem.setDate(date);
            mDateButton.setText(mToDoItem.getDate().toString());
        }
    }
    */
}

