   M S B A M L   `   `   `  ????D  @CLI_TImer, Version=0.2.1.0, Culture=neutral, PublicKeyToken=null%clr-namespace:CLI_TImer	CLI_TImer  5      C&clr-namespace:CLI_TImer.MVVM.ViewModelCLI_TImer.MVVM.ViewModel  5      9!clr-namespace:CLI_TImer.MVVM.ViewCLI_TImer.MVVM.View  5          CLI_TImer.App   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      )localclr-namespace:CLI_TImer     5      <	viewModel&clr-namespace:CLI_TImer.MVVM.ViewModel     5      2view!clr-namespace:CLI_TImer.MVVM.View     5        ?? 	Resources  5       ?? DataType4  .CLI_TImer.MVVM.ViewModel.MainSettingsViewModel*   $CLI_TImer.MVVM.View.MainSettingsView   Pointer ?? 
TargetType ?? Property ?? Value 
 Output  FocusVisual ?? StrokeDashArray J? SnapsToDevicePixels  Button.Static.Background ?? Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border 	 Button.Disabled.Background 
 Button.Disabled.Border  Button.Disabled.Foreground  FocusVisual1  Item.MouseOver.Background  Item.MouseOver.Border " Item.SelectedActive.Background  Item.SelectedActive.Border $  Item.SelectedInactive.Background   Item.SelectedInactive.Border  CommandHistoryItemStyle ?? RelativeSource	 ?? AncestorType
 ?? 
TargetType ??Name ?? Padding ?? Triggers ]? 
Conditions ?? Property ?? Value ?? 
TargetName P? Property P? Value  CloseButtonStyle ?? CornerRadius ??Name??%?  (??       5
      *, +)&	  .     &	     &	 n    &	 !    &	 D    &	 g    &	 ?    &	 ?    &	 ?    &		 ?    &	
     &	 9    &	 \    &	     &	 4    &	 Y    &	 ~    &	 ?    &	 ?    &	 ?    7  7  7  7  7  7  7  &	 ?
    7  7  7  7  7  7  7  7 	 7 
 7  ?? 6
   "  6   .R?5        5      ?? 5   
   " ??6    .?5      ??  ? ??$ /Fonts/#NanumGothic)?5      5      ?? 5       ? ??
 ?????5      ?? 5       ? ??$ 12(?5      ?? 5       ? ??$ Center=?5      5      ?? 5   
   " ??6   .?5       ??  ? ??$ /Fonts/#Chivo Mono)?5!      5"      ?? 5#   
   .?5$      ??  ? ?? 5%      ?? 5&      .R?5'      ?? $??2q?6$   $	 1 2R?6/   #??? ??6E   $
 true??6?   $??1??6?   5(      5*      5+      ?? 5-   
   $ 	#FFDDDDDD??6;   ?? 5.   
   $ 	#FF707070??67   ?? 5/   
   $ #20222e??6>   ?? 52   
   $ Transparent??6:   ?? 53   
   $ #191b24??6<   ?? 55   
   $ Transparent??68   ?? 56   
   $ 	#FFF4F4F4??6=   ?? 57   
   $ 	#FFADB2B5??69   ?? 58   
   $ 	#FF838383??6=   ?? 5<   
   .?5=      ??  ? ?? 5>      ?? 5?      .R?5@      ?? $??2q?6$   $	 1 2R?6/   #??? ??6E   $
 true??6?   $??1??6?   5A      5C      5D      ?? 5E   
   $ Transparent??6<   ?? 5F   
   $ Transparent??68   ?? 5G   
   $ Transparent??6A   ?? 5H   
   $ Transparent??6=   ?? 5I   
   $ Transparent??6C   ?? 5J   
   $ Transparent??6?   ?? 5K   
   " ??60   .?5L      ??  ?@J?SnapsToDevicePixels$
 True??64   ?? 5M       ? ??$	 4,1q?6(   ?? 5N       ? ?? 6;   ?? *HorizontalContentAlignment+ ?? "	 ???? 5O       ? ?? 69   ?? *VerticalContentAlignment+ ?? "	 ???? 5P       ? ??
 ???? 6+   ?? 5Q       ? ??
 ???? 6,   ?? 5R       ? ??$ 1q?60   ?? 5S       ?@?FocusVisualStyle8   61   ?? 5T       ? ?? 5U      ?? 5V      "
 ??6&   .R?5W      ?? $ Bd??6!   #??z??6-   #??z??6W   #??z??6?   # z??6?   $
 true??6?   ._?5X      ?? #??z??6/   # z 6r   #??z??6?   5Y       5Z      ]? 5[       5\   "   ?? 5]   &    ?@J?IsMouseOver$
 True??6G   ./?5_   "   ?? $ Bd??6?   8   ? ??6O   ?? 5`   "   $ Bd??6@   8   ? ??6P   5a      ]? 5b       5c   "   ?? 5d   &    ?@??IsSelectionActive$ False??6V   ?? 5e   &    ?@??
IsSelected$
 True??6F   ./?5g   "   ?? $ Bd??6?   8   ? ??6O   ?? 5h   "   $ Bd??6@   8   ? ??6P   5i      ]? 5j       5k   "   ?? 5l   &    ?@??IsSelectionActive$
 True??6V   ?? 5m   &    ?@??
IsSelected$
 True??6F   ./?5o   "   ?? $ Bd??6?   8   ? ??6O   ?? 5p   "   $ Bd??6@   8   ? ??6P   5q      P? 5r       ? {?$ False??6;   . ?5s   "   ?? $ Bd??6K   # ? ??6[    ? ??5t      5v      5x      5y      ?? 5z   
   " ??6)   .?5{      ??  ?@?FocusVisualStyle8   61   ?? 5|       ? ??8  6+   ?? 5}       ? ??8  6,   ?? 5~       ? ??# ? ??6+   ?? 5       ? ??$ 0q?60   ?? 5?       ? ??$ Center=?6;   ?? 5?       ? ??$ Center=?69   ?? 5?       ? ??$ 1q?6(   ?? 5?       ? ?? 5?      ?? 5?      "
 ??6&   .R?5?      ?? $ border??6!   #??z??61   #??z??6[   #??z??6?   $
 true??6?   $ 0, 10, 0, 0??6?   ._?5?      ?? $ contentPresenter??6/   $|?False??6I   #??z??6[   #??z??6?   $
??True??6?   # z 6?   #??z??6  5?       5?      P? 5?       ?@??IsDefaulted$
 true??6=   . ?5?   "   ?? $ border??6@   # ? ??6T    ? ??5?      P? 5?       ?@J?IsMouseOver$
 true??6=   . ?5?   "   ?? $ border??6?   8   ? ??6S   ?? 5?   "   $ border??6@   8   ? ??6T   5?      P? 5?       ? ??$
 true??6;   . ?5?   "   ?? $ border??6?   8   ? ??6S   ?? 5?   "   $ border??6@   8   ? ??6T   5?      P? 5?       ? {?$ false??6;   . ?5?   "   ?? $ border??6?   8   ? ??6S   ?? 5?   "   $ border??6@   8   ? ??6T   ?? 5?   "   $ contentPresenter??6K   8 	  ? ??6i   5?      5?      5?      5?      5?      5?      