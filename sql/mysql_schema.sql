DROP TABLE IF EXISTS users;
CREATE TABLE users (
	id INT(11) NOT NULL AUTO_INCREMENT,
	username VARCHAR(25) NULL,
	roles VARCHAR(50) NULL,
	last_seen DATETIME NULL,
	date_created DATETIME NULL,
	avatar VARCHAR(50) NULL,
	is_locked TINYINT(1) NOT NULL,
	PRIMARY KEY (id)
 );
 
DROP TABLE IF EXISTS users_login;
CREATE TABLE users_login(
	id INT(11) NOT NULL AUTO_INCREMENT,
	user_id INT(11) NOT NULL,
	email VARCHAR(250) NOT NULL,
	passwd VARCHAR(250) NULL,
	salt VARCHAR(16) NULL,
	external_id INT NULL,
	provider VARCHAR(10) NOT NULL,
	access_token TEXT NULL,
	date_created DATETIME NULL,	
	PRIMARY KEY (id)
 );
 
DROP TABLE IF EXISTS users_password;
CREATE TABLE users_password(
	id INT(11) NOT NULL AUTO_INCREMENT,
	user_login_id INT(11) NOT NULL,
	token_expiry_date DATETIME NULL,
	token VARCHAR(150) NULL,	
	date_modified DATETIME NULL,	
	date_created DATETIME NULL,
	PRIMARY KEY (id)
 );

DROP TABLE IF EXISTS users_email;
CREATE TABLE users_email(
	id INT(11) NOT NULL AUTO_INCREMENT,
	user_login_id INT(11) NOT NULL,
	newemail VARCHAR(250) NULL,
	token VARCHAR(150) NULL,
	token_expiry_date DATETIME NULL,
	date_modified DATETIME NULL,	
	date_created DATETIME NULL,
	PRIMARY KEY (id)
 );
 
DROP TABLE IF EXISTS users_username;
CREATE TABLE users_username(
	id INT(11) NOT NULL AUTO_INCREMENT,
	user_id INT(11) NOT NULL,
	provider VARCHAR(10) NOT NULL,
	token TEXT NULL,
	date_created DATETIME NULL,	
	PRIMARY KEY (id)
 );
 
DROP TABLE IF EXISTS email_template;
CREATE TABLE email_template (
  id INT(10) NOT NULL AUTO_INCREMENT,
  email_subject TEXT NOT NULL,
  email_body TEXT NOT NULL,
  template_type VARCHAR(15) DEFAULT NULL,
  sender_name VARCHAR(100) DEFAULT NULL,
  sender_email VARCHAR(100) DEFAULT NULL,
  PRIMARY KEY (id)
);

DROP TABLE IF EXISTS email_queue;
CREATE TABLE email_queue (
  id INT(10) NOT NULL AUTO_INCREMENT,
  receiver VARCHAR(200) NOT NULL,
  sender VARCHAR(200) NOT NULL,
  email_subject TEXT NOT NULL,
  email_body TEXT NOT NULL,
  date_sent DATETIME DEFAULT NULL,
  is_sent TINYINT(1) NOT NULL,
  PRIMARY KEY (id)
);


