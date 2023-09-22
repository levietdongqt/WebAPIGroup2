USE [master]
GO
if exists(SELECT * FROM master..sysdatabases WHERE name='DBSem3G2')
DROP DATABASE DBSem3G2
GO
create  database DBSem3G2
go
use DBSem3G2
go


CREATE TABLE [User] (
	[Id] int identity(1,1) PRIMARY KEY NOT NULL,
	[UserName] [nvarchar](256) NULL unique,
	[Password] [nvarchar](256) NULL,
	[FullName] [nvarchar] (256) NULL,
	[Email] [nvarchar](256) NULL unique,
	[EmailConfirmed] [bit] default 0,
	[DateOfBirth] datetime null,
	[Address] nvarchar(256) null,
	[PhoneNumber] [nvarchar](max) NULL,
	[Role] varchar(50) check (  [Role] in ('admin','user','vip')) default('user'),
	[Status] varchar(10) check ([Status] in ('Pending','Enabled','Disabled')) default('Pending'),
);

insert into [User] (username,password,fullname,email,EmailConfirmed,Status,Role)
values ('user01','123','Lê Văn A','levanA@gmail.com',1,'Enabled','user'),
		('admin','123','My Boss','admin@gmail.com',1,'Enabled','admin')
select * from [user]