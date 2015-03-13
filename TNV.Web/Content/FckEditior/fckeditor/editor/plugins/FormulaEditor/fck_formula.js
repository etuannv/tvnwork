var oEditor = window.parent.InnerDialogLoaded() ;
var FCK		= oEditor.FCK ;

// Set the language direction.
window.document.dir = oEditor.FCKLang.Dir ;

// Set the Skin CSS.
document.write( '<link href="' + oEditor.FCKConfig.SkinPath + 'fck_dialog.css" type="text/css" rel="stylesheet">' ) ;

var sAgent = navigator.userAgent.toLowerCase() ;

var is_ie = (sAgent.indexOf("msie") != -1); // FCKBrowserInfo.IsIE
var is_gecko = !is_ie; // FCKBrowserInfo.IsGecko

var oFormula = null;
var tstamp = null;
function cursorStyle(style)
{
    document.body.style.cursor = style;
    document.getElementById('txtFormulaName').style.cursor = style;
    document.getElementById('txtFormula').style.cursor = style;
    document.getElementById('btnRecalc').style.cursor = style;
    document.getElementById('selColor').style.cursor = style;
    document.getElementById('formulaPreview').style.cursor = style;
    parent.document.body.style.cursor = style;
    parent.document.getElementById('btnOk').style.cursor = style;
    parent.document.getElementById('btnOk').value = 'Upload in progress...';
}

function window_onload()
{
	// Translate the dialog box texts.
	oEditor.FCKLanguageManager.TranslatePage(document) ;
    
	// Load the selected element information (if any).
    jetzt = new Date();
    tstamp = jetzt.getTime();
    GetE('frmWdir').value = tstamp;
    GetE('frmTstamp').value = tstamp;
	LoadSelection() ;
	// Activate the "OK" button.
	window.parent.SetOkButton( true ) ;

    return;
}

/**
 * instantiates a new xmlhttprequest object
 *
 * @return xmlhttprequest object or false
 */
function getXMLRequester( )
{
    xmlHttp = false;
            
    // try to create a new instance of the xmlhttprequest object        
    try
    {
        // Internet Explorer
        if( window.ActiveXObject )
        {
            for( var i = 5; i; i-- )
            {
                try
                {
                    // loading of a newer version of msxml dll (msxml3 - msxml5) failed
                    // use fallback solution
                    // old style msxml version independent, deprecated
                    if( i == 2 )
                    {
                        xmlHttp = new ActiveXObject( "Microsoft.XMLHTTP" );    
                    }
                    // try to use the latest msxml dll
                    else
                    {
                        
                        xmlHttp = new ActiveXObject( "Msxml2.XMLHTTP." + i + ".0" );
                    }
                    break;
                }
                catch( excNotLoadable )
                {                        
                    xmlHttp = false;
                }
            }
        }
        // Mozilla, Opera und Safari
        else if( window.XMLHttpRequest )
        {
            xmlHttp = new XMLHttpRequest();
        }
    }
    // loading of xmlhttp object failed
    catch( excNotLoadable )
    {
        xmlHttp = false;
    }
    return xmlHttp ;
}

function LoadSelection()
{
	oFormula = new Formula();
	oFormula.setObjectElement(FCK.Selection.GetSelectedElement( 'IMG' ));
/*
	alert (
		"id: " + oFormula.id +
		"\nText: " + decodeBase64(oFormula.latexcode) + 
		"\nColor: " + oFormula.latexcolor
	);
*/
    GetE('txtFormula').value = decodeBase64(oFormula.latexcode);
    if (oFormula.latexcolor != '') {
        GetE('selColor').value = oFormula.latexcolor;
    }
    GetE('txtFormulaName').value = oFormula.formulaname;
    GetE('formulaPreview').src = oFormula.src;
    if (oFormula.timestamp != '') {
        GetE('frmTstamp').value = oFormula.timestamp;
    }
    updatePreview();
    return;
}

