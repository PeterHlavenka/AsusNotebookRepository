USE [master]
GO
/****** Object:  Database [Vesmir]    Script Date: 10/12/2011 16:28:41 ******/
CREATE DATABASE [Vesmir] ON  PRIMARY 
( NAME = N'Vesmir', FILENAME = N'C:\Vesmir.mdf' , SIZE = 10240KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Vesmir_log', FILENAME = N'C:\Vesmir_log.ldf' , SIZE = 20096KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'Vesmir', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Vesmir].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [Vesmir] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Vesmir] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Vesmir] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Vesmir] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Vesmir] SET ARITHABORT OFF 
GO
ALTER DATABASE [Vesmir] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Vesmir] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Vesmir] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Vesmir] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Vesmir] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Vesmir] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Vesmir] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Vesmir] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Vesmir] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Vesmir] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Vesmir] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Vesmir] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Vesmir] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Vesmir] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Vesmir] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Vesmir] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Vesmir] SET  READ_WRITE 
GO
ALTER DATABASE [Vesmir] SET RECOVERY FULL 
GO
ALTER DATABASE [Vesmir] SET  MULTI_USER 
GO
ALTER DATABASE [Vesmir] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Vesmir] SET DB_CHAINING OFF 

USE Vesmir
GO

CREATE TABLE [dbo].[Galaxie](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Jmeno] [varchar](20) NULL,
	[PolohaX] [bigint] NULL,
	[PolohaY] [bigint] NULL,
	[PolohaZ] [bigint] NULL,
 CONSTRAINT [PK_Galaxie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Planeta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Jmeno] [varchar](10) NOT NULL,
	[Velikost] [int] NULL,
	[GalaxieId] [int] NOT NULL,
	[Identifikator] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Planeta] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Planeta]  WITH CHECK ADD  CONSTRAINT [FK_Planeta_Galaxie] FOREIGN KEY([GalaxieId])
REFERENCES [dbo].[Galaxie] ([Id])
GO
ALTER TABLE [dbo].[Planeta] CHECK CONSTRAINT [FK_Planeta_Galaxie]

CREATE TABLE [dbo].[Vlastnost](
	[Id] [int] NOT NULL,
	[Nazev] [varchar](50) NULL,
 CONSTRAINT [PK_Vlastnost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE dbo.Planeta ADD CONSTRAINT
	DF_Planeta_Velikost DEFAULT 6378 FOR Velikost
GO

CREATE TABLE [dbo].[VlastnostiPlanet](
	[PlanetaId] [int] NOT NULL,
	[VlastnostId] [int] NOT NULL,
 CONSTRAINT [PK_VlastnostiPlanet] PRIMARY KEY CLUSTERED 
(
	[PlanetaId] ASC,
	[VlastnostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[VlastnostiPlanet]  WITH CHECK ADD  CONSTRAINT [FK_VlastnostiPlanet_Planeta] FOREIGN KEY([PlanetaId])
REFERENCES [dbo].[Planeta] ([Id])
GO
ALTER TABLE [dbo].[VlastnostiPlanet] CHECK CONSTRAINT [FK_VlastnostiPlanet_Planeta]
GO
ALTER TABLE [dbo].[VlastnostiPlanet]  WITH CHECK ADD  CONSTRAINT [FK_VlastnostiPlanet_Vlastnost] FOREIGN KEY([VlastnostId])
REFERENCES [dbo].[Vlastnost] ([Id])
GO
ALTER TABLE [dbo].[VlastnostiPlanet] CHECK CONSTRAINT [FK_VlastnostiPlanet_Vlastnost]
GO

INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (0, 'Kulatá')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (1, 'Plynná')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (2, 'Obyvatelná')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (3, 'S vodou')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (4, 'Se zlatem')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (5, 'Neviditelná')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (6, 'Voòavá')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (7, 'Modrá')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (8, 'Studená')
INSERT INTO dbo.Vlastnost (Id, Nazev)
VALUES (9, 'Elektrizující')

INSERT INTO dbo.Galaxie (Jmeno, PolohaX, PolohaY, PolohaZ)
SELECT '',
	CONVERT(int, 1000000000*RAND(v1.Id*100+v2.Id*10+v3.Id)) % 1000,
	CONVERT(int, 100000000*RAND(v1.Id*100+v2.Id*10+v3.Id)) % 1000,
	CONVERT(int, 10000000*RAND(v1.Id*100+v2.Id*10+v3.Id)) % 1000
FROM dbo.Vlastnost v1
	CROSS JOIN dbo.Vlastnost v2
	CROSS JOIN dbo.Vlastnost v3

WHILE (SELECT TOP 1 LEN(Jmeno) FROM dbo.Galaxie) < 10
BEGIN
	UPDATE g
	SET Jmeno = Jmeno + CHAR(65 + CONVERT(int, 1000000000*RAND(LEN(Jmeno)*1000+v1.Id*100+v2.Id*10+v3.Id)) % 26)
	FROM dbo.Galaxie g
		CROSS JOIN dbo.Vlastnost v1
		CROSS JOIN dbo.Vlastnost v2
		CROSS JOIN dbo.Vlastnost v3
	WHERE g.Id - 1 = v1.Id*100+v2.Id*10+v3.Id
END

UPDATE dbo.Galaxie
SET Jmeno = LEFT(Jmeno, 3 + (CONVERT(int, 1000000000*RAND(PolohaX+PolohaY+PolohaZ)) % 8))

INSERT INTO dbo.Galaxie (Jmeno, PolohaX, PolohaY, PolohaZ)
VALUES  ('Mléèná dráha', 0, 0, 0)

INSERT INTO dbo.Planeta (Jmeno, Velikost, GalaxieId, Identifikator)
SELECT '',
	CONVERT(int, 1000000000*RAND(v1.Id*1000+v2.Id*100+v3.Id*10+v4.Id)) % 1000,
	CONVERT(int, 100000000*RAND(v1.Id*1000+v2.Id*100+v3.Id*10+v4.Id)) % 1001,
	NEWID()
FROM dbo.Vlastnost v1
	CROSS JOIN dbo.Vlastnost v2
	CROSS JOIN dbo.Vlastnost v3
	CROSS JOIN dbo.Vlastnost v4

WHILE (SELECT TOP 1 LEN(Jmeno) FROM dbo.Planeta) < 10
BEGIN
	UPDATE p
	SET Jmeno = Jmeno + CHAR(65 + CONVERT(int, 1000000000*RAND(LEN(Jmeno)*1000+v1.Id*1000+v2.Id*100+v3.Id*10+v4.Id)) % 26)
	FROM dbo.Planeta p
		CROSS JOIN dbo.Vlastnost v1
		CROSS JOIN dbo.Vlastnost v2
		CROSS JOIN dbo.Vlastnost v3
		CROSS JOIN dbo.Vlastnost v4
	WHERE p.Id - 1 = v1.Id*1000+v2.Id*100+v3.Id*10+v4.Id
END

UPDATE dbo.Planeta
SET Jmeno = LEFT(Jmeno, 3 + (CONVERT(int, 1000000000*RAND(Velikost)) % 8))

INSERT INTO dbo.VlastnostiPlanet (PlanetaId, VlastnostId)
SELECT p.Id, v.Id
FROM dbo.Vlastnost v
	CROSS JOIN dbo.Planeta p
WHERE ((p.id+17) % (v.Id+7)) % 10 = 1
