   M S B A M L   `   `   `  ����   Multipack Launcher)    #_Hell_WPF_Multipack_Launcher.Update   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     N�Name$  
PageUpdate�� N� Title$ Update��$	��538��$	��600��.��� $��0q� � RowDefinitions �� �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ lDownloadFromLink��$b��\Новая версия доступна по ссылке в описании к видео��$	��0,5q�$��1��$	��Top=�$��Center=��� -    ��Name$ cbNotify��$C��=Не уведомлять меня об этой версии��$��Center=�$	��0,5q�$��2��$��Center=��� $��Center=�$��	0,10,0,30q�$��3��$��Center=�.+�� $��Center=�$��Center=� � ColumnDefinitions �� $
��133* ��� $
��135* �.+��� -    ��Name$ bUpdate��   ButtonOK#���   $��Скачать��$
��20,0q�$��Center=�$	��120��$��Center=��� -   $ bCancel��  ButtonCLOSE#���  $��Не надо��$��1��$��Center=�$
��20,0q�$��Center=�$	��120���� -    ��Name$ 	gbCaption��$3��-Обнаружена новая версия:��$��0q�$��Center=�$��Center=�.���  �� $
��Auto ��� .+��� -   $ 
newVersion��  
newVersion#��[ $��0.0.0 #0���� -    ��Name$ 	tbContent��$��5q�$��1��$
��Wrap=�$��Information��$
��Auto=�$��WordEllipsis=