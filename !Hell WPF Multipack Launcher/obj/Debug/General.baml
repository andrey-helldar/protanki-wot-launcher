   M S B A M L   `   `   `  ����  Multipack Launcher*    $_Hell_WPF_Multipack_Launcher.General   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          N�Name$  PageGeneral��5       N� Title$ General��5	      $
��Auto��6   $	��539��65   $	��600��6D    N� 
Background 5
      �� 5   
    �� ImageSource$&  Resources/General_Background.jpg��6    j� Stretch$
 Fill=�6D    j� TileMode$
 None=�6S   .��5      � -   5         �Name$ gGrid��5       � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��34 �6   �� 5      $��32 �6   �� 5      $��48 �6   �� 5      �� 5      $��70 �6   .+�5   
   �� -   5        	 ��Name$	 	bOptimize��5      $��1��6$   $��Right=�64      ButtonStyleOptimize#���   6P   $��0,0,10,0q�6~   $��4��6�   $��Center=�6�   �� 5   
   -   5        $	 	bLauncher��5      $��1��6$   $��Right=�64     ButtonStyleLauncher#���  6P   $��0,0,55,0q�6~   $��4��6�   6�   �� 5   
   -   5        $	 	bFeedback��5      $
��Left=�6$     ButtonStyleFeedback#���  6?   $��60,0,0,0q�6m   $��4��6   $��Center=�6�   �� 5   
   -   5        $	 	bSettings��5      $
��Left=�6$     ButtonStyleSettings#���  6?   $��15,0,0,0q�6m   $��4��6   $��Center=�6�   �� 5   
   -   5        $	 bNotification��5      $��1��6(     ButtonStyleNotify#���  68   $��0,5,35,0q�6d   $��Right=�6�   $��Center=�6�   �� 5    
   -   5        $	 bUpdate��5       $��1��6"     ButtonStyleUpdate#���  62   $��Right=�6t   $��0,3,65,0q�6�   �� 5!   
   -	   5        
 ��Name$
 lType��5!      $�� ��6     LabelStyleHeaderBlack#��[ 6*   $
��Left=�6Y   $��	200,0,0,1q�6t   $��Bottom=�6�   $��2��6�   �� 5=   
   -
   5        $
 lVideo��5=      $��VIDEO PROTanki��6    $��1��69     LabelStyleHeaderWhite#��[ 6I   $
��Left=�6x   $��10,0,0,0q�6�   $��2��6�   $��Center=�6�   $��Center=�6�   �� 5>   
   -   5        $
 lNews��5>      $��NEWS WARGAMING��6   $
��Left=�68   #��[ 6S   $��10,0,0,0q�6�   $��2��6�   $��Center=�6�   $��Center=�6�   �� 5I   
   -   5         ��Name$ lbNews��5I        ListBoxGeneral#���  6"   $��	0,15,10,0q�6K   $��3��6^   ��6k   Z� 5X      �� 5Y   
   -   5        $ lbVideo��5Y      #���  6#   $��1��6L   $��0,15,3,0q�6\   $��6,9,-4,9q�6n   $��3��6�   ��6�   Z� 5s      � 5t   
   -   5         �Name$ pbStatus��5t       	 pbStatus#��� 	 6(   $��6��6K   $��0q�6V   $��1��6a   $	��Top=�6n   $��2��6�    � Maximum$ 1Q�6�   �� 5u   
   $
��10,8q�6   $��1��6"   $
��Wrap=�6/   $��Center=�6C   $��2��6^   �� 5v      -   5         ��Name$ lStatus��5v      6%   5w      �� 5x   
   -   5        $	 bDonate��5x      $
��Left=�6"    
 ButtonStyleDonate#��� 
 6=   $��120,20,0,20q�6i   $��4��6~   $��Center=�6�    � ToolTip$ Donate��6�   5z      5{      