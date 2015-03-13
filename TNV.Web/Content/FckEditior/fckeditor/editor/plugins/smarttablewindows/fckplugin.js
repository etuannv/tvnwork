/*
 * FCKeditor - The text editor for internet
 * Copyright (C) 2003-2004 Frederico Caldeira Knabben
 * 
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * For further information visit:
 * 		http://www.fckeditor.net/
 * 
 * File Name: fckplugin.js
 * 	This is the plugin definition file.
 * 
 * Version:  2.1 RC1
 * Modified: 2007-03-29 11:20:10
 * 
 * File Authors:
 *		Tom Vergult (www.smartlounge.be)
 */

FCKCommands.RegisterCommand('smarttablewindows', new FCKMaximizeInDialogCommand('smarttablewindows'));

// Overwrite the functions of the standard table buttons       
FCKCommands.RegisterCommand('Table', new FCKDialogCommand(
        FCKLang['SmartTableWindowTitle'],
        FCKLang['SmartTableWindowTitle'],
        FCKConfig.PluginsPath + 'smarttablewindows/stw_table.html', 500, 275));
    
FCKCommands.RegisterCommand('TableProp', new FCKDialogCommand(
        FCKLang['SmartTableWindowTitle'],
        FCKLang['SmartTableWindowTitle'],
        FCKConfig.PluginsPath + 'smarttablewindows/stw_table.html?Parent', 500, 275));
            
FCKCommands.RegisterCommand('TableCellProp', new FCKDialogCommand(
        FCKLang['SmartTableCellWindowTitle'],
        FCKLang['SmartTableCellWindowTitle'],
        FCKConfig.PluginsPath + 'smarttablewindows/stw_tablecell.html', 630, 320));