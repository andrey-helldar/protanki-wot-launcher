   M S B A M L   `   `   `  ����   Multipack Launcher-    '_Hell_WPF_Multipack_Launcher.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  MainProject�� � Title$ Multipack Launcher��$	��600��$	��800�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � Icon$ Resources/WOT.ico�� � WindowStyle$
 None=�����  �� ImageSource$% Resources/Global_Background.jpg��.��� -    �Name$ 
GridGlobal�� � ColumnDefinitions �� �� $	��262 �	 � RowDefinitions	 �� $��34 ��� �� $
��Auto �$	��199���� $	��123 ��� $
��Auto �.+� � -   
  �Name$
 	MainFrame��$��0q�$��5��  � NavigationUIVisibility$ Hidden=��� -    ��Name$ bPlay��$��1��$��4��$	��188��   ButtonStylePlay#���   $��Right=�.���� -    ��Name$ bPlayTb�� �� Padding$ 0q�$��20,0,0,0q�PLAY�� -   $ bClose��$��Close��$��1��  ButtonStyleClose#���  $��Right=�$��0,0,9,7q�$��Bottom=��� -   $ 	bMinimize��$��Minimize��$��Right=�  ButtonStyleMin#���  $��0,6,45,0q�$	��Top=�$��1���� -   $ bAirus��$��1��  ButtonStyleAIRUS#���  $��
25,30,25,0q�$��3��$	��Top=�$��68�� � Cursor$
 Hand���� -	    ��Name$ rectLang��$��1��$
��Left=�$��20��$��
26,33,0,26q�$��4��$��Center=�$��20��$
��Fill=�$��Resources/flag_en.png��$
 Hand��� -
    �Name$ 
pbOptimize��$��1��$��7��$��0q�$��3��$	��Top=� � Maximum$ 1Q�  pbStatus#���   � Value$ 1Q� � -   $
 FramePreview��$��1��$��Center=�$	��141��$��0q�$��2��$��Center=�$	��240��$��Disabled=�$ Hidden=��� -   $
 bFAQ��  ButtonStyleFAQ#���  $��1��$
��Left=�$��	20,0,0,50q�$��1��$��Center=� � ToolTip$	 FAQ���� -   $	 bVK��  ButtonStyleVK#���  $��1��$��Right=�$��	0,0,20,50q�$��1��$��Center=�$ VK���� -   $ LoadingPanel��$
��Load��