function CheckUpload()
{
	var sFile = GetE('txtFormula').value ;
	
	if ( sFile.length == 0 )
	{
		alert( 'Please enter a Formula' ) ;
		return false ;
	}
	GetE('frmBase64Formula').value = encodeBase64(GetE('txtFormula').value)
	return true ;
}

function OnRecalcCompleted( errorNumber, fileUrl, fileName, customMsg )
{
	switch ( errorNumber )
	{
		case 0 :	// No errors
			break ;
		case 1 :	// Custom error
			alert( customMsg ) ;
			return ;
		case 101 :	// Custom warning
			alert( customMsg ) ;
			break ;
		case 201 :
			alert( 'A file with the same name is already available. The uploaded file has been renamed to "' + fileName + '"' ) ;
			break ;
		case 202 :
			alert( 'Invalid file type' ) ;
			return ;
		case 203 :
			alert( "Security error. You probably don't have enough permissions to upload. Please check your server." ) ;
			return ;
		default :
			alert( 'Error on file upload. Error number: ' + errorNumber ) ;
			return ;
	}

    updatePreview(fileUrl) ;
    return ;
}

//#### The OK button was hit.
function Ok()
{
    if ( GetE('txtFormula').value.length == 0 )
    {
        GetE('txtFormula').focus() ;
        alert( 'No Formula given' ) ;
        return false ;
    }
    if (GetE('txtFormulaName').value.length == 0 )
    {
        GetE('txtFormulaName').focus() ;
        alert( 'Please enter a formula name' ) ;
        return false ;
    }
    cursorStyle("wait");
    objHTTP = getXMLRequester();
    if (objHTTP) {
        var e = (oFormula || new Formula()) ;

        objHTTP.open( 'POST', oEditor.FCKConfig.FormulaCommand, false );
        objHTTP.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        objHTTP.send( 'frmBase64Formula='+encodeBase64(GetE('txtFormula').value)+'&selColor='+GetE('selColor').value+'&ImageID='+GetE('txtFormulaName').value+'&wdir='+tstamp+'&ImageTime='+GetE('frmTstamp').value );
        
        updateFormula(e,objHTTP.responseText) ;

        FCK.InsertHtml(e.getHTML()) ;
        cursorStyle("auto");

        return true ;

    } else {
        cursorStyle("auto");
        alert ("Error generating Formula");
        return true;
    }

}

function Cancel()
{
    objHTTP = getXMLRequester();
    if (objHTTP) {
        var e = (oFormula || new Formula()) ;
        //objHTTP.onreadystatechange = alertContents; 

        objHTTP.open( 'POST', oEditor.FCKConfig.FormulaCleanCommand, false );
        objHTTP.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        objHTTP.send( 'wdir='+tstamp );
    }
    window.close();
}

function updateFormula(e, newSrcUrl){
    e.latexcode = encodeBase64(GetE('txtFormula').value);
    e.latexcolor = GetE('selColor').value;
    e.formulaname = GetE('txtFormulaName').value;
    e.timestamp = GetE('frmTstamp').value;
    e.cls = 'formula';
    e.border = '0';
    e.title = GetE('txtFormulaName').value;
    e.alt = GetE('txtFormulaName').value;
    if (newSrcUrl) {
        e.src = newSrcUrl;
    } else {
        e.src = GetE('formulaPreview').src;
    }
    return;
}

function updatePreview(newUrl){
	if ( GetE('txtFormula').value.length == 0 ){
		ShowE('formulaPreview',false);
	}
	else {
		var preview = GetE('formulaPreview');
		updateFormula(oFormula, newUrl);
		
		// preview.innerHTML dies on IE.. why?? :S
		if (is_ie){
			preview.outerHTML = oFormula.getHTML('formulaPreview');
		}
		
		// preview.outerHTML does nothing on gecko..
		if (is_gecko){
			preview.outerHTML = oFormula.getHTML('formulaPreview');
			oFormula.replaceObject(preview);
		}
		
		ShowE('formulaPreview',true);	
	}
    return;
}

