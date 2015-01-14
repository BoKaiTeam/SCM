if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tMessageRecv') and o.name = 'FK_TMESSAGE_REFERENCE_TMESSAGE')
alter table tMessageRecv
   drop constraint FK_TMESSAGE_REFERENCE_TMESSAGE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUser') and o.name = 'FK_TUSER_REFERENCE_TDEPT')
alter table tUser
   drop constraint FK_TUSER_REFERENCE_TDEPT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUser') and o.name = 'FK_TUSER_REFERENCE_TUSERGRO')
alter table tUser
   drop constraint FK_TUSER_REFERENCE_TUSERGRO
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUserGroupFun') and o.name = 'FK_TUSERGRO_REFERENCE_TFUNCTIO')
alter table tUserGroupFun
   drop constraint FK_TUSERGRO_REFERENCE_TFUNCTIO
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tUserGroupFun') and o.name = 'FK_TUSERGRO_REFERENCE_TUSERGRO')
alter table tUserGroupFun
   drop constraint FK_TUSERGRO_REFERENCE_TUSERGRO
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tDept')
            and   type = 'U')
   drop table tDept
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tFunction')
            and   type = 'U')
   drop table tFunction
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tMessage')
            and   type = 'U')
   drop table tMessage
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tMessageRecv')
            and   type = 'U')
   drop table tMessageRecv
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tNotice')
            and   type = 'U')
   drop table tNotice
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tUser')
            and   type = 'U')
   drop table tUser
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tUserGroup')
            and   type = 'U')
   drop table tUserGroup
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tUserGroupFun')
            and   type = 'U')
   drop table tUserGroupFun
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tXtLog')
            and   type = 'U')
   drop table tXtLog
go

execute sp_revokedbaccess dbo
go

/*==============================================================*/
/* User: dbo                                                    */
/*==============================================================*/
execute sp_grantdbaccess dbo
go

/*==============================================================*/
/* Table: tDept                                                 */
/*==============================================================*/
create table tDept (
   Id                   numeric              identity,
   DeptCode             varchar(20)          not null,
   DeptName             nvarchar(20)         not null,
   ParentCode           varchar(20)          null,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TDEPT primary key (Id),
   constraint AK_DEPTCODE_TDEPT unique (DeptCode)
)
go

/*==============================================================*/
/* Table: tFunction                                             */
/*==============================================================*/
create table tFunction (
   Id                   numeric              identity,
   FunCode              varchar(20)          not null,
   FunName              nvarchar(50)         not null,
   Enabled              bit                  not null default 0,
   FunCmd               varchar(100)         null,
   SerialNo             numeric              not null,
   ParentCode           varchar(20)          null,
   FunType              numeric(1)           not null,
   GroupType            numeric(1)           not null default 1,
   constraint PK_TFUNCTION primary key (Id),
   constraint AK_SERIALNO_PARENTCOD_TFUNCTION unique (SerialNo, ParentCode),
   constraint AK_FUNCODE_TFUNCTION unique (FunCode)
)
go

/*==============================================================*/
/* Table: tMessage                                              */
/*==============================================================*/
create table tMessage (
   Id                   numeric              identity,
   MsgCode              varchar(15)          not null,
   MsgTitle             nvarchar(100)        not null,
   Content              nvarchar(1000)       not null,
   Status               numeric(1)           not null default 0,
   CreateUser           nvarchar(50)         not null,
   CreateDate           datetime             not null default getdate(),
   EditDate             datetime             not null default getdate(),
   SendDate             datetime             null,
   constraint PK_TMESSAGE primary key (Id),
   constraint AK_MSGCODE_TMESSAGE unique (MsgCode)
)
go

/*==============================================================*/
/* Table: tMessageRecv                                          */
/*==============================================================*/
create table tMessageRecv (
   Id                   numeric              identity,
   MsgCode              varchar(15)          not null,
   RecvUserCode         varchar(20)          not null,
   Status               numeric(1)           not null default 0,
   constraint PK_TMESSAGERECV primary key (Id),
   constraint AK_MSGCODE_RECVUSERCO_TMESSAGE unique (MsgCode, RecvUserCode)
)
go

/*==============================================================*/
/* Table: tNotice                                               */
/*==============================================================*/
create table tNotice (
   Id                   numeric              identity,
   Title                nvarchar(100)        not null,
   Content              nvarchar(1000)       not null,
   Enable               bit                  not null default 1,
   CreateUser           nvarchar(50)         not null,
   CreateDate           datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   SendDate             datetime             null,
   constraint PK_TNOTICE primary key (Id)
)
go

/*==============================================================*/
/* Table: tUser                                                 */
/*==============================================================*/
create table tUser (
   Id                   numeric              identity,
   UserCode             varchar(20)          not null,
   UserName             nvarchar(20)         not null,
   UPassword            binary(16)           not null,
   DeptCode             varchar(20)          null,
   GroupCode            varchar(20)          null,
   Enabled              bit                  not null default 1,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   Token                binary(16)           null,
   constraint PK_TUSER primary key (Id),
   constraint AK_USERCODE_TUSER unique (UserCode)
)
go

/*==============================================================*/
/* Table: tUserGroup                                            */
/*==============================================================*/
create table tUserGroup (
   Id                   numeric              identity,
   GroupCode            varchar(20)          not null,
   GroupName            nvarchar(20)         not null,
   GroupType            numeric(2)           not null,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TUSERGROUP primary key (Id),
   constraint AK_GROUPCODE_TUSERGROUP unique (GroupCode)
)
go

/*==============================================================*/
/* Table: tUserGroupFun                                         */
/*==============================================================*/
create table tUserGroupFun (
   Id                   numeric              identity,
   GroupCode            varchar(20)          not null,
   FunCode              varchar(20)          not null,
   Queriable            bit                  not null default 0,
   Creatable            bit                  not null default 0,
   Changable            bit                  not null default 0,
   Deletable            bit                  not null default 0,
   Checkable            bit                  not null default 0,
   BuildDate            datetime             not null default getdate(),
   BuildUser            nvarchar(50)         not null,
   EditDate             datetime             not null default getdate(),
   EditUser             nvarchar(50)         not null,
   constraint PK_TUSERGROUPFUN primary key (Id),
   constraint AK_GROUPCODE_FUNCODE_TUSERGROUP unique (GroupCode, FunCode)
)
go

/*==============================================================*/
/* Table: tXtLog                                                */
/*==============================================================*/
create table tXtLog (
   Id                   numeric              identity,
   LogDate              datetime             not null default getdate(),
   LogContent           nvarchar(1000)       not null,
   LogType              numeric(2)           not null,
   LogUser              nvarchar(50)         not null,
   constraint PK_TXTLOG primary key (Id)
)
go

alter table tMessageRecv
   add constraint FK_TMESSAGE_REFERENCE_TMESSAGE foreign key (MsgCode)
      references tMessage (MsgCode)
go

alter table tUser
   add constraint FK_TUSER_REFERENCE_TDEPT foreign key (DeptCode)
      references tDept (DeptCode)
go

alter table tUser
   add constraint FK_TUSER_REFERENCE_TUSERGRO foreign key (GroupCode)
      references tUserGroup (GroupCode)
go

alter table tUserGroupFun
   add constraint FK_TUSERGRO_REFERENCE_TFUNCTIO foreign key (FunCode)
      references tFunction (FunCode)
go

alter table tUserGroupFun
   add constraint FK_TUSERGRO_REFERENCE_TUSERGRO foreign key (GroupCode)
      references tUserGroup (GroupCode)
go
