/*
* FCKEditor OnlineVideo Plugin - Plugin
* Publisher Host (c) Creative Commons 2008
* http://creativecommons.org/licenses/by-sa/3.0/
* Author: Michael James | http://www.mjjames.co.uk
*/

// Register the related commands.
FCKCommands.RegisterCommand('OnlineVideo', new FCKDialogCommand(FCKLang['DlgOnlineVideoTitle'], FCKLang['DlgOnlineVideoTitle'], FCKConfig.PluginsPath + 'OnlineVideo/OnlineVideo.html', 450, 350));

// Create the "OnlineVideo" toolbar button.
var oFindItem		= new FCKToolbarButton( 'OnlineVideo', FCKLang['OnlineVideoTip'] ) ;
oFindItem.IconPath	= FCKConfig.PluginsPath + 'OnlineVideo/OnlineVideo.gif' ;

FCKToolbarItems.RegisterItem( 'OnlineVideo', oFindItem ) ;			// 'OnlineVideo' is the name used in the Toolbar config.
