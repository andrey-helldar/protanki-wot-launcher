   M S B A M L   `   `   `  ���� P  LMultipack Launcher, Version=2.9.48.931, Culture=neutral, PublicKeyToken=nullK*clr-namespace:ScottLogic.Controls.PieChartScottLogic.Controls.PieChart  )    #ScottLogic.Controls.PieChart.Legend   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     8c*clr-namespace:ScottLogic.Controls.PieChart     $	��300��$	��300��  � 	Resources  2   ,ScottLogic.Controls.PieChart.LegendConverter   legendConverter2   ,ScottLogic.Controls.PieChart.ColourConverter  colourConverter �� 
TargetType �� Property �� Value �� 
TargetType ��Name J� SnapsToDevicePixels �� Triggers P� Property	 P� Value
 �� 
TargetName �� Color��%�   &	        &	      '�� 
         �� " ��.���  � �� �� " ��.R��� $ border��
������ $��1q�._��� # z #��z��#��z�� P�  �@��
IsSelected	 . . ��� $
 border�� � �� �� $ Black��.��� .+���  �� Orientation$ Vertical=�.+���  �� TextAlignment$ Center=�$��1��$��20(�$
��Bold"�����  �� Path$( "(c:PieChartLayout.PlottedProperty)� �� RelativeSource ��  �� AncestorType)   #ScottLogic.Controls.PieChart.Legend"  �� -   $��legend��$	��170��$��Stretch=�$��0q�$ �� IsSynchronizedWithCurrentItem$
 TrueY�$��0q����� ���� .R�� $��Stretch=�$��3q����� $ #EBEBEB�� � ColumnDefinitions �� $��20 ��� $	��120 ���  � RowDefinitions �� .+��� $��0��$��13��$��13��
 � Tag �� ����  �� *Self+ �� 	Converter# [ �� $��1��$��3q����� $ Class��� $��2��$��3q� �� ����  �� *Self+# [  