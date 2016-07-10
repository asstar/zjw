/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50619
Source Host           : localhost:3306
Source Database       : zjw

Target Server Type    : MYSQL
Target Server Version : 50619
File Encoding         : 65001

Date: 2016-07-10 11:19:44
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `Case`
-- ----------------------------
DROP TABLE IF EXISTS `Master`;
CREATE TABLE `Master` (
  `ID` char(36) NOT NULL,
  `UserID` char(36) DEFAULT NULL,
  `MasterType` varchar(255) NOT NULL DEFAULT '案件',
  `CaseName` text,
  `CaseCode` text,
  `UnderTakenDept` text,
  `UnderTaker` text,
  `TargetName` text,
  `CaseFormedDate` timestamp NULL DEFAULT NULL,
  `Note` text,
  `Movie` text,
  `Affix` char(36) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL DEFAULT b'0',
  `Timestamp` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TurnInCode` text,
  `TurnInDate` timestamp NULL DEFAULT NULL,
  `TurnInAddr` text,
  `Content` text,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of Case
-- ----------------------------
INSERT INTO `Case` VALUES ('21bf637a-ecf4-4ec0-9f4c-8c854a82bc61', '2f3fb4e4-d7ca-4518-9b8a-97f2f8902e04', '案件', '一室专案', '1001', '第一纪检监察室', '刘康', '陆俊', '2016-07-08 00:00:00', '陆俊是个好同志', null, null, '', '2016-07-08 16:38:26');
