USE master;
GO

-- 开启事务
BEGIN TRY
    BEGIN TRANSACTION;
    
    -- 删除已存在的数据库
    IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MySchool')
    BEGIN
        ALTER DATABASE MySchool SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        DROP DATABASE MySchool;
        PRINT '数据库 MySchool 已成功删除';
    END
    
    -- 创建新数据库
    CREATE DATABASE MySchool;
    PRINT '数据库 MySchool 已成功创建';
    
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO


USE MySchool
GO

-- ============================================
-- 1. 班级表 (Grade) - 优化版本
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'Student')
    DROP TABLE Student
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'Grade')
    DROP TABLE Grade
GO

CREATE TABLE Grade
(
    GradeGUID       VARCHAR(50) PRIMARY KEY,				-- 班级GUID主键
    GradeName       NVARCHAR(50) NOT NULL,					-- 班级名称（支持中文）
    GradeCode       VARCHAR(20) NOT NULL UNIQUE,			-- 班级编码（如 G2024001）
    CreateDate      DATETIME NOT NULL DEFAULT GETDATE(),	-- 创建日期
    ZT              INT NOT NULL DEFAULT 0,					-- 状态（0.禁用 1.启用）
    Remark          NVARCHAR(500) NULL,						-- 备注信息
    TenantID        VARCHAR(36) NOT NULL					-- 租户标识（用于多租户隔离）
)
GO

-- 添加表注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='班级信息表', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade'
GO

-- 添加字段注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='班级GUID主键', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade', @level2type='COLUMN', @level2name='GradeGUID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='班级名称（支持中文）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade', @level2type='COLUMN', @level2name='GradeName'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='班级编码（如 G2024001）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade', @level2type='COLUMN', @level2name='GradeCode'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='创建日期', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade', @level2type='COLUMN', @level2name='CreateDate'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='状态（0.禁用 1.启用）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade', @level2type='COLUMN', @level2name='ZT'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='备注信息', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade', @level2type='COLUMN', @level2name='Remark'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='租户标识（用于多租户隔离）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Grade', @level2type='COLUMN', @level2name='TenantID'
GO

-- 插入测试数据
INSERT INTO Grade (GradeGUID, GradeName, GradeCode, TenantID) VALUES 
    (NEWID(), N'一年级', 'G2024001', '5D6D773B-E702-44F6-9A17-8653EDB750EC'),
    (NEWID(), N'二年级', 'G2024002', 'B9293E23-8BAC-4513-A074-B988CBC498B4'),
    (NEWID(), N'三年级', 'G2024003', '8977C3AF-1D71-4553-B431-5DA584FDBDC1')
GO

-- ============================================
-- 2. 学生表 (Student) - 优化版本
-- ============================================


CREATE TABLE Student
(
    GUID    VARCHAR(50) PRIMARY KEY,		-- 学生ID自增主键
    StudentCode    VARCHAR(20) NOT NULL UNIQUE,			-- 学号（如 S20240001）
    StudentName    NVARCHAR(50) NOT NULL,				-- 学生姓名（支持中文）
    LoginName      VARCHAR(50) NOT NULL UNIQUE,			-- 登录账号（唯一）
    Password       VARCHAR(128) NOT NULL,				-- 登录密码（实际应用应存储哈希值）
    Gender         INT NOT NULL DEFAULT 0,			-- 性别: 0-未知, 1-男, 2-女
    Age            INT NULL,						-- 年龄（可计算出生日期替代）
    Phone          VARCHAR(20) NULL,					-- 手机号码
    Address        NVARCHAR(200) NULL,					-- 家庭地址
    Status         INT NOT NULL DEFAULT 1,			-- 状态: 1-在读, 2-休学, 3-毕业, 4-退学
    Remark         NVARCHAR(500) NULL,					-- 备注信息
    CreateDate     DATETIME NOT NULL DEFAULT GETDATE(),	-- 创建日期
    GradeGUID      VARCHAR(50) NOT NULL,				-- 外键：关联班级GUID
    
    CONSTRAINT FK_Student_Grade FOREIGN KEY (GradeGUID) REFERENCES Grade(GradeGUID)
)
GO

-- 添加表注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='学生信息表', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student'
GO

