   M S B A M L   `   `   `  ����  Multipack Launcher0    *_Hell_WPF_Multipack_Launcher.WgOpenIdAIRUS   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          N� Title$  Authorization��5      .��5
      �  � ColumnDefinitions 5   
   �� 5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� $��Center=�6   $��	72,0,71,0q�62   $��2��6E   $��Center=�6R   .+�5      � $��Center=�6   $��Center=�60    5      �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   .+�5      �� -   5         ��Name$ bEnter��5      $��Enter��6)      ButtonOK#���   69   $��Center=�6\   $��10q�6y   $��Center=�6�   6�   �� 5      -   5        $ bCancel��5      $��Cancel��6*     ButtonCLOSE#���  6;   $��1��6a   $��Center=�6q   $��10q�6�   $��Center=�6�   6�   5      5      �� 5   
   -   5         ��Name$ 	tbCaption��5        FontSochi2014#��[ 6'   $
��Left=�6S   $��30q�6n   $
��Wrap=�6z   $��Center=�6�   $��20(�6�   �� 5      .�6   Autorization6     �� 5      -   5         ��Name$ CaptionStatus��5      
���  ��6<   5       �� 5!   
   $��Center=�6   $��0q�62   $��1��6=   $��Center=�6J   .+�5"      � $��Center=�6   $��Center=�60    5#      �� 5$      $
��Auto �6'   �� 5%      $	��180 �6'    5'      �� 5(      $
��Auto �6$   �� 5)      $
��Auto �6$   �� 5*      $
��Auto �6$   �� 5+      $
��Auto �6$   .+�5-      �� -   5         ��Name$ lEmail��5-      $��Email��6(   $
��Left=�68   $��0,4,0,3q�6S   $��Center=�6d   ���	#FF006699$��Normal"�6�   �� 5.      -   5        $ 	lPassword��5.      $��Password��6+   $
��Left=�6>   $��0,3,0,4q�6Y   $��1��6j   $��Center=�6w   ���	#FF006699$��Normal"�6�   �� 5/      -   5         ��Name$ tbEmail��5/      $��1��6+   $��10,5,0,5q�6;    �� TextWrapping$
 Wrap=�6M   $��Center=�6a   $��23��6|   G� 50      -	   5        	 G�Name$	 
pbPassword��50      $��1��62   $��10,5,0,5q�6B   $��1��6T   $��Center=�6a   $��23��6|   �� 51      -
   5        $ 	tbCaptcha��51      $��1��6-   $��22��6=   $��10,5,0,5q�6I   $��2��6[   $
 Wrap=�6h   $��Center=�6|   �� 52      -   5        
 ��Name$
 
imgCaptcha��52      $
��Left=�6,   $��50��6G   $��2��6S   $	��Top=�6`   $	��120��6x    � ToolTip$ Refresh captcha��6�   �� 53      -   5        $ bReloadCaptcha��53        ButtonRefresh#���  61   $��Reload captcha��6Y   $��1��6r   $��10,5,0,5q�6�   $��3��6�   $��Center=�6�   6�   54      55      (  "System.Windows.Controls.WebBrowser  56   
   -   5          Name$ WB��56      $
��Left=�6!   $	��128��6<   $��
100,30,0,0q�6I   $��2��6]   $	��Top=�6n   $	��323��6�   6�   58      59      