   M S B A M L   `   `   `  ����  Multipack Launcher)    #_Hell_WPF_Multipack_Launcher.Update   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          N�Name$  
PageUpdate��5       N� Title$ Update��5      $	��538��6   $	��600��6   .��5
      � $��0q�6    � RowDefinitions 5   
   �� 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ lDownloadFromLink��5      $b��\Новая версия доступна по ссылке в описании к видео��6+   $	��0,5q�6h   $��1��6u   $	��Top=�6�   $��Center=�6�   �� 5   
   -   5         ��Name$ cbNotify��5      $C��=Не уведомлять меня об этой версии��6%   $��Center=�6Q   $	��0,5q�6n   $��2��6{   $��Center=�6�   �� 5   
   $��Center=�6   $��	0,10,0,30q�62   $��3��6E   $��Center=�6R   .+�5      � $��Center=�6   $��Center=�6.    � ColumnDefinitions 5      �� 5      $
��133* �6'   �� 5      $
��135* �6'   .+�5      �� -   5         ��Name$ bUpdate��5         ButtonOK#���   6*   $��Скачать��6M   $
��20,0q�6_   $��Center=�6m   $	��120��6�   $��Center=�6�   6�   �� 5      -   5        $ bCancel��5        ButtonCLOSE#���  6*   $��Не надо��6P   $��1��6b   $��Center=�6r   $
��20,0q�6�   $��Center=�6�   $	��120��6�   6�   5      5      �� 5   
   -   5         ��Name$ 	gbCaption��5      $3��-Обнаружена новая версия:��6&   $��0q�6H   $��Center=�6S   $��Center=�6p   .��5      �  5      �� 5       $
��Auto �6$   �� 5!      .+�5#      �� -   5        $ 
newVersion��5#        
newVersion#��[ 6,   $��0.0.0 #0��6P   �� 5$      -   5         ��Name$ 	tbContent��5$      $��5q�6/   $��1��6:   $
��Wrap=�6G   $��Information��6[   $
��Auto=�6n   $��WordEllipsis=�6�   5%      5&      5(      5)      