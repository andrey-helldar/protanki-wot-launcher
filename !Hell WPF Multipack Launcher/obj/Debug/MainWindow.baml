   M S B A M L   `   `   `  ����  Multipack Launcher-    '_Hell_WPF_Multipack_Launcher.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  MainProject��5   	    � Title$ Multipack Launcher��5   	   $	��600��6$   $	��800��61    � 
ResizeMode$ NoResize=�6=    � WindowStartupLocation$ CenterScreen=�6S    � Icon$ Resources/WOT.ico��6x    � WindowStyle$
 None=�6�   ��5      �� 5   
    �� ImageSource$% Resources/Global_Background.jpg��6   .��5      � -   5         �Name$ 
GridGlobal��5       � ColumnDefinitions 5	   
   �� 5
      �� 5      $	��262 �6   	 � RowDefinitions	 5   
   �� 5      $��34 �6   �� 5      �� 5      $
��Auto �6   $	��199��6*   �� 5      $	��123 �6   �� 5      $
��Auto �6   .+�5   
    � -   5        
  �Name$
 	MainFrame��5      $��0q�6#   $��5��6.     � NavigationUIVisibility$ Hidden=�6?   �� 5   
   -   5         ��Name$ bPlay��5      $��1��6    $��4��60   $	��188��6=      ButtonStylePlay#���   6I   $��Right=�6�   .��5      �� -   5         ��Name$ bPlayTb��5       �� Padding$ 0q�6)   $��20,0,0,0q�65   PLAY6M   5      �� 5   
   -   5        $ bClose��5      $��Close��6!   $��1��61     ButtonStyleClose#���  6A   $��Right=�6l   $��0,0,9,7q�6�   $��Bottom=�6�   6�   �� 5   
   -   5        $ 	bMinimize��5      $��Minimize��6$   $��Right=�67     ButtonStyleMin#���  6S   $��0,6,45,0q�6|   $	��Top=�6�   $��1��6�   �� 5   
   -   5        $ bAirus��5      $��1��6!     ButtonStyleAIRUS#���  61   $��
25,30,25,0q�6\   $��3��6p   $	��Top=�6}   $��68��6�    � Cursor$
 Hand��6�   �� 5   
   -	   5         ��Name$ rectLang��5      $��1��6"   $
��Left=�62   $��20��6M   $��
26,33,0,26q�6Y   $��4��6m   $��Center=�6z   $��20��6�   $
��Fill=�6�   $��Resources/flag_en.png��6�   $
 Hand��6�   � 5   
   -
   5         �Name$ 
pbOptimize��5      $��1��6*   $��7��6:   $��0q�6E   $��3��6P   $	��Top=�6]    � Maximum$ 1Q�6u     pbStatus#���  6�    � Value$ 1Q�6�    � 5   
   -   5        $
 FramePreview��5      $��1��6&   $��Center=�66   $	��141��6S   $��0q�6`   $��2��6k   $��Center=�6x   $	��240��6�   $��Disabled=�6�   $ Hidden=�6�   �� 5#   
   -   5        $
 bFAQ��5#        ButtonStyleFAQ#���  6   $��1��6H   $
��Left=�6X   $��	20,0,0,50q�6s   $��1��6�   $��Center=�6�    � ToolTip$	 FAQ��6�   �� 5$   
   -   5        $	 bVK��5$        ButtonStyleVK#���  6   $��1��6F   $��Right=�6V   $��	0,0,20,50q�6r   $��1��6�   $��Center=�6�   $ VK��6�   �� 5&   
   -   5        $ LoadingPanel��5&      $
��Load��6'   5'      5(      