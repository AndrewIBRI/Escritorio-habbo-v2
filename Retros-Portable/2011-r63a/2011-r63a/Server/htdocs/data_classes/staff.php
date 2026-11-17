<?php
/*=======================================================================
| UberCMS - Advanced Website and Content Management System for uberEmu
| #######################################################################
| Copyright (c) 2010, Roy 'Meth0d' and updates by Matthew 'MDK'
| http://www.meth0d.org & http://www.sulake.biz
| #######################################################################
| This program is free software: you can redistribute it and/or modify
| it under the terms of the GNU General Public License as published by
| the Free Software Foundation, either version 3 of the License, or
| (at your option) any later version.
| #######################################################################
| This program is distributed in the hope that it will be useful,
| but WITHOUT ANY WARRANTY; without even the implied warranty of
| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
| GNU General Public License for more details.
\======================================================================*/

define('TAB_ID', 5);
define('PAGE_ID', 41);
require_once "global.php";

if (!LOGGED_IN)
{
	header("Location: " . WWW . "/");
	exit;
}
else if ($users->GetUserVar(USER_ID, 'newbie_status') == "0")
{
	header("Location: " . WWW . "/register/welcome");
	exit;
}
$tpl->AddIncludeFile(new IncludeFile('text/css', 'http://fizzhotel.net/web-gallery/styles/personal.css', 'stylesheet'));

$tpl->Init();

$tpl->AddGeneric('head-init');

$tpl->AddIncludeSet('generic');
$tpl->AddIncludeFile(new IncludeFile('text/css', 'http://images.habbo.nl/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1831/web-gallery/v2/styles/newcredits.css', 'stylesheet'));
$tpl->AddIncludeFile(new IncludeFile('text/javascript', 'http://images.habbo.nl/habboweb/63_1dc60c6d6ea6e089c6893ab4e0541ee0/1831/web-gallery/static/js/newcredits.js'));
$tpl->WriteIncludeFiles();

$tpl->AddGeneric('head-overrides-generic');
$tpl->AddGeneric('head-bottom');
$tpl->AddGeneric('generic-top');

$tpl->Write('<div id="columnstaff" class="column">');
$tpl->AddGeneric('comp-admins');
$tpl->AddGeneric('cramped');
$tpl->Write('</div>');

$tpl->Write('<div id="columnstaff" class="column">');
$tpl->AddGeneric('comp-mods');
$tpl->Write('</div>');


$tpl->AddGeneric('generic-column3');
$tpl->AddGeneric('footer');

$tpl->SetParam('page_title', 'Hotel Staff');
$tpl->SetParam('body_id', 'home');

$tpl->Output();

?>