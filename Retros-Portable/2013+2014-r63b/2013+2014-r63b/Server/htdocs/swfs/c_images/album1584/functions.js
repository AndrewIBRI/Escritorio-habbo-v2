function doIframe(){
	o = document.getElementsByTagName('iframe');
	for(i=0;i<o.length;i++){
		if (/\bautoHeight\b/.test(o[i].className)){
			setHeight(o[i]);
			addEvent(o[i],'load', doIframe);
		}
	}
}

function setHeight(e){
	if(e.contentDocument){
		e.height = e.contentDocument.body.offsetHeight + 35;
	} else {
		e.height = e.contentWindow.document.body.scrollHeight;
	}
}

function addEvent(obj, evType, fn){
	if(obj.addEventListener)
	{
	obj.addEventListener(evType, fn,false);
	return true;
	} else if (obj.attachEvent){
	var r = obj.attachEvent("on"+evType, fn);
	return r;
	} else {
	return false;
	}
}

if (document.getElementById && document.createTextNode){
 addEvent(window,'load', doIframe);	
}
//Checa o campo E-Mail
function verifica_mail(email){
	var campo = email;
	//se a "@" nÃ£o existe e Ã© o primeiro caractere 
	if (campo.indexOf("@") < 1) return false
	//se o caracter antes da "@" Ã© "." 
	if(campo.charAt(campo.indexOf("@")-1) == ".") return false;
	//se a Ãºltima incidÃªncia de "." estÃ¡ antes da @ 
	if (campo.lastIndexOf(".") <= campo.indexOf("@")) return false;
	//se o Ãºltimo caracter Ã© ponto,
	if (campo.lastIndexOf(".")  == (campo.length-1)) return false; 
	
	return true;
}
// FUNÃ‡ÃƒO QUE FORMATA CAMPO DE ACORDO COM A ESPECIFICAÃ‡ÃƒO '/'
// COMO UTILIZAR:    onKeyPress="return txtBoxFormat(document.form1, 'telefone', '(99) 9999-9999', event);"
function txtBoxFormat(objForm, strField, sMask, evtKeyPress) {
var i, nCount, sValue, fldLen, mskLen,bolMask, sCod, nTecla;

if(document.all) { // Internet Explorer
nTecla = evtKeyPress.keyCode; }
else if(document.layers) { // Nestcape
nTecla = evtKeyPress.which;
}

sValue = objForm[strField].value;

// Limpa todos os caracteres de formataÃ§Ã£o que
// jÃ¡ estiverem no campo.
sValue = sValue.toString().replace( "-", "" );
sValue = sValue.toString().replace( "-", "" );
sValue = sValue.toString().replace( ".", "" );
sValue = sValue.toString().replace( ".", "" );
sValue = sValue.toString().replace( "/", "" );
sValue = sValue.toString().replace( "/", "" );
sValue = sValue.toString().replace( "(", "" );
sValue = sValue.toString().replace( "(", "" );
sValue = sValue.toString().replace( ")", "" );
sValue = sValue.toString().replace( ")", "" );
sValue = sValue.toString().replace( " ", "" );
sValue = sValue.toString().replace( " ", "" );
sValue = sValue.toString().replace( ":", "" );
fldLen = sValue.length;
mskLen = sMask.length;

i = 0;
nCount = 0;
sCod = "";
mskLen = fldLen;

while (i <= mskLen) {
bolMask = ((sMask.charAt(i) == "-") || (sMask.charAt(i) == ".") || (sMask.charAt(i) == "/"))
bolMask = bolMask || ((sMask.charAt(i) == "(") || (sMask.charAt(i) == ")") || (sMask.charAt(i) == " "))
bolMask = bolMask || (sMask.charAt(i) == ":")

if (bolMask) {
sCod += sMask.charAt(i);
mskLen++; }
else {
sCod += sValue.charAt(nCount);
nCount++;
}

i++;
}

objForm[strField].value = sCod;

if (nTecla != 8) { // backspace
if (sMask.charAt(i-1) == "9") { // apenas nÃºmeros...
return ((nTecla > 47) && (nTecla < 58)); } // nÃºmeros de 0 a 9
else { // qualquer caracter...
return true;
} }
else {
return true;
}
}
// COMO UTILIZAR: onkeypress="evita_letra2(event)"
// NÃ£o permite digitar letras em um input type=text
function evita_letra2(tecla) {
	if (tecla.keyCode < 48 || tecla.keyCode > 57) 
		tecla.returnValue = false;
}

