<!DOCTYPE html>
<html lang="en">
    <head>
        <meta http-equiv="content-type" content="text/html; charset=utf-8">
        <title>{hotelName} - Client</title>
        
        <link rel="stylesheet" href="{url}/app/tpl/skins/habbologic/css/client.css" type="text/css">
        
        <script type="text/javascript" src="{url}/app/tpl/skins/habbologic/js/swfobject.js"></script>
        <script type="text/javascript">
            var BaseUrl = "{swf_folder}";
            var flashvars =
            {
                "client.starting" : "Please wait, {hotelName} is loading", 
                "client.allow.cross.domain" : "1", 
                "client.notify.cross.domain" : "0", 
                "connection.info.host" : "{server_ip}", 
                "connection.info.port" : "30300", 
                "site.url" : "{url}", 
                "url.prefix" : "{url}", 
                "client.reload.url" : "{url}/client", 
                "client.fatal.error.url" : "{url}/me", 
                "client.connection.failed.url" : "{url}/me",  
                "external.variables.txt" : "{external_vars}", 
                "external.texts.txt" : "{external_texts}", 
                "productdata.load.url" : "{url}/swf/gamedata/productdata.txt", 
                "furnidata.load.url" : "{furni_data}",
                "use.sso.ticket" : "1", 
                "sso.ticket" : "{sso}", 
                "processlog.enabled" : "0", 
                "flash.client.url" : "{swf_folder}/", 
                "flash.client.origin" : "popup" 
            };
            var params =
            {
                "base" : BaseUrl + "/",
                "allowScriptAccess" : "always",
                "menu" : "false"                
            };
            swfobject.embedSWF(BaseUrl + "/Habbo.swf", "client", "100%", "100%", "10.0.0", BaseUrl + "/expressInstall.swf", flashvars, params, null);
        </script>
    </head>
    
    <body>
    
        <div id="client">
            <div style="color: white; text-align: center; padding-top: 80px; font-family: 'Segoe UI', Arial, sans-serif;">
                <img src="{url}/app/tpl/skins/habbologic/images/logo.png" style="margin-bottom: 20px;" /><br/>
                <h2>O cliente Flash requer Adobe Flash Player ativado</h2>
                <p style="color: #bbb; max-width: 600px; margin: 15px auto; line-height: 1.6;">
                    Navegadores modernos (como Brave, Chrome e Edge) removeram o suporte nativo ao Flash.<br/>
                    Para jogar este hotel (r63c PlusEMU), acesse através de um <b>navegador com Flash integrado</b> (como o <b>FlashBrowser</b>, <b>Pale Moon</b> ou <b>Basilisk</b>).
                </p>
                <div style="margin-top: 25px;">
                    <a href="https://github.com/Cipslock/FlashBrowser/releases" target="_blank" style="background: #27ae60; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; font-weight: bold; margin-right: 10px;">Baixar FlashBrowser</a>
                    <a href="{url}/me" style="background: #444; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px;">Voltar para o Perfil</a>
                </div>
            </div>
        </div>
    
    </body>
</html>