   M S B A M L   `   `   `  ����A  =WillDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WillDo.MyControlsWillDo.MyControls  5   	   /clr-namespace:WillDo.MyClassWillDo.MyClass  5   	       WillDo.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6
mycontrolsclr-namespace:WillDo.MyControls     5   	   0myclassclr-namespace:WillDo.MyClass     5   	     � Title$  
MainWindow��5   	   $	��700��6   $	��100��6,    � WindowStartupLocation$ CenterScreen=�6;    � 	Resources 5      *   $WillDo.MyClass.DeleteToTextDecorator   deleteConverter  titleDT ��Name �� TextDecorations �� Path �� 	Converter  
progressDT ��Name �� TextAlignment  	createdDT 	 dueDT  
categoryDT  
priorityDT	 �� Orientation  
locationDT  
completeDT
 ��Name d� 	IsChecked��%  &	        &	      7   &	 �     7   &	 o    7   &	     7   &	 �    7   &	 |    &	 �    7   &	 3      5	   
   6B   �� 5   
   .R�5      �� $ titleTBX��6   -   5        $	��750��5   �   ��6(   �� *Title+ 6?   �� $ 	IsDeleted�8   ��6�   �� *Color+6�   5      �� 5   
   .R�5      �� $ progressTBk��6   $ Center=�6�   ��6-   �� *
Progress+��6G   �� $ 	IsDeleted�8   ��6�   �� *Color+5      �� 5   
   .R�5      �� $ 
createdTBK��6   $ Center=�6�   ��6,   �� *	Created+��6E   �� $ 	IsDeleted�8   ��6�   �� *Color+5      �� 5   
   .R�5      �� $ dueTBK��6   $��Center=�6�   $ Center=�6�   ��6(   �� *Due+��6=   �� $ 	IsDeleted�8   ��6�   �� *Color+5      �� 5   
   .R�5      �� $ categoryTBK��6   $��Center=�6�   $ Center=�6�   ��6-   �� *
Category+��6G   �� $ 	IsDeleted�8   ��6�   �� *Color+5      �� 5   
   .R�5       �� $	 
Horizontal=�6   .+�5!      �� $��15��6;   $��15��6F   $��3q�6R   ��6   �� *PriorityColor+6R   �� 5"      $ priorityTBX��6   $ Center=�6K   ��61   �� *
Priority+��6b   �� *Color+5#      5$      �� 5%   
   .R�5&      �� $ locationTBK��6   $ Center=�6�   ��6-   �� *
Location+��6H   �� $ 	IsDeleted�8   ��6�   �� *Color+5'      �� 5(   
   .R�5)      �� $
 
completeCB��6    6,   �� *
IsComplete+��6N   �� *Color+5*      5+      .��5,      �  � ColumnDefinitions 5-   
   �� 5.      $��* �6   �� 5/      $	��120 �6    � RowDefinitions 51   
   �� 52      $	��100 �6   �� 53      $��* �6   �� 54      $	��100 �6   .+�57   
   �� $��	Filter by��6   $��2��6&   $��0��6:   $��3q�6G   .��58      �� $	 
Horizontal=�6   .+�5<      )   #WillDo.MyControls.AttributeItemView  -   5          Name$ showItem��5<   /   $	��120��6A   $��10q�6M   )   #WillDo.MyControls.TitleComboControl  5=      -   5          Name$ 	titleItem��5=   /   $	��120��6B   $��5q�6N     5>      -   5        $ dueItem��5>   /   $	��150��6@   $��10q�6L   '   !WillDo.MyControls.PriorityControl  5?      -   5          Name$ abovePriorityItem��5?   -   $	��120��6H   $��10q�6T   (   "WillDo.MyControls.HCategoryControl  5@      -   5          Name$ categoryItem��5@   .   $	��120��6D   $��5q�6P   (   "WillDo.MyControls.HLocationControl  5A      -   5          Name$ hLocationItem��5A   .   $	��140��6E   $��5q�6Q   5B      5C      � 5F   
   $��0��6   $��1��6   .+�5H      (   "WillDo.MyControls.SortableListView  -   5          Name$ listViewItem��5H   *   (   "WillDo.MyControls.SortableListView   	LostFocus$ listViewItem_LostFocus��6@      SelectionChanged$# listViewItem_SelectionChanged��6c    �� View 5I      �� 5J      .P�5N      �� $��Title��6)    �� Width$	 100��68      SortPropertyName$ Title��6D    �� CellTemplate# [ 6y   �� 5O      $��%��6)   $ 50��64   # [ 6?   �� 5P      $��Created��6)   $	 150��6:   $ Created��6F   # [ 6}   �� 5Q      $	��Due��6)   $	 150��66   $	 Due��6B   # [ 6u   �� 5R      $
��Cat.��6)   $	 100��67   $ Category��6C   # [ 6{   �� 5S      $��Priority��6)   $ 50��6;   $ Priority��6F   # [ 6~   �� 5T      $��Location��6)   $	 100��6;   $ Location��6G   # [ 6   �� 5U      $	��√��6)   $ 50��64   $ 
IsComplete��6?   # [ 6y   5V      5X      5Y      � 5[   
   $��1��6   $��1��6    5\      �� 5]      $��20 �6    �� 5^      $��* �6    �� 5_      $��42 �6    �� 5`      $��42 �6    �� 5a      $��42 �6    �� 5b      $��42 �6    �� 5c      $��42 �6    �� 5d      $��42 �6    .+�5g      �� $��0��6   $��Comment��6&   �� 5h      -	   5        $ commentTextBox��5h      $��1��6S   $	��100��6`    �� TextWrapping$
 Wrap=�6p    �� TextAlignment$ Justify=�6�   �� 5i      -
   5         ��Name$ 	addButton��5i      $��2��6@   $	��Add��6M   $��40��6[   $��1q�6g   �� 5j      -   5        $ restoreButton��5j      $y�	Collapsed=�6H   $��2��6_   $��Restore��6l   $��40��6~   $��1q�6�   �� 5k      -   5        $ deleteButton��5k      $��3��6F   $��Delete��6T   $��40��6f   $��1q�6r   �� 5m      -   5        $ 	pwdButton��5m      $��4��6(   $��Password��65   $��40��6I   $��1q�6U   �� 5n      -   5        $ synchButton��5n      $��5��6D   $��Synch��6Q   $��40��6b   $��1q�6n   �� 5o      -   5        $ 
todoButton��5o      $��6��6B   $��ToDoList��6O   $��40��6c   $��1q�6o   �� 5p      -   5        $ trashButton��5p      $��7��6D   $��Trash��6Q   $��40��6b   $��1q�6n   5q      �� 5t   
   $��Edition��6   $��2��6$   $��2��68   $��3q�6E   .��5u      �� $	 
Horizontal=�6   .+�5v        -   5        $ priorityItem��5v   -   $	��130��6C   $��10q�6O     5w      -   5        $ progressItem��5w   /   $	��100��6E   $��10q�6Q   *	   $WillDo.MyControls.CalendarBoxControl	  5{      -   5         	 Name$ fDueItem��5{   0   $	��150��6B   $��10q�6N   ,
   &WillDo.MyControls.ColorSelectorControl
  5|      -   5         
 Name$ 	colorItem��5|   2   $	��120��6E   $��10q�6Q   (   "WillDo.MyControls.FCategoryControl  5}      -   5          Name$ fCategoryItem��5}   .   $	��120��6E   $��5q�6Q   (   "WillDo.MyControls.FLocationControl  5~      -   5           Name$  fLocationItem��5~   .   $	��140��6E   $��5q�6Q   5      5�      5�      5�      