USE [CRMDB]
GO
/****�����˵�*****/
INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType],[GroupType])
select '00','ϵͳ����',1 ,'',0,null,0,0
union all 
select '01','��Ӧ�̹���',1 ,'',1,null,0,1
union all 
select '02','���ݹ���',1 ,'',2,null,0,1
union all 
select '03','��Ϣ��ѯ',1 ,'',3,null,0,1
union all 
select '04','����Ƶ��',1 ,'',4,null,0,1
go

INSERT INTO [tFunction]([FunCode],[FunName],[Enabled],[FunCmd],[SerialNo],[ParentCode],[FunType],[GroupType])
select '0001','��������',1 ,'/Config/DeptManage',0,'00',1,0
union all 
select '0002','�û���',1 ,'/Config/UserGroupManage',1,'00',1,0
union all 
select '0003','�û�����',1 ,'/Config/UserManage',2,'00',1,0
union all 
select '0004','ϵͳ��־',0 ,'',3,'00',1,0
go


/****��������*****/

INSERT INTO [tDept]([DeptCode],[DeptName],[ParentCode],[BuildUser],[EditUser])
 select 'root' ,'��˾',null,'',''
 GO    

 /****�����û���*****/
INSERT INTO [dbo].[tUserGroup]([GroupCode],[GroupName],[GroupType],[BuildUser],[EditUser])
select 'admin','����Ա��',0,'�ű�����',''
GO

 /****�����û�*****/
INSERT INTO [dbo].[tUser]([UserCode],[UserName],[UPassword],[Token],[DeptCode],[GroupCode],[Enabled],[BuildUser],[EditUser])
select 'admin','����Ա',0xC0E024D9200B5705BC4804722636378A,null,'root','admin',1,'�ű�����',''
GO

 /****�����û��˵�*****/
INSERT INTO [dbo].[tUserGroupFun]([GroupCode],[FunCode],[BuildUser],[EditUser],[Queriable])
select a.GroupCode,b.FunCode,'','',1 from tUserGroup a,tFunction b 

