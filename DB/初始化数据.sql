USE [CRMDB]
GO
/****建立菜单*****/
INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType],[GroupType])
select '00','系统设置',1 ,'',0,null,0,0
union all 
select '01','供应商管理',1 ,'',1,null,0,1
union all 
select '02','单据管理',1 ,'',2,null,0,1
union all 
select '03','信息查询',1 ,'',3,null,0,1
union all 
select '04','结算频道',1 ,'',4,null,0,1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType],[GroupType])
select '0001','部门设置',1 ,'/Config/DeptManage',0,'00',1,0
union all 
select '0002','用户组',1 ,'/Config/UserGroupManage',1,'00',1,0
union all 
select '0003','用户管理',1 ,'/Config/UserManage',2,'00',1,0
union all 
select '0004','系统日志',0 ,'',3,'00',1,0
go


/****建立部门*****/

INSERT INTO [tDept]([DeptCode],[DeptName],[ParentCode],[BuildUser],[EditUser])
 select 'root' ,'公司',null,'',''
 GO    

 /****建立用户组*****/
INSERT INTO [dbo].[tUserGroup]([GroupCode],[GroupName],[GroupType],[BuildUser],[EditUser])
select 'admin','管理员组',0,'脚本建立',''
GO

 /****建立用户*****/
INSERT INTO [dbo].[tUser]([UserCode],[UserName],[UPassword],[Token],[DeptCode],[GroupCode],[Enabled],[BuildUser],[EditUser])
select 'admin','管理员',0xC0E024D9200B5705BC4804722636378A,null,'root','admin',1,'脚本建立',''
GO

 /****建立用户菜单*****/
INSERT INTO [dbo].[tUserGroupFun]([GroupCode],[FunCode],[BuildUser],[EditUser],[Queriable])
select a.GroupCode,b.FunCode,'','',1 from tUserGroup a,tFunction b 

