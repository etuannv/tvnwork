/*
* FCKEditor OnlineVideo Plugin
* Publisher Host (c) Creative Commons 2008
* http://creativecommons.org/licenses/by-sa/3.0/
* Author: Michael James | http://www.mjjames.co.uk
*/

var dialog = window.parent;
var oEditor = dialog.InnerDialogLoaded();
var FCK = oEditor.FCK;
var FCKLang = oEditor.FCKLang;
var FCKConfig = oEditor.FCKConfig;
var FCKDebug = oEditor.FCKDebug;
var FCKTools = oEditor.FCKTools;

//#### Dialog Tabs

// Set the dialog tabs.
dialog.AddTab('Info', oEditor.FCKLang.DlgInfoTab);

// Get the selected flash embed (if available).
var oFakeImage = FCK.Selection.GetSelectedElement();
var oEmbed;
var ePreview;

if (oFakeImage) {
	if (oFakeImage.tagName == 'IMG' && oFakeImage.getAttribute('_fckflash')) {
		oEmbed = FCK.GetRealElement(oFakeImage);
	}
	else {
		oFakeImage = null;
	}
}


function GetVideoURLFromEmbedSrc(url) {
	var embedUrl;
	
	//work out if we are a youtube url etc
	if (url.indexOf("youtube.com/") > -1) {
		//dirty replace to get the original ID
		id = url.replace('http://www.youtube.com/v/', '').replace('%26hl=en%26fs=1%26rel=0', '');
		
		
		//if hd replace out and then set the hd flag
		if (url.indexOf('%26ap=%2526fmt=18') > -1){
			id = id.replace('%26ap=%2526fmt=18', '');
			GetE('radioHigh').checked = true;
		}

		embedUrl = 'http://youtube.com/watch?v=' + id;
	}
	if (url.indexOf("vimeo.com/") > -1) {
		//dirty replace to get the original ID
		id = url.replace('http://vimeo.com/moogaloop.swf?clip_id=', '').replace('&amp;server=vimeo.com&amp;show_title=1&amp;show_byline=1&amp;show_portrait=0&amp;color=&amp;fullscreen=1', '');
		
		//vimeo hd is not supported by a param its automatic
		embedUrl = 'http://vimeo.com/' + id; 
	}
	if (url.indexOf("flickr.com/") > -1) {
		id = url.slice(url.lastIndexOf('/') + 1);
		embedUrl = "http://flickr.com/photo.gne?id=" + id;
	}
	return embedUrl;
}



function GetVideoURL(url) {
	var embedUrl;
	var bHQ = GetE('radioHigh').checked;
	var id;

	//trim off end /
	if (url.charAt(url.length -1) == '/') {
		url = url.substring(0, url.length - 1);
	}

	//work out if we are a youtube url etc
	if (url.indexOf("youtube.com/") > -1) {
		id = url.slice(url.search(/\?v=/i) + 3);

		if (bHQ) {
			embedUrl = 'http://www.youtube.com/v/' + id + '%26hl=en%26fs=1%26rel=0%26ap=%2526fmt=18';
		}
		else {
			embedUrl = 'http://www.youtube.com/v/' + id + '%26hl=en%26fs=1%26rel=0';
		}
	}
	if (url.indexOf("vimeo.com/") > -1) {
		id = url.slice(url.lastIndexOf('/') + 1);
		//vimeo hd is not supported by a param its automatic
		embedUrl = 'http://vimeo.com/moogaloop.swf?clip_id=' + id + '&amp;server=vimeo.com&amp;show_title=1&amp;show_byline=1&amp;show_portrait=0&amp;color=&amp;fullscreen=1';
	}
	
	if (url.indexOf("flickr.com/") > -1) {
		embedUrl="http://www.flickr.com/apps/video/stewart.swf?v=71377";
	}
	return embedUrl;
}



