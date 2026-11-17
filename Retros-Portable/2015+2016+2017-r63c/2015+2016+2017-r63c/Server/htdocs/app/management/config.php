<?php
if(!defined('IN_INDEX')) { die('Sorry, you cannot access this file.'); }
#Please fill this all out.

#NOTE: To set up TheHabbos.ORG's API go to wwwroot/mysite/thehabbos_api for IIS, OR, htdocs/thehabbos_api for XAMPP and others.

/*
*
*	MySQL management
*
*/

$_CONFIG['mysql']['connection_type'] = 'pconnect'; //Type of connection: It must be connect, or pconnect: if you want a persistent connection.

$_CONFIG['mysql']['hostname'] = '127.0.0.1'; //MySQL host

$_CONFIG['mysql']['username'] = 'root'; //MySQL username

$_CONFIG['mysql']['password'] = ''; //MySQL password

$_CONFIG['mysql']['database'] = 'r63c'; //MySQL database

$_CONFIG['mysql']['port'] = '3306'; //MySQL's port

/*
*
*	Hotel management  - All URLs do not end with an "/"
*
*/

$_CONFIG['hotel']['server_ip'] = 'localhost'; //IP of VPS/DEDI/etc

$_CONFIG['hotel']['url'] = 'http://localhost'; //Does not end with a "/"

$_CONFIG['hotel']['name'] = 'Habbo'; // Hotel's name

$_CONFIG['hotel']['desc'] = 'Retro Portable'; //Hotel's description 

$_CONFIG['hotel']['email'] = 'Support@hotmail.com'; //Where the help queries from users are emailed to.@Priv skin

$_CONFIG['hotel']['in_maint'] = False; //False if hotel is NOT in maintenance. True if hotel IS in maintenance

$_CONFIG['hotel']['motto'] = 'I <3 Habbo!'; //Default motto users will register with.

$_CONFIG['hotel']['credits'] = 3000000000; //Default number of credits users will register with.

$_CONFIG['hotel']['pixels'] = 150000000; //Default number of pixels users will register with.

$_CONFIG['hotel']['figure'] = '-'; //Default figure users will register with.

$_CONFIG['hotel']['web_build'] = '63_1dc60c6d6ea6e089c6893ab4e0541ee0/527'; //Web_Build

$_CONFIG['hotel']['external_vars'] = 'http://localhost/swf/gamedata/external_variables.txt'; //URL to your external 

$_CONFIG['hotel']['external_texts'] = 'http://localhost/swf/gamedata/external_flash_texts.txt'; //URL to your external texts

$_CONFIG['hotel']['product_data'] = 'http://localhost/swf/gamedata/productdata.txt'; //URL to your productdata

$_CONFIG['hotel']['furni_data'] = 'http://localhost/swf/gamedata/furnidata.xml'; //URL to your furnidata

$_CONFIG['hotel']['swf_folder'] = 'http://localhost/swf/gordon/PRODUCTION-201701242205-837386173'; //URL to your SWF folder(does not end with a '/')
/*
*
*	Templating management - Pick one of our default styles or make yours by following our examples!
*
*/

#RevCMS has 2 default styles, 'Mango' by dannyy94 and 'Priv' by joopie - Others styles are to come, such as RastaLulz's ProCMS style and Nominal's PhoenixCMS 4.0 style.

$_CONFIG['template']['style'] = 'habbologic'; 

/*
*
*	Other topsites.. thing
*
*/

$_CONFIG['thehabbos']['username'] = 'Habbam';
$_CONFIG['retro_top']['user'] = 'Habbam'; 

/*
*
*	Recaptcha management - Fill the information below if you have one, else leave it like that and don't worry, be happy.
*
*/

$_CONFIG['recaptcha']['priv_key'] = '6LcZ58USAAAAABSV5px9XZlzvIPaBOGA6rQP2G43';
$_CONFIG['recaptcha']['pub_key'] = '6LcZ58USAAAAAAQ6kquItHl4JuTBWs-5cSKzh6DD';


/*
*
*	Social Networking stuff
*
*/

$_CONFIG['social']['twitter'] = ''; //Hotel's Twitter account

$_CONFIG['social']['facebook'] = 'habbamcom'; //Hotel's Facebook account

error_reporting(0)
?>