-- 添加字段注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='学生GUID主键', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='GUID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='学号（如 S20240001）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='StudentCode'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='学生姓名（支持中文）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='StudentName'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='登录账号（唯一）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='LoginName'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='登录密码（实际应用应存储哈希值）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='Password'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='性别: 0-未知, 1-男, 2-女', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='Gender'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='年龄', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='Age'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='手机号码', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='Phone'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='家庭地址', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='Address'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='状态: 1-在读, 2-休学, 3-毕业, 4-退学', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='Status'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='备注信息', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='Remark'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='创建日期', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='CreateDate'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='外键：关联班级GUID', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Student', @level2type='COLUMN', @level2name='GradeGUID'
GO

-- 插入测试数据（注意：GradeGUID需要使用Grade表中实际的GUID）
INSERT INTO Student (GUID,StudentCode, StudentName, LoginName, Password, Gender, Age, Phone, Address, Status, Remark, GradeGUID) 
SELECT newid(), 
    'S2024001', N'张楚', 'zhangchu', '123456', 1, 19, '13635842365', N'重庆', 1, N'爱耍小聪明', GradeGUID 
FROM Grade WHERE GradeName = N'一年级'
UNION ALL
SELECT  newid(),  'S2024002', N'李阳', 'liyang', '123456', 1, 20, '13998213628', N'北京', 1, N'脾气温和的好人', GradeGUID 
FROM Grade WHERE GradeName = N'一年级'
UNION ALL
SELECT  newid(),'S2024003', N'杨涵', 'yanghan', '123456', 1, 21, '18736522854', N'上海', 1, N'富二代', GradeGUID 
FROM Grade WHERE GradeName = N'二年级'
UNION ALL
SELECT newid(),'S2024004', N'周梦飞', 'zhoumf', '123456', 1, 21, '18998522247', N'南京', 1, N'刚从国外留学回来', GradeGUID 
FROM Grade WHERE GradeName = N'二年级'
UNION ALL
SELECT newid(),'S2024005', N'余瑶', 'yuyao', '123456', 2, 19, '13169852364', N'广州', 1, N'学霸', GradeGUID 
FROM Grade WHERE GradeName = N'一年级'
UNION ALL
SELECT newid(),'S2024006', N'何汉阳', 'hehanyang', '123456', 1, 20, '18954753698', N'三亚', 1, N'外省过来的朋友', GradeGUID 
FROM Grade WHERE GradeName = N'三年级'
UNION ALL
SELECT newid(),'S2024007', N'冷月', 'lengyue', '123456', 2, 22, '15987463698', N'台北', 1, N'喜欢游泳和健身', GradeGUID 
FROM Grade WHERE GradeName = N'三年级'
UNION ALL
SELECT newid(),'S2024008', N'向勇', 'xiangyong', '123456', 1, 18, '13336589854', N'天津', 1, N'校学生会主席', GradeGUID 
FROM Grade WHERE GradeName = N'二年级'
UNION ALL
SELECT newid(),'S2024009', N'范雪', 'fanxue', '123456', 2, 18, '13996859742', N'深圳', 1, N'在学校附近租的房子', GradeGUID 
FROM Grade WHERE GradeName = N'三年级'
UNION ALL
SELECT newid(),'S2024010', N'刘晴冰', 'liuqingbing', '123456', 2, 25, '18998523647', N'青岛', 1, N'比较时尚', GradeGUID 
FROM Grade WHERE GradeName = N'一年级'
UNION ALL
SELECT newid(),'S2024011', N'张莉', 'zhangli', '123456', 2, 22, '15963587935', N'福州', 1, N'独立生活能力强', GradeGUID 
FROM Grade WHERE GradeName = N'三年级'
GO

-- ============================================
-- 3. 星座表 (Constellation) - 优化版本
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'Constellation')
    DROP TABLE Constellation
GO

CREATE TABLE Constellation
(
    ConstellationID   INT IDENTITY(1,1) PRIMARY KEY,	-- 星座ID自增主键
    ConstellationName NVARCHAR(20) NOT NULL UNIQUE,	-- 星座名称（支持中文）
    ImagePath         VARCHAR(100) NULL,				-- 星座图标图片路径
    IsShow            INT NOT NULL DEFAULT 1,			-- 是否显示（0.不显示 1.显示）
    SortOrder         INT NOT NULL DEFAULT 0,			-- 排序序号（数值越小越靠前）
    Remark            NVARCHAR(200) NULL				-- 备注信息
)
GO

-- 添加表注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='星座信息表', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Constellation'
GO

