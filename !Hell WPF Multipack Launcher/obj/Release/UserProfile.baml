   M S B A M L   `   `   `  ���� P  LMultipack Launcher, Version=2.9.48.931, Culture=neutral, PublicKeyToken=nullK*clr-namespace:ScottLogic.Controls.PieChartScottLogic.Controls.PieChart  5clr-namespace:ScottLogic.ShapesScottLogic.Shapes  .    (_Hell_WPF_Multipack_Launcher.UserProfile   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     8c*clr-namespace:ScottLogic.Controls.PieChart     -bclr-namespace:ScottLogic.Shapes     -     N� Title$  UserProfile��$
��Auto��$
��Auto�� � 	Resources ��  �� Type" ��   Keybrushes  .s���  �� Color$ 	#FF159927���� $ 	#FFFF7575���� $ 	#FF2679A8��.���  � ColumnDefinitions ��  � RowDefinitions �� �� $
��Auto �.+��� -    ��Name$ bClose��$��Закрыть��$��Right=�$��	0,0,10,30q�$��Bottom=�  ButtonCLOSE#���  $��1���� -    ��Name$ tcInfo��$��5,0,5,5q���Z� ��Z� .@��� $3��-Персональная информация��.���  �� $
��Auto ���  �� $
��Auto ��� .+��� $��Center=�$	��130��$	��Top=�$	��132��$��Resources/ava.png��$��0��$��40,0,0,0q��� $��1��$��15,0,0,0q�$��Bottom=�$��0��.+�� $��Bottom=� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $
��Left=�$��0��$��0q�$
��Wrap=�$��Center=�$
��Bold"�$��Arial)�$��18(����	#FF3A6696�� -    ��Name$ 
PlayerName��.�Guest �� -   $ 
PlayerClan���� $��2��.+�� $��0q� �� $
��Auto ��� �� $
��Auto ��� $
��Auto �.+��� -   	 ��Name$	 
ClanEmblem��$��0��$��32��$��32��$
��Left=�$��Center=�$��0,4,0,5q���Z� �� $
��Left=�$��1��$
��Wrap=�$��Center=����	#FF676767$��Arial)�$��10,7,0,6q��� -   $ PlayerClan2��.�---�� �� -   $ PlayerZvanie��.�---�� $��Right=�$��2��$
��Wrap=�$��Center=����	#FF676767$��Arial)�$��0,0,5,-1q�
 �� TextAlignment$
 Right=��� -	   $ 
PlayerGold�����	#FFFFD100$
��Bold"�.�---�� �� -
   $ PlayerCredit��.�---�� �� -   $ PlayerXP��.�---�� $��Right=�$��3��$
��Wrap=�$��Center=����	#FF676767$��Arial)�$	��0,1q��� $��Resources/gold.png��$��5,0,0,0q��� �� $��Resources/credit.png��$��5,0,0,0q��� �� $'��!Resources/currency-experience.png��$��5,0,0,0q��� $��1��$
��Left=�$
��0,10q�$	��Top=�$��Resources/line.png���� $��3��$
��Left=�$
��0,10q�$	��Top=�$��Resources/line.png���� -   $	 iAccountType��$��Right=�$��25��$��0q�$��Center=�$��25��$$��Resources/ico-account-base.png�� � ToolTip$# Базовый аккаунт���� $��2��$��0q�$��1��.+��  �� $
��Auto ��� $
��Auto �.+��� $��0q�.+��  �� $	��26* ��� $	��37* ��� $	��27* �.+��� $��Center=�$��0q�$
��Wrap=�$��Center=�$
 Center=��� $"��Resources/stat_perc_vins.png��$��50��$��50��$
��Fill=��� �� -   $ PercWins��  statcapt#���  .�Процент побед�� �� �� -   $ PercWinsPerc��  stattext#���  .�0�� #���  .�%�� $��Center=�$��1��$��0q�$
��Wrap=�$��Center=�$
 Center=��� $��Resources/stat_rating.png��$	��166��$	��165���� �� -   $ MyRating��#���  .�Личный рейтинг�� �� �� -   $ MyRatingPerc��#���  .�0�� $��Center=�$��2��$��0q�$
��Wrap=�$��Center=�$
 Center=��� $#��Resources/stat_perc_avgxp.png��$��50��$��50���� �� -   $ AvgXP��#���  .�%#Средний опыт за бой�� �� �� -   $ 	AvgXPPerc��#���  .�0�� $��0q�$��1��.+��  �� $��1* ��� $��1* �.+��� $��Center=�$��0q�$
��Wrap=�$��Center=�$
 Center=��� $%��Resources/stat_perc_battles.png��$��50��$��50���� �� -   $ BattleCount��#���  .�Количество боев�� �� �� -   $ BattleCountPerc��#���  .�0�� $��Center=�$��2��$��0q�$
��Wrap=�$��Center=�$
 Center=��� $'��!Resources/stat_perc_avgdamage.png��$��50��$��50���� �� -   $ 	AvgDamage��#���  .�:8Средний нанесенный урон за бой�� �� �� -   $ AvgDamagePerc��#���  .�0�� -    ��Name$ 
tiClanInfo��$%��Общая информация��.���  �� $
��Auto ��� .+��� $��0��$��0,10,0,0q�.+�� $��0,0,0,10q� �� $
��Auto ��� .+��� -   $	 ClanEmblem2��$��0��$��32��$��32��$
��Left=�$��0q�$	��Top=���Z� �� $
��Left=�$��1��$��10,0,0,0q�$
��Wrap=�$��Bottom=����	#FF676767$��Arial)��� -   $ ClanFullname��$
��Bold"�$
��12pt(�.�---�� �� -   $ ClanDesc��$	��9pt(�.�---�� -    ��Name$ gbSostav��$ ��Личный состав:��$��5,0,5,5q�$��1��.��� $��0q� �� �� $
��Auto � �� $
��Auto ��� .+��� $
��Left=�$��20,0,0,0q�$
��Wrap=�$��Bottom=��� $��Resources/soldiers.png��$��77��$��39��$
��Left=�$��Bottom=� �� $��Resources/(.png��$��8��$��24��$��Center=�$��Bottom=�$��0,0,0,5q� �� -   $ 	ClanCount��$��20(�$��Verdana)� �� BaselineAlignment$ Center=����	#FF3A6696$
��Bold"�.�0 �� $��Resources/).png��$��8��$��24��$��Center=�$��Bottom=�$��0,0,0,5q��� $��Resources/line1.png��$��2��$��Bottom=��� $��1��$��Right=�$��0,0,20,7q�$
��Wrap=�$��Bottom=����	#FF676767�� -   $ dataonTitle��.�Данные на �� -   $ dataon��.�
1.1.1970�� -    ��Name$ ClanMembers��$��0q�$��1��$��2��$	��Top=�$��Center=��� -    $ tiClanBattles��$��Список боев��.���  �� $
��Auto ��� .+��� -!   $ ClanBattles��$��0q�$��1��.@���   rec_not_found#���  .���� -"    ��Name$ ClanBattlesNoRecords��$H��BУ клана отсутствуют назначенные бои���� -#   $ 	tbBattles��$/��)Назначенные бои клана:�� 	 Title#���  �� -$   $ tiClanProvinces��$#��Список владений��.���  �� $
��Auto ��� .+��� -%   $ ClanProvinces��$��0q�$��1��.@��� #���  .���� -&   $ ClanProvincesNoRecords��$R��LУ клана нет провинций на Глобальной карте���� -'   $ tbProvinces��$$��Провинции клана:��#���  