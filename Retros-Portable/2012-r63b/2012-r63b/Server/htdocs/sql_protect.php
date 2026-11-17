
<head>
<style type="text/css">
<!--
.style20 {
	font-size: 24px;
	color: #CCCCCC;
}
.style30 {
	font-size: 16px
}
body,td,th {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 10px;
	color: #FF0000;
}
a {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 10px;
	color: #FF0000;
}
a:link {
	text-decoration: none;
}
a:visited {
	text-decoration: none;
	color: #FF0000;
}
a:hover {
	text-decoration: none;
	color: #FF0000;
}
a:active {
	text-decoration: none;
	color: #FF0000;
}
-->
</style>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" /></head>

<?
$bloquiados = array(";","\"","%","'","+","#","$","--","=="); 
foreach($_POST as $valor)
{
	foreach($bloquiados as $bloquiados2)
	{
		if(substr_count(strtolower($valor), strtolower($bloquiados2)) > 0) 
		{
		  die("<div align=\"center\">
  <p><br>
    <p>&nbsp;</p>
  <p>&nbsp;</p>
    <img src=\"images/no-page.gif\" /><br />
    <br />
      <span class=\"textbox style20\">N&atilde;o use Caracteres Especiais! </span></p>
  <p><br />
    <a href=\"javascript: history.back(-1);\" class=\"style30\">Voltar</a></p>
</div>");
		}
	}
}
foreach($_GET as $valor)
{
	foreach($bloquiados as $bloquiados2)
	{
		if(substr_count(strtolower($valor), strtolower($bloquiados2)) > 0) 
		{
		  die("<div align=\"center\">
  <p><br>
    <p>&nbsp;</p>
  <p>&nbsp;</p>
    <img src=\"images/no-page.gif\" /><br />
    <br />
      <span class=\"textbox style20\">N&atilde;o use Caracteres Especiais! </span></p>
  <p><br />
    <a href=\"javascript: history.back(-1);\" class=\"style30\">Voltar</a></p>
</div>");
		}
	}
}
foreach($_COOKIE as $valor)
{
	foreach($bloquiados as $bloquiados2)
	{
		if(substr_count(strtolower($valor), strtolower($bloquiados2)) > 0) 
		{
		  die("<div align=\"center\">
  <p><br>
    <p>&nbsp;</p>
  <p>&nbsp;</p>
    <img src=\"images/no-page.gif\" /><br />
    <br />
      <span class=\"textbox style20\">N&atilde;o use Caracteres Especiais! </span></p>
  <p><br />
    <a href=\"javascript: history.back(-1);\" class=\"style30\">Voltar</a></p>
</div>");
		}
	}
} ?>