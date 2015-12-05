package com.hearen.willdo.Activity;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentStatePagerAdapter;
import android.support.v4.view.ViewPager;

import java.util.ArrayList;
import java.util.UUID;
import com.hearen.willdo.BasicClass.*;
import com.hearen.willdo.Fragments.*;
import com.hearen.willdo.R;

/**
 * Created by Hearen on 2015/4/13 013.
 */
public class ItemPagerActivity extends FragmentActivity {
    private ViewPager mViewPager;///To make the fragment can be slide to the left and right using ViewPager
    private ArrayList<ToDoItem> mToDoItems;

    ///using ViewPager for sliding feature;
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        ///using ViewPager to slide
        mViewPager = new ViewPager(this);
        mViewPager.setId(R.id.viewPager);
        setContentView(mViewPager);


        mToDoItems = User.get(this).getToDoItems();

        FragmentManager fm = getSupportFragmentManager();
        mViewPager.setAdapter(new FragmentStatePagerAdapter(fm) {
            @Override
            public Fragment getItem(int i) {
                ToDoItem item = mToDoItems.get(i);
                return ItemFragment.newInstance(item.getId());///passing data by this method ensure further security;
            }

            @Override
            public int getCount() {
                return mToDoItems.size();
            }
        });

        mViewPager.setOnPageChangeListener(new ViewPager.OnPageChangeListener()
        {
            public void onPageScrollStateChanged(int state){}

            public void onPageScrolled(int pos, float posOffset, int posOffsetPixels){}

            public void onPageSelected(int pos)
            {
                ToDoItem item = mToDoItems.get(pos);
                if(item.getTitle() != null)
                {
                    setTitle(item.getTitle());
                }
            }
        });

        int itemId = (int)getIntent().getSerializableExtra(ItemFragment.EXTRA_ITEM_ID);
        for(int i = 0; i < mToDoItems.size(); i++)
        {
            if(mToDoItems.get(i).getId() == itemId)
            {
                mViewPager.setCurrentItem(i);
                break;
            }
        }

        //////using ViewPager to slide
    }
}

