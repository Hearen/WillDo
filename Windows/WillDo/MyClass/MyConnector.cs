using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using WillDo.MyClass;
using WillDo;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
namespace WillDo.MyClass
{
    public class MyConnector
    {
        private Globals Global = (App.Current as App).Global;
        private string ip = "127.0.0.1";
        private int port = 8888;

        public void synch(User user)
        {
            TcpClient tcpClient = null;
            NetworkStream stream = null;
            try
            {
                tcpClient = new TcpClient(ip, port);
                stream = tcpClient.GetStream();
                //string stringJson = JsonConvert.SerializeObject(Global.allItemList);
                string stringJson = JsonConvert.SerializeObject(user);
                string stringForSend = stringJson.Length + stringJson;
                byte[] dataForSend = Encoding.UTF8.GetBytes(stringForSend);

                stream.Write(dataForSend, 0, dataForSend.Length);
                stream.Flush();

                byte[] dataReceived = new byte[1024 * 60];
                string stringReceived;
                while (true)
                {
                    int receivetCount = tcpClient.Client.Receive(dataReceived);
                    dataReceived[receivetCount] = (byte)'\0';
                    stringReceived = Encoding.UTF8.GetString(dataReceived);
                    Array.Clear(dataReceived, 0, dataReceived.Length);
                    if (stringReceived.Contains('{'))//only this formatted string can be further processed;
                    {
                        int startPos = stringReceived.IndexOf('{');
                        int stringLength;
                        if (int.TryParse(stringReceived.Substring(0, startPos), out stringLength))
                        {
                            stringReceived = stringReceived.Substring(startPos);
                        }
                        while (true)///receiving data from server;
                        {
                            if (stringLength <= stringReceived.Length)
                            {
                                break;
                            }
                            tcpClient.Client.Receive(dataReceived);
                            stringReceived += Encoding.UTF8.GetString(dataReceived);
                        }

                        User user1 = getUserFromJson(stringReceived);
                        Global.refreshAllItems(user1);
                        break;
                    }
                    Console.WriteLine(stringReceived);
                    break;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Connection error! Please check your network connection.", "Warning");
                Console.WriteLine("Synchronization process error!**************" + e.ToString());
            }
            finally
            {
                if(stream != null)
                    stream.Close();
                if(tcpClient != null && tcpClient.Client != null)
                    tcpClient.Client.Close();
                if(tcpClient != null)
                    tcpClient.Close();
            }
        }

        /// <summary>
        /// according to the rule, get the object of class User from a string - json;
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private User getUserFromJson(string jsonString)
        {
            User user = new User();
            try
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonString);
                user.user_id = int.Parse(jObject["user_id"].ToString());
                user.user_name = jObject["user_name"].ToString();
                user.password = jObject["password"].ToString();

                JArray jArray;
                ObservableCollection<string> titles = new ObservableCollection<string>();
                if(jObject["titleList"].HasValues)
                {
                    jArray = (JArray)jObject["titleList"];
                    foreach (string o in jArray)
                    {
                        titles.Add(o);
                    }
                }
                

                ObservableCollection<string> categories = new ObservableCollection<string>();
                if(jObject["categoryList"].HasValues)//null cannote be converted to JArray!
                {
                    jArray = (JArray)jObject["categoryList"];
                    foreach (string o in jArray)
                    {
                        categories.Add(o);
                    }
                }
                

                ObservableCollection<string> locations = new ObservableCollection<string>();
                if (jObject["locationList"].HasValues)
                {
                    jArray = (JArray)jObject["locationList"];
                    foreach (string o in jArray)
                    {
                        locations.Add(o);
                    }
                }
                

                List<ToDoItem> itemsList = new List<ToDoItem>();
                if(jObject["allItemsList"].HasValues)
                {
                    jArray = (JArray)jObject["allItemsList"];
                    foreach (JObject o in jArray)
                    {
                        ToDoItem item = new ToDoItem();
                        item.ID = int.Parse(o["ID"].ToString());
                        item.Title = o["Title"].ToString();
                        item.Created = getDateTimeFromString(o["Created"].ToString());
                        item.Due = getDateTimeFromString(o["Due"].ToString());
                        item.Color = o["Color"].ToString();
                        item.LastModified = getDateTimeFromString(o["LastModified"].ToString());
                        item.Category = o["Category"].ToString();
                        item.Location = o["Location"].ToString();
                        item.Priority = int.Parse(o["Priority"].ToString());
                        item.Progress = int.Parse(o["Progress"].ToString());
                        item.IsComplete = (o["IsComplete"].ToString() == "1" ? true : false);
                        item.IsDeleted = (o["IsDeleted"].ToString() == "1" ? true : false);
                        item.Comment = o["Comment"].ToString();
                        itemsList.Add(item);
                    }
                }
                
                user.titleList = titles;
                user.categoryList = categories;
                user.locationList = locations;
                user.allItemsList = itemsList;
            }
            catch(JsonException e)
            {
                MessageBox.Show("Abnormal failure! Please contact the software supporter.");
                Console.WriteLine("json parsing failed in MyConnector\n\n" + e.ToString());
            }
            
            return user;
        }

        /// <summary>
        /// get date from a unformatted string;
        /// 2015/4/21 T00:00:00 or
        /// 2015-4-21 T00:00:00
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private DateTime getDateTimeFromString(string dateString)
        {
            int TPos = dateString.IndexOf(' ');
            dateString = dateString.Substring(0, TPos);
            string[] ss = dateString.Split(new char[] { '/' });
            if (ss.Length == 1)///in case different versions;
                ss = dateString.Split(new char[] { '-' });
            int year = int.Parse(ss[0]), month = int.Parse(ss[1]), day = int.Parse(ss[2]);
            return new DateTime(year, month, day);
        }

    }
}
