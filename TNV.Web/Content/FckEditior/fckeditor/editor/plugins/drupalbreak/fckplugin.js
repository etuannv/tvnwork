// Define the command.

var FCKDrupal = function( name )
{
	this.Name = name ;
	this.EditMode = FCK.EditMode;
}

FCKDrupal.prototype.Execute = function()
{
	switch ( this.Name )
	{
		case 'Break' :
			var oBreak = FCK.InsertHtml('<!--break-->') ;
			break;
		case 'PageBreak' :
			var oPageBreak = FCK.InsertHtml('<!--pagebreak-->') ;
			break;
		default :
	}
}

FCKDrupal.prototype.GetState = function()
{
	return FCK_TRISTATE_OFF ;
}

// Register the Drupal tag commands.
FCKCommands.RegisterCommand( 'DrupalBreak', new FCKDrupal( 'Break' ) ) ;
FCKCommands.RegisterCommand( 'DrupalPageBreak', new FCKDrupal( 'PageBreak' ) ) ;

// Create the Drupal tag buttons.
var oDrupalItem = new FCKToolbarButton( 'DrupalBreak', 'Break', null, FCK_TOOLBARITEM_ICONTEXT, true, true ) ;
oDrupalItem.IconPath = FCKConfig.PluginsPath + 'drupalbreak/drupalbreak.gif';
FCKToolbarItems.RegisterItem( 'DrupalBreak', oDrupalItem ) ;

var oDrupalItem = new FCKToolbarButton( 'DrupalPageBreak', 'PageBreak', null, FCK_TOOLBARITEM_ICONTEXT, true, true ) ;
oDrupalItem.IconPath = FCKConfig.PluginsPath + 'drupalbreak/drupalbreak.gif';
FCKToolbarItems.RegisterItem( 'DrupalPageBreak', oDrupalItem ) ;
