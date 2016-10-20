/*
SQLyog Enterprise - MySQL GUI v8.1 
MySQL - 5.6.21 : Database - ly
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;
/*!40101 SET SQL_MODE=''*/;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE DATABASE /*!32312 IF NOT EXISTS*/`ly` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ly`;

/*Table structure for table `sys_role` */
DROP TABLE IF EXISTS `sys_role`;

CREATE TABLE `sys_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '角色ID',
  `name` varchar(255) NOT NULL COMMENT '角色名称',
  `description` varchar(255) DEFAULT NULL COMMENT '角色描述',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='角色表';

/*Table structure for table `sys_user` */
DROP TABLE IF EXISTS `sys_user`;

CREATE TABLE `sys_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `account` archar(16) NOT NULL COMMENT '账号',
  `password` varchar(255) DEFAULT NULL COMMENT '密码',
  `name` varchar(16) DEFAULT NULL COMMENT '名称',
  `mobile` varchar(16) DEFAULT NULL COMMENT '电话',
  `laston` datetime NULL COMMENT '最后登录时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `UQ_sys_user_account` (`account`),
  UNIQUE KEY `UQ_sys_user_name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='用户表';

/*Table structure for table `sys_user_sys_role` */

DROP TABLE IF EXISTS `sys_roleusermapping`;

CREATE TABLE `sys_roleusermapping` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `userid` int(11) NOT NULL COMMENT '用户ID',
  `roleid` int(11) NOT NULL COMMENT '角色ID',
  PRIMARY KEY (`id`),
  KEY `userid` (`userid`),
  KEY `roleid` (`roleid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='角色用户映射表';
