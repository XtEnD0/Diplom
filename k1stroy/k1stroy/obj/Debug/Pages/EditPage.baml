   M S B A M L   `   `   `  ����B  >k1stroy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:k1stroy.Pagesk1stroy.Pages  5          k1stroy.Pages.EditPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:k1stroy.Pages     5        N� Title$"  Редактирование��5	      .��5      �  � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      6   �� 5      $��50 �6   6*   .+�5   
   �� -   5         ��Name$ 
BackButton��5      $��1��6   $��
Назад��62   $��10,0,0,0q�6B   $��18(�5   *   $	��100��68   $��30��6D   $��Center=�5      $
��Left=�6,   6I   �� 5   
   -   5        $ 	DelButton��5   .   $��1��6   $��1��6   $��Удалить��6A   $��0,0,10,0q�6S   $��18(�5   *   $	��100��68   $��30��6D   $��Center=�5      $��Right=�6,   6J   �� 5   
   $��0��6   $��0��6%   $	��250��62   $��Center=�5      $��Center=�60   .+�5      �� $��Наименование:��6   $��0,0,0,0q�6,   6?   �� 5       -   5         ��Name$ NameTB��5       $��30��6'    �� TextWrapping$
 Wrap=�63   $��Center=�5!      $��Center=�68   6^   �� 5#      $!��Производитель:��6   $��0,10,0,0q�6.   6B   �� 5$      -   5         ��Name$ ManufacturerComboBox��5$       �� DisplayMemberPath$ ManufacturerName��65   $��30��6[   $��Center=�5%      $��Center=�69   5&      �� 5(      $��Категория:��6   $��0,10,0,0q�6*   6>   �� 5)      -   5        $ 
CategoryCB��5)      $��Center=�6+   $��Center=�5*      $ CategoryName��5+      $��30��5,      5-      �� 5/      $��Тип продукта:��6   $��0,10,0,0q�6-   6A   �� 50      -   5        $ TypeCB��50      $��30��6(   $ TypeName��51      $��Center=�52      $��Center=�53      6=   �� 55      $��Артикул:��6   $��0,10,0,0q�6(   6<   �� 56      -   5        $ 	ArticleTB��56      $��30��6*   $
 Wrap=�66   $��Center=�57      $��Center=�68   6^   �� 59      $��Стоимость:��6   $��0,10,0,0q�6*   6>   �� 5:      -   5        $ PriceTB��5:      $��30��6(   $
 Wrap=�64   $��Center=�5;      $��Center=�68   6^   5?      �� 5B   
   $��1��6   $��0��6%   $	��250��62   $��Center=�5C      $��Center=�60   .+�5E      �� $-��'Количество на складе:��6   $��0,0,0,0q�65   6H   �� 5F      -	   5        $ 	InStockTB��5F      $��30��6*   $
 Wrap=�66   $��Center=�5G      $��Center=�68   6^   �� 5I      $��Описание:��6   $��0,10,0,0q�6)   6=   �� 5J      -
   5        $ DescTB��5J      $��30��6'   $
 Wrap=�63   $��Center=�5K      $��Center=�68   6^   �� 5M      $#��Характеристики:��6   $��0,10,0,0q�6.   6B   �� 5N      -   5        $ CharTB��5N      $��30��6&   $
 Wrap=�62   $��Center=�5O      $��Center=�68   6^   �� 5Q      $��Изображение:��6   $��0,10,0,0q�6,   6@   �� 5R      -   5         ��Name$ ProductPhotoSelecter��5S      $	��150��5R      $	��120��6    $
��Fill=�6-   	 � Cursor$
	 Hand��6<   ��5T      �� 5U      
 �� Path$
 Photo�6    �� TargetNullValue 5V      �� 5W       �� 	UriSource$ /Res/imagenull.png?�6*   6K   5Y      5[      �� 5\      -   5        $ 
SaveButton��5\      $��0,10,0,0q�6)   $��30��6<   $��Сохранить��5]      6D   5_      5a      5b      