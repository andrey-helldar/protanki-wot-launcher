   M S B A M L   `   `   `  ����   Multipack Launcher+    %_Hell_WPF_Multipack_Launcher.Feedback   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     N�Name$  PageFeedback�� N� Title$ Feedback��$
��Auto��$
��Auto�� N� 
Background �� .��� .+��� $��10q�$��Center=�$��Center=�.+��  � RowDefinitions �� $
��Auto ��� $
��Auto �.+��� -    ��Name$ 
gbFeedback��   FeedbackgbFeedback#���   $!��Обратная связь��$��0q�$��Center=�$��Center=�.���  � ColumnDefinitions �� �� �� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ 	tbComment��$��	10,10,9,5q�$
��Wrap=�$����Если у Вас возникли проблемы в работе лаунчера и/или модов, или же есть какие-либо пожелания, Вы можете заполнить форму ниже и отправить сообщение разработчикам:��$��2�����	#FF006699$��Normal"��� -    ��Name$ lSelectCategory��  lSelectCategory#���  $*��$Выберите категорию:��$��1��$��2��	� -    	�Name$ rbWishMultipack��$0��*Пожелания к мультипаку��$��2��	 d� 	IsChecked$
	 TrueY�$��Normal"�	� -   $ rbWishLauncher��$,��&Пожелания к лаунчеру��$��3��$��Normal"�	� -   $ rbErrorMultipack��$9��3Найдена ошибка в мультипаке��$��2��$��1��$��Normal"�	� -   $ rbErrorLauncher��$5��/Найдена ошибка в лаунчере��$��3��$��1��$��Normal"�	� -	   $ rbWishInstaller��$2��,Пожелания к установщику��$��4��$��Normal"�	� -
   $ rbErrorInstaller��$;��5Найдена ошибка в установщике��$��1��$��4��$��Normal"��� $��5��$��2��$��Center=�$��2��$��0q�.+�� $��10q����	#FFF4F4F4 �� �� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+���   BorderShadow#���  $��4��._���   BorderShadowSub#���  �� -   
 ��Name$
 	tbMessage��  TextFeedback#���   �� TextWrapping$
 Wrap=�$	��140��$��2�� �� AcceptsReturn . $	��Top=�" �� VerticalScrollBarVisibility$
 Auto=�$	��200�� �� 	IsEnabled . $��
10,10,10,0q�$��Normal"����	#FF006699 �� 
CaretBrush �	#FF1B89BF�� -   $ 	lSetEmail��  	lSetEmail#���  $"��Введите Ваш email:��$��2��$��2���� -   $
 tbEmail��  	TextEmail#���  $��	10,5,10,0q�$
 Wrap=�$	��Top=�$��3��$��Normal"��� $
��10,0q�$��1��
������ $��2��.+��� $��NoWrap=����	#FF1B89BF$��Normal"�$��Right=�$��5q�$��Center=��� -    ��Name$ SymbolsTitle��.�" Набрано символов: �� -   $ SymbolsCount�����	#FFFF6262.�0�� $��Center=�$��0q�$��Center=�$��1��.+�� $��Center=�$��Center=� �� $
��155* ��� $
��179* �.+��� -    ��Name$ bClose��$��
Выход��$��1��  ButtonCLOSE#���  $��Center=��� -   $ bSend��$��Отправить��  ButtonOK#���  $��Center=�$��Center=