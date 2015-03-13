/*
 * FCKeditor - The text editor for internet
 * Copyright (C) 2003-2006 Frederico Caldeira Knabben
 * 
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * For further information visit:
 * 		http://www.fckeditor.net/
 * 
 * "Support Open Source software. What about a donation today?"
 * 
 * File Name: fckplugin.js
 * 	Plugin for transparent background
 *
 * 
 * File Authors:
 * 		Yogananthar Ananthapavan (rollbond@gmail.com)
 */

// Create the "Transparent" toolbar button
var isTransparent;
isTransparent = false;
var oTransparentItem = new FCKToolbarButton('Transparent', FCKLang.TransparentBtn, null, null, false, true);
oTransparentItem.IconPath = FCKPlugins.Items['transparent'].Path + 'transparent.gif';

FCKToolbarItems.RegisterItem('Transparent', oTransparentItem);

// The object used for all Transparent operations.
var FCKTransparent = new Object();

FCKTransparent = function(name){
	this.Name = name;
}

FCKTransparent.prototype.GetState = function() {
	if(FCKeditorAPI.GetInstance("richText").EditorDocument.body.style.backgroundColor == "transparent"){
		isTransparent = true;
	}
	return ( isTransparent ? FCK_TRISTATE_ON : FCK_TRISTATE_OFF );
}

FCKTransparent.prototype.Execute = function(){
	if(isTransparent == false){
		FCKeditorAPI.GetInstance("richText").EditorDocument.body.style.cssText += "BACKGROUND-COLOR: transparent";
		isTransparent = true;
		oTransparentItem.RefreshState();
	}
	else{
		FCKeditorAPI.GetInstance("richText").EditorDocument.body.style.cssText += "BACKGROUND-COLOR: #FFFFFF";
		isTransparent = false;
		oTransparentItem.RefreshState();
	}
}

// Register the related command
FCKCommands.RegisterCommand('Transparent', new FCKTransparent('transparent'));