//load the links in the dropdown list
addEvent(window, 'load', initializeWindow);

// oLink: The actual selected link in the editor.
var oEditor		= window.parent.InnerDialogLoaded() ;
var FCK			= oEditor.FCK ;
var FCKLang		= oEditor.FCKLang ;
var FCKConfig	= oEditor.FCKConfig ;
var FCKRegexLib	= oEditor.FCKRegexLib ;
var oLink = FCK.Selection.MoveToAncestorNode( 'A' ) ;
if ( oLink ) FCK.Selection.SelectNode( oLink ) ;

function initializeWindow() {
	if(setLinksInDropdown()) {
		// Activate the "OK" button.
		window.parent.SetOkButton( true ) ;
	}
}

function setLinksInDropdown() {
	//get our website's main page
	try {
			gwMainPage = window.parent.parent;
			
			if(!gwMainPage) {
				alert("No main page");
				
				return;
			}
	} catch (e) {
		return false;
	}
	
	//get the array containing the links (fcuntion 'getGwInternalLinks' must be available on the main page)
	try {
	    arrAddbuttondemo = gwMainPage.getAddbuttondemo();
	} catch (e) {
		alert("Error getting internal links from the main page!\n\nThis function might not be implemented on the current page.");
		return false;
	}
	if(arrAddbuttondemo.length<=0) {
		alert("Error getting internal links from the main page - seems to be empty!");
		return false;
	}
	
	//loop through the internal links and fill the dropdown box-html
	optionHTML = "<select id=\"addbuttondemos\"><option value=\"\"></option>";
	for(i = 0; i < arrAddbuttondemo.length; i++) {
		optionHTML += "<option value=\""+ arrAddbuttondemo[i][1] +"\">" + arrAddbuttondemo[i][0] + "</option>";
	}
	optionHTML += "</select>";
	
	//set the html in the dropdownbox
	document.getElementById("addbuttondemoDiv").innerHTML = optionHTML;
	
	return true;
}

//### functions from fck_dialog_common.js
function SetAttribute( element, attName, attValue )
{
	if ( attValue == null || attValue.length == 0 )
		element.removeAttribute( attName, 0 ) ;			// 0 : Case Insensitive
	else
		element.setAttribute( attName, attValue, 0 ) ;	// 0 : Case Insensitive
}

//#### The OK button was hit.
function Ok() {
	var sUri, sInnerHtml ;

	ilSelect = document.getElementById("addbuttondemos");

	if(ilSelect.selectedIndex==0) {
		alert(FCKLang.DlnLnkMsgNoUrl);
		return false;
	} else {
		sUri = ilSelect[ilSelect.selectedIndex].value;
	}

	if ( sUri.length == 0 ) {
	 	alert( FCKLang.DlnLnkMsgNoUrl ) ;
		return false ;
	}

	// If no link is selected, create a new one (it may result in more than one link creation - #220).
	var aLinks = oLink ? [ oLink ] : oEditor.FCK.CreateLink( sUri ) ;

	// If no selection, no links are created, so use the uri as the link text (by dom, 2006-05-26)
	var aHasSelection = ( aLinks.length > 0 ) ;
	if ( !aHasSelection ) {
		sInnerHtml = sUri;
		var oLinkPathRegEx = new RegExp("//?([^?\"']+)([?].*)?$") ;
		var asLinkPath = oLinkPathRegEx.exec( sUri ) ;
		if (asLinkPath != null) sInnerHtml = asLinkPath[1];  // use matched path
		// Create a new (empty) anchor.
		aLinks = [ oEditor.FCK.CreateElement( 'a' ) ] ;
	}

	oEditor.FCKUndo.SaveUndoStep() ;

	for ( var i = 0 ; i < aLinks.length ; i++ ) {
		oLink = aLinks[i] ;

		if ( aHasSelection )
			sInnerHtml = oLink.innerHTML ;		// Save the innerHTML (IE changes it if it is like an URL).

		oLink.href = sUri ;
		SetAttribute( oLink, '_fcksavedurl', sUri ) ;

		oLink.innerHTML = sInnerHtml ;		// Set (or restore) the innerHTML

		// Target
		SetAttribute( oLink, 'target', '_self' ) ;
	}

	// Select the (first) link.
	oEditor.FCKSelection.SelectNode( aLinks[0] );

	return true ;
}