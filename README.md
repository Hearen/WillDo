# Introduction
--------------
Nowadays, the to-do list management is becoming more and more critical in our daily life for memos and reminders to improve our life quality and working efficiency. This little project is trying to provide a handy tool to manage them not only in PC but in Mobile Phone which is gradually becoming the most important part of our everyday life - an essential equipment.

With the development of the mobile phones, more and more attention and manpower poured into mobile applications development. Nowadays lots of applications have been produced but there are still some ideas that can be further fulfilled and polished, considering this situation I chose a topic about ToDoList which is called formally GTD system abbreviated from Get Things Done; though there are many developers at home and abroad, I still want to have a try to challenge myself in this field with bran-new developing skills (for me only), mobile development in Android, Windows client using WPF as presenter and server using C++ network programming which are completely strange for me who only tried C, java, Win Form, Windows Phone and have some basic project experience. Here I will try my best to display my current work concretely including server side, Windows client and Android client.

# Specification
There are two different platforms where this application can run smoothly Windows 7 or Windows 8 for Windows client and Android for mobile client and to support these clients, there is a server designed to synchronize the data between them to stay consistent.

There are several parts about this system and in each part we can have so many features to accomplish and polish but due to the time limitation and personal reason I will only achieve four main parts in this system including a server accomplished by C++ which I plan to use Node.js to replace in the near future, a Windows client by C# using WPF to accomplish the presenter layer one of whose outstanding features is binding which will be displayed more vividly in doc part, an Android client by java and a Web client which will be finished in the near future too.
As for the server, I planned to use non-block multi-thread mechanism as the basic model to achieve multi-users and multi-clients network communication and the realization details will be revealed in the realization part in doc part too.
When it comes to the Windows client programmed in C# whose presenter layer is accomplished by WPF using binding mechanism; in this client the user can freely use add, delete, update, sort, filter, modify and the other features. For example, the user can simply click an add button and the to-do list will contain a wholly new to-do task with default values according to the situation and when the user tries to edit it, he or she can simply select it and then edit at the bottom of the main panel, changing its priority using different color to indicate different levels, changing its due time using date picker, changing its progress any time he or she wants, customizing the color of the whole item, changing the category and location of the task and there are still more which will be detailed in doc part.
Given the Android client, due to its simplicity, only some of the features are accomplished compared to the Windows client except for one specialty, the location which is to be retrieved by GPS or network provider instead of manually input in


Windows client and also there are still some features that the Windows client cannot provide, let's see the specifics in the doc part.
In the doc part, section one and section two will be used to explain some basic ideas and terms used in the system; section three is arranged to discuss the details of the features of the whole system currently including server side, Windows client and Android client and the corresponding realization details; after section three we will further discuss the performance, feasibility, robustness, portability and future improvements about this system; before we end the doc in section five there will be several Gantt graphs used to present the whole developing process and arrangements and of course references in section six.

# Additional
For more detailed information, you may check it in doc.
There are still many problems inside this system, if these problems ever make you confused, please do not hesitate to contact me.
Author: LHearen
E-mail: LHearen@126.com
