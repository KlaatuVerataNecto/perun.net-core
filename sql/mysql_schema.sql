DROP TABLE IF EXISTS users;
CREATE TABLE users (
	id INT(11) NOT NULL AUTO_INCREMENT,
	username VARCHAR(25) NOT NULL,
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

DROP TABLE IF EXISTS email_template;
CREATE TABLE email_template (
  id int(10) NOT NULL AUTO_INCREMENT,
  email_subject text NOT NULL,
  email_body text NOT NULL,
  template_type varchar(15) DEFAULT NULL,
  sender_name varchar(100) DEFAULT NULL,
  sender_email varchar(100) DEFAULT NULL,
  PRIMARY KEY (id)
);

DROP TABLE IF EXISTS email_queue;
CREATE TABLE email_queue (
  id int(10) NOT NULL AUTO_INCREMENT,
  receiver varchar(200) NOT NULL,
  sender varchar(200) NOT NULL,
  email_subject text NOT NULL,
  email_body text NOT NULL,
  date_sent datetime DEFAULT NULL,
  is_sent tinyint(1) NOT NULL,
  PRIMARY KEY (id)
);