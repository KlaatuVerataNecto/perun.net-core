/*
SQLyog Community v12.4.3 (64 bit)
MySQL - 10.1.21-MariaDB : Database - peruncore
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


/*Table structure for table `email_queue` */

DROP TABLE IF EXISTS `email_queue`;

CREATE TABLE `email_queue` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `receiver` varchar(200) NOT NULL,
  `sender` varchar(200) NOT NULL,
  `email_subject` text NOT NULL,
  `email_body` text NOT NULL,
  `date_sent` datetime DEFAULT NULL,
  `is_sent` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `email_template` */

DROP TABLE IF EXISTS `email_template`;

CREATE TABLE `email_template` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `email_subject` text NOT NULL,
  `email_body` text NOT NULL,
  `template_type` varchar(15) DEFAULT NULL,
  `sender_name` varchar(100) DEFAULT NULL,
  `sender_email` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

/*Table structure for table `posts` */

DROP TABLE IF EXISTS `posts`;

CREATE TABLE `posts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(250) NOT NULL,
  `date_created` datetime NOT NULL,
  `is_published` tinyint(1) NOT NULL,
  `postimage` varchar(50) NOT NULL,
  `guid` char(16) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(25) DEFAULT NULL,
  `roles` varchar(50) DEFAULT NULL,
  `last_seen` datetime DEFAULT NULL,
  `date_created` datetime DEFAULT NULL,
  `avatar` varchar(50) DEFAULT NULL,
  `is_locked` tinyint(1) NOT NULL,
  `cover` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

/*Table structure for table `users_email` */

DROP TABLE IF EXISTS `users_email`;

CREATE TABLE `users_email` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_login_id` int(11) NOT NULL,
  `newemail` varchar(250) DEFAULT NULL,
  `token` varchar(150) DEFAULT NULL,
  `token_expiry_date` datetime DEFAULT NULL,
  `date_modified` datetime DEFAULT NULL,
  `date_created` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `users_login` */

DROP TABLE IF EXISTS `users_login`;

CREATE TABLE `users_login` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `email` varchar(250) NOT NULL,
  `passwd` varchar(250) DEFAULT NULL,
  `salt` varchar(16) DEFAULT NULL,
  `external_id` varchar(50) DEFAULT NULL,
  `provider` varchar(15) NOT NULL,
  `access_token` text,
  `date_created` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

/*Table structure for table `users_password` */

DROP TABLE IF EXISTS `users_password`;

CREATE TABLE `users_password` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_login_id` int(11) NOT NULL,
  `token_expiry_date` datetime DEFAULT NULL,
  `token` varchar(150) DEFAULT NULL,
  `date_modified` datetime DEFAULT NULL,
  `date_created` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `users_username` */

DROP TABLE IF EXISTS `users_username`;

CREATE TABLE `users_username` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `provider` varchar(10) NOT NULL,
  `token` text,
  `date_created` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
