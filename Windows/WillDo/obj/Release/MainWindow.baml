   M S B A M L   `   `   `  ���� A  =WillDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WillDo.MyControlsWillDo.MyControls  /clr-namespace:WillDo.MyClassWillDo.MyClass      WillDo.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6
mycontrolsclr-namespace:WillDo.MyControls     0myclassclr-namespace:WillDo.MyClass       � Title$  
MainWindow��$	��700��$	��100�� � WindowStartupLocation$ CenterScreen=� � 	Resources *   $WillDo.MyClass.DeleteToTextDecorator   deleteConverter  titleDT ��Name �� TextDecorations �� Path �� 	Converter  
progressDT ��Name �� TextAlignment  	createdDT 	 dueDT  
categoryDT  
priorityDT	 �� Orientation  
locationDT  
completeDT
 ��Name d� 	IsChecked��%$  &	        &	      7   &	 t     7   &	 �     7   &	 X    7   &	 �    7   &	 N    &	 �    7   &	 i      �� .R��� $ titleTBX��-   $	��750������ *Title+ �� $ 	IsDeleted�8   ���� *Color+�� .R��� $ progressTBk��$ Center=����� *
Progress+���� $ 	IsDeleted�8   ���� *Color+�� .R��� $ 
createdTBK��$ Center=����� *	Created+���� $ 	IsDeleted�8   ���� *Color+�� .R��� $ dueTBK��$��Center=�$ Center=����� *Due+���� $ 	IsDeleted�8   ���� *Color+�� .R��� $ categoryTBK��$��Center=�$ Center=����� *
Category+���� $ 	IsDeleted�8   ���� *Color+�� .R��� $	 
Horizontal=�.+��� $��15��$��15��$��3q����� *PriorityColor+�� $ priorityTBX��$ Center=����� *
Priority+���� *Color+�� .R��� $ locationTBK��$ Center=����� *
Location+���� $ 	IsDeleted�8   ���� *Color+�� .R��� $
 
completeCB�� �� *
IsComplete+���� *Color+.���  � ColumnDefinitions �� $��* ��� $	��120 � � RowDefinitions �� $	��100 ��� $��* ��� $	��100 �.+��� $��	Filter by��$��2��$��0��$��3q�.���� $	 
Horizontal=�.+�)   #WillDo.MyControls.AttributeItemView  -     Name$ showItem��$	��120��$��10q�)   #WillDo.MyControls.TitleComboControl  -     Name$ 	titleItem��$	��120��$��5q�  -   $ dueItem��$	��150��$��10q�'   !WillDo.MyControls.PriorityControl  -     Name$ abovePriorityItem��$	��120��$��10q�(   "WillDo.MyControls.HCategoryControl  -     Name$ categoryItem��$	��120��$��5q�(   "WillDo.MyControls.HLocationControl  -     Name$ hLocationItem��$	��140��$��5q�� $��0��$��1��.+�(   "WillDo.MyControls.SortableListView  -     Name$ listViewItem��(   "WillDo.MyControls.SortableListView   	LostFocus$ listViewItem_LostFocus��   SelectionChanged$# listViewItem_SelectionChanged�� �� View �� .P��� $��Title�� �� Width$	 100��   SortPropertyName$ Title�� �� CellTemplate# [ �� $��%��$ 50��# [ �� $��Created��$	 150��$ Created��# [ �� $	��Due��$	 150��$	 Due��# [ �� $
��Cat.��$	 100��$ Category��# [ �� $��Priority��$ 50��$ Priority��# [ �� $��Location��$	 100��$ Location��# [ �� $	��√��$ 50��$ 
IsComplete��# [ � $��1��$��1�� �� $��20 ��� $��* ��� $��42 ��� $��42 ��� $��42 ��� $��42 ��� $��42 ��� $��42 �.+��� $��0��$��Comment���� -	   $ commentTextBox��$��1��$	��100�� �� TextWrapping$
 Wrap=� �� TextAlignment$ Justify=��� -
    ��Name$ 	addButton��$��2��$	��Add��$��40��$��1q��� -   $ restoreButton��$y�	Collapsed=�$��2��$��Restore��$��40��$��1q��� -   $ deleteButton��$��3��$��Delete��$��40��$��1q��� -   $ 	pwdButton��$��4��$��Password��$��40��$��1q��� -   $ synchButton��$��5��$��Synch��$��40��$��1q��� -   $ 
todoButton��$��6��$��ToDoList��$��40��$��1q��� -   $ trashButton��$��7��$��Trash��$��40��$��1q��� $��Edition��$��2��$��2��$��3q�.���� $	 
Horizontal=�.+�  -   $ priorityItem��$	��130��$��10q�  -   $ progressItem��$	��100��$��10q�*	   $WillDo.MyControls.CalendarBoxControl	  -    	 Name$ fDueItem��$	��150��$��10q�,
   &WillDo.MyControls.ColorSelectorControl
  -    
 Name$ 	colorItem��$	��120��$��10q�(   "WillDo.MyControls.FCategoryControl  -     Name$ fCategoryItem��$	��120��$��5q�(   "WillDo.MyControls.FLocationControl  -      Name$  fLocationItem��$	��140��$��5q