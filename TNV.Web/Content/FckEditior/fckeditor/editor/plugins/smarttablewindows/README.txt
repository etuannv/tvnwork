When enabled, this plugin removes certain options from the table popup windows so that users can't add a border, cellspacing and cellpadding to tables.

The cell property window also has a style selection combo box that allows the users to select 1 or more styles from the pre-configured list and attach it to a cell.

I included standard FCKEditor classes in the plugin because I don't know how to link to the FCKEditor core from inside a plugin. Any inside on this would be appreciated.

We would like this functionality to be added in the standard release of FCKEditor and would be more than happy to make the needed adaptations to make this happen, so feel free to contact us.

To activate the plugin add it to your FCKEditor plugin directory and add this line of code to your fckconfig.js:

FCKConfig.Plugins.Add('smarttablewindows','en') ;

Suggestions, tips and comments are very welcome.

Tom Vergult
Smartlounge
Internet application development & hosting
plugin-development [at] smartlounge.be
http://www.smartlounge.be