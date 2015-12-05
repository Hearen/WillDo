package com.hearen.willdo.Fragments;

import android.annotation.TargetApi;
import android.app.AlertDialog;
import android.app.ListFragment;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.view.ActionMode;
import android.view.ContextMenu;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.ArrayAdapter;
import android.widget.CheckBox;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Date;

import com.hearen.willdo.Activity.ItemPagerActivity;
import com.hearen.willdo.BasicClass.*;
import com.hearen.willdo.R;

/**
 * Created by Hearen on 2015/4/13 013.
 */
public class ItemListFragment extends ListFragment {
    private static final String TAG = "ItemListFragment";
    ArrayList<ToDoItem> mToDoItems;

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true); ///telling FragmentManager to register ancestral navigation feature;
        mToDoItems = User.get(getActivity()).getToDoItems();
        getActivity().setTitle(R.string.to_do_list_title);

        ItemAdapter adapter = new ItemAdapter(mToDoItems);
        setListAdapter(adapter);


        ///this method can be used in any place as you want it to be;
        //new SynchWithServer().execute();
    }

    @Override
    public void onPause()
    {
        super.onPause();
        User.get(getActivity()).saveTasks();
    }

    ///creating the layout for the current fragment and initialize deleting mode;
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup parent, Bundle savedInstanceState)
    {
        View v = super.onCreateView(inflater, parent, savedInstanceState);

        ///used to enter context operations model to delete or something like that;
        ///to make the context more human and friendly, we have to make the background of selected items outstanding and more obvious;
        ///using background_activated.xml and list_item.xml;
        ListView listView = (ListView)v.findViewById(android.R.id.list);
        listView.setChoiceMode(ListView.CHOICE_MODE_MULTIPLE_MODAL);
        listView.setMultiChoiceModeListener(new AbsListView.MultiChoiceModeListener() {
            @Override
            public void onItemCheckedStateChanged(ActionMode mode, int position, long id, boolean checked) {

            }

            @Override
            public boolean onCreateActionMode(ActionMode mode, Menu menu) {
                MenuInflater inflater = mode.getMenuInflater();
                inflater.inflate(R.menu.list_context, menu);
                return true;
            }

            @Override
            public boolean onPrepareActionMode(ActionMode mode, Menu menu) {
                return false;
            }

            @Override
            public boolean onActionItemClicked(ActionMode mode, MenuItem item) {
                switch (item.getItemId())
                {
                    case R.id.menu_item_delete:
                        deleteItems(mode);
                        return true;
                    default: return false;
                }

            }

            @Override
            public void onDestroyActionMode(ActionMode mode) {

            }
        });
        return v;
    }

    ///delete the selected items and refresh the whole list;
    private void deleteItems(final ActionMode mode)
    {
        new AlertDialog.Builder(getActivity())
                .setTitle("Delete the selected")
                .setMessage("Are you sure to delete all the selected tasks?")
                .setPositiveButton("Delete", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        ItemAdapter adapter = (ItemAdapter)getListAdapter();
                        User toDoList = User.get(getActivity());
                        for(int i = adapter.getCount() - 1; i >= 0; i--)
                        {
                            if(getListView().isItemChecked(i))
                            {
                                toDoList.deleteItem(adapter.getItem(i));
                            }
                        }
                        mode.finish();
                        adapter.notifyDataSetChanged();
                    }
                })
                .setNegativeButton("Cancel", null)
                .create()
                .show();
    }

    ///used to create optionsMenu new;
    @TargetApi(11)
    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater)
    {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.fragment_list, menu);
        /*
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
        {
            MenuItem searchItem = menu.findItem(R.id.menu_item_search);
            SearchView searchView = (SearchView)searchItem.getActionView();

            SearchManager searchManager = (SearchManager)getActivity().getSystemService(Context.SEARCH_SERVICE);
            ComponentName name = getActivity().getComponentName();
            SearchableInfo searchInfo = searchManager.getSearchableInfo(name);

            searchView.setSearchableInfo(searchInfo);
        }
        */
    }

    ///used to intialize the context delete menu;
    @Override
    public void onCreateContextMenu(ContextMenu menu, View v, ContextMenu.ContextMenuInfo menuInfo)
    {
        getActivity().getMenuInflater().inflate(R.menu.list_context, menu);
    }

    ///to handle the optionsMenu selected event;
    @Override
    public boolean onOptionsItemSelected(MenuItem item)
    {
        switch (item.getItemId())
        {
            /*case R.id.delete_item:
                Toast.makeText(getActivity(), "Long press to select the item to delete", Toast.LENGTH_LONG).show();
                return true;*/
            case R.id.list_item_new:
                ToDoItem todoItem = new ToDoItem();
                todoItem.setCreate(new Date(System.currentTimeMillis()).toString());
                User.get(getActivity()).addItem(todoItem);

                Intent i = new Intent(getActivity(), ItemPagerActivity.class);
                i.putExtra(ItemFragment.EXTRA_ITEM_ID, todoItem.getId());
                startActivity(i);
                return true;
            case R.id.list_synch:
                try {
                    //new SynchWithServer().execute();
                }catch (Exception e)
                {
                    Toast.makeText(getActivity(), "Synchronization failed, check your connection!", Toast.LENGTH_LONG);
                }
                return true;
            default: return super.onOptionsItemSelected(item);
        }
    }

    //make sure it's updated after deactivation and activation operations;
    @Override
    public void onResume()
    {
        super.onResume();
        ((ItemAdapter)getListAdapter()).notifyDataSetChanged();
    }

    public void onListItemClick(ListView l, View v, int position, long id)
    {
        ToDoItem item = ((ItemAdapter)getListAdapter()).getItem(position);
        //Log.d(TAG, item.getTitle() + " was clicked");
        Intent i = new Intent(getActivity(), ItemPagerActivity.class);
        i.putExtra(ItemFragment.EXTRA_ITEM_ID, item.getId());
        startActivity(i);
    }

    private class ItemAdapter extends ArrayAdapter<ToDoItem>
    {
        public ItemAdapter(ArrayList<ToDoItem> toDoItems)
        {
            super(getActivity(), 0, toDoItems);
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent)
        {
            if(convertView == null)
            {
                convertView = getActivity().getLayoutInflater()
                        .inflate(R.layout.list_item, null);
            }

            ToDoItem item = getItem(position);

            TextView titleTextView = (TextView) convertView.findViewById(R.id.list_item_titleTextView);
            titleTextView.setText(item.getTitle());
            TextView dateTextView = (TextView)convertView.findViewById(R.id.list_item_dateTextView);
            dateTextView.setText(item.getDueDate().toString());
            CheckBox solvedCheckBox = (CheckBox)convertView.findViewById(R.id.list_item_solvedCheckBox);
            solvedCheckBox.setChecked(item.isResolved());

            return convertView;
        }
    }

    /*
    ///used to run a synching thread background;
    ///we should excute it in a event handler like onCreate;
    private class SynchWithServer extends AsyncTask<Void, Void, ArrayList<ToDoItem>>
    {///the first parameter is used to specify the type the execute method will take;
        ///the second parameter is used to specify the type to notify the progress
        ///one more thing, AsyncTask can be cancelled;
        @Override
        protected ArrayList<ToDoItem> doInBackground(Void...params)
        {
            try
            {

            }catch (IOException ioe)
            {
                Log.e("", "");
            }
            return new Syncher().fetchItems();
        }

        @Override
        protected void onPostExecute(ArrayList<ToDoItem> items)
        {
            mToDoItems = items;
            ////refresh the list using adapter;
            ((ItemAdapter)getListAdapter()).notifyDataSetChanged();
        }
    }
    */
}
