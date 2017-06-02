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
	PASSWORD VARCHAR(250) NULL,
	salt VARCHAR(16) NULL,
	external_id INT NULL,
	provider VARCHAR(10) NOT NULL,
	access_token TEXT NULL,
	date_created DATETIME NULL,	
	PRIMARY KEY (id)
 );
 
DROP TABLE IF EXISTS users_reset;
CREATE TABLE users_reset(
	id INT(11) NOT NULL AUTO_INCREMENT,
	user_login_id INT(11) NOT NULL,
	reminder_token_expiry_date DATETIME NULL,
	reminder_token VARCHAR(150) NULL,
	newemail VARCHAR(250) NULL,
	newemail_token VARCHAR(150) NULL,
	new_email_token_expiry_date DATETIME NULL,
	date_created DATETIME NULL,
	PRIMARY KEY (id)
 );