INSERT  INTO `email_template`(`id`,`email_subject`,`email_body`,`template_type`,`sender_email`,`sender_name`) VALUES (1,'Your reset password request','<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> <title>Rest your password @ peruncore.com</title> <style type=\"text/css\"> body { padding-top: 0 !important; padding-bottom: 0 !important; padding-top: 0 !important; padding-bottom: 0 !important; margin:0 !important; width: 100% !important; -webkit-text-size-adjust: 100% !important; -ms-text-size-adjust: 100% !important; -webkit-font-smoothing: antialiased !important; } .tableContent img { border: 0 !important; display: block !important; outline: none !important; } a{ color:#382F2E; }  p, h1{ color:#382F2E; margin:0; }  div,p,ul,h1{ margin:0; }   </style>  </head> <body paddingwidth=\"0\" paddingheight=\"0\" bgcolor=\"#ffffff\"  style=\"padding-top: 0; padding-bottom: 0; padding-top: 0; padding-bottom: 0; background-repeat: repeat; width: 100% !important; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; -webkit-font-smoothing: antialiased;\" offset=\"0\" toppadding=\"0\" leftpadding=\"0\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tableContent\" align=\"center\" bgcolor=\"#ffffff\" style=\'font-family:Helvetica, Arial,serif;\'>   <tr><td height=\'35\'></td></tr>  <tr> <td> <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td width=\'40\'></td> <td width=\'520\'> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">  <!-- =============================== Header ====================================== -->   <tr><td height=\'75\'></td></tr>  <tr> <td valign=\'top\' align=\'center\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\"> <p style=\'text-align:center;margin:0;font-family:Georgia,Time,sans-serif;font-size:26px;color:#222222;\'><span style=\'color:#172d43;\'>Perun Core</span></p> </div> </div> </td> </tr> <tr><td height=\'35\'></td></tr>  <!-- =============================== Body ====================================== -->  <tr> <td class=\'movableContentContainer\' valign=\'top\'>  <div class=\'movableContent\'> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr><td height=\'20\'></td></tr> <tr> <td align=\'left\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <h1 style=\'text-align:left; color:#222222; font-size:19px;font-weight:normal;\'>Hi,</h1> </div> </div> </td> </tr>  <tr><td height=\'15\'> </td></tr>  <tr> <td align=\'left\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <p  style=\'text-align:left;color:#222222;font-size:14px;font-weight:normal;line-height:19px;\'> You have recieved this email because you have requested to reset your password. </p> </div> </div> </td> </tr>  <tr><td height=\'55\'></td></tr>  <tr> <td align=\'center\'> <table> <tr> <td align=\'center\' bgcolor=\'#2c3e50\' style=\'background:#2c3e50; padding:15px 18px;-webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px;\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <a href=\'{link}\' style=\'font-size:16px;text-decoration:none;color:#ffffff;\'>Click here to reset your password</a> </div> </div> </td> </tr> </table> </td> </tr> <tr><td height=\'20\'></td></tr> </table> </div>  </td> </tr>  <tr><td height=\'65\'></td></tr>  <!-- =============================== footer ====================================== -->  <tr><td  style=\'border-bottom:1px solid #DDDDDD;\'></td></tr>  <tr><td height=\'25\'></td></tr>  <tr> <td> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td valign=\'top\' align=\'left\' width=\'370\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <p  style=\'text-align:left;color:#CCCCCC;font-size:12px;font-weight:normal;line-height:20px;\'> <span style=\'font-weight:bold;\'>peruncore.com</span> </p> </div> </div> </td> </tr> </table> </td> </tr>   </table> </td> <td width=\'40\'></td> </tr> </table> </td> </tr>  <tr><td height=\'88\'></td></tr>   </table>  <!--Default Zone    --> <!--Default Zone End-->  </body> </html> ','resend_password','peruncore@gmail.com','Perun Core'),(2,'Your request for email change','<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">  <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta name=\"generator\" content=\"HTML Tidy for HTML5 (experimental) for Windows https://github.com/w3c/tidy-html5/tree/c63cc39\" /> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />  <title>peruncore.com</title> <style type=\"text/css\"> /*<![CDATA[*/ body { padding-top: 0 !important; padding-bottom: 0 !important; padding-top: 0 !important; padding-bottom: 0 !important; margin:0 !important; width: 100% !important; -webkit-text-size-adjust: 100% !important; -ms-text-size-adjust: 100% !important; -webkit-font-smoothing: antialiased !important; } .tableContent img { border: 0 !important; display: block !important; outline: none !important; } a{ color:#382F2E; }  p, h1{ color:#382F2E; margin:0; }  div,p,ul,h1{ margin:0; } /*]]>*/ </style> </head>  <body paddingwidth=\"0\" paddingheight=\"0\" bgcolor=\"#FFFFFF\" style= \"padding-top: 0; padding-bottom: 0; padding-top: 0; padding-bottom: 0; background-repeat: repeat; width: 100% !important; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; -webkit-font-smoothing: antialiased;\" offset=\"0\" toppadding=\"0\" leftpadding=\"0\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tableContent\" align=\"center\" bgcolor=\"#FFFFFF\" style= \'font-family:Helvetica, Arial,serif;\'> <tr> <td height=\'35\'></td> </tr>  <tr> <td> <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td width=\'40\'></td>  <td width=\'520\'> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <!-- =============================== Header ====================================== -->  <tr> <td height=\'75\'></td> </tr>  <tr> <td valign=\'top\' align=\'center\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\"> <p style=\'text-align:center;margin:0;font-family:Georgia,Time,sans-serif;font-size:26px;color:#222222;\'> <span style=\'color:#172d43;\'>Perun Core</span></p> </div> </div> </td> </tr>  <tr> <td height=\'35\'></td> </tr><!-- =============================== Body ====================================== -->  <tr> <td class=\'movableContentContainer\' valign=\'top\'> <div class=\'movableContent\'> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td height=\'20\'></td> </tr>  <tr> <td align=\'left\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <h1 style=\'text-align:left; color:#222222; font-size:19px;font-weight:normal;\'>Hi,</h1> </div> </div> </td> </tr>  <tr> <td height=\'15\'></td> </tr>  <tr> <td align=\'left\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <p style=\'text-align:left;color:#222222;font-size:14px;font-weight:normal;line-height:19px;\'> You have requested to change email for an account asocciatd with <strong>peruncore.com</strong>. To activate your new email click the button below (Valid only 24 hours)</p> </div> </div> </td> </tr>  <tr> <td height=\'55\'></td> </tr>  <tr> <td align=\'center\'> <table> <tr> <td align=\'center\' bgcolor=\'#2c3e50\' style= \'background:#2c3e50; padding:15px 18px;-webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px;\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <a href=\'{link}\' style=\'font-size:16px;text-decoration:none;color:#ffffff;\'>Activate new email</a> </div> </div> </td> </tr> </table> </td> </tr>  <tr> <td height=\'20\'></td> </tr> </table> </div> </td> </tr>  <tr> <td height=\'65\'></td> </tr><!-- =============================== footer ====================================== -->  <tr> <td style=\'border-bottom:1px solid #DDDDDD;\'></td> </tr>  <tr> <td height=\'25\'></td> </tr>  <tr> <td> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td valign=\'top\' align=\'left\' width=\'370\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <p style=\'text-align:left;color:#CCCCCC;font-size:12px;font-weight:normal;line-height:20px;\'> <span style=\'font-weight:bold;\'>peruncore.com</span></p> </div> </div> </td> </tr> </table> </td> </tr> </table> </td>  <td width=\'40\'></td> </tr> </table> </td> </tr>  <tr> <td height=\'88\'></td> </tr> </table><!--Default Zone    --><!--Default Zone End--> </body> </html> L','change_email','peruncore@gmail.com','Perun Core'),(3,'Welcome to Perun Core','<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta name=\"generator\" content=\"HTML Tidy for HTML5 (experimental) for Windows https://github.com/w3c/tidy-html5/tree/c63cc39\" /> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> <title>peruncore.com</title> <style type=\"text/css\"> /*<![CDATA[*/ body { padding-top: 0 !important; padding-bottom: 0 !important; padding-top: 0 !important; padding-bottom: 0 !important; margin:0 !important; width: 100% !important; -webkit-text-size-adjust: 100% !important; -ms-text-size-adjust: 100% !important; -webkit-font-smoothing: antialiased !important; } .tableContent img { border: 0 !important; display: block !important; outline: none !important; } a{ color:#382F2E; }  p, h1{ color:#382F2E; margin:0; }  div,p,ul,h1{ margin:0; } /*]]>*/ </style> </head> <body paddingwidth=\"0\" paddingheight=\"0\" bgcolor=\"#FFFFFF\" style= \"padding-top: 0; padding-bottom: 0; padding-top: 0; padding-bottom: 0; background-repeat: repeat; width: 100% !important; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; -webkit-font-smoothing: antialiased;\" offset=\"0\" toppadding=\"0\" leftpadding=\"0\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tableContent\" align=\"center\" bgcolor=\"#FFFFFF\" style= \'font-family:Helvetica, Arial,serif;\'> <tr> <td height=\'35\'></td> </tr> <tr> <td> <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td width=\'40\'></td> <td width=\'520\'> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <!-- =============================== Header ====================================== --> <tr> <td height=\'75\'></td> </tr> <tr> <td valign=\'top\' align=\'center\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\"> <p style=\'text-align:center;margin:0;font-family:Georgia,Time,sans-serif;font-size:26px;color:#222222;\'> <span style=\'color:#172d43;\'>Perun Core</span></p> </div> </div> </td> </tr> <tr> <td height=\'35\'></td> </tr> <!-- =============================== Body ====================================== --> <tr> <td class=\'movableContentContainer\' valign=\'top\'> <div class=\'movableContent\'> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td height=\'20\'></td> </tr> <tr> <td align=\'left\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <h1 style=\'text-align:left; color:#222222; font-size:19px;font-weight:normal;\'>Welcome to Perun Core!</h1> </div> </div> </td> </tr> <tr> <td height=\'15\'></td> </tr> <tr> <td align=\'left\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <p style=\'text-align:left;color:#222222;font-size:14px;font-weight:normal;line-height:19px;\'>text here</p> </div> </div> </td> </tr> <tr> <td height=\'55\'></td> </tr> <tr> <td align=\'center\'> <table> <tr> <td align=\'center\' bgcolor=\'#2c3e50\' style= \'background:#2c3e50; padding:15px 18px;-webkit-border-radius: 4px; -moz-border-radius: 4px; border-radius: 4px;\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <a href=\'{link}\' style=\'font-size:16px;text-decoration:none;color:#ffffff;\'>Go to Perun Core now</a> </div> </div> </td> </tr> </table> </td> </tr> <tr> <td height=\'20\'></td> </tr> </table> </div> </td> </tr> <tr> <td height=\'65\'></td> </tr> <!-- =============================== footer ====================================== --> <tr> <td style=\'border-bottom:1px solid #DDDDDD;\'></td> </tr> <tr> <td height=\'25\'></td> </tr> <tr> <td> <table width=\"520\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> <tr> <td valign=\'top\' align=\'left\' width=\'370\'> <div class=\"contentEditableContainer contentTextEditable\"> <div class=\"contentEditable\" align=\'center\'> <p style=\'text-align:left;color:#CCCCCC;font-size:12px;font-weight:normal;line-height:20px;\'> <span style=\'font-weight:bold;\'>peruncore.com</span></p> </div> </div> </td> </tr> </table> </td> </tr> </table> </td> <td width=\'40\'></td> </tr> </table> </td> </tr> <tr> <td height=\'88\'></td> </tr> </table> <!--Default Zone    --><!--Default Zone End--> </body> </html>','new_account','peruncore@gmail.com','Perun Core');
