<?php

if($user_rank > 4){

if($hkzone !== true){ header("Location: index.php?throwBack=true"); exit; }
if(!session_is_registered(acp)){ header("Location: index/p/login"); exit; }

if(!isset($_POST['category'])){
			if(isset($_POST['credits'])) {
				if(is_numeric($_POST['credits'])) {
				$sql = mysql_query("SELECT id,credits,pixels FROM users");
				$count = mysql_num_rows($sql);
				while($row = mysql_fetch_assoc($sql)) {
				mysql_query("UPDATE users SET credits = credits + '".mysql_real_escape_string($_POST['credits'])."', pixels = pixels + '".mysql_real_escape_string($_POST['activity_points'])."' WHERE id='".$row['id']."' LIMIT 1");
				}
				$msg = "<div class='rounded rounded-green'><center> (Total: ".$count.") ".mysql_real_escape_string($_POST['credits'])." cr&eacute;ditos y ".$_POST['activity_points']." pixeles dados </center></div> ";
				}else{
				$msg = "<div class='rounded rounded-red'><center>Errro! verifique todos os campos!</center></div>";
				}
				
				
			}elseif(isset($_POST['code']) || isset($_POST['type'])) {
			$sql = mysql_query("SELECT id FROM users");
			$count = mysql_num_rows($sql);
			while($row = mysql_fetch_assoc($sql)) {
			mysql_query("UPDATE users_badges SET id_slot = '0' WHERE id_user='".$row['userid']."' AND id_slot = '1'") or die(mysql_error());
			mysql_query("INSERT INTO users_badges (id_user,id_badge,id_slot) VALUES('".$row['id']."','".mysql_real_escape_string($_POST['code'])."','0')") or die(mysql_error());
			}
			
			$msg = "<div class='rounded rounded-green'><center>(Total: ".$count.") placa ".mysql_real_escape_string($_POST['code'])." agregada.</center></div>";
			}elseif(isset($_POST['user']) || isset($_POST['badgec'])) {
			$sql = mysql_query("SELECT id FROM users WHERE name='".mysql_real_escape_string($_POST['user'])."'");
			if(mysql_num_rows($sql) == 1) {
			$sql = mysql_query("SELECT id FROM users WHERE name='".mysql_real_escape_string($_POST['user'])."'");
			$row = mysql_fetch_assoc($sql);
			mysql_query("DELETE FROM users_badges WHERE id_user='".$row['id']."' AND id_badge='".mysql_real_escape_string($_POST['badgec'])."'");
			if(mysql_error == 0) {
			$msg = "<div class='rounded rounded-green'><center>Das Badge ".mysql_real_escape_string($_POST['badgec'])." wurde von ".mysql_real_escape_string($_POST['user'])." entfernt.</center></div>";
			}else{
			$msg = "<div class='rounded rounded-red'><center>Este usuário já possui o Emblema selecionado</center></div>";
			}
			}else{
			$msg = "<div class='rounded rounded-red'><center>Não foi possível encontrar este usuário</center></div>";
			}
		}elseif(isset($_POST['take'])) {
	$sql = mysql_query("SELECT * FROM users_badges WHERE badge_id ='".mysql_real_escape_string($_POST['take'])."'");
	$count = mysql_num_rows($sql);
	while($row = mysql_fetch_assoc($sql)) {
	mysql_query("DELETE FROM users_badges WHERE badge_id = '".mysql_real_escape_string($_POST['take'])."'");
	}
	$msg = "<div class='rounded rounded-green'><center>O Emblema ".mysql_real_escape_string($_POST['take'])." foi removido (Total: ".$count.")</center></div>";
	}
}

$pagename = "Enviar Moedas";
$pageid = "massa";

$catId = FilterText($_POST['category']);

if(empty($catId) || !is_numeric($catId) || $catId < 1 || $catId > 5){
    $catId = 1;
} else {
    $catId = $catId;
}

@include('subheader.php');

if(isset($msg)){ ?><p><strong><?php echo $msg; ?></strong></p><?php } ?>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<form action='index/p/massa&do=jumpCategory' method='post' name='Jumper!' id='Jumper!'>
<div class='tableborder'>
<div class='tableheaderalt'>Categoria</div>
<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>
<tr>
    <td class='tablerow2'  width='100%'  valign='middle'  align='center'>
        <select name='category' class='dropdown'>
            <option value='1' <?php if($catId == "1"){ echo "selected='selected'"; } ?>>Moedas e Pixels</option>
            <option value='2' <?php if($catId == "2"){ echo "selected='selected'"; } ?>>Enviar Emblemas</option>
            <option value='3' <?php if($catId == "3"){ echo "selected='selected'"; } ?>>Remover Emblemas</option>
			<option value='4' <?php if($catId == "4"){ echo "selected='selected'"; } ?>>Remover emblema de todos</option>
        </select>
        &nbsp;
        <input type='submit' value='Continuar' class='realbutton' accesskey='s'>
    </td>
</tr>
</table>
</div>
</form>

<br />
 
<form action='index/p/massa&do=save' method='post' name='theAdminForm' id='theAdminForm'>
<div class='tableborder'>
<div class='tableheaderalt'>Editor</div>

<table width='100%' cellspacing='0' cellpadding='5' align='center' border='0'>

<?php if($catId == 1) { ?>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Moedas</strong><div class='graytext'>Número de Moedas a enviar</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='credits' value="" size='3' maxlength='5' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Pixels</strong><div class='graytext'>Número de Pixels a enviar</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='activity_points' value="" size='3' maxlength='5' class='textinput'></td>
</tr>
<?php }elseif($catId == 2) { ?>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Código do Emblema</strong><div class='graytext'>Código do Emblema</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='code' value="" size='5' maxlength='5' class='textinput'></td>
</tr>
<?php }elseif($catId == 3) { ?>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Nome do usuário</strong><div class='graytext'>Nom do usuário que irá dar o Emblema</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='user' value="" size='20' maxlength='100' class='textinput'></td>
</tr>

<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Código do Emblema</strong><div class='graytext'>Código do Emblema</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='badgec' value="" size='20' maxlength='5' class='textinput'></td>
</tr>
<?php }elseif($catId == 4) { ?>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Badge code</strong><div class='graytext'>Welches Badge soll komplett gelöscht werden?</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='take' value="" size='5' maxlength='5' class='textinput'></td>
</tr>
<?php }else{ ?>
<tr>
<td class='tablerow1'  width='40%'  valign='middle'><strong>Tanto em Números</strong><div class='graytext'>Número indicado para dar</div></td>
<td class='tablerow2'  width='60%'  valign='middle'><input type='text' name='credits' value="" size='5' maxlength='5' class='textinput'></td>
</tr>
<?php } ?>

<tr>
<tr><td align='center' class='tablesubheader' colspan='2' ><input type='submit' value='Enviar' class='realbutton' accesskey='s' name='massa'></td></tr>
</form></table></div><br />	 </div><!-- / RIGHT CONTENT BLOCK -->
	 </td></tr>
</table>
</div><!-- / OUTERDIV -->

<?php require_once('footer.php'); } else { require_once('error.php'); exit; } ?>