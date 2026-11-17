 <?php
$dbhost = 'localhost'; 
$dbuser = 'root'; 
$dbpass = 'vertrigo'; 
$db = 'Hobbaweb'; 

mysql_connect($dbhost,$dbuser,$dbpass);   
mysql_select_db($db);   

$u = mysql_real_escape_string( strip_tags( $_GET["user"] ) );  
$query = "SELECT * FROM users WHERE username = '$u' LIMIT 1"; //velger fra qurey
$result = mysql_query($query) or die('Banco de dados não selecionado: ' . mysql_error()); //Okay
if(mysql_num_rows($result)) { //thihi<3
while($row = mysql_fetch_assoc($result)) { //resultat...

    echo "<img src=\"http://habbo.com/habbo-imaging/avatarimage?figure=".$row['look']."\" alt='".$row['username']."'>"; 
        } 
    }

// $result = mysql_query($query) or die('Query failed: ' . mysql_error()); //Okay
// if(mysql_num_rows($result)) { //thihi<3
// while($row = mysql_fetch_assoc($result)) { //resultat...
// echo "<img src=\"http://habbo.com/habbo-imaging/avatarimage?figure=".$row['look']."\" alt='".$row['username']."'>";
//        }
// }
?> 