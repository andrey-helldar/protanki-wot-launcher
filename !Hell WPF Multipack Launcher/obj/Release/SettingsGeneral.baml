   M S B A M L   `   `   `  ����   Multipack Launcher3    -_Hell_WPF_Multipack_Launcher.SettingsOptimize   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     N�Name$  PageSettingsGeneral�� N� Title$ SettingsOptimize��$
��Auto��$
��Auto��.��� .+��� $��10q�.��� $��0q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ 
gbOptimize��$	��Top=�$��0,5,0,0q�$.��(Оптимизация ПК и игры:��$��0��$
��Bold"�.��� $��5q�$	��Top=� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ cbKill��$H��BЗакрывать процессы при запуске игры��$
��Left=�$��10,5,0,0q�$	��Top=�$��Normal"��� -   $ cbForce��$D��>Принудительно закрывать процессы��$
��Left=�$��30,5,0,0q�$	��Top=�$��1��$��Normal"��� -   $ cbVideo��$D��>Уменьшить качество графики в игре��$
��Left=�$��10,5,0,0q�$	��Top=�$��2��$��Normal"��� -   $ cbAero��$%��Отключать Windows Aero��$
��Left=�$��10,5,0,0q�$	��Top=�$��4��$��Normal"��� -   $ cbWeak��$0��*Очень слабый компьютер��$
��Left=�$��30,5,0,0q�$	��Top=�$��3��$��Normal"��� -    ��Name$ 
cbPriority��$	��Top=�$��
10,10,10,5q� �� SelectedIndex$ 0��$��5��$��Normal"�.@��� $	��---���� ���	#FF8E8E8C$��1,0,0,1q�$
��Left=�$��12��$��16,2,0,0q�$	��Top=�$��11��$��1���� ���	#FF8E8E8C$��1,0,0,1q�$
��Left=�$��12��$��16,2,0,0q�$	��Top=�$��11��$��3���� -	   $ gbOther��$	��Top=�$��0,5,0,0q�$��Другое:��$��1��$
��Bold"�.��� $��5q� � ColumnDefinitions ��  �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -
   $ cbNotifyVideo��$3��-Уведомлять о новых видео��$��10,5,0,4q�$��Normal"��� -   $ cbNotifyNews��$9��3Уведомлять о новых новостях��$��10,6,0,3q�$��1��$��Normal"��� -   $ 
cbLauncher��$	��Top=�$��10q�$ 0��$��2��$��Normal"�.@��� $	��---���� -   $ gbInterface��$��Интерфейс��$��0q�$��2��.��� $��5q� �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   $ cbLang��$��
10,10,10,0q�$ 0��$��Normal"�$	��Top=�.@��� $	��---���� -   $ cbLangPriority��$��
10,0,10,10q�$��2��$	��Top=�$ 0��.@��� $	��---���� -    ��Name$ cbLangLocale��$G��AПриоритет определения локализации:��$
��10,0q�$	��Top=�$��1��