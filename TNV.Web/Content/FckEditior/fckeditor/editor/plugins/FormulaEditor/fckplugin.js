// Register the related commands.
var dialogPath = FCKConfig.PluginsPath + 'FormulaEditor/fck_formula.html';
var feditDialogCmd = new FCKDialogCommand( FCKLang["DlgFEditTitle"], FCKLang["DlgFEditTitle"], dialogPath, 480, 470 );
FCKCommands.RegisterCommand( 'FormulaEditor', feditDialogCmd ) ;

// Create the Formula Editor toolbar button.
var oFEditItem		= new FCKToolbarButton( 'FormulaEditor', FCKLang["DlgFEditTitle"]) ;
oFEditItem.IconPath	= FCKConfig.PluginsPath + 'FormulaEditor/button.functioneditor.gif' ;

FCKToolbarItems.RegisterItem( 'FormulaEditor', oFEditItem ) ;			

