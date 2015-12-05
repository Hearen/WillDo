package com.hearen.willdo.BasicClass;

import android.util.Log;
import android.widget.Toast;

import java.io.BufferedWriter;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;
/**
 * Created by Hearen on 2015/5/25 0025.
 */
public class Syncher {

    private static final String TAG = "syncher";


    private static final String IP = "127.0.0.1";

    private static final int PORT = 8888;

    public void synch() throws IOException
    {
        Socket client = null;

        InputStream in = null;
        PrintWriter printWriter  = null;
        try
        {
            client = new Socket(IP, PORT);

            in = client.getInputStream();
            printWriter = new PrintWriter(new BufferedWriter(new OutputStreamWriter(client.getOutputStream())),true);
            printWriter.print(User.getExistedUser().getJsonString());

            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            String stringReceived;
            int receivedLength = 0;
            boolean justConnected = true;
            while(true)///receive data from server;
            {
                char c = (char)in.read();
                if(c == '{' && justConnected)///find the header length; before the left brace bracket added to the outputStream;
                {
                    stringReceived = new String(byteArrayOutputStream.toByteArray());
                    receivedLength =Integer.parseInt(stringReceived);
                    byteArrayOutputStream.reset();
                    justConnected = false;
                }
                byteArrayOutputStream.write(c);//add the first '{' and the rest of the bytes in;

                if(byteArrayOutputStream.size() == receivedLength)
                {
                    stringReceived = new String(byteArrayOutputStream.toByteArray());
                    break;
                }
            }
            ///parse the received json string;
            if(User.getExistedUser().initFromJsonString(stringReceived))
                Log.e(TAG, "Cannot handle the string sent by server!");
        }
        catch (IOException e)
        {
            Toast.makeText(null, "Connection error!", Toast.LENGTH_LONG).show();
        }
        finally {
            if(in != null)
                in.close();
            if(printWriter != null)
                printWriter.close();
            if(client != null)
                client.close();
        }
    }


}
