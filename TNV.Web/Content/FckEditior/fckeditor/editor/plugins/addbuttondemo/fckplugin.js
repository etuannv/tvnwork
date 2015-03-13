/*
FCKCommands.RegisterCommand(commandName, command)
       commandName - Command name, referenced by the Toolbar, etc...
       command - Command object (must provide an Execute() function).
*/
// Register the related commands.
FCKCommands.RegisterCommand('addbuttondemo',
    new FCKDialogCommand(
        FCKLang['DlgAddbuttondemo'],
        FCKPlugins.Items['addbuttondemo'].Path + 'addbuttondemo.html', 340, 170));
FCKCommands.RegisterCommand('addbuttondemo',
    	new FCKDialogCommand(
        FCKLang['DlgAddbuttondemo'],
        FCKLang['DlgAddbuttondemo'],
        FCKPlugins.Items['addbuttondemo'].Path + 'addbuttondemo.html', 340, 200));

// Create the "addbuttondemos Link" toolbar button.
var oIntLinkItem = new FCKToolbarButton('addbuttondemo', FCKLang['DlgAddbuttondemo']);
oIntLinkItem.IconPath = FCKPlugins.Items['addbuttondemo'].Path + 'topclanky.png';
// 'addbuttondemos' is the name used in the Toolbar config.
FCKToolbarItems.RegisterItem('addbuttondemo', oIntLinkItem);