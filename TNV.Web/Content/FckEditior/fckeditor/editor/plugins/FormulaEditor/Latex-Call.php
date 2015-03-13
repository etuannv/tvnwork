<?php
session_start();
global $DOCUMENT_ROOT,$_psenv;
global $noecho;
$noecho=1;
// This is the function that sends the results of the uploading process.
function SendResults( $errorNumber, $fileUrl = '', $fileName = '', $customMsg = '' )
{
	echo '<script type="text/javascript">' ;
	echo 'window.parent.OnRecalcCompleted(' . $errorNumber . ',"' . str_replace( '"', '\\"', $fileUrl ) . '","' . str_replace( '"', '\\"', $fileName ) . '", "' . str_replace( '"', '\\"', $customMsg ) . '") ;' ;
	echo '</script>' ;
	exit ;
}
header('Cache-Control: must-revalidate, post-check=0, pre-check=0', true);
$articleid = $_SESSION['ps_skin_eventobject']->nodeid;
$tstamp = time();
$value = base64_decode($_REQUEST['frmBase64Formula']);
$color = $_REQUEST['selColor'];
$PATHTOLATEX = 'latex';
$PATHTODVIPS = 'dvips';
$PATHTOCONVERT = 'convert';
$PATHTODVIPNG = '/usr/bin/dvipng';
$EXT = 'png';
$LATEXBASENAME = 'cms_math';
$outimg = $LATEXBASENAME.'_'.$tstamp.'.'.$EXT;
$LATEXFILENAME = $LATEXBASENAME . '.tex';
$LATEXWDIR = $_SERVER['DOCUMENT_ROOT'].'/ps/tmp/'.$_REQUEST['module_name'].'/'.$_REQUEST['wdir'];
if (!is_dir($_SERVER['DOCUMENT_ROOT'].'/ps/tmp/'.$_REQUEST['module_name'])) {
    mkdir($_SERVER['DOCUMENT_ROOT'].'/ps/tmp/'.$_REQUEST['module_name']);
}
if (!is_dir($LATEXWDIR)) {
    mkdir($LATEXWDIR);
}
$density = 116;
$gamma = 2;
$preamble = '\documentclass{article}
\RequirePackage{color}
\usepackage{latexsym}
\usepackage{amsfonts}
\usepackage{amssymb}
\usepackage{amsmath}
\definecolor{Red}{rgb}{1,0,0}
\definecolor{Blue}{rgb}{0,0,1}
\definecolor{Yellow}{rgb}{1,1,0}
\definecolor{Orange}{rgb}{1,0.4,0}
\definecolor{Pink}{rgb}{1,0,1}
\definecolor{Purple}{rgb}{0.5,0,0.5}
\definecolor{Teal}{rgb}{0,0.5,0.5}
\definecolor{Navy}{rgb}{0,0,0.5}
\definecolor{Aqua}{rgb}{0,1,1}
\definecolor{Lime}{rgb}{0,1,0}
\definecolor{Green}{rgb}{0,0.5,0}
\definecolor{Olive}{rgb}{0.5,0.5,0}
\definecolor{Maroon}{rgb}{0.5,0,0}
\definecolor{Brown}{rgb}{0.6,0.4,0.2}
\definecolor{Black}{gray}{0}
\definecolor{Gray}{gray}{0.5}
\definecolor{Silver}{gray}{0.75}
\definecolor{White}{gray}{1}
\begin{document}
\pagestyle{empty}
\clearpage
\begin{displaymath}
';
### create the temporary Latex Working Directory...
#does the topic's attachment directory exist?
if (!is_dir($LATEXWDIR)) {
    mkdir( $LATEXWDIR );
}
chdir( $LATEXWDIR );
$LATEXLOG = $LATEXWDIR.'/latexlog';
$mathout = fopen($LATEXWDIR.'/'.$LATEXFILENAME, 'w');
fwrite($mathout, $preamble);
$pure_latex = preg_replace('/^%.*$/m','',$value);
$pure_latex = preg_replace('/^.\[.*$/m','',$pure_latex);
$pure_latex = preg_replace('/^.\].*$/m','',$pure_latex);
$pure_latex = rtrim(ltrim($pure_latex));
$pure_latex = preg_replace('/^\$(.*)\$$/','$1',$pure_latex);
if ($color && $color <> 'black') {
    fwrite($mathout, '\textcolor{'.$color.'}{');
    fwrite($mathout, $pure_latex);
    fwrite($mathout, "}\n");
} else {
    fwrite($mathout, $pure_latex."\n");
}
fwrite($mathout, '\end{displaymath}
\clearpage
\end{document}');
fclose($mathout);
if ($_REQUEST['ImageID']) {
    exec("$PATHTOLATEX -interaction=nonstopmode $LATEXFILENAME");
} else {
    echo '<img src=\'/ps/tmp/'.$_REQUEST['module_name'].'/'.$_REQUEST['wdir']."/$outimg' latexsrc='".base64_encode($value)."' latexcolor='$color'>";
    echo "<br>LaTeX Conversion Transcript:<br>";
    passthru("$PATHTOLATEX -interaction=nonstopmode $LATEXFILENAME");
}
if (filesize($LATEXWDIR."/".$LATEXBASENAME.".dvi") > 0) {
    if (is_executable($PATHTODVIPNG)) {
        $cmd = "$PATHTODVIPNG -D ".$density." -T tight".
                " --".$EXT.
                " -gamma ".$gamma+1.0.
                " -bg transparent ".
                " -pp 1 -o $outimg $LATEXBASENAME.dvi";
        if ($_REQUEST['ImageID']) {
            exec($cmd);
        } else {
            passthru($cmd);
        }
    } else {
        # OTW, use dvips/convert ...
        if ($_REQUEST['ImageID']) {
            exec("$PATHTODVIPS -E -pp 1 -o $LATEXBASENAME.eps $LATEXBASENAME.dvi");
        } else {
            passthru("$PATHTODVIPS -E -pp 1 -o $LATEXBASENAME.eps $LATEXBASENAME.dvi");
        }
    	$cmd = "-density $density ";
        $cmd .= "-trim -gamma $gamma -transparent white ";
#        $cmd .= "-antialias -trim -gamma $gamma -transparent white ";
        $cmd .= "$LATEXBASENAME.eps $outimg";
        if ($_REQUEST['ImageID']) {
            exec("$PATHTOCONVERT $cmd");
        } else {
            passthru("$PATHTOCONVERT $cmd");
        }
    }
}
if ($_REQUEST['ImageID']) {
    echo '/ps/tmp/'.$_REQUEST['module_name'].'/'.$_REQUEST['wdir'].'/'.$outimg;
} else {
    SendResults( 0, '/ps/tmp/'.$_REQUEST['module_name'].'/'.$_REQUEST['wdir'].'/'.$outimg, '', '' );
}
?>
