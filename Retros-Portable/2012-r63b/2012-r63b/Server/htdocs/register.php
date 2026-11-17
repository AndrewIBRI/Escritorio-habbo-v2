<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');

if($logged_in){
	header("Location: ".$path."/me");
	exit;
}

$ip_check = mysql_query("SELECT ip_last,username,id FROM users WHERE ip_last = '".$remote_ip."'");

if(isset($_GET['cancal'])){
	session_unset();
	header("Location: $path/");
	exit;	
}


switch($_GET['stap']){
	case "1":
		if(isset($_SESSION['jjp']['register'][1])){
			header("Location: ".$path."/quickregister/email_password");
		}

		require_once('./templates/register_subheader.php');
		require_once('./templates/quickregister/page-register-1.php');

		break;
		
	case "2":
		$bday_day = FilterText($_POST['bean_day']);
		$bday_month = FilterText($_POST['bean_month']);
		$bday_year = FilterText($_POST['bean_year']);
		$gender = FilterText($_POST['bean_gender']);

		if ($bday_year > 1999)
		{
			$errors = "Desculpe, mas você deve ter mais de 13 anos.";
			
		}else if (!is_numeric($bday_day) || !is_numeric($bday_month) || !is_numeric($bday_year) || $bday_day <= 0 || $bday_day > 31 ||
			$bday_month <= 0 || $bday_month > 12 || $bday_year < 1900 || $bday_year > 2010)
		{
			$errors = "Por favor, você deve fornecer uma data válida.";
			
		}else if(!empty($gender)){
			$_SESSION['jjp']['register'][1]['bday_day'] = $bday_day;
			$_SESSION['jjp']['register'][1]['bday_month'] = $bday_month;
			$_SESSION['jjp']['register'][1]['bday_year'] = $bday_year;
			$_SESSION['jjp']['register'][1]['gender'] = $gender;
			header("Location: ".$path."/quickregister/email_password");
			exit;
		}else{
			$errors = "Por favor, você deve fornecer uma data válida";
		}

		require_once('./templates/register_subheader.php');
		require_once('./templates/quickregister/page-register-1.php');

		exit;
		break;
	
	case "3":
		if(!isset($_SESSION['jjp']['register'][1])){
			header("Location: ".$path."/quickregister/start");
		} elseif(isset($_SESSION['jjp']['register'][2])){
			header("Location: ".$path."/quickregister/captcha");
		}
		
		require_once('./templates/register_subheader.php');
		require_once('./templates/quickregister/page-register-2.php');

		break;		
	
	case "4":
		$name = FilterText($_POST['bean_name']);
		$email = FilterText($_POST['bean_email']);
		$email2 = FilterText($_POST['bean_retypedEmail']);
		$pass1 = FilterText($_POST['bean_password']);
		$pass2 = FilterText($_POST['bean_retypedPassword']);

		$filter = preg_replace("/[^a-z\d\-=\?!@:\.]/i", "", $name);
		$email_check = preg_match("/^[a-z0-9_\.-]+@([a-z0-9]+([\-]+[a-z0-9]+)*\.)+[a-z]{2,7}$/i", $email);
		
		$tmp = mysql_query("SELECT id FROM users WHERE username = '".$name."' LIMIT 1") or die(mysql_error());
		$tmp = mysql_num_rows($tmp);

		$emailu = mysql_query("SELECT id FROM users WHERE mail = '".$email."' LIMIT 1") or die(mysql_error());
		$emailu = mysql_num_rows($emailu);

		$first = substr($name, 0, 4);

		// CHEQUEO DE NOMBRE

      		if(strnatcasecmp($first,"MOD-") == false) {
			$errors = "O nome fornecido não é permitida.";
		}elseif($tmp > 0){
			$errors = "O nome já está em uso.";
		}elseif($filter !== $name){
			$errors = "Em nome não pode incluir sinais.";
		}elseif(strlen($name) > 15){
			$errors = "O nome é muito longo.";
		}elseif(strlen($name) < 3){
			$errors = "O nome é muito curto.";

		// CHEQUEO DE EMAIL

		}elseif($email_check !== 1){
			$errors = "E-mail não é válido.";
		}elseif(strlen($email) < 6){
			$errors = "E-mail é muito curto.";
		}elseif($emailu > 0){
			$errors = "O e-mail já está em uso.";
		}elseif($email !== $email2){
			$errors = "Os e-mails não coincidem.";

		// CHEQUEO DE CONTRASEÑA

		}elseif($pass1 !== $pass2){
			$errors = "As senhas não combinam.";
		}elseif(strlen($pass1) < 6){
			$errors = "A senha é muito curta.";
		}elseif(isset($_POST['bean_termsOfServiceSelection'])){
			$_SESSION['jjp']['register'][2]['name'] = $name;
			$_SESSION['jjp']['register'][2]['email'] = $email;
			$_SESSION['jjp']['register'][2]['pass'] = $pass1;
			
			header("Location: ".$path."/quickregister/captcha");	
			exit;	
		}else{
			$errors = "Você deve concordar com os Termos e Condições.";
		}
		
		require_once('./templates/register_subheader.php');
		require_once('./templates/quickregister/page-register-2.php');

		exit;
		break;
		
	case "5":
		if(!isset($_SESSION['jjp']['register'][1])){
			header("Location: ".$path."/quickregister/start");
		}elseif(!isset($_SESSION['jjp']['register'][2])){
			header("Location: ".$path."/quickregister/email_password");
		}
	
		require_once('./templates/register_subheader.php');
		require_once('./templates/quickregister/page-register-3.php');
	break;	
	
	case "6":

	if($_SESSION['register-captcha-bubble'] !== strtolower($_POST['captchaResponse'])){
		$errors = "O código digitado está correto.";
	}else{
			if($_SESSION['jjp']['register'][1]['gender'] == "male"){
				$look = 'hd-180-1.ch-210-66.lg-270-82.sh-290-91.hr-100-';
				$gender = 'M';
			}else{
				$look = 'hd-180-1.ch-210-66.lg-270-82.sh-290-91.hr-100-';
				$gender = 'F';
			}
				
			mysql_query("INSERT INTO `users` (username,real_name,password,auth_ticket,motto,mail,rank,look,gender,account_created,last_online,online,ip_last,ip_reg,working,secretcode,mymusik) VALUES ('".$_SESSION['jjp']['register'][2]['name']."','Javvox','".HoloHashMD5($_SESSION['jjp']['register'][2]['pass'])."','-/-','Habbe Hotel','".$_SESSION['jjp']['register'][2]['email']."','1','".$look."','".$gender."','".time()."','".time()."','1','".$remote_ip."','".$remote_ip."','','','')") or die(mysql_error());

			$userdata2 = mysql_query("SELECT * FROM users WHERE username = '".$_SESSION['jjp']['register'][2]['name']."'");
			$userdata = mysql_fetch_assoc($userdata2);

			mysql_query("INSERT INTO `user_info` (user_id,reg_timestamp) VALUES ('".$userdata['id']."','".time()."')");
			mysql_query("INSERT INTO `user_stats` (id) VALUES ('".$userdata['id']."')");

			$_SESSION['username'] = $_SESSION['jjp']['register'][2]['name'];
			$_SESSION['password'] = HoloHashMD5($_SESSION['jjp']['register'][2]['pass'], $_SESSION['jjp']['register'][2]['name']);
			
			unset($_SESSION['jjp']['register']);
			
			header("Location: ../me");
			exit;
		}
		
				require_once('./templates/register_subheader.php');
		require_once('./templates/quickregister/page-register-3.php');
		exit;
		break;		
}

?>