var JGCookie = {
  date: new Date(),
  set: function(name, content, days) {
    var expires = "";
    if(days) {
	  this.date.setTime(this.date.getTime()+(days*24*60*60*1000));
      expires = this.date.toGMTString() + "; ";
	}
	document.cookie = name + "=" + content + "; " + expires + "path=/";
	return true;
  },
  get: function( name ) {
  	var nameE = name + "=";
	var cookies = document.cookie.split(";");
	for(var i = 0, Cookie; Cookie = cookies[i]; i++) {
	  while(Cookie.charAt(0) == " ") {
	  	Cookie = Cookie.substring(1,Cookie.length);
	  }
	  if(Cookie.indexOf(nameE) == 0) {
	  	return Cookie.substring(nameE.length,Cookie.length);
	  }
	}
	return false;
  },
  unset: function( name ) {
  	this.set(name, "", -1);
  	return true;
  }
  
}
//JGCookie.set("Key", "Value", 1); // Setamos (nome, valor, duracao)
//JGCookie.get("Key"); // Lemos (nome)
//JGCookie.unset("Key"); // Apagamos (nome)
//JANELAS POP UP
	function pop_tir(id){
		window.open("http://www.habbid.com.br/modulos/tirinhas/tirinhas_popup.php?id="+id, "TIRINHA", "height=600, width=600, scrollbars=yes, status=yes, menubar=yes, toolbar = no, location = no");
	}
	function pop_valores(id_rare){
		window.open("http://www.habbid.com.br/modulos/valores/pop_mobi.php?id="+id_rare, "TIRINHA", "height=600, width=600, scrollbars=yes, status=yes, menubar=yes, toolbar = no, location = no");
	}

	function pop_pix(id){
		window.open("http://www.habbid.com.br/modulos/pixelclub/pixelclub_popup.php?id="+id, "PixelClub", "height=600, width=600, scrollbars=yes, status=yes, menubar=yes, toolbar = no, location = no");
	}
	
//CORES DO SITE
	function hexFromRGB (r, g, b) {
		var hex = [
			r.toString(16),
			g.toString(16),
			b.toString(16)
		];
		$.each(hex, function (nr, val) {
			if (val.length == 1) {
				hex[nr] = '0' + val;
			}
		});
		return hex.join('').toUpperCase();
	}

 function rgb2hex(rgb) {  
  rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);  
  function hex(x) {  
   hexDigits = new Array("0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f");  
   return isNaN(x) ? "00" : hexDigits[(x - x % 16) / 16] + hexDigits[x % 16];  
  }  
  return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);  
 } 

//HABBID SEARCH
function setInput(obj,t,c){
	if(obj.value == c){ obj.value = t; }
}
jQuery.cookie = function(name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }
        var path = options.path ? '; path=' + options.path : '';
        var domain = options.domain ? '; domain=' + options.domain : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};
function processResize(selector, size){
	$(selector).each(function(){
		$(this).load(function myFunction(){
			if ($(this).get(0).width > size){
				diminuir($(this),size);
			}
		});
		
		if ($(this).get(0).width > size && $(this).get(0).complete){
			//alert($(this));
			diminuir($(this),size);
		}			
	});	
}
function diminuir(objeto, tamanho){
	var prop = objeto.get(0).width/tamanho;
	var height = objeto.get(0).height/prop;
	var naturalWidth = objeto.get(0).width;
	var naturalHeight = objeto.get(0).height;
	objeto.attr("width",tamanho);
	objeto.attr("height",height);
	objeto.wrap("<div style='float:left; width:100%;'><div style='float:left;'><a href='"+objeto.get(0).src+"' class='habbidbox' target='_blank'></a><div id='resize'>Esta imagem foi redimensionada. Clique para ver em tamanho original "+naturalWidth+"x"+naturalHeight+".</div></div></div>");
}
function strip_tags (str, allowed_tags) {
    var key = '', allowed = false;
    var matches = [];    var allowed_array = [];
    var allowed_tag = '';
    var i = 0;
    var k = '';
    var html = ''; 
    var replacer = function (search, replace, str) {
        return str.split(search).join(replace);
    };
     // Build allowes tags associative array
    if (allowed_tags) {
        allowed_array = allowed_tags.match(/([a-zA-Z0-9]+)/gi);
    }
     str += '';
 
    // Match tags
    matches = str.match(/(<\/?[\S][^>]*>)/gi);
     // Go through all HTML tags
    for (key in matches) {
        if (isNaN(key)) {
            // IE7 Hack
            continue;        }
 
        // Save HTML tag
        html = matches[key].toString();
         // Is tag not in allowed list? Remove from str!
        allowed = false;
 
        // Go through all allowed tags
        for (k in allowed_array) {            // Init
            allowed_tag = allowed_array[k];
            i = -1;
 
            if (i != 0) { i = html.toLowerCase().indexOf('<'+allowed_tag+'>');}            if (i != 0) { i = html.toLowerCase().indexOf('<'+allowed_tag+' ');}
            if (i != 0) { i = html.toLowerCase().indexOf('</'+allowed_tag)   ;}
 
            // Determine
            if (i == 0) {                allowed = true;
                break;
            }
        }
         if (!allowed) {
            str = replacer(html, "", str); // Custom replace. No regexing
        }
    }
     return str;
}
function estrela(v, c){
		//v = numero do id
		//c = class a ser mudada - se for 1 é para selecionar
		var nota = document.getElementById("estrelaV").value;
		var i = 1;
		if(c=="1"){
			while(i<=v){
				document.getElementById("estrela"+i).className="seleciona";
				i++;
			}
		}else{
			i=1;
			while(i<=5){
				if(nota!=parseInt(nota) && parseInt(nota)==i-1){
					document.getElementById("estrela"+i).className="meia";
				}else if(i<=nota){
					document.getElementById("estrela"+i).className="cheia";
				}else if(i>nota && i<=5){
					document.getElementById("estrela"+i).className="vazia";
				}
				i++;
			}
		}
	}