-- 添加字段注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='星座ID自增主键', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Constellation', @level2type='COLUMN', @level2name='ConstellationID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='星座名称（支持中文）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Constellation', @level2type='COLUMN', @level2name='ConstellationName'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='星座图标图片路径', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Constellation', @level2type='COLUMN', @level2name='ImagePath'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='是否显示（0.不显示 1.显示）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Constellation', @level2type='COLUMN', @level2name='IsShow'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='排序序号（数值越小越靠前）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Constellation', @level2type='COLUMN', @level2name='SortOrder'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='备注信息', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Constellation', @level2type='COLUMN', @level2name='Remark'
GO

INSERT INTO Constellation (ConstellationName, ImagePath, SortOrder) VALUES
    (N'白羊座', 'baiyang.gif', 1),
    (N'金牛座', 'jinniu.gif', 2),
    (N'双子座', 'shuangzi.gif', 3),
    (N'巨蟹座', 'juxie.gif', 4),
    (N'狮子座', 'shizi.gif', 5),
    (N'处女座', 'chunv.gif', 6),
    (N'天秤座', 'tiancheng.gif', 7),
    (N'天蝎座', 'tianxie.gif', 8),
    (N'射手座', 'sheshou.gif', 9),
    (N'摩羯座', 'mojie.gif', 10),
    (N'水瓶座', 'shuiping.gif', 11),
    (N'双鱼座', 'shuangyu.gif', 12)
GO

-- ============================================
-- 4. 公司组织架构表 (Company) - 优化版本
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'Company')
    DROP TABLE Company
GO

CREATE TABLE Company
(
    CompanyGUID   VARCHAR(50) PRIMARY KEY,				-- 公司GUID主键
    CompanyName   NVARCHAR(100) NOT NULL,				-- 公司/部门名称（支持中文）
    ParentGuID    VARCHAR(50) NULL,						-- 父级GUID（NULL表示根节点）
    CreateDate    DATETIME NOT NULL DEFAULT GETDATE(),	-- 创建日期
    ZT			  INT NOT NULL DEFAULT 0,				-- 状态（0.禁用 1.启用）
    Level         TINYINT NOT NULL DEFAULT 1,			-- 组织层级（1.根节点 2.一级 3.二级）
    SortOrder     INT NOT NULL DEFAULT 0,				-- 排序序号（同级排序）
    
    CONSTRAINT FK_Company_Parent FOREIGN KEY (ParentGuID) REFERENCES Company(CompanyGUID)
)
GO

-- 添加表注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='公司组织架构树形表', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company'
GO

-- 添加字段注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='公司GUID主键', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company', @level2type='COLUMN', @level2name='CompanyGUID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='公司/部门名称（支持中文）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company', @level2type='COLUMN', @level2name='CompanyName'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='父级GUID（NULL表示根节点）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company', @level2type='COLUMN', @level2name='ParentGuID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='创建日期', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company', @level2type='COLUMN', @level2name='CreateDate'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='状态（0.禁用 1.启用）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company', @level2type='COLUMN', @level2name='ZT'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='组织层级（1.根节点 2.一级 3.二级）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company', @level2type='COLUMN', @level2name='Level'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='排序序号（同级排序）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Company', @level2type='COLUMN', @level2name='SortOrder'
GO

-- 插入根节点
INSERT INTO Company (CompanyGUID, CompanyName, ParentGuID, Level, SortOrder) 
VALUES ('812F3810-4FF2-4DE0-9039-178536FB1143', N'公司总部', NULL, 1, 0)
GO

-- 插入子节点
INSERT INTO Company (CompanyGUID, CompanyName, ParentGuID, Level, SortOrder) VALUES
    ('BEDEBED5-E48D-4420-AFA5-92B9BDFD7965', N'北京分部', '812F3810-4FF2-4DE0-9039-178536FB1143', 2, 1),
    ('49E3C679-EAB0-41CB-8FF4-B58CA9A4FF48', N'上海分部', '812F3810-4FF2-4DE0-9039-178536FB1143', 2, 2),
    ('D233201C-DBBF-4A5E-8A84-A0A9C5CA004C', N'重庆分部', '812F3810-4FF2-4DE0-9039-178536FB1143', 2, 3),
    (NEWID(), N'策划部', 'BEDEBED5-E48D-4420-AFA5-92B9BDFD7965', 3, 1),
    (NEWID(), N'销售部', 'BEDEBED5-E48D-4420-AFA5-92B9BDFD7965', 3, 2),
    (NEWID(), N'统筹部', 'BEDEBED5-E48D-4420-AFA5-92B9BDFD7965', 3, 3),
    (NEWID(), N'人事部', '49E3C679-EAB0-41CB-8FF4-B58CA9A4FF48', 3, 1),
    (NEWID(), N'IT部', '49E3C679-EAB0-41CB-8FF4-B58CA9A4FF48', 3, 2),
    (NEWID(), N'营销部', '49E3C679-EAB0-41CB-8FF4-B58CA9A4FF48', 3, 3),
    (NEWID(), N'工程部', 'D233201C-DBBF-4A5E-8A84-A0A9C5CA004C', 3, 1),
    (NEWID(), N'财务部', 'D233201C-DBBF-4A5E-8A84-A0A9C5CA004C', 3, 2),
    (NEWID(), N'行政部', 'D233201C-DBBF-4A5E-8A84-A0A9C5CA004C', 3, 3)