function SetUrl( url )
{
	document.getElementById('txtFormula').value = url ;
	updatePreview() ;
    return;
}

var Formula = function (o){
	this.alt = '';
	this.width = '';
	this.height = '';
	this.vspace = '';
	this.hspace = '';
	this.border = '';
	this.align = '';
	this.dir = '';
	this.lang = '';
	this.title = '';
	this.cls = '';
	this.longDesc = '';
	this.src = '';
	this.latexcolor = '';
    this.latexcode = '';
    this.formulaname = '';
    this.timestamp = '';
    this.id = '';
	
	if (o) 
		this.setObjectElement(o);
};

Formula.prototype.setObjectElement = function (e){
	if (!e) return ;
	
	this.alt = GetAttribute( e, 'alt', this.alt );
	this.width = GetAttribute( e, 'width', this.width );
	this.height = GetAttribute( e, 'height', this.height );
	this.vspace = GetAttribute( e, 'vspace', this.vspace );
	this.hspace = GetAttribute( e, 'hspace', this.hspace );
	this.border = GetAttribute( e, 'border', this.border);
	this.align = GetAttribute( e, 'align', this.align );
	this.dir = GetAttribute( e, 'dir', this.dir );
	this.lang = GetAttribute( e, 'lang', this.lang );
	this.title = GetAttribute( e, 'title', this.title );
	this.cls = GetAttribute( e, 'class', this.cls );
	this.longDesc = GetAttribute( e, 'longDesc', this.longDesc );
	this.id = GetAttribute( e, 'id', this.id );
    this.src = GetAttribute( e, 'src', this.src );
	this.latexcolor = GetAttribute( e, 'latexcolor', this.latexcolor );
	this.latexcode = GetAttribute( e, 'latexcode', this.latexcode );
	this.formulaname = GetAttribute( e, 'formulaname', this.formulaname );
	this.timestamp = GetAttribute( e, 'timestamp', this.timestamp );
};

Formula.prototype.replaceObject = function(o){
	if (!o) return;
	
	if (this.latexcolor != '')	 SetAttribute(o, 'latexcolor', this.latexcolor);
	if (this.latexcode != '')	SetAttribute(o, 'latexcode', this.latexcode);
	if (this.formulaname != '')	SetAttribute(o, 'formulaname', this.formulaname);
	if (this.timestamp != '')	SetAttribute(o, 'timestamp', this.timestamp);
	if (this.src != '')	SetAttribute(o, 'src', this.src);
	o.innerHTML = this.getHTML();
};

Formula.prototype.getHTML = function (objectId){
    var s;
    s = '<img ';
    if (objectId) s+= this.createAttribute('id',objectId);
    else if (this.id != '') s+= this.createAttribute('id',this.id);  
    if (this.latexcolor != '') 	s+= this.createAttribute('latexcolor',this.latexcolor);
    if (this.latexcode != '') 	s+= this.createAttribute('latexcode',this.latexcode);
    if (this.formulaname != '') 	s+= this.createAttribute('formulaname',this.formulaname);
    if (this.timestamp != '') 	s+= this.createAttribute('timestamp',this.timestamp);
    if (this.src != '') s+= this.createAttribute('src', this.src);
    if (this.alt != '') s+= this.createAttribute('alt', this.alt );
    if (this.width != '') s+= this.createAttribute('width', this.width );
    if (this.height != '') s+= this.createAttribute('height', this.height );
    if (this.vspace != '') s+= this.createAttribute('vspace', this.vspace );
    if (this.hspace != '') s+= this.createAttribute('hspace', this.hspace );
    if (this.border != '') s+= this.createAttribute('border', this.border);
    if (this.align != '') s+= this.createAttribute('align', this.align );
    if (this.dir != '') s+= this.createAttribute('dir', this.dir );
    if (this.lang != '') s+= this.createAttribute('lang', this.lang );
    if (this.title != '') s+= this.createAttribute('title', this.title );
    if (this.cls != '') s+= this.createAttribute('class', this.cls );
    if (this.longDesc != '') s+= this.createAttribute('longDesc', this.longDesc );

    s+= '>';

    return s;
};

