IdUsuario	NombreCompleto	Correo	Clave	IdRol
1	Administrador Principal         //admin@medicita.com	        //    123456	// 1
2	Dr. Carlos Rodríguez Pérez      //arlos.rodriguez@medicita.com  //    medico123	 //2
3	Dra. María Elena Torres         //maria.torres@medicita.com	    //    medico123	//2
4	Dr. José Luis Martínez	        //jose.martinez@medicita.com	//    medico123	//2
5	Dra. Ana Patricia Fernández	    //ana.fernandez@medicita.com	//    medico123	//2
6	Dr. Roberto Carlos Sánchez	    //roberto.sanchez@medicita.com	//    medico123	//2
7	Dra. Laura Isabel González	    //laura.gonzalez@medicita.com	//    medico123	//2
8	Dr. Miguel Ángel Ramírez	    //miguel.ramirez@medicita.com	//    medico123	//2
9	Dr. Fernando Javier López	    //fernando.lopez@medicita.com	//    medico123	//2
10	Dra. Carmen Rosa Díaz	        //carmen.diaz@medicita.com	    //    medico123	//2
11	Dr. Ricardo Alberto Herrera	    //ricardo.herrera@medicita.com	//    medico123	//2
12	Juan Pérez García	            //paciente1@test.com	        //    pass123	//3
13	María López Santos	            //paciente2@test.com	        //    pass123	//3
14	Cliente Prueba	                //cliente.prueba@test.com	    //    password123	//3
15	Cliente Prueba Test	            //cliente.test@ejemplo.com	    //    password123	//3
16	Ciro TEST	                    //ciroTEST@test.com	            //    123456        //3
17	Ciro TEST	                    //test@test.com	                //    123456	//3
18	Ciro1 TEST	                    //test1@test.com	            //    123456	//3
20	Cliente1	                    //cliente@test.com	            //    123456	//3
21	Cliente2	                    //cliente1@test.com	            //    123456	//3




