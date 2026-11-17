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
			$errors = "Você deve ser maior de 13 anos para registrar-se!";
			
		}else if (!is_numeric($bday_day) || !is_numeric($bday_month) || !is_numeric($bday_year) || $bday_day <= 0 || $bday_day > 31 ||
			$bday_month <= 0 || $bday_month > 12 || $bday_year < 1900 || $bday_year > 2010)
		{
			$errors = "Você deve selecionar uma data de nascimento válida!";
			
		}else if(!empty($gender)){
			$_SESSION['jjp']['register'][1]['bday_day'] = $bday_day;
			$_SESSION['jjp']['register'][1]['bday_month'] = $bday_month;
			$_SESSION['jjp']['register'][1]['bday_year'] = $bday_year;
			$_SESSION['jjp']['register'][1]['gender'] = $gender;
			header("Location: ".$path."/quickregister/email_password");
			exit;
		}else{
			$errors = "Você deve selecionar uma data de nascimento válida!";
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
		$pass1 = FilterText($_POST['bean_password']);
		$pass2 = FilterText($_POST['bean_retypedPassword']);

		$filter = preg_replace("/[^a-z\d\-=\?!@:\.]/i", "", $name);
		$email_check = preg_match("/^[a-z0-9_\.-]+@([a-z0-9]+([\-]+[a-z0-9]+)*\.)+[a-z]{2,7}$/i", $email);
		
		$tmp = mysql_query("SELECT id FROM users WHERE username = '".$name."' LIMIT 1") or die(mysql_error());
		$tmp = mysql_num_rows($tmp);

		$emailu = mysql_query("SELECT id FROM users WHERE mail = '".$email."' LIMIT 1") or die(mysql_error());
		$emailu = mysql_num_rows($emailu);

		$first = substr($name, 0, 4);

		// Checagem de Nome

      		if(strnatcasecmp($first,"MOD-") == false) {
			$errors = "Este nome é proibido!";
		}elseif($tmp > 0){
			$errors = "O nome já está em uso!";
		}elseif($filter !== $name){
			$errors = "O nome não pode conter carácteres especiais";
		}elseif(strlen($name) > 15){
			$errors = "O Nome é muito largo!";
		}elseif(strlen($name) < 3){
			$errors = "O nome é muito curto!";

		// CHECAR E-MAIL@

		}elseif($email_check !== 1){
			$errors = "Digite um endereço de email válido!";
		}elseif(strlen($email) < 6){
			$errors = "O Email fornecido é muito curto, tente novamente.";
		}elseif($emailu > 0){
			$errors = "O Email já está em uso";

		// CHECAR SENHA@

		}elseif($pass1 !== $pass2){
			$errors = "As senhas não coencidem!";
		}elseif(strlen($pass1) < 6){
			$errors = "A senha fornecida é muito curta!";
		}elseif(isset($_POST['bean_termsOfServiceSelection'])){
			$_SESSION['jjp']['register'][2]['name'] = $name;
			$_SESSION['jjp']['register'][2]['email'] = $email;
			$_SESSION['jjp']['register'][2]['pass'] = $pass1;
			
			header("Location: ".$path."/quickregister/captcha");	
			exit;	
		}else{
			$errors = "Você deve concordar com nossos Termos e Condições";
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
		$errors = "O Código está incorreto, tente novamente.";
	}else{
			if($_SESSION['jjp']['register'][1]['gender'] == "male"){
				$look = 'hd-180-1.ch-210-66.lg-270-82.sh-290-91.hr-100-'; // VISUAL MASCULINO@
				$gender = 'M';
			}else{
				$look = 'ch-635-70.hd-600-1.lg-716-66-62.sh-735-68.hr-515-3'; // VISUAL FEMININO@
				$gender = 'F';
			}
				
			mysql_query("INSERT INTO `users` (username,real_name,password,auth_ticket,motto,mail,rank,look,gender,account_created,last_online,online,ip_last,ip_reg,working,secretcode,mymusik) VALUES ('".$_SESSION['jjp']['register'][2]['name']."','default','".HoloHashMD5($_SESSION['jjp']['register'][2]['pass'])."','-/-',' I <3 Habbie','".$_SESSION['jjp']['register'][2]['email']."','1','".$look."','".$gender."','".time()."','".time()."','1','".$remote_ip."','".$remote_ip."','','','')") or die(mysql_error());

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