Formula.prototype.createAttribute = function(n,v){
	return ' '+n+'="'+v+'" ';
}

var END_OF_INPUT = -1;

var base64Chars = new Array(
    'A','B','C','D','E','F','G','H',
    'I','J','K','L','M','N','O','P',
    'Q','R','S','T','U','V','W','X',
    'Y','Z','a','b','c','d','e','f',
    'g','h','i','j','k','l','m','n',
    'o','p','q','r','s','t','u','v',
    'w','x','y','z','0','1','2','3',
    '4','5','6','7','8','9','+','/'
);

var reverseBase64Chars = new Array();
for (var i=0; i < base64Chars.length; i++){
    reverseBase64Chars[base64Chars[i]] = i;
}

var base64Str;
var base64Count;

function setBase64Str(str){
    base64Str = str;
    base64Count = 0;
}

function readBase64(){    
    if (!base64Str) return END_OF_INPUT;
    if (base64Count >= base64Str.length) return END_OF_INPUT;
    var c = base64Str.charCodeAt(base64Count) & 0xff;
    base64Count++;
    return c;
}

function encodeBase64(str){
    setBase64Str(str);
    var result = '';
    var inBuffer = new Array(3);
    var lineCount = 0;
    var done = false;
    while (!done && (inBuffer[0] = readBase64()) != END_OF_INPUT){
        inBuffer[1] = readBase64();
        inBuffer[2] = readBase64();
        result += (base64Chars[ inBuffer[0] >> 2 ]);
        if (inBuffer[1] != END_OF_INPUT){
            result += (base64Chars [(( inBuffer[0] << 4 ) & 0x30) | (inBuffer[1] >> 4) ]);
            if (inBuffer[2] != END_OF_INPUT){
                result += (base64Chars [((inBuffer[1] << 2) & 0x3c) | (inBuffer[2] >> 6) ]);
                result += (base64Chars [inBuffer[2] & 0x3F]);
            } else {
                result += (base64Chars [((inBuffer[1] << 2) & 0x3c)]);
                result += ('=');
                done = true;
            }
        } else {
            result += (base64Chars [(( inBuffer[0] << 4 ) & 0x30)]);
            result += ('=');
            result += ('=');
            done = true;
        }
        lineCount += 4;
        if (lineCount >= 76){
            result += ('\n');
            lineCount = 0;
        }
    }
    return result;
}

function readReverseBase64(){   
    if (!base64Str) return END_OF_INPUT;
    while (true){      
        if (base64Count >= base64Str.length) return END_OF_INPUT;
        var nextCharacter = base64Str.charAt(base64Count);
        base64Count++;
        if (reverseBase64Chars[nextCharacter]){
            return reverseBase64Chars[nextCharacter];
        }
        if (nextCharacter == 'A') return 0;
    }
    return END_OF_INPUT;
}

function ntos(n){
    n=n.toString(16);
    if (n.length == 1) n="0"+n;
    n="%"+n;
    return unescape(n);
}

function decodeBase64(str){
    setBase64Str(str);
    var result = "";
    var inBuffer = new Array(4);
    var done = false;
    while (!done && (inBuffer[0] = readReverseBase64()) != END_OF_INPUT
        && (inBuffer[1] = readReverseBase64()) != END_OF_INPUT){
        inBuffer[2] = readReverseBase64();
        inBuffer[3] = readReverseBase64();
        result += ntos((((inBuffer[0] << 2) & 0xff)| inBuffer[1] >> 4));
        if (inBuffer[2] != END_OF_INPUT){
            result +=  ntos((((inBuffer[1] << 4) & 0xff)| inBuffer[2] >> 2));
            if (inBuffer[3] != END_OF_INPUT){
                result +=  ntos((((inBuffer[2] << 6)  & 0xff) | inBuffer[3]));
            } else {
                done = true;
            }
        } else {
            done = true;
        }
    }
    return result;
}



