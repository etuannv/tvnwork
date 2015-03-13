/*
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * File Name: ajaxPost.js
 * 	ajaxPost Object.
 * 
 * File Authors:
 * 		Paul Moers (http://www.saulmade.nl, http://www.saulmade.nl/FCKeditor/FCKPlugins.php)
*/


	// AxpObject constructor
	var AxpObject = function (editorInstance)
	{
		this.editorInstance		= editorInstance;
		this.FCKConfig			= editorInstance.Config;
		this.FCKLang				= editorInstance.EditorWindow.parent.FCKLang;
	}


	// initialize
	AxpObject.prototype.initialize = function ()
	{
		parentObject = this;

		// create requestObject
		if (window.XMLHttpRequest) // Mozilla, Safari, IE7, ...
		{
			requestObject = new XMLHttpRequest();
		}
		else if (window.ActiveXObject) // IE
		{
			requestObject = new ActiveXObject('MsXml2.XmlHttp');
		}
		this.requestObject = requestObject;

		// set function to do on completion of the request
		requestObject.onreadystatechange = this.onReadyStateChange;
	}


	// post
	AxpObject.prototype.post = function ()
	{
		// set up the requestObject
		this.initialize();

		// make request
		requestObject.open('POST', this.FCKConfig.ajaxPostTargetUrl, true);
		requestObject.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
		requestObject.send('action=save&content=' + encodeURIComponent(this.editorInstance.GetXHTML()));
	}


	// the readystatechange event
	AxpObject.prototype.onReadyStateChange = function ()
	{
		if (requestObject.readyState == 4)
		{
			if (requestObject.status == 200)
			{
				// error node available?
				if (errorNode = requestObject.responseXML.getElementsByTagName('error')[0])
				{
					parentObject.feedback(errorNode.attributes.getNamedItem('errorNumber').value, errorNode.attributes.getNamedItem('errorData').value);
				}
				// success
				else if (resultNode = requestObject.responseXML.getElementsByTagName('result')[0])
				{
					parentObject.feedback(0);
				}
				else
				{
					parentObject.feedback(104, parentObject.FCKLang.ajaxPostRequestedURL + ': ' + parentObject.FCKConfig.ajaxPostTargetUrl + '<br />' + parentObject.FCKLang.ajaxPostResponseText + ':<br />' + requestObject.responseText);
				}
			}
			else
			{
				parentObject.feedback(105, requestObject.statusText + ' (' + requestObject.status + ')');
			}
		}
	}


	// feedback
	AxpObject.prototype.feedback = function (errorNumber, errorData)
	{
		// set toolbar icon and re-enable editor
		if (!this.FCKConfig.showDialog)
		{
			toggleFCKeditor(this.editorInstance);

			if (this.toolbarButtonIcon)
			{
				if (parseInt(errorNumber) > 0)
				{
					this.toolbarButtonIcon.src = this.toolbarButtonIcon.src.replace(/[^/]*$/, 'cross_animated.gif');
				}
				else
				{
					this.toolbarButtonIcon.src = this.toolbarButtonIcon.src.replace(/[^/]*$/, 'tick_animated.gif');
				}
			}
		}

		switch (parseInt(errorNumber))
		{
			case 0 :		this.setMessage(this.FCKLang.ajaxPostSaveCompleted);
							this.editorInstance.ResetIsDirty();
							if (this.FCKConfig.showDialog)
							{
								this.getDialogCancelButton(parent.document.getElementById('btnOk').parentNode).value = this.FCKLang.DlgBtnOK;
							}
							break;
			case 101 :	this.setMessage(this.FCKLang.ajaxPostNoContentReceived, errorData);
							break;
			case 102 :	this.setMessage(this.FCKLang.ajaxPostDBConnectError, errorData);
							break;
			case 103 :	this.setMessage(this.FCKLang.ajaxPostQueryError, errorData);
							break;
			case 104 :	this.setMessage(this.FCKLang.ajaxPostErrMssgBadXMLResponse, errorData);
							break;
			case 105 :	this.setMessage(this.FCKLang.ajaxPostErrMssgXMLRequestError, errorData);
							break;
			default :	this.setMessage(this.FCKLang.ajaxPostErrMssgDefault + ' ' + errorNumber, errorData);
							break;
		}

		if (!this.FCKConfig.showDialog)
		{
			if (this.toolbarButtonIcon)
			{
				if (parseInt(errorNumber) > 0)
				{
					setTimeout(this.resetToolbarButton, 12000);
				}
				else
				{
					setTimeout(this.resetToolbarButton, 5000);
				}
			}
		}
	}


	// set message
	AxpObject.prototype.setMessage = function (errorMessage, errorData)
	{
		var message;

		if (this.FCKConfig.showDialog)
		{
			message = errorMessage + (errorData ? '<br /><br />' + errorData : '');
			document.getElementById("contentCell").innerHTML = message;
		}
		else
		{
			message = errorMessage + (errorData ? ' ' + errorData : '');
			if (this.toolbarButtonIcon)
			{
				this.toolbarButtonIcon.title = this.toolbarButtonIcon.alt = message;
			}
			else
			{
				alert(message);
			}
		}
	}


	// reset the toolbar button
	AxpObject.prototype.resetToolbarButton = function ()
	{
		this.toolbarButtonIcon.src = this.toolbarButtonIcon.src.replace(/[^/]*$/, 'ajaxPost.gif');
		this.toolbarButtonIcon.title = this.toolbarButtonIcon.alt = this.toolbarButtonIcon.parentNode.parentNode.title;
	}

	// get the cancel button
	AxpObject.prototype.getDialogCancelButton = function (container)
	{
		for (i = 0; childNode = container.childNodes[i]; i++)
		{
			if (childNode.getAttribute && childNode.getAttribute("fcklang") == "DlgBtnCancel")
			{
				return childNode;
			}
		}
	}

