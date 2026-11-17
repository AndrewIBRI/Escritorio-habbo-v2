<?php
    if (!defined('STDIN'))
    {
        echo 'Please run it as a cmd ({path to your php}/php.exe {path to badges.php} -f)';
        exit;
    }    
    
    define('BASE', 'http://kekospy.com/dcr/hof_furni/');
    
    $furnidata = file_get_contents('http://swf.kekospy.net/gamedata/furnidata/r7EiXuomsbBr4Zg9HnRfDYU71cEV5co69ENJpubPD3aKvP4CJuRsEQFvys11.txt');
    
    $pos = 0;
    while ($pos = strpos($furnidata, '["', $pos +1))
    {
        $pos1 = strpos($furnidata, '"]', $pos);
        $rule = substr($furnidata, $pos, ($pos1 -$pos));
        
        $rule = explode(',', $rule);
        
        $revision = str_replace('"', '', $rule[3]);
        $furni = current(explode('*', str_replace('"', '', $rule[2])));
        
        if (file_exists('C:/furni/'.$furni.'.swf'))
        {
            echo 'Este furni ya existe: '.$furni."\r\n";
            continue;
        }
            
        echo 'Descargando Furni: '.$furni."\r\n";
        if (!@copy(BASE.'/'.$furni.'.swf', 'C:/furni/'.$furni.'.swf'))
        {
            echo 'Error en la decarga del : '.$furni."\r\n";                
        }        
        
    }
    
    
?>