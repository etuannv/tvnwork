/*
 * File Name: fckplugin.js
 *      Plugin to insert language switch to editor.
 *
 * This script is mainly based on :
 *
 * File Name: fckplugin.js
 *      Plugin to insert predefined Tag into the editor.
 * 
 * Version: 2.3
 *
 * File Authors:
 *	      Lee, Joon (alogblog_dot_com [ a t ] gmail_dot_com)
 */
var FCKLangSwitcher = function(name)
{
	this.Name = name;

	var oWindow;

	if(FCKBrowserInfo.IsIE) oWindow = window;
	// In FF, _IFrame may be not yet allocated in this plugin install step.
	// else if(FCK.ToolbarSet._IFrame != null) oWindow = FCKTools.GetElementWindow(FCK.ToolbarSet._IFrame);
	else	oWindow = window.parent;

	this._Panel = new FCKPanel(oWindow, true);
	this._Panel.AppendStyleSheet(FCKConfig.SkinPath + 'fck_editor.css');
	this._Panel.MainNode.className = 'FCK_Panel';
	this._CreatePanelBody(this._Panel.Document, this._Panel.MainNode);

	FCKTools.DisableSelection(this._Panel.Document.body);
}

FCKLangSwitcher.prototype.Execute = function(panelX, panelY, relElement)
{
	this._Panel.Show(panelX, panelY, relElement);
}

FCKLangSwitcher.prototype.SetTag = function(LanguageShort, LanguageName, LanguageRtl)
{

	var hrefStartHtml	= '<span lang="'+LanguageShort+'" xml:lang="'+LanguageShort+'"' + ((LanguageRtl) ? ' dir="rtl"' : '') +'>';
	var hrefEndHtml		= '</span>';

	mySelection = FCKSelection.GetSelectedHTML();

	hrefHtml = hrefStartHtml+mySelection+hrefEndHtml;

	FCK.InsertHtml(hrefHtml);
}


function FCKLangSwitcherCommand_OnMouseOver()
{
	this.className='ColorSelected';
}

function FCKLangSwitcherCommand_OnMouseOut()
{
	this.className='ColorDeselected';
}

function FCKLangSwitcherCommand_OnClick()
{
	this.className = 'ColorDeselected';
	this.Command.SetTag(this.LanguageShort, this.LanguageName, this.LanguageRtl);
	this.Command._Panel.Hide();
}

FCKLangSwitcher.prototype._CreatePanelBody = function(targetDocument, targetDiv)
{
	function CreateSelectionDiv()
	{
		var oDiv = targetDocument.createElement("DIV") ;
		oDiv.className		= 'ColorDeselected' ;
		oDiv.onmouseover	= FCKLangSwitcherCommand_OnMouseOver ;
		oDiv.onmouseout		= FCKLangSwitcherCommand_OnMouseOut ;
		return oDiv ;
	}

	var oTable = targetDiv.appendChild(targetDocument.createElement('TABLE'));
	oTable.style.tableLayout = 'fixed';
	oTable.cellPadding = 0;
	oTable.cellSpacing = 0;
	oTable.border = 0;
	oTable.width = 50;

	var oCell = oTable.insertRow(-1).insertCell(-1);
	oCell.colSpan = 2;

	var aLangSwitcherLanguages = FCKConfig.LangSwitcherLanguages;
	var iCounter = 0;
	while(iCounter < aLangSwitcherLanguages.length)
	{
		var oRow = oTable.insertRow(-1);
		
		for(var i=0; i<oCell.colSpan && iCounter<aLangSwitcherLanguages.length; i++, iCounter++)
		{
			oDiv = oRow.insertCell(-1).appendChild(CreateSelectionDiv());
			oDiv.innerHTML = '<div><div title="' + aLangSwitcherLanguages[iCounter][1] + '" style="width:16px;height:11px;background:url(' + FCKConfig.PluginsPath + 'LangSwitcher/flags/' + aLangSwitcherLanguages[iCounter][0] + '.png) no-repeat center center;"></div></div>';
			oDiv.LanguageShort = aLangSwitcherLanguages[iCounter][0];
			oDiv.LanguageName = aLangSwitcherLanguages[iCounter][1];
			oDiv.LanguageRtl = aLangSwitcherLanguages[iCounter][2];
			oDiv.Command = this;
			oDiv.onclick = FCKLangSwitcherCommand_OnClick;
		}
	}
}

FCKLangSwitcher.prototype.GetState = function()
{
	return FCK_TRISTATE_OFF;
} 

FCKCommands.RegisterCommand('LangSwitcher', new FCKLangSwitcher('LangSwitcher'));
FCKToolbarItems.RegisterItem('LangSwitcher', new FCKToolbarPanelButton('LangSwitcher', FCKLang.LangSwitcher, null, null));
