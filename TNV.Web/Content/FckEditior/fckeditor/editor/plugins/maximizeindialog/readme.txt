MaximizeInDialog plugin
Written By Jan Meijs, 2006
jamit@users.sourceforge.net


FCK Editor - Written By Frederico Caldeira Knabben
http://www.fckeditor.net 


Installation Manual:
===================

Dowload and copy
----------------
Installation is quite simple. After download, copy the
MaximizeInDialog directory to the plugin directory of your fckEditor.

fckconfig.js
------------
1. Edit your toolbar and add the button:	'MaximizeInDialog'
2. Add these 6 lines:
	// --- config settings for the MaximizeInDialog plugin ---
	FCKConfig.Plugins.Add( 'MaximizeInDialog', 'de,en,fr,nl' ) ;
	FCKConfig.MaximizeInDialog_ScreenWidth = FCKConfig.ScreenWidth * .9 ;	//90% of the screen Width
	FCKConfig.MaximizeInDialog_ScreenHeight = FCKConfig.ScreenHeight * .9 ;	//90% of the screen Height
	FCKConfig.MaximizeInDialog_ShowEditorName = false ;	//show editor name in title of Dialog window (Yes:true - No:false)
	FCKConfig.MaximizeInDialog_ToolbarSet = '' ;	//use this toolbar for the editor in the Dialog window ('':same toolbar)
3. The height and width of the Dialog window can be modified by changing 
FCKConfig.FitMaximizeInDialog_ScreenWidth and FCKConfig.FitMaximizeInDialog_ScreenHeight:
	e.g.	FCKConfig.FitMaximizeInDialog_ScreenWidth = 400 ;
		FCKConfig.FitMaximizeInDialog_ScreenHeight = 300 ;
4. If you want to show the name of the editor that is being maximized in the Dialog window,
set FCKConfig.MaximizeInDialog_ShowEditorName = true;
5. If you don't want to use the same toolbar in the Dialog window as in the opener page,
you need to set FCKConfig.MaximizeInDialog_ToolbarSet to an existing toolbarSet.
	e.g.	FCKConfig.MaximizeInDialog_ToolbarSet = 'Basic';


MaximizeInDialog.html
---------------------
If you don't use the default configuration in the fckconfig.js, but change
the config while creating the original instance (e.g. in PHP or ASP), you need
to change the HTML file. Change the items in the loadConfig array to
define which config items should be loaded from the original instance and copied
in the MaximizeInDialog instance.


Behavior:
========
The plugin creates a button in the specified toolbar that opens a new maximized fckeditor
in a Dialog window with the content of the opener fckeditor. 
After modifications have been made, a click on the OK button transfers the content from
the maximized fckeditor to the original opener fckeditor. A click on the cancel button
ignores the content of the maximized fckeditor and leaves the opener fckeditor untouched.
Since the MaximizeInDialog button has no function in the MaximizeInDialog window, 
it has been disabled.
The MaximizeInDialog window has been given the same look-and-feel as the standard Dialog
windows of the fckeditor (title, buttons, skins,...) to improve usability.

That's all folks... enjoy if you want... improve if you can... :-)


Licence:
========
LGPL - Lesser General Public License (see LICENSE.txt file)
