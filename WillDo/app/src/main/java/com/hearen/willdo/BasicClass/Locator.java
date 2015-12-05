package com.hearen.willdo.BasicClass;

import android.content.Context;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationManager;
import android.util.Log;

import com.google.android.maps.GeoPoint;

import java.util.List;
import java.util.Locale;

/**
 * Created by Hearen on 2015/4/16 016.
 */
public class Locator {
    static public final String TAG = "locator";

    static public String getCurrentAddress(Context context)
    {
        Location location = getLocation(context);
        if(location != null)
        {
            GeoPoint gp = getGeoByLocation(location);
            Address currentAddress = getAddressByGeoPoint(context, gp);
            return currentAddress.getCountryName() + ", " + currentAddress.getLocality();
        }
        return null;
    }

    static private Location getLocation(Context context)
    {
        LocationManager locationManager = (LocationManager)context.getSystemService(Context.LOCATION_SERVICE);

        //GPS_PROVIDER will provide us with more exact position via the closest AP;
        //but this method is not very quick compared with NETWORK_PROVIDER
        Location location = locationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
        if(location == null)
            location = locationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
        return location;
    }

    static private GeoPoint getGeoByLocation(Location location)
    {
        GeoPoint gp = null;
        try
        {
            if(location != null)
            {
                double geoLatitude = location.getLatitude() * 1E6;
                double geoLontitude = location.getLatitude() * 1E6;
                gp = new GeoPoint((int)geoLatitude, (int)geoLontitude);
            }
        }catch (Exception e)
        {
            Log.e(TAG, "getGeoByLocation fail");
        }
        return gp;
    }

    static private Address getAddressByGeoPoint(Context context, GeoPoint gp)
    {
        Address result = null;
        try
        {
            if(gp != null)
            {
                Geocoder gc = new Geocoder(context , Locale.CHINA);
                double geoLatitude = (int)gp.getLatitudeE6();
                double geoLontitude = (int)gp.getLongitudeE6();

                List<Address> addressList = gc.getFromLocation(geoLatitude, geoLontitude, 1);
                if(addressList.size() > 0)
                    result = addressList.get(0);
            }
        }catch (Exception e)
        {
            Log.e(TAG, "getAddressByGeoPoint fail!");
        }
        return result;
    }
}