GO

-- ============================================
-- 5. 地址字典表 (Address) - 优化版本
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'Address')
    DROP TABLE Address
GO

CREATE TABLE Address
(
    AddressID     INT IDENTITY(1,1) PRIMARY KEY,		-- 地址ID自增主键
    AddressName   NVARCHAR(50) NOT NULL UNIQUE,			-- 城市名称（唯一）
    CityCode      VARCHAR(10) NULL,						-- 城市编码
    Province      NVARCHAR(20) NULL,					-- 所属省份
    CreateUser    NVARCHAR(20) NOT NULL DEFAULT 'admin',	-- 创建人
    CreateDate    DATETIME NOT NULL DEFAULT GETDATE(),	-- 创建日期
    ZT			  INT NOT NULL DEFAULT 1					-- 状态（0.禁用 1.启用）
)
GO

-- 添加表注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='城市地址字典表', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address'
GO

-- 添加字段注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='地址ID自增主键', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address', @level2type='COLUMN', @level2name='AddressID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='城市名称（唯一）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address', @level2type='COLUMN', @level2name='AddressName'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='城市编码', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address', @level2type='COLUMN', @level2name='CityCode'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='所属省份', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address', @level2type='COLUMN', @level2name='Province'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='创建人', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address', @level2type='COLUMN', @level2name='CreateUser'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='创建日期', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address', @level2type='COLUMN', @level2name='CreateDate'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='状态（0.禁用 1.启用）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Address', @level2type='COLUMN', @level2name='ZT'
GO

INSERT INTO Address (AddressName, Province) VALUES
    (N'重庆市', N'重庆市'),
    (N'北京市', N'北京市'),
    (N'天津市', N'天津市'),
    (N'成都市', N'四川省'),
    (N'上海市', N'上海市'),
    (N'广州市', N'广东省'),
    (N'深圳市', N'广东省'),
    (N'南京市', N'江苏省'),
    (N'福州市', N'福建省'),
    (N'青岛市', N'山东省'),
    (N'三亚市', N'海南省')
GO

-- ============================================
-- 6. 菜单权限表 (Menu) - 优化版本
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'Menu')
    DROP TABLE Menu
GO

CREATE TABLE Menu
(
    MenuGUID      VARCHAR(50) PRIMARY KEY,				-- 菜单GUID主键
    MenuCode      VARCHAR(50) NOT NULL UNIQUE,			-- 菜单编码（唯一）
    MenuName      NVARCHAR(50) NOT NULL,				-- 菜单名称（支持中文）
    ParentGUID    VARCHAR(50) NULL,						-- 父菜单GUID（NULL表示根菜单）
    Level         TINYINT NOT NULL DEFAULT 1,			-- 菜单层级（1.一级 2.二级 3.三级）
    Path          VARCHAR(200) NULL,					-- 路由路径（前端路由）
    Component     VARCHAR(100) NULL,					-- 组件路径（前端组件）
    Icon          VARCHAR(50) NULL,						-- 图标名称
    IconBgColor   VARCHAR(20) NULL,						-- 图标背景色
    Label         NVARCHAR(50) NULL,					-- 标签（显示名称）
    SortOrder     INT NOT NULL DEFAULT 0,				-- 排序序号（同级排序）
    IsVisible     INT NOT NULL DEFAULT 1,				-- 是否可见（0.隐藏 1.显示）
    
    CONSTRAINT FK_Menu_Parent FOREIGN KEY (ParentGUID) REFERENCES Menu(MenuGUID)
)
GO

-- 添加表注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='系统菜单权限表', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu'
GO

