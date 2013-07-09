# --------------------------------------------------------
# Host:                         127.0.0.1
# Server version:               5.1.41
# Server OS:                    Win32
# HeidiSQL version:             6.0.0.3603
# Date/time:                    2011-01-28 11:59:48
# --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

# Dumping database structure for dev_pv_membership
DROP DATABASE IF EXISTS `dev_pv_membership`;
CREATE DATABASE IF NOT EXISTS `dev_pv_membership` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci */;
USE `dev_pv_membership`;


# Dumping structure for table dev_pv_membership.my_aspnet_applications
CREATE TABLE IF NOT EXISTS `my_aspnet_applications` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(256) DEFAULT NULL,
  `description` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

# Dumping data for table dev_pv_membership.my_aspnet_applications: 1 rows
DELETE FROM `my_aspnet_applications`;
/*!40000 ALTER TABLE `my_aspnet_applications` DISABLE KEYS */;
INSERT INTO `my_aspnet_applications` (`id`, `name`, `description`) VALUES
	(1, '/', 'MySQL default application');
/*!40000 ALTER TABLE `my_aspnet_applications` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_membership
CREATE TABLE IF NOT EXISTS `my_aspnet_membership` (
  `userId` int(11) NOT NULL DEFAULT '0',
  `Email` varchar(128) DEFAULT NULL,
  `Comment` varchar(255) DEFAULT NULL,
  `Password` varchar(128) NOT NULL,
  `PasswordKey` char(32) DEFAULT NULL,
  `PasswordFormat` tinyint(4) DEFAULT NULL,
  `PasswordQuestion` varchar(255) DEFAULT NULL,
  `PasswordAnswer` varchar(255) DEFAULT NULL,
  `IsApproved` tinyint(1) DEFAULT NULL,
  `LastActivityDate` datetime DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  `LastPasswordChangedDate` datetime DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `IsLockedOut` tinyint(1) DEFAULT NULL,
  `LastLockedOutDate` datetime DEFAULT NULL,
  `FailedPasswordAttemptCount` int(10) unsigned DEFAULT NULL,
  `FailedPasswordAttemptWindowStart` datetime DEFAULT NULL,
  `FailedPasswordAnswerAttemptCount` int(10) unsigned DEFAULT NULL,
  `FailedPasswordAnswerAttemptWindowStart` datetime DEFAULT NULL,
  PRIMARY KEY (`userId`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COMMENT='2';

# Dumping data for table dev_pv_membership.my_aspnet_membership: 1 rows
DELETE FROM `my_aspnet_membership`;
/*!40000 ALTER TABLE `my_aspnet_membership` DISABLE KEYS */;
INSERT INTO `my_aspnet_membership` (`userId`, `Email`, `Comment`, `Password`, `PasswordKey`, `PasswordFormat`, `PasswordQuestion`, `PasswordAnswer`, `IsApproved`, `LastActivityDate`, `LastLoginDate`, `LastPasswordChangedDate`, `CreationDate`, `IsLockedOut`, `LastLockedOutDate`, `FailedPasswordAttemptCount`, `FailedPasswordAttemptWindowStart`, `FailedPasswordAnswerAttemptCount`, `FailedPasswordAnswerAttemptWindowStart`) VALUES
	(1, 'Patrickeps@gmx.de', '', 'mSeaLDnyTT1ZJRlRz9Um1zrAYCE=', 'apHHwUmTMlZdeG3Ed2qupg==', 1, NULL, NULL, 1, '2011-01-26 13:10:29', '2011-01-26 13:10:29', '2011-01-19 12:20:04', '2011-01-19 12:20:04', 0, '2011-01-21 14:52:02', 2, '2011-01-25 08:56:40', 0, '2011-01-19 12:20:04');
/*!40000 ALTER TABLE `my_aspnet_membership` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_profiles
CREATE TABLE IF NOT EXISTS `my_aspnet_profiles` (
  `userId` int(11) NOT NULL,
  `valueindex` longtext,
  `stringdata` longtext,
  `binarydata` longblob,
  `lastUpdatedDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`userId`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

# Dumping data for table dev_pv_membership.my_aspnet_profiles: 0 rows
DELETE FROM `my_aspnet_profiles`;
/*!40000 ALTER TABLE `my_aspnet_profiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `my_aspnet_profiles` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_roles
CREATE TABLE IF NOT EXISTS `my_aspnet_roles` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `applicationId` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

# Dumping data for table dev_pv_membership.my_aspnet_roles: 2 rows
DELETE FROM `my_aspnet_roles`;
/*!40000 ALTER TABLE `my_aspnet_roles` DISABLE KEYS */;
INSERT INTO `my_aspnet_roles` (`id`, `applicationId`, `name`) VALUES
	(1, 1, 'Admin'),
	(2, 1, 'Benutzer');
