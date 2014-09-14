CREATE DATABASE  IF NOT EXISTS `Luval`;
USE `Luval`;


DROP TABLE IF EXISTS `UserLogin`;
DROP TABLE IF EXISTS `UserRole`;
DROP TABLE IF EXISTS `UserClaim`;
DROP TABLE IF EXISTS `Role`;
DROP TABLE IF EXISTS `User`;

CREATE TABLE `User`
(
	Id varchar(100) NOT NULL PRIMARY KEY,
	UserName varchar(100) NOT NULL,
	LoweredUserName varchar(100) NOT NULL,
	PrimaryEmail varchar(100) NOT NULL,
	Name varchar(100) NOT NULL,
	LastName varchar(100) NULL,
	CountryCode varchar(4) NULL,
	State varchar(20) NULL,
	Region varchar(100) NULL,
	Address varchar(100) NULL,
	ZipCode varchar(100) NULL,
	MsTimeZone varchar(100) NULL,
	IsoTimeZone varchar(100) NULL,
	Birthday datetime NULL,
	JsonSettings varchar(4000) NULL,
	PasswordHash varchar(200) NULL,
	PasswordSalt varchar(200) NULL,
	TemporaryPasswordHash varchar(200) NULL,
	TemporaryPasswordSalt varchar(200) NULL,
	IsActive bit NOT NULL,
	IsLocked bit NOT NULL,
	RequirePasswordChange  bit NOT NULL,
	FailedPasswordAttemptCount int NOT NULL,
	UtcFailedPasswordAnswerAttemptWindowStart datetime NULL,
	UtcLastLoginDate datetime NULL,
	UtcLastLockedOutDate datetime NULL,
	UtcLastFailedAttempt datetime NULL,
	Version int NOT NULL DEFAULT 1,
	UpdatedFrom varchar(100) NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL
);

INSERT INTO `User` VALUES('B13221A21AB44994A6F998BBF63E4243', 'oscar@marin.cr', 'oscar@marin.cr', 'oscar@marin.cr', 'Oscar', 'Marin', 'CR', 'Heredia', 'San Rafael', 'Los Angeles, Calle Ebais, 100E y 350S', 
'40504', 'Central America', null, '1983-01-19', null, 
'JNDw7Avrw8puojkmp48Tci/tnhKKHsIQ0P64gx9JYLtQ1ZRQDBVyAYFPujGvM/t+2qSgR8+6IZlDc0bQwntvJEWYhtW/bG3ODLukp/pVuh8JnP5mjEvaiSbYQ+4izXuJakZjtki9lxckPVsVfIefoIPLbRvfR32ixNiy+4f+1NI=',
'x+a8VzqzXHxrYMNEDeRXrx7Tl6Jiaa6BYdYSb0b7G+FsYeiIwW2Vfi5rvCnRULDdewkUkxU+MAgG3dp9TmWy0NsPSy3LjrY9YOa3QS84kOMlyIrVeJazn85P+8SFyBnt4xODQplep/7SP2pBnAsfHj/pLoIuTFrkhilrJqzrGVk=',
null, null, true, false, false, 0, null,null,null,null,1, null, utc_timestamp(), utc_timestamp());

CREATE TABLE `UserLogin`
(
	Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
	UserId varchar(100) NOT NULL,
	Provider varchar(100) NOT NULL,
	ProviderType varchar(100) NOT NULL,
	ProviderKey varchar(100) NOT NULL,
	Version int NOT NULL DEFAULT 1,
	UpdatedFrom varchar(100) NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	FOREIGN KEY (`UserId`) REFERENCES `User`(`Id`)
);

INSERT INTO `UserLogin` 
(UserId, Porvider, ProviderType, ProviderKey, Version, UtcCreatedOn, UtcUpdatedOn)
SELECT Id, 'Application', 'Application', Id, 1, utc_timestamp(), utc_timestamp()
FROM `User` WHERE UserName = 'oscar@marin.cr';

CREATE TABLE `Role`
(
	Id varchar(100) NOT NULL PRIMARY KEY,
	Name varchar(100) NOT NULL,
	Version int NOT NULL DEFAULT 1,
	UpdatedFrom varchar(100) NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	CreatedBy varchar(100) NOT NULL,
	UpdatedBy varchar(100) NOT NULL
);

INSERT INTO `Role` VALUES ('DEF5AA7952674BB98ED2E7625CFBFC0D', 'Administrator', 1, null, utc_timestamp(), utc_timestamp(), 'oscar@marin.cr', 'oscar@marin.cr');

CREATE TABLE `UserRole`
(
	Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
	UserId varchar(100) NOT NULL,
	RoleId varchar(100) NOT NULL,
	Version int NOT NULL DEFAULT 1,
	UpdatedFrom varchar(100) NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	CreatedBy varchar(100) NOT NULL,
	UpdatedBy varchar(100) NOT NULL,
	FOREIGN KEY (`UserId`) REFERENCES `User`(`Id`),
	FOREIGN KEY (`RoleId`) REFERENCES `Role`(`Id`)
);

INSERT INTO `UserRole` (UserId, RoleId, Version, UtcCreatedOn, UtcUpdatedOn, CreatedBy, UpdatedBy) VALUES 
((SELECT Id from `User` WHERE UserName = 'oscar@marin.cr'), (SELECT Id FROM `Role` WHERE Name = 'Administrator'), 
1, utc_timestamp(), utc_timestamp(), 'oscar@marin.cr', 'oscar@marin.cr');


CREATE TABLE `UserClaim`
(
	Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
	UserId varchar(100) NOT NULL,
	Porvider varchar(100) NOT NULL,
	Value varchar(100) NOT NULL,
	ValueType varchar(100) NOT NULL,
	Type varchar(100) NOT NULL,
	Version int NOT NULL DEFAULT 1,
	UpdatedFrom varchar(100) NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	FOREIGN KEY (`UserId`) REFERENCES `User`(`Id`)
);

