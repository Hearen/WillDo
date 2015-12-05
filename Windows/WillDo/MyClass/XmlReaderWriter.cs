using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WillDo.MyClass
{
    public class XmlReaderWriter
    {
        private Globals Global = (App.Current as App).Global;
        /// <summary>
        /// customized writer, used to write all the user related info down to the local storage;
        /// the source is Global; all the contents including:
        /// user basic info - id, encrypted password and nickname;
        /// to-do items - id, title, progress, priority, location, category, color, comment, isComplete, created, due, lastModified
        /// frequenctly used categories, locations, and titles;
        /// </summary>
        public void writeDownToXml(string path = "info.xml")
        {
            XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;///for better readability;

            writer.WriteStartDocument();

            ///start info tag;
            writer.WriteStartElement("info");

            ///start user tag;
            writer.WriteStartElement("user");
            writer.WriteElementString("id", Global.user_id.ToString());
            writer.WriteElementString("pwd", Global.encrypted_pwd);
            writer.WriteElementString("name", Global.user_name);
            writer.WriteElementString("current_max_id", Global.current_max_id.ToString());
            writer.WriteEndElement();///end user tag;

            ///start items tag;
            writer.WriteStartElement("items");
            foreach(ToDoItem item in Global.allItemList)
            {
                ///start the inner single item tag;
                writer.WriteStartElement("item");
                writer.WriteElementString("id", item.ID.ToString());
                writer.WriteElementString("title", item.Title);
                writer.WriteElementString("progress", item.Progress.ToString());
                writer.WriteElementString("priority", item.Priority.ToString());
                writer.WriteElementString("location", item.Location);
                writer.WriteElementString("category", item.Category);
                writer.WriteElementString("color", item.Color);
                writer.WriteElementString("comment", item.Comment);
                writer.WriteElementString("isComplete", item.IsComplete.ToString());
                writer.WriteElementString("isDeleted", item.IsDeleted.ToString());
                writer.WriteElementString("created", item.Created.ToLongDateString());
                writer.WriteElementString("due", item.Due.ToLongDateString());
                writer.WriteElementString("lastModified", item.LastModified.ToLongDateString());
                writer.WriteEndElement();
                ///end the singular item tag;
            }
            writer.WriteEndElement();///end items tag;
            
            ///start the frequently used categories tag;
            writer.WriteStartElement("types");///replacing category with type for the plural form; category - categories;
            foreach(string s in Global.categories)
            {
                writer.WriteElementString("type", s);
            }
            writer.WriteEndElement();///end the frequently used categories tag;
                                    
            ///start the frequently used locations tag;
            writer.WriteStartElement("locations");
            foreach (string s in Global.locations)
            {
                writer.WriteElementString("location", s);
            }
            writer.WriteEndElement();///end the frequently used categories tag;
                                     
            ///start the frequently used titles tag;
            writer.WriteStartElement("titles");
            foreach (string s in Global.titles)
            {
                writer.WriteElementString("title", s);
            }
            writer.WriteEndElement();///end the frequently used categories tag;

            writer.WriteEndElement();///end info tag;
                                     ///
            writer.Close();///this operation is pretty important to make sure the date complete;
        }

        /// <summary>
        /// using thie method, read all the local stored date into the current program circumstance;
        /// </summary>
        public void readFromXml(string path = "info.xml")
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(path);
            }
            catch (Exception e)///if there is not data, create a temporary list first;
                               ///and then read it to avoid a cold start;
            {
                createXmlFile(path);
                xmlDoc.Load(path);
            }
            

            ///reading user first;
            XmlNode userNode = xmlDoc.GetElementsByTagName("user")[0];
            foreach(XmlNode child in userNode.ChildNodes)///traverse all the children of the current user node;
            {
                switch(child.Name)
                {
                    case "id": Global.user_id = int.Parse(child.InnerText); break;
                    case "name": Global.user_name = child.InnerText; break;
                    case "pwd": Global.encrypted_pwd = child.InnerText; break;
                    case "current_max_id": Global.current_max_id = int.Parse(child.InnerText); break;
                    default: break;
                }
            }

            XmlNodeList itemsNodeList = xmlDoc.GetElementsByTagName("item");
            foreach(XmlNode node in itemsNodeList)///traversing all the items in tag items;
            {
                ToDoItem item = new ToDoItem();
                foreach (XmlNode child in node)
                {
                    switch (child.Name)
                    {
                        case "id": item.ID = int.Parse(child.InnerText); break;
                        case "title": item.Title = child.InnerText; break;
                        case "progress": item.Progress = int.Parse(child.InnerText); break;
                        case "priority": item.Priority = int.Parse(child.InnerText); break;
                        case "location": item.Location = child.InnerText; break;
                        case "category": item.Category = child.InnerText; break;
                        case "color": item.Color = child.InnerText; break;
                        case "comment": item.Comment = child.InnerText; break;
                        case "isComplete": item.IsComplete = bool.Parse(child.InnerText); break;
                        case "isDeleted": item.IsDeleted = bool.Parse(child.InnerText); break;
                        case "created": item.Created = DateTime.Parse(child.InnerText); break;
                        case "due": item.Due = DateTime.Parse(child.InnerText); break;
                        case "lastModifies": item.LastModified = DateTime.Parse(child.InnerText); break;
                        default: break;
                    }
                }///the end of the foreach for item;
                Global.allItemList.Add(item);
                ///needless to check both of them at the same time any more; 
                ///using binding already - change them at the same time;
                ///if (item.IsComplete == false && item.Progress < 100)
                if(!item.IsDeleted)
                    Global.shownItemList.Add(item);
                if (item.IsDeleted)
                    Global.deletedItemList.Add(item);
            }///the end of the foreach for items;
            
            ///using a little method to prevent repeated same operations;
            /*
            ///retrieve categories;
            ///XmlNodeList categoriesNodeList = xmlDoc.GetElementsByTagName("category");
            ///this method will just return all the category tags including in items;
            ///
            XmlNode tmpNode = xmlDoc.SelectSingleNode("categories");
            if(tmpNode != null)
            {
                XmlNodeList categoriesNodeList = tmpNode.ChildNodes;
                foreach (XmlNode node in categoriesNodeList)///traversing all the items in tag items;
                {
                    Global.categories.Add(node.InnerText);
                }///the end of the foreach for categories;
            
            }

            
            ///retrieve categories;
            ///XmlNodeList locationsNodeList = xmlDoc.GetElementsByTagName("location");
            ///this method will just return all the category tags including in items;
            tmpNode = xmlDoc.SelectSingleNode("locations");
            if(tmpNode != null)
            {
                XmlNodeList locationsNodeList = tmpNode.ChildNodes;
                foreach (XmlNode node in locationsNodeList)///traversing all the items in tag items;
                {
                    Global.locations.Add(node.InnerText);
                }///the end of the foreach for locations;
            }
            */
            Global.categories = readStringNodes(xmlDoc, "type");

            Global.locations = readStringNodes(xmlDoc, "location");

            Global.titles = readStringNodes(xmlDoc, "title");

            ///set the Global.current_max_id;
            ///when the current_max_id is less than the existing allocated id, reset it;
            for(int i = 0; i < Global.allItemList.Count; i++)
            {
                if (Global.current_max_id < Global.allItemList[i].ID)
                    Global.current_max_id = Global.allItemList[i].ID + 1;
            }
        }///end of read;
        
        ///<summary>
        ///according to types-type-type-types to get the type collection return string list
        ///</summary>
        private ObservableCollection<string> readStringNodes(XmlDocument xmlDoc, string tag)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            ///retrieve categories;
            ///XmlNodeList locationsNodeList = xmlDoc.GetElementsByTagName("location");
            ///this method will just return all the category tags including in items;
            XmlNode tmpNode = xmlDoc.GetElementsByTagName(tag + "s")[0];
            if (tmpNode != null)
            {
                foreach (XmlNode node in tmpNode.ChildNodes)///traversing all the items in tag items;
                {
                    list.Add(node.InnerText);
                }///the end of the foreach for locations;
            }
            return list;
        }///end of readStringNodes;
         ///

        private void createXmlFile(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            fileStream.Close();
            for (int i = 0; i < 2; i++)
            {
                ToDoItem item0 = new ToDoItem();
                item0.Due = DateTime.Now.AddHours(6 * i);
                item0.Created = DateTime.Now.AddHours(10 * i);
                item0.Location = "Company";
                item0.Priority += i;
                item0.Title += i;
                item0.ID = i;
                item0.Priority = item0.Priority > 10 ? 10 : item0.Priority;
                Global.allItemList.Add(item0);
                Global.shownItemList.Add(item0);
            }
            Global.categories.Add("Work");
            Global.locations.Add("Company");
            Global.titles.Add("do");
            writeDownToXml(path);////after this operation, clear both the allItemList and shownItemList;
            ///make sure they are clean before readFromXml;
            Global.allItemList.Clear();
            Global.shownItemList.Clear();
        }
    }
}