/*!40000 ALTER TABLE `my_aspnet_roles` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_schemaversion
CREATE TABLE IF NOT EXISTS `my_aspnet_schemaversion` (
  `version` int(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

# Dumping data for table dev_pv_membership.my_aspnet_schemaversion: 1 rows
DELETE FROM `my_aspnet_schemaversion`;
/*!40000 ALTER TABLE `my_aspnet_schemaversion` DISABLE KEYS */;
INSERT INTO `my_aspnet_schemaversion` (`version`) VALUES
	(6);
/*!40000 ALTER TABLE `my_aspnet_schemaversion` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_sessioncleanup
CREATE TABLE IF NOT EXISTS `my_aspnet_sessioncleanup` (
  `LastRun` datetime NOT NULL,
  `IntervalMinutes` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

# Dumping data for table dev_pv_membership.my_aspnet_sessioncleanup: 1 rows
DELETE FROM `my_aspnet_sessioncleanup`;
/*!40000 ALTER TABLE `my_aspnet_sessioncleanup` DISABLE KEYS */;
INSERT INTO `my_aspnet_sessioncleanup` (`LastRun`, `IntervalMinutes`) VALUES
	('2011-01-15 08:41:12', 10);
/*!40000 ALTER TABLE `my_aspnet_sessioncleanup` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_sessions
CREATE TABLE IF NOT EXISTS `my_aspnet_sessions` (
  `SessionId` varchar(255) NOT NULL,
  `ApplicationId` int(11) NOT NULL,
  `Created` datetime NOT NULL,
  `Expires` datetime NOT NULL,
  `LockDate` datetime NOT NULL,
  `LockId` int(11) NOT NULL,
  `Timeout` int(11) NOT NULL,
  `Locked` tinyint(1) NOT NULL,
  `SessionItems` longblob,
  `Flags` int(11) NOT NULL,
  PRIMARY KEY (`SessionId`,`ApplicationId`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

# Dumping data for table dev_pv_membership.my_aspnet_sessions: 0 rows
DELETE FROM `my_aspnet_sessions`;
/*!40000 ALTER TABLE `my_aspnet_sessions` DISABLE KEYS */;
/*!40000 ALTER TABLE `my_aspnet_sessions` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_users
CREATE TABLE IF NOT EXISTS `my_aspnet_users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `applicationId` int(11) NOT NULL,
  `name` varchar(256) NOT NULL,
  `isAnonymous` tinyint(1) NOT NULL DEFAULT '1',
  `lastActivityDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

# Dumping data for table dev_pv_membership.my_aspnet_users: 1 rows
DELETE FROM `my_aspnet_users`;
/*!40000 ALTER TABLE `my_aspnet_users` DISABLE KEYS */;
INSERT INTO `my_aspnet_users` (`id`, `applicationId`, `name`, `isAnonymous`, `lastActivityDate`) VALUES
	(1, 1, 'Epstone', 0, '2011-01-26 13:10:29');
/*!40000 ALTER TABLE `my_aspnet_users` ENABLE KEYS */;


# Dumping structure for table dev_pv_membership.my_aspnet_usersinroles
CREATE TABLE IF NOT EXISTS `my_aspnet_usersinroles` (
  `userId` int(11) NOT NULL DEFAULT '0',
  `roleId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`userId`,`roleId`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

# Dumping data for table dev_pv_membership.my_aspnet_usersinroles: 1 rows
DELETE FROM `my_aspnet_usersinroles`;
/*!40000 ALTER TABLE `my_aspnet_usersinroles` DISABLE KEYS */;
INSERT INTO `my_aspnet_usersinroles` (`userId`, `roleId`) VALUES
	(1, 1);
/*!40000 ALTER TABLE `my_aspnet_usersinroles` ENABLE KEYS */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
