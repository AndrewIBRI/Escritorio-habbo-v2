<!DOCTYPE html>
<html lang="en">
    <head>
        <meta http-equiv="content-type" content="text/html; charset=utf-8">
        <title>{hotelName} - Client</title>
        
        <link rel="stylesheet" href="{url}/app/tpl/skins/habbologic/css/client.css" type="text/css">
        
        <script type="text/javascript" src="{url}/app/tpl/skins/habbologic/js/swfobject.js"></script>
        <script type="text/javascript">
            var BaseUrl = "http://localhost/swf/gordon/PRODUCTION-201701242205-837386173";
            var flashvars =
            {
                "client.starting" : "Please wait, {hotelName} is loading", 
                "client.allow.cross.domain" : "1", 
                "client.notify.cross.domain" : "0", 
                "connection.info.host" : "localhost", 
                "connection.info.port" : "30000", 
                "site.url" : "http://localhost", 
                "url.prefix" : "http://localhost", 
                "client.reload.url" : "http://localhost/client", 
                "client.fatal.error.url" : "http://localhost/me", 
                "client.connection.failed.url" : "http://localhost/me",  
                "external.variables.txt" : "http://localhost/swf/gamedata/external_variables.txt", 
                "external.texts.txt" : "http://localhost/swf/gamedata/external_flash_texts.txt", 
                "productdata.load.url" : "http://localhost/swf/gamedata/productdata.txt", 
                "furnidata.load.url" : "http://localhost/swf/gamedata/furnidata.xml",
                "use.sso.ticket" : "1", 
                "sso.ticket" : "{sso}", 
                "processlog.enabled" : "0", 
                "flash.client.url" :"http://localhost/swf/gordon/PRODUCTION-201701242205-837386173/", 
                "flash.client.origin" : "popup" 
            };
            var params =
            {
                "base" : BaseUrl + "/",
                "allowScriptAccess" : "always",
                "menu" : "false"                
            };
            swfobject.embedSWF(BaseUrl + "/Habbo.swf", "client", "100%", "100%", "10.0.0", "http://localhost/swf/gordon/PRODUCTION-201701242205-837386173/expressInstall.swf", flashvars, params, null);
        </script>
    </head>
    
    <body>
    
        <div id="client"></div>
    
    </body>
</html>