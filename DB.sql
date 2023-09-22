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
	[Id] [nvarchar](450) PRIMARY KEY NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[Password] [nvarchar](256) NULL,
	[FullName] [nvarchar] (256) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] default 0,
	[DateOfBirth] datetime null,
	[Address] nvarchar(256) null,
	[PhoneNumber] [nvarchar](max) NULL,
	[Status] varchar(10) check ([Status] in ('Pending','Enabled','Disabled')) default 'Pending',
);
CREATE TABLE UserRole (
  [Id] int PRIMARY KEY identity(1,1),
  [Role] varchar(50) null  check (  [Role] in ('admin','normal','vip')) default 'normal',
  [UserID] [nvarchar](450) foreign key references [User](Id),
);
insert into [User] (id,username,password,fullname,email,EmailConfirmed,Status)
values ('U01','user01','123','Lê Văn A','levanA@gmail.com',1,'Enabled'),
		('U02','admin','123','My Boss','admin@gmail.com',1,'Enabled')
go
insert into UserRole (Role,UserID) values
('normal','U01'),('admin','U02')
go

