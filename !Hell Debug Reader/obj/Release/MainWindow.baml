   M S B A M L   `   `   `  ����   !Hell Debug Reader#    _Hell_Debug_Reader.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  !Hell Debug Reader��$	��600��$	��800�� � WindowStartupLocation$ CenterScreen=�$��Center=�$��Center=�$��Center=�$��Center=�$��10q�$	��800��$	��600�� � Icon$ favicon.ico������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFF3F3F3�� � Offset$ 0Q�� $ 	#FFD2B8FF��$ 1Q�.���  � RowDefinitions �� $	��99* ��� $
��422* ��� $��49 � � ColumnDefinitions �� $	��279 ��� .+��� -   	 ��Name$	 lvDebugs��$��10,10,10,61q�$��2����Z� �� -   $��Закрыть��$��1��$��	0,0,10,10q�$��2��$��Right=�$	��139��$��29��$��Bottom=��� -   
 ��Name$
 tbPath��$
��Left=�$��	10,0,0,10q�$��2��$	��469��$��2��$��Center=� �� 
IsReadOnly . $��29��$��Bottom=���Z� �� -   $��1��$��
219,0,0,10q�$��2��$��Bottom=�$��29��$��Обзор...��$
��Left=�$��75���� -   $)��#Удалить выделенный��$��
0,0,124,10q�$��1��$��Right=�$	��145��$��29��$��Bottom=��� $��Модуль:��$
��Left=�$��	10,12,0,0q�$	��Top=�$��26��$��55��$��1���� $��Функция:��$
��Left=�$��	10,43,0,0q�$	��Top=�$��26��$��60��$��1���� -    ��Name$ lModule��$	��---��$
��Left=�$��	86,12,0,0q�$	��Top=�$
��Bold"�$��1���� -   $ 	lFunction��$	��---��$
��Left=�$��	86,43,0,0q�$	��Top=�$
��Bold"�$��1���� $��Версия:��$
��Left=�$��
281,12,0,0q�$	��Top=�$��26��$��51��$��1���� -   $ lVersion��$	��---��$
��Left=�$��
346,12,0,0q�$	��Top=�$��1��$
��Bold"��� $��	Дата:��$
��Left=�$��
281,43,0,0q�$	��Top=�$��26��$��38��$��1���� -	   $ lDate��$	��---��$
��Left=�$��
346,43,0,0q�$	��Top=�$��1��$
��Bold"��� $��User ID:��$
��Left=�$��	10,74,0,0q�$	��Top=�$��26��$��51��$��1��$��2���� -
   $ lUserID��$	��---��$
��Left=�$��	86,74,0,0q�$	��Top=�$
��Bold"�$��1��$��2���� -   $
 tbParams��$��10q� �� TextWrapping$
 Wrap=�$	��---��" �� VerticalScrollBarVisibility$
 Auto=� � UseLayoutRounding .  . $��1��$��1����Z� �� -   $��Удалить все��$��	0,0,10,10q�$��1��$��Right=�$	��109��$��29��$��Bottom=��� -   $��Обновить��$��1��$��	0,10,10,0q�$	��Top=�$��Right=�$��75��