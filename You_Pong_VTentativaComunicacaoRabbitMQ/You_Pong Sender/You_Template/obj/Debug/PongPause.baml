   M S B A M L   `   `   `  ����F  BYouInteract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\=clr-namespace:YouInteract.YouInteractAPI;assembly=YouInteractYouInteract.YouInteractAPI  5       You_Pong   You_Pong.PongPause   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      h dMicrosoft.Kinect.Toolkit.Controls, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35_ [Microsoft.Kinect.Toolkit, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e352k(http://schemas.microsoft.com/kinect/2013   5      Gu=clr-namespace:YouInteract.YouInteractAPI;assembly=YouInteract   5      -   5          N�Name$  YouPongPause��5      #��� ��5	      #��� ��6\    � Cursor$
 Hand��6�    N� Title$ 	PongPause��5      .��5      � 
���   �.+�5   
   4  .Microsoft.Kinect.Toolkit.Controls.KinectRegion  -   5          Name$ YouPongPauseRegion��5      .��5      �� -   5         ��Name$ PongPauseCanvas��5      .+�5      *   $YouInteract.YouInteractAPI.YouButton  -   5          Name$ Resume��5      ��6E   Z� ��6\   Z� ��6r   Z� 8  2Microsoft.Kinect.Toolkit.Controls.KinectTileButton   LabelBackground 6�   Z� .��5      �� $>��8/You_Pong;component\Images\Themes\Theme1\Pong\resume.png��6   $
��Fill=�6^   6o   5        5      -   5        $
 Quit��5         Label$ Return to Main Menu��6B   ��6^   Z� ��6u   Z� ��6�   Z�  6�   Z� .��5      �� $<��6/You_Pong;component\Images\Themes\Theme1\Pong\quit.png��6   $
��Fill=�6\   6m   5        5      -   5        $ Restart��5      $ Restart Game��6E   ��6Z   Z� ��6q   Z� ��6�   Z�  6�   Z� .��5      �� $?��9/You_Pong;component\Images\Themes\Theme1\Pong\restart.png��6   $
��Fill=�6_   6p   5      A� 5      -   5        $��PBall��5      
�������
���   � J� Opacity$	 0.3Q�6H   �� 5      -   5        $��P1��5      
�������$	 0.3Q�68   �� 5       -	   5        $��P2��5       
�������$	 0.3Q�68   �� 5"      -
   5        $��
pauseblock��5"      
���   �$��2q�6?   $	��Top=�6S   	 �� CornerRadius$	 8��6k   
���   �._�5#      �� -   5        $��PauseMsg��5#       $��GAME PAUSED��60   $��60(�6C   
������ 
�������$)��#/You_Pong;component/#Press Start 2P)�6}   5$      5&      5'      5(      5)      