-- 添加字段注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='菜单GUID主键', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='MenuGUID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='菜单编码（唯一）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='MenuCode'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='菜单名称（支持中文）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='MenuName'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='父菜单GUID（NULL表示根菜单）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='ParentGUID'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='菜单层级（1.一级 2.二级 3.三级）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='Level'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='路由路径（前端路由）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='Path'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='组件路径（前端组件）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='Component'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='图标名称', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='Icon'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='图标背景色', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='IconBgColor'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='标签（显示名称）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='Label'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='排序序号（同级排序）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='SortOrder'
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='是否可见（0.隐藏 1.显示）', @level0type='SCHEMA', @level0name='dbo', @level1type='TABLE', @level1name='Menu', @level2type='COLUMN', @level2name='IsVisible'
GO

-- ============================================
-- 7. 创建索引优化查询性能
-- ============================================
-- 学生表索引
CREATE NONCLUSTERED INDEX IX_Student_GradeGUID ON Student(GradeGUID)
CREATE NONCLUSTERED INDEX IX_Student_LoginName ON Student(LoginName)
CREATE NONCLUSTERED INDEX IX_Student_StudentCode ON Student(StudentCode)
CREATE NONCLUSTERED INDEX IX_Student_Status ON Student(Status)
CREATE NONCLUSTERED INDEX IX_Student_CreateDate ON Student(CreateDate)

-- 公司表索引
CREATE NONCLUSTERED INDEX IX_Company_ParentGuID ON Company(ParentGuID)
CREATE NONCLUSTERED INDEX IX_Company_Level ON Company(Level)
CREATE NONCLUSTERED INDEX IX_Company_ZT ON Company(ZT)

-- 菜单表索引
CREATE NONCLUSTERED INDEX IX_Menu_ParentGUID ON Menu(ParentGUID)
CREATE NONCLUSTERED INDEX IX_Menu_Level ON Menu(Level)
CREATE NONCLUSTERED INDEX IX_Menu_IsVisible ON Menu(IsVisible)

-- 地址表索引
CREATE NONCLUSTERED INDEX IX_Address_ZT ON Address(ZT)
CREATE NONCLUSTERED INDEX IX_Address_Province ON Address(Province)

-- 星座表索引
CREATE NONCLUSTERED INDEX IX_Constellation_IsShow ON Constellation(IsShow)
CREATE NONCLUSTERED INDEX IX_Constellation_SortOrder ON Constellation(SortOrder)

-- Grade表索引
CREATE NONCLUSTERED INDEX IX_Grade_ZT ON Grade(ZT)
CREATE NONCLUSTERED INDEX IX_Grade_TenantID ON Grade(TenantID)

-- ============================================
-- 8. 创建视图便于查询（示例）
-- ============================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'V_StudentInfo')
    DROP VIEW V_StudentInfo
GO

CREATE VIEW V_StudentInfo
AS SELECT 
    s.GUID,
    s.StudentCode AS 学号,
    s.StudentName AS 姓名,
    s.LoginName AS 登录名,
    CASE s.Gender 
        WHEN 1 THEN N'男' 
        WHEN 2 THEN N'女' 
        ELSE N'未知' 
    END AS 性别,
    s.Age AS 年龄,
    s.Phone AS 手机号,
    s.Address AS 地址,
    CASE s.Status 
        WHEN 1 THEN N'在读' 
        WHEN 2 THEN N'休学' 
        WHEN 3 THEN N'毕业' 
        WHEN 4 THEN N'退学' 
        ELSE N'未知' 
    END AS 状态,
    s.CreateDate AS 创建时间,
    g.GradeName AS 班级名称,
    g.GradeCode AS 班级编码
FROM Student s
INNER JOIN Grade g ON s.GradeGUID = g.GradeGUID
WHERE s.Status = 1  -- 默认只查在读学生
GO

-- 添加视图注释
EXEC sys.sp_addextendedproperty @name='MS_Description', @value='学生信息视图（仅显示在读学生）', @level0type='SCHEMA', @level0name='dbo', @level1type='VIEW', @level1name='V_StudentInfo'
GO

-- ============================================
-- 9. 验证数据
-- ============================================
SELECT 'Grade表数据' AS 表名, COUNT(*) AS 记录数 FROM Grade
UNION ALL
SELECT 'Student表数据', COUNT(*) FROM Student
UNION ALL
SELECT 'Constellation表数据', COUNT(*) FROM Constellation
UNION ALL
SELECT 'Company表数据', COUNT(*) FROM Company
UNION ALL
SELECT 'Address表数据', COUNT(*) FROM Address
UNION ALL
SELECT 'Menu表数据', COUNT(*) FROM Menu
GO

-- 查询示例
SELECT * FROM V_StudentInfo
GO