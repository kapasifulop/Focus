   M S B A M L   `   `   `  ���� @  <Focus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Focus.PropertiesFocus.Properties  clr-namespace:FocusFocus      Focus.MainWindow   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,pclr-namespace:Focus.Properties     %localclr-namespace:Focus     -     �Name$  MWindow�� � Title$ Focus�� � WindowStyle$
 None=�$	��200��$	��200�� � AllowsTransparency$
 True��
������ $
��2000��$
��2000�� �� BitmapScalingMode$ HighQuality=� � 	Resources    ResizeB �� 
TargetName �� TargetProperty W� From		 W� To
 g� Duration g� 	BeginTime  ResizeS 
 BStyle �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value N� EnterActions	 �� To N� ExitActions 
 IStyle 	 DGrid  DUSGrid �� Binding �� ElementName �� Path �� Value  BUSGrid  TUSGrid  FocusVisual �� StrokeDashArray J� SnapsToDevicePixels 	 OptionMarkFocusVisual  
 OptionMark.Static.Background �� Color  OptionMark.Static.Border  OptionMark.Static.Glyph # OptionMark.MouseOver.Background  OptionMark.MouseOver.Border  OptionMark.MouseOver.Glyph ! OptionMark.Pressed.Background  OptionMark.Pressed.Border  OptionMark.Pressed.Glyph " OptionMark.Disabled.Background  OptionMark.Disabled.Border  OptionMark.Disabled.Glyph  SettingsCboxes �� 
TargetType �Name � ColumnDefinitions ��Name  F�Name! J� Opacity" �� Stretch# ��Name$ ��Name% �� Triggers& �� 
TargetName��%e  &	        &	 �    &	 $    &	 w    &	 �    &	     &	 �    &	 �    &	 e    &		 �    &	
 	    &	 %	    &	 :	    &	 O	    &	 d	    &	 y	    &	 �	    &	 �	    &	 �	    &	 �	    &	 �	    &	 �	    &	 
    7  7 
 7  7  7  7 	 7  7  7  7  7  7  7  7  7  7  �� .��W� $ OGrid��$ Opacity� 1	 0$
 0:0:0.6D�W� $ 	SettingsV��$ Opacity� 0	 1$
 0:0:0.6D�W� $ MWindow��$ MaxWidth� 200	 800$
 0:0:0.3D�W� $ MWindow��$ Width� 200	 800$
 0:0:0.3D�W� $ MWindow��$ 	MaxHeight� 200	 500$
 0:0:0.3D� 0:0:0.3W� $ MWindow��$ Height� 200	 500 0:0:0.3$
 0:0:0.3D��� .��W� $ OGrid��$ Opacity� 0	 1$
 0:0:0.6D�W� $ 	SettingsV��$ Opacity� 1	 0$
 0:0:0.6D�W� $ MWindow��$ MaxWidth� 800	 200 0:0:0.3$
 0:0:0.3D�W� $ MWindow��$ Width� 800	 200 0:0:0.3$
 0:0:0.3D�W� $ MWindow��$ 	MaxHeight� 500	 200$
 0:0:0.3D�W� $ MWindow��$ Height� 500	 200$
 0:0:0.3D��� " ��.���  � �� �#dcdde1��  � ��$ 7q���  �@��CornerRadius$ 20�� P�  �@J�IsMouseOver$
 True�� �� .���� .����  #00a8ff$1 +(Border.Background).(SolidColorBrush.Color)�$
 0:0:0.2D� �� .���� .����  #f5f2f2$1 +(Border.Background).(SolidColorBrush.Color)�$
 0:0:0.5D��� " ��.���  � ��$ Center=���  � ��$ Center=���  �@��BitmapScalingMode$ HighQuality=���  � ��$ 10q��� " �.���  � �� �#f5f6fa P�  �@J�IsMouseOver$
 True�� �� .���� .����  #dcdde1$
 0:0:0.01D�$/ )(Grid.Background).(SolidColorBrush.Color)� �� .���� .����  #f5f6fa$
 0:0:0.5D�$/ )(Grid.Background).(SolidColorBrush.Color)��� " � ��  �� $ DesktopSettingsG��$ IsMouseOver� True �� .���� .��W� 	 200$ Height�$
 0:0:0.3D� �� .���� .��W�  200$ Height�$
 0:0:0.3D��� " � ��  �� $ DesktopSettingsB��$ IsMouseOver� True �� .���� .��W� 	 200$ Height�$
 0:0:0.3D� �� .���� .��W� 	 0$ Height�$
 0:0:0.3D��� " � ��  �� $ DesktopSettingsT��$ IsMouseOver� True �� .���� .��W� 	 200$ Height�$
 0:0:0.3D� �� .���� .��W� 	 0$ Height�$
 0:0:0.3D��� .���  � �� �� .R��� $��2q�$	 1 2R�$
 true��$��1��#��� ���� .���  � �� �� .R��� $��14,0,0,0q�$	 1 2R�$
 true��$��1��#��� ���� $ 	#FFFFFFFF���� $ 	#FF707070���� $ 	#FF212121���� $ 	#FFF3F9FF���� $ 	#FF5593FF���� $ 	#FF212121���� $ 	#FFD9ECFF���� $ 	#FF3C77DD���� $ 	#FF212121���� $ 	#FFE6E6E6���� $ 	#FFBCBCBC���� $ 	#FF707070���� " ��.���  �@�FocusVisualStyle8   ��  � ��8  ��  � ��8  ��  � ��# � ����  � ��$ 1q���  � �� �� " ��.R�� $ templateRoot��
������ $
 True�� �� $
��Auto ��� $��* �.+��� $ checkBoxBorder��#��z��#��z��#��z��#��z��$��1q�#��z��._�� $ markGrid��$��Center=�$��LeftToRight=�.+�F� $  
optionMark��Y�����4� �� Ѽ�G dԊ Ѩ�D dԊ �    ^>O ��z r�5 ��@ ��\ 
ܺ��� @�j�  �>��4� �� 8�� $��4q�$! 0Q�$
" None=��� $# indeterminateMark��8�� $��8q�$! 0Q��� $$ contentPresenter��$��1��$|�False��#��z��#��z��$
��True��# z #��z��% P�  � ��$
 true��. ��� 8   �@�FocusVisualStyle��  � ��$ 4,-1,0,0q�P�  �@J�IsMouseOver$
 true��. ��� $& checkBoxBorder��8   � ���� $& checkBoxBorder�� � �� �#3498db�� $& 
optionMark��8   � ���� $& indeterminateMark��8   � ��P�  � {�$ false��. ��� $& checkBoxBorder��8 	  � ���� $& checkBoxBorder��8 
  � ���� $& 
optionMark��8   � ���� $& indeterminateMark��8   � ��P�  � ��$
 true��. ��� $& checkBoxBorder��8   � ���� $& checkBoxBorder�� � �� �#2980b9�� $& 
optionMark��8   � ���� $& indeterminateMark��8   � ��P�  �@d�	IsChecked$
 trueY�. ��� $& 
optionMark�� �@J�Opacity$ 1Q��� $& indeterminateMark�� �@J�Opacity$ 0Q�P�  Z�  �@d�	IsChecked. ��� $& 
optionMark�� �@J�Opacity$ 0Q��� $& indeterminateMark�� �@J�Opacity$ 1Q�.��� .+��� ' �� CornerRadius$' 30��
��������� -   ���#bdc3c7$��50,7.5,50,0q�$	��Top=�$��Stretch=�$��6��$' 2��� -   $ 	SettingsV��$��15q�$y�Hidden=�( � RowDefinitions( �� $��50 ��� .+��� -   $��images\back.png��$��Stretch=�$
��Left=�$��10q��� $��1��$|�False��.���� ) �� Orientation$) Vertical=�$|�False��.+�� -   $ DesktopSettingsG��#���  ( �� $��80 ��� .+�� $��80��$��Center=� �� $��1* ��� $��5* �.+��� $��images/desktop.png��$ HighQuality=�$��10q�$��Center=��� $��Separated desktop icons��$��1��$��arial)�$��25(�$��Center=�$��10q�� -   $ DesktopSettingsUG��#���  $��20,0,0,0q����#dcdde1$��1��$��0��( �� �� �� .+�� $��10q� �� $��1* ��� $��5* �.+��� $��images/moon.png��$��10q�$ HighQuality=��� -   * ��Name$* MMCbox��#���  $��RightToLeft=�$��1��$��Center=�$
��Left=�+ d� 	IsChecked+ ��   Focus.Properties.Settings,   Default- �� Source#- Z, $ MDesktop�.���� $��LeftToRight=�$1��+Separated desktop icons during night mode: ��$��20(�$��arial)�� $��1��$��10q� �� $��1* ��� $��5* �.+��� $��images/game.png��$��10q�$ HighQuality=��� -   $* GMCbox��#���  $��RightToLeft=�$��1��$��Center=�$
��Left=�+ �� #- Z, $ GDesktop�.���� $��LeftToRight=�$0��*Separated desktop icons during game mode: ��$��20(�$��arial)�� $��2��$��10q� �� $��1* ��� $��5* �.+��� $��images/work.png��$��10q�$ HighQuality=��� -	   $* WMCbox��#���  $��RightToLeft=�$��1��$��Center=�$
��Left=�+ �� #- Z, $ WDesktop�.���� $��LeftToRight=�$0��*Separated desktop icons during work mode: ��$��20(�$��arial)�� -
   $ DesktopSettingsB��#���  ( �� $��80 ��� .+�� $��80��$��Center=� �� $��1* ��� $��5* �.+��� $��images/background.png��$ HighQuality=�$��10q�$��Center=��� $!��Separated background images��$��1��$��arial)�$��25(�$��Center=�$��10q�� -   $ BakcgroundSettingsUG��#���  $��20,0,0,0q����#dcdde1$��1��$��0��( �� �� �� .+�� $��10q� �� $��1* ��� $��5* ��� $��1* �.+��� $��images/moon.png��$��10q�$ HighQuality=��� -   $* MBCbox��#���  $��RightToLeft=�$��1��$��Center=�$
��Left=�+ �� #- Z, $ MBackground�.���� $��LeftToRight=�$5��/Separated background images during night mode: ��$��20(�$��arial)��� $��1��$��Bottom=�$
��Left=�$��10(����� #- Z, $	 MBg��� -   $ SMBFile�����#dcdde1$��10q�$��2��$
��Left=�._��� $��images/loupe.png��$ HighQuality=�� $��1��$��10q� �� $��1* ��� $��5* ��� $��1* �.+��� $��images/game.png��$��10q�$ HighQuality=��� -   $* GBCbox��#���  $��RightToLeft=�$��1��$��Center=�$
��Left=�+ �� #- Z, $ GBackground�.���� $��LeftToRight=�$4��.Separated background images during game mode: ��$��20(�$��arial)��� $��1��$��Bottom=�$
��Left=�$��10(����� #- Z, $	 GBg��� -   $ SGBFile�����#dcdde1$��10q�$��2��$
��Left=�._��� $��images/loupe.png��$ HighQuality=�� $��2��$��10q� �� $��1* ��� $��5* ��� $��1* �.+��� $��images/work.png��$��10q�$ HighQuality=��� -   $* WBbox��#���  $��RightToLeft=�$��1��$��Center=�$
��Left=�+ �� #- Z, $ WBackground�.���� $��LeftToRight=�$4��.Separated background images during work mode: ��$��20(�$��arial)��� $��1��$��Bottom=�$
��Left=�$��10(����� #- Z, $	 WBg��� -   $ SWBFile�����#dcdde1$��10q�$��2��$
��Left=�._��� $��images/loupe.png��$ HighQuality=�� -   $ DesktopSettingsT��#���  ���#e84118${�False��( �� $��80 ��� .+�� $��80��$��Center=� �� $��1* ��� $��5* �.+��� $��images/clock.png��$ HighQuality=�$��10q�$��Center=��� $��Timing��$��1��$��arial)�$��25(�$��Center=�$��10,10,10,20q��� $��available soon��$��1��$��arial)�$��15(�$��Bottom=�$��10,10,10,12q�� -   $ DesktopSettingsUT��#���  $��20,0,0,0q����#dcdde1$��1��$��0��.+�� ( �� ��  �� �� .+��� $��Start time:��$��Right=�$��Center=��� $��	End time:��$��Right=�$��1��$��Center=�� -   $ OGrid��$��15q�$y�Visible=�( �� ��  �� �� .+��� -   $ Night��#���     Activate��%T   &	       �� $ Night��.���� $ Background.Color� #4cd137$
 0:0:0.1D�._�� .+��� #���  $��images/moon.png���� -   $ Gaming��#���  $��1�� ��%U   &	       �� $ Gaming��.���� $ Background.Color� #4cd137$
 0:0:0.1D�._�� .+��� #���  $��images/game.png���� -   $ Working��#���  $��1�� ��%V   &	       �� $ Working��.���� $ Background.Color� #4cd137$
 0:0:0.1D�._�� .+��� #���  $��images/work.png���� -   $ Settings��#���  $' 20��$��1��$��1�� ��%T   &	       �� $ Night��.���� $ Background.Color� #4cd137$
 0:0:0.1D�._�� .+��� #���  $��images\settings.png��