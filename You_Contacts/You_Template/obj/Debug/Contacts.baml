   M S B A M L   `   `   `  ����F  BYouInteract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\=clr-namespace:YouInteract.YouInteractAPI;assembly=YouInteractYouInteract.YouInteractAPI  5       You_Contacts   You_Contacts.Contacts   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      h dMicrosoft.Kinect.Toolkit.Controls, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35_ [Microsoft.Kinect.Toolkit, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e352k(http://schemas.microsoft.com/kinect/2013   5      Gu=clr-namespace:YouInteract.YouInteractAPI;assembly=YouInteract   5      -   5          N�Name$  YouContacts��5      #��� ��5
      #��� ��6\    � Cursor$
 Hand��6�    N� Title$ Contacts��5       � 	Resources 5         ChevronGeometry  ChevronRight �� Stretch
 �� Top �� FallbackValue �� Path �� RelativeSource	 �� AncestorType2   ,YouInteract.YouInteractAPI.KinectHoverButton
 J� RenderTransformOrigin �� Angle  ChevronLeft  KinectHoverButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value % !KinectHoverButtonScrollRightStyle �� BasedOn $  KinectHoverButtonScrollLeftStyle��%2  &	       &	 �    7   &	 Z   7   &	 0    &	 L    7  7  &	 �    7  7  B�5   
   miF1M150.001,146.9932L126.565,169.3822L126.565,153.5962L150.001,130.6182L173.435,153.5962L173.435,169.3822z�5      F� 5   
   $��48��6$   $��48��60   $ Uniform=�6;   $ 0��6M   8��  ��5      �� $ White��$ 
Foreground� �� "	  $
 0.5,0.5&�5   M   z�5      �� 5      $ 90Q�6"   5      F� 5   
   $��48��6#   $��48��6/   $ Uniform=�6:   $ 0��6L   8��  ��5      �� $ White��$ 
Foreground� �� "	  $
 0.5,0.5&�5   M   z�5      �� 5       $	 270Q�6"   5"      �� 5$   
   "  6/   .�5%      ��  � ��
 �����6+   �� 5&       � ��
 �����6,   �� 5'       � ��$ 0q�60   6<   �� 5(       � ��
 �����6;   �� 5)       � �� 5*      �� 5+      "  6&   .R�5,      � $��Stretch=�6   $��Stretch=�6=   #��z��6Y   #��z��6�   #��z��6�   .+�5-      �� #��z��6%   #��z��6Y   ._�5.   "   �� $��Stretch=�63   $��Stretch=�6Q   #��z��6m   6�   5/      50      51      53       54      P� 55       � {� .  . �56      ��  � ��
 �����63   �� 57       � ��
 �����64   �� 58       � ��
 ���� 6I   59      P� 5:       �@ IsHandPointerOver . . �5;      ��  � �� �#5A5B5B63   �� 5<       � ��
 �����63   5=      5?      �� 5A   
   "  6:   8   .�5B      ��  � ��8  6(   5C      �� 5E   
   "  69   8   .�5F      ��  � ��8  6(   5G      5H      .��5J      � .+�5K   
   4  .Microsoft.Kinect.Toolkit.Controls.KinectRegion  -   5          Name$ YouContactsRegion��5K      .��5L      �� -   5         ��Name$ YouContactsCanvas��5L      .+�5M      �� -   5         ��Name$ Titulo��5M      $
��Fill=�6(   *   $YouInteract.YouInteractAPI.YouButton  5N      -   5          Name$ MainMenuButton��5N      ��5O      Z� ��64   Z� ��6K   Z� 8  2Microsoft.Kinect.Toolkit.Controls.KinectTileButton   LabelBackground 6a   Z� .��5P      �� $F��@/You_Contacts;component/Images/Themes/Theme1/Navigation/back.png��6   $
��Fill=�6f   5Q      :  4Microsoft.Kinect.Toolkit.Controls.KinectScrollViewer  5S      -   5        $��ScrollViewer��5S   '   $��Hidden=�6;   $��Disabled=�6b   .��5T      � -   5         �Name$ WrapScrollPanel��5T        � Orientation$ Vertical=�69   5U        5W      -   5          Name$ ScrollRight��5W   &   #��[ 6;   ���#327b9b{�6�   ��  �� ElementName$ ControlsBasicsWindow��$ PageRightEnabled�6�     5X      -	   5        $ 
ScrollLeft��5X   &   #��[ 6:   ���#327b9b{�6�   �� $ ControlsBasicsWindow��$ PageLeftEnabled�6�   5Z      5[      ��5\   
   �� 5]       �� ImageSource$L F/You_Contacts;component/Images/Themes/Theme1/Background/background.jpg��6   6p   5_      5`      