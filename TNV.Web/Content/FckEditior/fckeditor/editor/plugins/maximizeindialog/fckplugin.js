/*
 * FCKeditor - The text editor for internet
 * Copyright (C) 2003-2005 Frederico Caldeira Knabben
 * 
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * For further information visit:
 * 		http://www.fckeditor.net/
 * 
 * Version:  1.0
 * 
 * File Author:
 * 		Jan Meijs (jamit@users.sourceforge.net)
 */

/* ---------------------------------------------------------------- Command(s) ------------------------------------------------------------- */
// Define the command
var FCKMaximizeInDialogCommand = function(name) 
{ 
	this.Name = name; 
}

// Execute the command
FCKMaximizeInDialogCommand.prototype.Execute = function() 
{ 
	var oDialogInfo = new Object() ;
	oDialogInfo.Editor = window;

	var dialogTitleDescr = '' ;
	if (FCKConfig.MaximizeInDialog_ShowEditorName) {
		dialogTitleDescr = FCK.Name ;
	}
	dialogTitleDescr = (dialogTitleDescr!='')?'"'+dialogTitleDescr+'"':dialogTitleDescr ;
	var dialogTitle = FCKLang.DlgMaximizeInDialogTitle.replace(/%1/g,dialogTitleDescr) ;
	FCKDialog.OpenDialog( 'MaximizeInDialog', dialogTitle, FCKConfig.PluginsPath + 'MaximizeInDialog/MaximizeInDialog.html?name=' + FCK.Name, FCKConfig.MaximizeInDialog_ScreenWidth, FCKConfig.MaximizeInDialog_ScreenHeight, window ) ;
} 
 
// Manage the plugins' button behavior 
FCKMaximizeInDialogCommand.prototype.GetState = function() 
{ 
  if (FCK.Name=='editorMaximizeInDialog') {
	return FCK_TRISTATE_DISABLED ; 
  } else {
	return FCK_TRISTATE_OFF ; 
  }
} 

// Register the related command.
FCKCommands.RegisterCommand( 'MaximizeInDialog', new FCKMaximizeInDialogCommand('MaximizeInDialog'));


/* ---------------------------------------------------------------- Button ----------------------------------------------------------------- */
// Create the "MaximizeInDialog" toolbar button.
var oMaximizeInDialogItem = new FCKToolbarButton( "MaximizeInDialog", FCKLang.MaximizeInDialog ) ;
oMaximizeInDialogItem.IconPath = FCKConfig.PluginsPath + 'MaximizeInDialog/MaximizeInDialog.gif' ;

// 'MaximizeInDialog' is the name used in the Toolbar config.
FCKToolbarItems.RegisterItem( 'MaximizeInDialog', oMaximizeInDialogItem ) ;
