
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/16/2018 13:44:36
-- Generated from EDMX file: c:\Users\Petr\source\repos\clientbox\clientbox\Models\ModelCBOX.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Data4995];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AG_tblRegisterRestrictions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AG_tblRegisterRestrictions];
GO
IF OBJECT_ID(N'[dbo].[AG_tblRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AG_tblRoles];
GO
IF OBJECT_ID(N'[dbo].[AG_tblSignalRUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AG_tblSignalRUsers];
GO
IF OBJECT_ID(N'[dbo].[AG_tblUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AG_tblUsers];
GO
IF OBJECT_ID(N'[dbo].[AG_tblUsersLastPosition]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AG_tblUsersLastPosition];
GO
IF OBJECT_ID(N'[Data4995ModelStoreContainer].[AGvw_FA_PracakySeznam]', 'U') IS NOT NULL
    DROP TABLE [Data4995ModelStoreContainer].[AGvw_FA_PracakySeznam];
GO
IF OBJECT_ID(N'[Data4995ModelStoreContainer].[AGvw_FirmyAdresyFakturacni]', 'U') IS NOT NULL
    DROP TABLE [Data4995ModelStoreContainer].[AGvw_FirmyAdresyFakturacni];
GO
IF OBJECT_ID(N'[Data4995ModelStoreContainer].[AGvw_FirmyDodavatele]', 'U') IS NOT NULL
    DROP TABLE [Data4995ModelStoreContainer].[AGvw_FirmyDodavatele];
GO
IF OBJECT_ID(N'[Data4995ModelStoreContainer].[AGvwrr_Dodavatel]', 'U') IS NOT NULL
    DROP TABLE [Data4995ModelStoreContainer].[AGvwrr_Dodavatel];
GO
IF OBJECT_ID(N'[Data4995ModelStoreContainer].[AGvwrr_StavPracaku]', 'U') IS NOT NULL
    DROP TABLE [Data4995ModelStoreContainer].[AGvwrr_StavPracaku];
GO
IF OBJECT_ID(N'[Data4995ModelStoreContainer].[AGvwrr_TypykontaktnichUdaju]', 'U') IS NOT NULL
    DROP TABLE [Data4995ModelStoreContainer].[AGvwrr_TypykontaktnichUdaju];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AG_tblRegisterRestrictions'
CREATE TABLE [dbo].[AG_tblRegisterRestrictions] (
    [Register] varchar(50)  NOT NULL,
    [IDOrder] smallint  NOT NULL,
    [Val] nvarchar(150)  NOT NULL,
    [Val2] nvarchar(100)  NULL
);
GO

-- Creating table 'AG_tblRoles'
CREATE TABLE [dbo].[AG_tblRoles] (
    [IDRole] int  NOT NULL,
    [RoleName] nvarchar(30)  NULL,
    [Authorize] varchar(255)  NULL
);
GO

-- Creating table 'AG_tblUsers'
CREATE TABLE [dbo].[AG_tblUsers] (
    [IDUser] int  NOT NULL,
    [IDRole] int  NOT NULL,
    [UserFirstName] nvarchar(30)  NULL,
    [UserLastName] nvarchar(30)  NULL,
    [UserLogin] nvarchar(80)  NOT NULL,
    [UserPWD] nvarchar(128)  NULL,
    [UserMobilePhone] varchar(16)  NULL,
    [UserAccountEnabled] bit  NOT NULL,
    [HodinovaSazba] decimal(19,4)  NULL,
    [VzdalenkaMalyZasah] decimal(19,4)  NULL,
    [VzdalenakVelkyZasah] decimal(19,4)  NULL,
    [ProcentoZHodinoveSazby] decimal(6,4)  NULL,
    [ProcentoZMaleVzdalenky] decimal(6,4)  NULL,
    [ProcentoZVelkeVzdalenky] decimal(6,4)  NULL,
    [SumaPosledniCHProvizi] decimal(10,4)  NOT NULL
);
GO

-- Creating table 'AG_tblSignalRUsers'
CREATE TABLE [dbo].[AG_tblSignalRUsers] (
    [IDU] int IDENTITY(1,1) NOT NULL,
    [ConnectionID] nvarchar(256)  NOT NULL,
    [Login] nvarchar(100)  NOT NULL,
    [Name] nvarchar(30)  NOT NULL,
    [Surname] nvarchar(30)  NOT NULL,
    [Lat] decimal(18,0)  NOT NULL,
    [Lng] decimal(18,0)  NOT NULL,
    [Color] nvarchar(7)  NOT NULL,
    [Time] datetime  NOT NULL,
    [OnOff] nvarchar(15)  NOT NULL
);
GO

-- Creating table 'AGvwrr_TypykontaktnichUdaju'
CREATE TABLE [dbo].[AGvwrr_TypykontaktnichUdaju] (
    [Hodnota] varchar(255)  NOT NULL
);
GO

-- Creating table 'AGvw_FA_PracakySeznam'
CREATE TABLE [dbo].[AGvw_FA_PracakySeznam] (
    [IDVykazPrace] int  NOT NULL,
    [DatVzniku] datetime  NOT NULL,
    [Firma] varchar(30)  NOT NULL,
    [rr_StavPracaku] tinyint  NOT NULL,
    [rr_StavPracakuHodnota] nvarchar(150)  NOT NULL,
    [rr_TypServisniSmlouvy] tinyint  NOT NULL,
    [rr_TypServisniSmlouvyHodnota] nvarchar(150)  NOT NULL,
    [CisloFaktury] varchar(30)  NULL,
    [IDUserZalozil] int  NULL,
    [UserZalozil] nvarchar(30)  NULL,
    [IDUserUpravil] int  NULL,
    [UserUpravil] nvarchar(30)  NULL,
    [rr_FakturovatNaFirmu] tinyint  NOT NULL,
    [rr_FakturovatNaFirmuHodnota] nvarchar(150)  NOT NULL,
    [HtmlZnacka] nvarchar(100)  NULL
);
GO

-- Creating table 'AGvw_FirmyDodavatele'
CREATE TABLE [dbo].[AGvw_FirmyDodavatele] (
    [Dodavatel] varchar(30)  NOT NULL,
    [Nazev_firmy] varchar(255)  NULL
);
GO

-- Creating table 'AGvw_FirmyAdresyFakturacni'
CREATE TABLE [dbo].[AGvw_FirmyAdresyFakturacni] (
    [Kontakt] varchar(30)  NOT NULL,
    [Adresa_rozpis] varchar(255)  NULL
);
GO

-- Creating table 'AGvwrr_Dodavatel'
CREATE TABLE [dbo].[AGvwrr_Dodavatel] (
    [rr_Dodavatel] smallint  NOT NULL,
    [rr_DodavatelHodnota] nvarchar(150)  NOT NULL
);
GO

-- Creating table 'AGvwrr_StavPracaku'
CREATE TABLE [dbo].[AGvwrr_StavPracaku] (
    [rr_StavPracaku] smallint  NOT NULL,
    [rr_StavPracakuHodnota] nvarchar(150)  NOT NULL,
    [HtmlZnacka] nvarchar(100)  NULL
);
GO

-- Creating table 'AG_tblUsersLastPosition'
CREATE TABLE [dbo].[AG_tblUsersLastPosition] (
    [IDG] bigint IDENTITY(1,1) NOT NULL,
    [IDUser] int  NOT NULL,
    [GPS] geography  NULL,
    [GPSTime] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Register], [IDOrder] in table 'AG_tblRegisterRestrictions'
ALTER TABLE [dbo].[AG_tblRegisterRestrictions]
ADD CONSTRAINT [PK_AG_tblRegisterRestrictions]
    PRIMARY KEY CLUSTERED ([Register], [IDOrder] ASC);
GO

-- Creating primary key on [IDRole] in table 'AG_tblRoles'
ALTER TABLE [dbo].[AG_tblRoles]
ADD CONSTRAINT [PK_AG_tblRoles]
    PRIMARY KEY CLUSTERED ([IDRole] ASC);
GO

-- Creating primary key on [IDUser] in table 'AG_tblUsers'
ALTER TABLE [dbo].[AG_tblUsers]
ADD CONSTRAINT [PK_AG_tblUsers]
    PRIMARY KEY CLUSTERED ([IDUser] ASC);
GO

-- Creating primary key on [IDU] in table 'AG_tblSignalRUsers'
ALTER TABLE [dbo].[AG_tblSignalRUsers]
ADD CONSTRAINT [PK_AG_tblSignalRUsers]
    PRIMARY KEY CLUSTERED ([IDU] ASC);
GO

-- Creating primary key on [Hodnota] in table 'AGvwrr_TypykontaktnichUdaju'
ALTER TABLE [dbo].[AGvwrr_TypykontaktnichUdaju]
ADD CONSTRAINT [PK_AGvwrr_TypykontaktnichUdaju]
    PRIMARY KEY CLUSTERED ([Hodnota] ASC);
GO

-- Creating primary key on [IDVykazPrace], [DatVzniku], [Firma], [rr_StavPracaku], [rr_StavPracakuHodnota], [rr_TypServisniSmlouvy], [rr_TypServisniSmlouvyHodnota], [rr_FakturovatNaFirmu], [rr_FakturovatNaFirmuHodnota] in table 'AGvw_FA_PracakySeznam'
ALTER TABLE [dbo].[AGvw_FA_PracakySeznam]
ADD CONSTRAINT [PK_AGvw_FA_PracakySeznam]
    PRIMARY KEY CLUSTERED ([IDVykazPrace], [DatVzniku], [Firma], [rr_StavPracaku], [rr_StavPracakuHodnota], [rr_TypServisniSmlouvy], [rr_TypServisniSmlouvyHodnota], [rr_FakturovatNaFirmu], [rr_FakturovatNaFirmuHodnota] ASC);
GO

-- Creating primary key on [Dodavatel] in table 'AGvw_FirmyDodavatele'
ALTER TABLE [dbo].[AGvw_FirmyDodavatele]
ADD CONSTRAINT [PK_AGvw_FirmyDodavatele]
    PRIMARY KEY CLUSTERED ([Dodavatel] ASC);
GO

-- Creating primary key on [Kontakt] in table 'AGvw_FirmyAdresyFakturacni'
ALTER TABLE [dbo].[AGvw_FirmyAdresyFakturacni]
ADD CONSTRAINT [PK_AGvw_FirmyAdresyFakturacni]
    PRIMARY KEY CLUSTERED ([Kontakt] ASC);
GO

-- Creating primary key on [rr_Dodavatel], [rr_DodavatelHodnota] in table 'AGvwrr_Dodavatel'
ALTER TABLE [dbo].[AGvwrr_Dodavatel]
ADD CONSTRAINT [PK_AGvwrr_Dodavatel]
    PRIMARY KEY CLUSTERED ([rr_Dodavatel], [rr_DodavatelHodnota] ASC);
GO

-- Creating primary key on [rr_StavPracaku], [rr_StavPracakuHodnota] in table 'AGvwrr_StavPracaku'
ALTER TABLE [dbo].[AGvwrr_StavPracaku]
ADD CONSTRAINT [PK_AGvwrr_StavPracaku]
    PRIMARY KEY CLUSTERED ([rr_StavPracaku], [rr_StavPracakuHodnota] ASC);
GO

-- Creating primary key on [IDG] in table 'AG_tblUsersLastPosition'
ALTER TABLE [dbo].[AG_tblUsersLastPosition]
ADD CONSTRAINT [PK_AG_tblUsersLastPosition]
    PRIMARY KEY CLUSTERED ([IDG] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IDRole] in table 'AG_tblUsers'
ALTER TABLE [dbo].[AG_tblUsers]
ADD CONSTRAINT [FK_AG_tblUsers_AG_tblRoles]
    FOREIGN KEY ([IDRole])
    REFERENCES [dbo].[AG_tblRoles]
        ([IDRole])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AG_tblUsers_AG_tblRoles'
CREATE INDEX [IX_FK_AG_tblUsers_AG_tblRoles]
ON [dbo].[AG_tblUsers]
    ([IDRole]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------