USE [master]
GO
/****** Object:  Database [BD_MediCita]    Script Date: 21/12/2025 23:00:02 ******/
CREATE DATABASE [BD_MediCita]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BD_MediCita', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BD_MediCita.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BD_MediCita_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BD_MediCita_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BD_MediCita] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BD_MediCita].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BD_MediCita] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BD_MediCita] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BD_MediCita] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BD_MediCita] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BD_MediCita] SET ARITHABORT OFF 
GO
ALTER DATABASE [BD_MediCita] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BD_MediCita] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BD_MediCita] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BD_MediCita] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BD_MediCita] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BD_MediCita] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BD_MediCita] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BD_MediCita] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BD_MediCita] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BD_MediCita] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BD_MediCita] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BD_MediCita] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BD_MediCita] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BD_MediCita] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BD_MediCita] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BD_MediCita] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BD_MediCita] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BD_MediCita] SET RECOVERY FULL 
GO
ALTER DATABASE [BD_MediCita] SET  MULTI_USER 
GO
ALTER DATABASE [BD_MediCita] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BD_MediCita] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BD_MediCita] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BD_MediCita] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BD_MediCita] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BD_MediCita] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BD_MediCita', N'ON'
GO
ALTER DATABASE [BD_MediCita] SET QUERY_STORE = ON
GO
ALTER DATABASE [BD_MediCita] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BD_MediCita]
GO
/****** Object:  Table [dbo].[tb_Citas]    Script Date: 21/12/2025 23:00:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Citas](
	[IdCita] [int] IDENTITY(1,1) NOT NULL,
	[IdPaciente] [int] NULL,
	[IdMedico] [int] NULL,
	[FechaCita] [datetime] NOT NULL,
	[Estado] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_DetalleVenta]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_DetalleVenta](
	[IdDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdVenta] [int] NULL,
	[IdMedicamento] [int] NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
	[SubTotal] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Especialidades]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Especialidades](
	[IdEspecialidad] [int] IDENTITY(1,1) NOT NULL,
	[NombreEspec] [varchar](50) NOT NULL,
	[Descripcion] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEspecialidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Medicamentos]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Medicamentos](
	[IdMedicamento] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Laboratorio] [varchar](50) NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[Stock] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdMedicamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Medicos]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Medicos](
	[IdMedico] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[IdEspecialidad] [int] NULL,
	[CMP] [varchar](20) NOT NULL,
	[Telefono] [varchar](15) NULL,
	[Correo] [varchar](100) NULL,
	[NombreCompleto] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdMedico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Roles]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Usuarios]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[NombreCompleto] [varchar](100) NOT NULL,
	[Correo] [varchar](100) NOT NULL,
	[Clave] [varchar](100) NOT NULL,
	[IdRol] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Ventas]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Ventas](
	[IdVenta] [int] IDENTITY(1,1) NOT NULL,
	[IdPaciente] [int] NULL,
	[FechaVenta] [datetime] NULL,
	[Total] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tb_Citas] ON 

INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (1, 1, 1, CAST(N'2025-12-04T00:18:52.900' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (2, 1, 1, CAST(N'2025-12-03T00:19:08.463' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (3, 1, 1, CAST(N'2025-12-03T00:21:07.140' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (4, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (5, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (6, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (7, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (8, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (9, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (10, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (11, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (12, 21, 8, CAST(N'2025-12-22T17:29:20.150' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (13, 21, 9, CAST(N'2025-12-22T17:29:29.237' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (14, 21, 9, CAST(N'2025-12-22T17:29:29.237' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (15, 21, 9, CAST(N'2025-12-22T17:29:29.237' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (16, 21, 1, CAST(N'2025-12-22T17:29:33.897' AS DateTime), N'P')
INSERT [dbo].[tb_Citas] ([IdCita], [IdPaciente], [IdMedico], [FechaCita], [Estado]) VALUES (17, 21, 7, CAST(N'2025-12-22T17:29:36.977' AS DateTime), N'P')
SET IDENTITY_INSERT [dbo].[tb_Citas] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_DetalleVenta] ON 

INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (1, 1, 1, 1, CAST(1.50 AS Decimal(10, 2)), CAST(1.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (2, 1, 2, 1, CAST(2.80 AS Decimal(10, 2)), CAST(2.80 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (3, 1, 3, 1, CAST(3.50 AS Decimal(10, 2)), CAST(3.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (4, 2, 1, 1, CAST(1.50 AS Decimal(10, 2)), CAST(1.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (5, 2, 2, 1, CAST(2.80 AS Decimal(10, 2)), CAST(2.80 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (6, 2, 3, 1, CAST(3.50 AS Decimal(10, 2)), CAST(3.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (7, 3, 6, 6, CAST(8.50 AS Decimal(10, 2)), CAST(51.00 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (8, 3, 3, 1, CAST(3.50 AS Decimal(10, 2)), CAST(3.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (9, 4, 3, 2, CAST(3.50 AS Decimal(10, 2)), CAST(7.00 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (10, 5, 1, 3, CAST(1.50 AS Decimal(10, 2)), CAST(4.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (11, 6, 1, 3, CAST(1.50 AS Decimal(10, 2)), CAST(4.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (12, 7, 6, 2, CAST(8.50 AS Decimal(10, 2)), CAST(17.00 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (13, 8, 2, 2, CAST(2.80 AS Decimal(10, 2)), CAST(5.60 AS Decimal(10, 2)))
INSERT [dbo].[tb_DetalleVenta] ([IdDetalle], [IdVenta], [IdMedicamento], [Cantidad], [PrecioUnitario], [SubTotal]) VALUES (14, 9, 2, 2, CAST(2.80 AS Decimal(10, 2)), CAST(5.60 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[tb_DetalleVenta] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_Especialidades] ON 

INSERT [dbo].[tb_Especialidades] ([IdEspecialidad], [NombreEspec], [Descripcion]) VALUES (1, N'Medicina General', N'Atención primaria')
INSERT [dbo].[tb_Especialidades] ([IdEspecialidad], [NombreEspec], [Descripcion]) VALUES (2, N'Pediatría', N'Niños')
INSERT [dbo].[tb_Especialidades] ([IdEspecialidad], [NombreEspec], [Descripcion]) VALUES (3, N'Cardiología', N'Corazón')
SET IDENTITY_INSERT [dbo].[tb_Especialidades] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_Medicamentos] ON 

INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (1, N'Paracetamol 500mg', N'Genfar', CAST(1.50 AS Decimal(10, 2)), 92)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (2, N'Ibuprofeno 400mg', N'Bayer', CAST(2.80 AS Decimal(10, 2)), 44)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (3, N'Amoxicilina 500mg', N'Genfar', CAST(3.50 AS Decimal(10, 2)), 75)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (6, N'Paracetamol 500mg', N'Bayer', CAST(8.50 AS Decimal(10, 2)), 492)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (7, N'Ibuprofeno 400mg', N'Pfizer', CAST(12.00 AS Decimal(10, 2)), 450)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (8, N'Naproxeno 250mg', N'Genfar', CAST(15.50 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (9, N'Diclofenaco 50mg', N'Novartis', CAST(18.00 AS Decimal(10, 2)), 250)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (10, N'Ketorolaco 10mg', N'Roemmers', CAST(22.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (11, N'Amoxicilina 500mg', N'Bayer', CAST(25.00 AS Decimal(10, 2)), 400)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (12, N'Azitromicina 500mg', N'Pfizer', CAST(35.00 AS Decimal(10, 2)), 350)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (13, N'Ciprofloxacino 500mg', N'Farmex', CAST(28.00 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (14, N'Clindamicina 300mg', N'Abbott', CAST(45.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (15, N'Cefalexina 500mg', N'Genfar', CAST(32.00 AS Decimal(10, 2)), 280)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (16, N'Loratadina 10mg', N'Bayer', CAST(18.00 AS Decimal(10, 2)), 400)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (17, N'Cetirizina 10mg', N'Pfizer', CAST(20.00 AS Decimal(10, 2)), 380)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (18, N'Difenhidramina 25mg', N'Farmex', CAST(15.00 AS Decimal(10, 2)), 320)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (19, N'Losartán 50mg', N'Novartis', CAST(38.00 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (20, N'Enalapril 10mg', N'Roemmers', CAST(35.00 AS Decimal(10, 2)), 280)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (21, N'Amlodipino 5mg', N'Pfizer', CAST(42.00 AS Decimal(10, 2)), 250)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (22, N'Metformina 850mg', N'Genfar', CAST(28.00 AS Decimal(10, 2)), 350)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (23, N'Glibenclamida 5mg', N'Bayer', CAST(32.00 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (24, N'Complejo B', N'Abbott', CAST(25.00 AS Decimal(10, 2)), 400)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (25, N'Vitamina C 1000mg', N'Bayer', CAST(22.00 AS Decimal(10, 2)), 450)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (26, N'Calcio + Vitamina D', N'Pfizer', CAST(35.00 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (27, N'Multivitamínico', N'Genfar', CAST(30.00 AS Decimal(10, 2)), 350)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (28, N'Omeprazol 20mg', N'Novartis', CAST(28.00 AS Decimal(10, 2)), 400)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (29, N'Ranitidina 150mg', N'Farmex', CAST(20.00 AS Decimal(10, 2)), 350)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (30, N'Hidróxido de Aluminio', N'Genfar', CAST(12.00 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (31, N'Dextrometorfano Jarabe', N'Abbott', CAST(18.00 AS Decimal(10, 2)), 250)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (32, N'Ambroxol 30mg', N'Roemmers', CAST(15.00 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (33, N'Acetilcisteína 600mg', N'Pfizer', CAST(25.00 AS Decimal(10, 2)), 280)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (34, N'Montelukast 10mg', N'Bayer', CAST(48.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (35, N'Prednisona 5mg', N'Genfar', CAST(22.00 AS Decimal(10, 2)), 280)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (36, N'Hioscina 10mg', N'Novartis', CAST(16.00 AS Decimal(10, 2)), 320)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (37, N'Trimebutina 200mg', N'Farmex', CAST(28.00 AS Decimal(10, 2)), 250)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (38, N'Clotrimazol Crema', N'Bayer', CAST(22.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (39, N'Hidrocortisona Crema 1%', N'Abbott', CAST(25.00 AS Decimal(10, 2)), 180)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (40, N'Betametasona Crema', N'Pfizer', CAST(30.00 AS Decimal(10, 2)), 150)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (41, N'Aspirina 100mg', N'Bayer', CAST(12.00 AS Decimal(10, 2)), 500)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (42, N'Warfarina 5mg', N'Pfizer', CAST(35.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (43, N'Alprazolam 0.5mg', N'Roemmers', CAST(45.00 AS Decimal(10, 2)), 150)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (44, N'Clonazepam 2mg', N'Novartis', CAST(42.00 AS Decimal(10, 2)), 180)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (45, N'Fluoxetina 20mg', N'Pfizer', CAST(38.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (46, N'Sertralina 50mg', N'Genfar', CAST(40.00 AS Decimal(10, 2)), 180)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (47, N'Lágrimas Artificiales', N'Abbott', CAST(18.00 AS Decimal(10, 2)), 250)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (48, N'Colirio Antibiótico', N'Bayer', CAST(35.00 AS Decimal(10, 2)), 150)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (49, N'Salbutamol Inhalador', N'Pfizer', CAST(55.00 AS Decimal(10, 2)), 120)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (50, N'Budesonida Inhalador', N'Novartis', CAST(85.00 AS Decimal(10, 2)), 100)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (51, N'Albendazol 400mg', N'Genfar', CAST(15.00 AS Decimal(10, 2)), 300)
INSERT [dbo].[tb_Medicamentos] ([IdMedicamento], [Nombre], [Laboratorio], [Precio], [Stock]) VALUES (52, N'Mebendazol 100mg', N'Farmex', CAST(12.00 AS Decimal(10, 2)), 280)
SET IDENTITY_INSERT [dbo].[tb_Medicamentos] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_Medicos] ON 

INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (1, NULL, 1, N'CMP-998877', N'999888777', N'médico.1@medicita.com', N'Médico 1')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (6, 2, 3, N'CMP45678', N'987654321', N'carlos.rodriguez@medicita.com', N'Dr. Carlos Rodríguez Pérez')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (7, 3, 3, N'CMP45679', N'987654322', N'maria.torres@medicita.com', N'Dra. María Elena Torres')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (8, 4, 2, N'CMP45680', N'987654323', N'jose.martinez@medicita.com', N'Dr. José Luis Martínez')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (9, 5, 2, N'CMP45681', N'987654324', N'ana.fernandez@medicita.com', N'Dra. Ana Patricia Fernández')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (10, 6, 2, N'CMP45682', N'987654325', N'roberto.sanchez@medicita.com', N'Dr. Roberto Carlos Sánchez')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (11, 7, 2, N'CMP45683', N'987654326', N'laura.gonzalez@medicita.com', N'Dra. Laura Isabel González')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (12, 8, 2, N'CMP45684', N'987654327', N'miguel.ramirez@medicita.com', N'Dr. Miguel Ángel Ramírez')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (13, 9, 2, N'CMP45685', N'987654328', N'fernando.lopez@medicita.com', N'Dr. Fernando Javier López')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (14, 10, 2, N'CMP45686', N'987654329', N'carmen.diaz@medicita.com', N'Dra. Carmen Rosa Díaz')
INSERT [dbo].[tb_Medicos] ([IdMedico], [IdUsuario], [IdEspecialidad], [CMP], [Telefono], [Correo], [NombreCompleto]) VALUES (15, 11, 2, N'CMP45687', N'987654330', N'ricardo.herrera@medicita.com', N'Dr. Ricardo Alberto Herrera')
SET IDENTITY_INSERT [dbo].[tb_Medicos] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_Roles] ON 

INSERT [dbo].[tb_Roles] ([IdRol], [NombreRol]) VALUES (1, N'Administrador')
INSERT [dbo].[tb_Roles] ([IdRol], [NombreRol]) VALUES (2, N'Medico')
INSERT [dbo].[tb_Roles] ([IdRol], [NombreRol]) VALUES (3, N'Paciente')
SET IDENTITY_INSERT [dbo].[tb_Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_Usuarios] ON 

INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (1, N'Administrador Principal', N'admin@medicita.com', N'123456', 1)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (2, N'Dr. Carlos Rodríguez Pérez', N'carlos.rodriguez@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (3, N'Dra. María Elena Torres', N'maria.torres@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (4, N'Dr. José Luis Martínez', N'jose.martinez@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (5, N'Dra. Ana Patricia Fernández', N'ana.fernandez@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (6, N'Dr. Roberto Carlos Sánchez', N'roberto.sanchez@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (7, N'Dra. Laura Isabel González', N'laura.gonzalez@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (8, N'Dr. Miguel Ángel Ramírez', N'miguel.ramirez@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (9, N'Dr. Fernando Javier López', N'fernando.lopez@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (10, N'Dra. Carmen Rosa Díaz', N'carmen.diaz@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (11, N'Dr. Ricardo Alberto Herrera', N'ricardo.herrera@medicita.com', N'medico123', 2)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (12, N'Juan Pérez García', N'paciente1@test.com', N'pass123', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (13, N'María López Santos', N'paciente2@test.com', N'pass123', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (14, N'Cliente Prueba', N'cliente.prueba@test.com', N'password123', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (15, N'Cliente Prueba Test', N'cliente.test@ejemplo.com', N'password123', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (16, N'Ciro TEST', N'ciroTEST@test.com', N'123456', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (17, N'Ciro TEST', N'test@test.com', N'123456', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (18, N'Ciro1 TEST', N'test1@test.com', N'123456', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (20, N'Cliente1', N'cliente@test.com', N'123456', 3)
INSERT [dbo].[tb_Usuarios] ([IdUsuario], [NombreCompleto], [Correo], [Clave], [IdRol]) VALUES (21, N'Cliente2', N'cliente1@test.com', N'123456', 3)
SET IDENTITY_INSERT [dbo].[tb_Usuarios] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_Ventas] ON 

INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (1, 1, CAST(N'2025-12-03T00:05:29.930' AS DateTime), CAST(7.80 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (2, 1, CAST(N'2025-12-21T15:15:24.083' AS DateTime), CAST(7.80 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (3, 18, CAST(N'2025-12-21T15:42:17.767' AS DateTime), CAST(54.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (4, 18, CAST(N'2025-12-21T16:13:26.497' AS DateTime), CAST(7.00 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (5, 18, CAST(N'2025-12-21T16:27:23.147' AS DateTime), CAST(4.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (6, 21, CAST(N'2025-12-21T17:20:23.867' AS DateTime), CAST(4.50 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (7, 21, CAST(N'2025-12-21T17:31:00.300' AS DateTime), CAST(17.00 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (8, 21, CAST(N'2025-12-21T18:56:33.413' AS DateTime), CAST(5.60 AS Decimal(10, 2)))
INSERT [dbo].[tb_Ventas] ([IdVenta], [IdPaciente], [FechaVenta], [Total]) VALUES (9, 20, CAST(N'2025-12-21T19:16:48.097' AS DateTime), CAST(5.60 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[tb_Ventas] OFF
GO
/****** Object:  Index [UQ__tb_Medic__5B65BF96BBA42231]    Script Date: 21/12/2025 23:00:03 ******/
ALTER TABLE [dbo].[tb_Medicos] ADD UNIQUE NONCLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tb_Usuar__60695A199AAAE1F8]    Script Date: 21/12/2025 23:00:03 ******/
ALTER TABLE [dbo].[tb_Usuarios] ADD UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tb_Citas] ADD  DEFAULT ('P') FOR [Estado]
GO
ALTER TABLE [dbo].[tb_Ventas] ADD  DEFAULT (getdate()) FOR [FechaVenta]
GO
ALTER TABLE [dbo].[tb_Citas]  WITH CHECK ADD FOREIGN KEY([IdMedico])
REFERENCES [dbo].[tb_Medicos] ([IdMedico])
GO
ALTER TABLE [dbo].[tb_Citas]  WITH CHECK ADD FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[tb_Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[tb_DetalleVenta]  WITH CHECK ADD FOREIGN KEY([IdMedicamento])
REFERENCES [dbo].[tb_Medicamentos] ([IdMedicamento])
GO
ALTER TABLE [dbo].[tb_DetalleVenta]  WITH CHECK ADD FOREIGN KEY([IdVenta])
REFERENCES [dbo].[tb_Ventas] ([IdVenta])
GO
ALTER TABLE [dbo].[tb_Medicos]  WITH CHECK ADD FOREIGN KEY([IdEspecialidad])
REFERENCES [dbo].[tb_Especialidades] ([IdEspecialidad])
GO
ALTER TABLE [dbo].[tb_Medicos]  WITH CHECK ADD FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[tb_Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[tb_Usuarios]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[tb_Roles] ([IdRol])
GO
ALTER TABLE [dbo].[tb_Ventas]  WITH CHECK ADD FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[tb_Usuarios] ([IdUsuario])
GO
/****** Object:  StoredProcedure [dbo].[usp_ActualizarEstadoCita]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ActualizarEstadoCita]
    @IdCita INT,
    @NuevoEstado VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.tb_Citas 
    SET Estado = @NuevoEstado
    WHERE IdCita = @IdCita;
    
    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ContarCitasDelDia]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- 1. CONTAR CITAS DEL DÍA
-- ================================================
CREATE PROCEDURE [dbo].[usp_ContarCitasDelDia]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) 
    FROM tb_Citas
    WHERE CAST(FechaCita AS DATE) = CAST(GETDATE() AS DATE);
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_CrearUsuario]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 1. CREAR USUARIO (MEJORADO)
-- ================================================
CREATE PROCEDURE [dbo].[usp_CrearUsuario]
    @NombreCompleto VARCHAR(100),
    @Correo VARCHAR(100),
    @Clave VARCHAR(100),
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM tb_Usuarios WHERE Correo = @Correo)
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END

    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES (@NombreCompleto, @Correo, @Clave, @IdRol);

    SELECT 1 AS Resultado;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_EditarMedicamento]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 4. Editar
CREATE PROCEDURE [dbo].[usp_EditarMedicamento]
    @IdMedicamento INT,
    @Nombre VARCHAR(100),
    @Laboratorio VARCHAR(50),
    @Precio DECIMAL(10,2),
    @Stock INT
AS
BEGIN
    UPDATE tb_Medicamentos
    SET Nombre = @Nombre,
        Laboratorio = @Laboratorio,
        Precio = @Precio,
        Stock = @Stock
    WHERE IdMedicamento = @IdMedicamento
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_EditarMedico]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_EditarMedico]
    @IdMedico INT,
    @NombreCompleto VARCHAR(100),
    @IdEspecialidad INT,
    @CMP VARCHAR(20),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE tb_Medicos
    SET 
        NombreCompleto = @NombreCompleto,
        IdEspecialidad = @IdEspecialidad,
        CMP = @CMP,
        Correo = @Correo,
        Telefono = @Telefono
    WHERE IdMedico = @IdMedico
END
GO
/****** Object:  StoredProcedure [dbo].[usp_EditarUsuario]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 2. EDITAR USUARIO (MEJORADO)
-- ================================================
CREATE PROCEDURE [dbo].[usp_EditarUsuario]
    @IdUsuario INT,
    @NombreCompleto VARCHAR(100),
    @Correo VARCHAR(100),
    @Clave VARCHAR(100),
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM tb_Usuarios WHERE Correo = @Correo AND IdUsuario <> @IdUsuario)
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END

    IF @Clave IS NULL OR @Clave = ''
    BEGIN
        UPDATE tb_Usuarios
        SET NombreCompleto = @NombreCompleto,
            Correo = @Correo,
            IdRol = @IdRol
        WHERE IdUsuario = @IdUsuario;
    END
    ELSE
    BEGIN
        UPDATE tb_Usuarios
        SET NombreCompleto = @NombreCompleto,
            Correo = @Correo,
            Clave = @Clave,
            IdRol = @IdRol
        WHERE IdUsuario = @IdUsuario;
    END

    IF @@ROWCOUNT > 0
        SELECT 1 AS Resultado;
    ELSE
        SELECT 0 AS Resultado;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_EliminarMedicamento]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 5. Eliminar
CREATE PROCEDURE [dbo].[usp_EliminarMedicamento]
    @IdMedicamento INT
AS
BEGIN
    DELETE FROM tb_Medicamentos WHERE IdMedicamento = @IdMedicamento
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_EliminarMedico]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_EliminarMedico]
    @IdMedico INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar si tiene citas asociadas
    IF EXISTS (SELECT 1 FROM tb_Citas WHERE IdMedico = @IdMedico)
    BEGIN
        RAISERROR('No se puede eliminar el médico porque tiene citas registradas', 16, 1)
        RETURN
    END
    
    DELETE FROM tb_Medicos
    WHERE IdMedico = @IdMedico
END
GO
/****** Object:  StoredProcedure [dbo].[usp_EliminarUsuario]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 3. ELIMINAR USUARIO (MEJORADO)
-- ================================================
CREATE PROCEDURE [dbo].[usp_EliminarUsuario]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM tb_Usuarios
    WHERE IdUsuario = @IdUsuario;

    IF @@ROWCOUNT > 0
        SELECT 1 AS Resultado;
    ELSE
        SELECT 0 AS Resultado;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarCitasPendientes]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 2. LISTAR CITAS PENDIENTES
-- ================================================
CREATE PROCEDURE [dbo].[usp_ListarCitasPendientes]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 10 
        c.IdCita,
        c.FechaCita,
        c.Estado,
        u.NombreCompleto AS NombrePaciente,
        m.NombreCompleto AS NombreMedico,
        e.NombreEspec   AS NombreEspecialidad
    FROM tb_Citas c
        INNER JOIN tb_Usuarios      u ON c.IdPaciente      = u.IdUsuario
        INNER JOIN tb_Medicos       m ON c.IdMedico        = m.IdMedico
        INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
    WHERE c.Estado = 'Pendiente'
    ORDER BY c.FechaCita ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarCitasPorMedico]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 3. LISTAR CITAS POR MÉDICO
-- ================================================
CREATE PROCEDURE [dbo].[usp_ListarCitasPorMedico]
    @IdMedico INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.IdCita,
        c.FechaCita,
        c.Estado,
        u.NombreCompleto AS NombrePaciente,
        e.NombreEspec    AS NombreEspecialidad
    FROM tb_Citas c
        INNER JOIN tb_Usuarios      u ON c.IdPaciente      = u.IdUsuario
        INNER JOIN tb_Medicos       m ON c.IdMedico        = m.IdMedico
        INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
    WHERE c.IdMedico = @IdMedico
    ORDER BY c.FechaCita ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarCitasPorUsuario]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ListarCitasPorUsuario]
    @IdPaciente INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.IdCita,
        c.FechaCita,
        m.NombreCompleto AS NombreMedico,
        e.NombreEspec AS NombreEspecialidad,
        c.Estado
    FROM tb_Citas c
    INNER JOIN tb_Medicos m ON c.IdMedico = m.IdMedico
    INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
    WHERE c.IdPaciente = @IdPaciente
    ORDER BY c.FechaCita DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarMedicamentos]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- 1. Listar todo
CREATE PROCEDURE [dbo].[usp_ListarMedicamentos]
AS
BEGIN
    SELECT IdMedicamento, Nombre, Laboratorio, Precio, Stock FROM tb_Medicamentos
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarMedicos]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ListarMedicos]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        m.IdMedico,
        m.NombreCompleto,
        m.IdEspecialidad,
        e.NombreEspec AS Especialidad,
        ISNULL(m.CMP, '') AS CMP,
        ISNULL(m.Correo, '') AS Correo,
        ISNULL(m.Telefono, '') AS Telefono
    FROM tb_Medicos m
    LEFT JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
    ORDER BY m.NombreCompleto
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarMedicosPorEspecialidad]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- 1. Listar Médicos filtrando por Especialidad
CREATE PROCEDURE [dbo].[usp_ListarMedicosPorEspecialidad]
    @IdEspecialidad INT
AS
BEGIN
    SELECT m.IdMedico, u.NombreCompleto, e.NombreEspec as Especialidad, m.CMP
    FROM tb_Medicos m
    INNER JOIN tb_Usuarios u ON m.IdUsuario = u.IdUsuario
    INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
    WHERE m.IdEspecialidad = @IdEspecialidad
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarRoles]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 6. LISTAR ROLES
-- ================================================
CREATE PROCEDURE [dbo].[usp_ListarRoles]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdRol, NombreRol
    FROM tb_Roles
    ORDER BY NombreRol;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ListarUsuarios]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- 1. LISTAR TODOS LOS USUARIOS
-- ================================================
CREATE PROCEDURE [dbo].[usp_ListarUsuarios]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.IdUsuario,
        u.NombreCompleto,
        u.Correo,
        u.IdRol,
        r.NombreRol
    FROM tb_Usuarios u
    INNER JOIN tb_Roles r ON u.IdRol = r.IdRol
    ORDER BY u.NombreCompleto;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ObtenerMedicamento]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 2. Obtener uno solo (Para editar)
CREATE PROCEDURE [dbo].[usp_ObtenerMedicamento]
    @IdMedicamento INT
AS
BEGIN
    SELECT IdMedicamento, Nombre, Laboratorio, Precio, Stock 
    FROM tb_Medicamentos WHERE IdMedicamento = @IdMedicamento
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_ObtenerMedico]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ObtenerMedico]
    @IdMedico INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        IdMedico,
        NombreCompleto,
        IdEspecialidad,
        ISNULL(CMP, '') AS CMP,
        ISNULL(Correo, '') AS Correo,
        ISNULL(Telefono, '') AS Telefono
    FROM tb_Medicos
    WHERE IdMedico = @IdMedico
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ObtenerUsuario]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- 2. OBTENER USUARIO POR ID
-- ================================================
CREATE PROCEDURE [dbo].[usp_ObtenerUsuario]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.IdUsuario,
        u.NombreCompleto,
        u.Correo,
        u.Clave,
        u.IdRol,
        r.NombreRol
    FROM tb_Usuarios u
    INNER JOIN tb_Roles r ON u.IdRol = r.IdRol
    WHERE u.IdUsuario = @IdUsuario;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrarCita]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_RegistrarCita]
    @IdPaciente INT,
    @IdMedico INT,
    @FechaCita DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO tb_Citas (IdPaciente, IdMedico, FechaCita, Estado)
    VALUES (@IdPaciente, @IdMedico, @FechaCita, 'P'); -- P = Pendiente
    
    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrarCliente]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_RegistrarCliente]
    @NombreCompleto VARCHAR(100),
    @Correo VARCHAR(100),
    @Clave VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Insertar SIEMPRE como Paciente (IdRol = 3)
    INSERT INTO dbo.tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES (@NombreCompleto, @Correo, @Clave, 3);
    
    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrarDetalle]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_RegistrarDetalle]
    @IdVenta INT,
    @IdMedicamento INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2), -- ✅ CORREGIDO
    @SubTotal DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- ✅ Insertar con PrecioUnitario
    INSERT INTO tb_DetalleVenta (IdVenta, IdMedicamento, Cantidad, PrecioUnitario, SubTotal)
    VALUES (@IdVenta, @IdMedicamento, @Cantidad, @PrecioUnitario, @SubTotal);
    
    -- Actualizar stock del medicamento
    UPDATE tb_Medicamentos 
    SET Stock = Stock - @Cantidad 
    WHERE IdMedicamento = @IdMedicamento;
    
    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrarMedicamento]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 3. Guardar (Insertar)
CREATE PROCEDURE [dbo].[usp_RegistrarMedicamento]
    @Nombre VARCHAR(100),
    @Laboratorio VARCHAR(50),
    @Precio DECIMAL(10,2),
    @Stock INT
AS
BEGIN
    INSERT INTO tb_Medicamentos(Nombre, Laboratorio, Precio, Stock)
    VALUES (@Nombre, @Laboratorio, @Precio, @Stock)
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrarMedico]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_RegistrarMedico]
    @NombreCompleto VARCHAR(100),
    @IdEspecialidad INT,
    @CMP VARCHAR(20),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO tb_Medicos (NombreCompleto, IdEspecialidad, CMP, Correo, Telefono)
    VALUES (@NombreCompleto, @IdEspecialidad, @CMP, @Correo, @Telefono)
    
    SELECT SCOPE_IDENTITY() AS IdMedico
END
GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrarVenta]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_RegistrarVenta]
    @IdPaciente INT,
    @Total DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO tb_Ventas (IdPaciente, Total, FechaVenta)
    VALUES (@IdPaciente, @Total, GETDATE());
    
    -- Devolver el ID generado
    SELECT SCOPE_IDENTITY() AS IdVentaGenerado;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ValidarUsuario]    Script Date: 21/12/2025 23:00:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Procedimiento para el Login
CREATE PROCEDURE [dbo].[usp_ValidarUsuario]
    @Correo VARCHAR(100),
    @Clave VARCHAR(100)
AS
BEGIN
    -- Seleccionamos también el NombreRol haciendo un JOIN
    SELECT u.IdUsuario, u.NombreCompleto, u.Correo, u.Clave, u.IdRol, r.NombreRol
    FROM tb_Usuarios u
    INNER JOIN tb_Roles r ON u.IdRol = r.IdRol
    WHERE u.Correo = @Correo AND u.Clave = @Clave
END;

GO
USE [master]
GO
ALTER DATABASE [BD_MediCita] SET  READ_WRITE 
GO

