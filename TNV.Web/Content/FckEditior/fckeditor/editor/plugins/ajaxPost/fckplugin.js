/*
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * File Name: fckplugin.js
 * 	Plugin to post the editor's cotent to the server through AJAX 
 * 
 * File Authors:
 * 		Paul Moers (http://www.saulmade.nl/FCKeditor/FCKPlugins.php)
 */

	// ajaxPostObject constructor
	var ajaxPostToolbarCommand = function()
	{
		var tempNode;

		if (FCKConfig.showDialog)
		{
			// preload dialog loading image
			tempNode = new Image(); 
			tempNode.src = FCKConfig.PluginsPath + "ajaxPost/images/loadingBig.gif"; 
		}
		else
		{
			// include plugin's javascript
			tempNode = document.createElement("script");
			tempNode.type = "text/javascript";
			tempNode.src = FCKConfig.PluginsPath + "ajaxPost/ajaxPost.js";
			document.getElementsByTagName("head")[0].appendChild(tempNode);
			// and the toggleFCKeditor function
			tempNode = document.createElement("script");
			tempNode.type = "text/javascript";
			tempNode.src = FCKConfig.PluginsPath + "ajaxPost/toggleFCKeditor.js";
			document.getElementsByTagName("head")[0].appendChild(tempNode);

			// preload toolbar loading image
			tempNode = new Image(); 
			tempNode.src = FCKConfig.PluginsPath + "ajaxPost/images/loadingSmall.gif";
		}
	}

	// Register the command
	FCKCommands.RegisterCommand('ajaxPost', new ajaxPostToolbarCommand());

	// Create the toolbar  button
	var ajaxPostButton = new FCKToolbarButton('ajaxPost', FCKLang.ajaxPostButton);
	ajaxPostButton.IconPath = FCKPlugins.Items['ajaxPost'].Path + 'images/ajaxPost.gif';
	FCKToolbarItems.RegisterItem('ajaxPost', ajaxPostButton);

	// manage the plugins' button behavior
	ajaxPostToolbarCommand.prototype.GetState = function()
	{
		return FCK_TRISTATE_OFF;
	}

	// what do we do when the butotn is clicked
	ajaxPostToolbarCommand.prototype.Execute = function()
	{
		if (FCKConfig.showDialog)
		{
			var dialog = new FCKDialogCommand('ajaxPost', FCKLang.ajaxPostDialogTitle, FCKPlugins.Items['ajaxPost'].Path + 'ajaxPost.html', 340, 205);
			dialog.Execute();
		}
		else
		{
			toggleFCKeditor(FCK);
			if (ajaxPostButton.DOMDiv) // fCKeditor 2.2-
			{
				toolbarButtonIcon = ajaxPostButton.DOMDiv.getElementsByTagName('IMG')[0];
			}
			else // FCKeditor 2.3+
			{
				toolbarButtonIcon = ajaxPostButton._UIButton.MainElement.getElementsByTagName('IMG')[0];
			}
			toolbarButtonIcon.src = FCKConfig.PluginsPath + "ajaxPost/images/loadingSmall.gif";

			// instantiate ajaxPost Object
			ajaxPostObject = new AxpObject(FCK);
			// giving the Object a reference to the toolbar button icon
			ajaxPostObject.toolbarButtonIcon = toolbarButtonIcon;
			// save
			ajaxPostObject.post();
		}
	}