/*
	Updates the Embed Object with the video parameters
	@e - embed object
*/
function UpdateEmbed(e) {

	var url = GetE('txtUrl').value;
	var embedUrl = GetVideoURL(url);

	if (url.indexOf("flickr.com/") > -1) {
		if (url.charAt(url.length - 1) == '/') {
			url = url.substring(0, url.length - 1);
		}
		if (url.indexOf("photo.gne") > -1) {
			id = url.replace("http://flickr.com/photo.gne?id=", "");
		}
		else {
			id = url.slice(url.lastIndexOf('/') + 1);
		}
		
		SetAttribute(e, "flashvars", "intl_lang=en-us&photo_id=" + id);
	}
	
	
	SetAttribute(e, 'src', embedUrl);

	SetAttribute(e, 'type', 'application/x-shockwave-flash');
	SetAttribute(e, 'pluginspage', 'http://www.macromedia.com/go/getflashplayer');

	SetAttribute(e, "width", GetE('txtWidth').value === '' ? 360 : GetE('txtWidth').value);
	SetAttribute(e, "height", GetE('txtHeight').value === '' ? 150 : GetE('txtHeight').value);
	SetAttribute(e, "allowscriptaccess", "always");
	SetAttribute(e, "allowfullscreen", "true");
}


function UpdatePreview() {
	if (!ePreview) {
		return;
	}

	while (ePreview.firstChild) {
		ePreview.removeChild(ePreview.firstChild);
	}

	if (GetE('txtUrl').value.length === 0) {
		ePreview.innerHTML = '&nbsp;';
	}
	else {
		var oDoc = ePreview.ownerDocument || ePreview.document;
		var e = oDoc.createElement('EMBED');

		UpdateEmbed(e);
		ePreview.appendChild(e);
	}
}

function LoadSelection() {
	if (!oEmbed) {
		return;
	}
	var url = oEmbed.getAttribute('src');
	if (url.indexOf("flickr.com/") > -1) {
		url = oEmbed.getAttribute('flashvars').replace("intl_lang=en-us&photo_id=", "http://flickr.com/");
	}

	GetE('txtUrl').value = GetVideoURLFromEmbedSrc(url);
	GetE('txtWidth').value = GetAttribute(oEmbed, 'width', '');
	GetE('txtHeight').value = GetAttribute(oEmbed, 'height', '');
	//update the preview video
	UpdatePreview();
}


function SetPreviewElement(previewEl) {
	ePreview = previewEl;

	if (GetE('txtUrl').value.length > 0) {
		UpdatePreview();
	}
}

//#### The OK button was hit.
function Ok() {
	var url = GetE('txtUrl').value;
	if (url.length === 0) {
		dialog.SetSelectedTab('Info');
		GetE('txtUrl').focus();

		alert(oEditor.FCKLang.DlgNoVideo);

		return false;
	}

	if (url.indexOf("youtube.com/") == -1 && url.indexOf("flickr.com/") == -1 && url.indexOf("vimeo.com/") == -1) {
		dialog.SetSelectedTab('Info');
		GetE('txtUrl').focus();

		alert(oEditor.FCKLang.DlgInvalidVideoUrl);

		return false;
	}

	oEditor.FCKUndo.SaveUndoStep();
	if (!oEmbed) {
		oEmbed = FCK.EditorDocument.createElement('EMBED');
		oFakeImage = null;
	}
	UpdateEmbed(oEmbed);

	if (!oFakeImage) {
		oFakeImage = oEditor.FCKDocumentProcessor_CreateFakeImage('FCK__Flash', oEmbed);
		oFakeImage.setAttribute('_fckflash', 'true', 0);
		oFakeImage = FCK.InsertElement(oFakeImage);
	}

	oEditor.FCKEmbedAndObjectProcessor.RefreshView(oFakeImage, oEmbed);

	return true;
}

//onload get the exising url and prepopulate the txt field
window.onload = function() {
	// Translate the dialog box texts.
	oEditor.FCKLanguageManager.TranslatePage(document);

	// Load the selected element information (if any).
	LoadSelection();


	dialog.SetAutoSize(true);
	// Activate the "OK" button.
	dialog.SetOkButton(true);
	SelectField('txtUrl');
};