//fckplugin.js
/*
* your plugin must be put in the 'editor/plugins/#plug-in name#' (the name is specified in fckconfig.js -> addPlugin, see below)
* in my case this is 'editor/plugins/insertvariables/'
*
* insert variable editor
* @author: Tim Struyf, Roots Software (http://www.roots.be), tim.struyf@roots.be
*/
var InsertVariableCommand = function() {
    //create our own command, we dont want to use the FCKDialogCommand because it uses the default fck layout and not our own
};
InsertVariableCommand.GetState = function() {
    return FCK_TRISTATE_OFF; //we dont want the button to be toggled
}
InsertVariableCommand.Execute = function() {
    //open a popup window when the button is clicked
window.open('plugins/insertvariables/insertVariable.html', 'insertVariable', 'width=500,height=400,scrollbars=no,scrolling=no,location=no,toolbar=no');
}
FCKCommands.RegisterCommand('Insert_Variables', InsertVariableCommand); //otherwise our command will not be found
var oInsertVariables = new FCKToolbarButton('Insert_Variables', 'insert variable');
oInsertVariables.IconPath = FCKConfig.PluginsPath + 'insertvariables/variable.png'; //specifies the image used in the toolbar
FCKToolbarItems.RegisterItem('Insert_Variables', oInsertVariables);