Language Switcher for FCKeditor
----------------------------------------------------------------------
By ohira1 ohira@web.de

[Based on the Tagging plugin from Lee, Joon (alogblog_dot_com [ a t ] gmail_dot_com)

What's this?
---------------
With this plugin you can switch the Language from a Word or sentence.
This is neccessary for Accessibility and screenreader.

E.g.: He is driving over the <span lang="de" xml:lang="de">Autobahn</span>.


INSTALLATION:
------------------------------
- unzip the files into your plugin folder
- copy the LangSwitcher.gif to your skin folders
(e.g.: fckeditor/editor/skins/silver/toolbar/)

- add the following to your fckconfig.js

FCKConfig.Plugins.Add('LangSwitcher', 'de,en');
FCKConfig.LangSwitcherLanguages = [
	['de', 'Deutsch'],
	['en-GB', 'English'],
	['en-US', 'US English'],
	['fr', 'French'],
	['es', 'Spanish'],
	['it', 'Italian'],
	['nl', 'Netherlands']
//	['ar', 'Arabian', 1] //Example for a "rtl" language
	
];

- add 'LangSwitcher' to your fckconfig.js
    FCKConfig.ToolbarSets["Default"] = [['...','...','LangSwitcher','...']];

More flags are available here: http://www.linkmatrix.de/index.php?id=icon

- ready...  