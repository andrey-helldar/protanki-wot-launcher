   M S B A M L   `   `   `  ����   Multipack Launcher*    $_Hell_WPF_Multipack_Launcher.General   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     N�Name$  PageGeneral�� N� Title$ General��$
��Auto��$	��539��$	��600�� N� 
Background ��  �� ImageSource$&  Resources/General_Background.jpg�� j� Stretch$
 Fill=� j� TileMode$
 None=�.��� -    �Name$ gGrid�� � ColumnDefinitions �� ��  � RowDefinitions �� $��34 ��� $��32 ��� $��48 ��� �� $��70 �.+��� -   	 ��Name$	 	bOptimize��$��1��$��Right=�   ButtonStyleOptimize#���   $��0,0,10,0q�$��4��$��Center=��� -   $	 	bLauncher��$��1��$��Right=�  ButtonStyleLauncher#���  $��0,0,55,0q�$��4���� -   $	 	bFeedback��$
��Left=�  ButtonStyleFeedback#���  $��60,0,0,0q�$��4��$��Center=��� -   $	 	bSettings��$
��Left=�  ButtonStyleSettings#���  $��15,0,0,0q�$��4��$��Center=��� -   $	 bNotification��$��1��  ButtonStyleNotify#���  $��0,5,35,0q�$��Right=�$��Center=��� -   $	 bUpdate��$��1��  ButtonStyleUpdate#���  $��Right=�$��0,3,65,0q��� -	   
 ��Name$
 lType��$�� ��  LabelStyleHeaderBlack#��[ $
��Left=�$��	200,0,0,1q�$��Bottom=�$��2���� -
   $
 lVideo��$��VIDEO PROTanki��$��1��  LabelStyleHeaderWhite#��[ $
��Left=�$��10,0,0,0q�$��2��$��Center=�$��Center=��� -   $
 lNews��$��NEWS WARGAMING��$
��Left=�#��[ $��10,0,0,0q�$��2��$��Center=�$��Center=��� -    ��Name$ lbNews��  ListBoxGeneral#���  $��	0,15,10,0q�$��3����Z� �� -   $ lbVideo��#���  $��1��$��0,15,3,0q�$��6,9,-4,9q�$��3����Z� � -    �Name$ pbStatus�� 	 pbStatus#��� 	 $��6��$��0q�$��1��$	��Top=�$��2�� � Maximum$ 1Q��� $
��10,8q�$��1��$
��Wrap=�$��Center=�$��2���� -    ��Name$ lStatus���� -   $	 bDonate��$
��Left=� 
 ButtonStyleDonate#��� 
 $��120,20,0,20q�$��4��$��Center=� � ToolTip$ Donate��