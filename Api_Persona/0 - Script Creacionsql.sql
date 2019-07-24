/****** Object:  Table [dbo].[Pais]    Script Date: 23/07/2019 23:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pais](
	[Id] [int] NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Pais] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Persona]    Script Date: 23/07/2019 23:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persona](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Apellido] [nvarchar](50) NOT NULL,
	[NumeroDocumento] [nvarchar](12) NOT NULL,
	[IdTipoDocumento] [int] NOT NULL,
	[IdPais] [int] NOT NULL,
	[FechaNacimiento] [datetime] NOT NULL,
	[IdSexo] [int] NOT NULL,
 CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonaContacto]    Script Date: 23/07/2019 23:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonaContacto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPersona] [int] NOT NULL,
	[Valor] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Contacto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonaRelacion]    Script Date: 23/07/2019 23:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonaRelacion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPersona1] [int] NOT NULL,
	[IdPersona2] [int] NOT NULL,
	[IdTipoRelacion] [int] NOT NULL,
 CONSTRAINT [PK_Relacion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Relacion] UNIQUE NONCLUSTERED 
(
	[IdPersona1] ASC,
	[IdPersona2] ASC,
	[IdTipoRelacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sexo]    Script Date: 23/07/2019 23:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sexo](
	[Id] [int] NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sexo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TipoDocumento]    Script Date: 23/07/2019 23:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDocumento](
	[Id] [int] NOT NULL,
	[Descripcion] [varchar](150) NULL,
 CONSTRAINT [PK_TipoDocumento_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TipoRelacion]    Script Date: 23/07/2019 23:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoRelacion](
	[Id] [int] NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TipoRelacion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Persona_Pais] FOREIGN KEY([IdPais])
REFERENCES [dbo].[Pais] ([Id])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_Pais]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Persona_Sexo] FOREIGN KEY([IdSexo])
REFERENCES [dbo].[Sexo] ([Id])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_Sexo]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Persona_TipoDocumento] FOREIGN KEY([IdTipoDocumento])
REFERENCES [dbo].[TipoDocumento] ([Id])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_TipoDocumento]
GO
ALTER TABLE [dbo].[PersonaContacto]  WITH CHECK ADD  CONSTRAINT [FK_Contacto_Persona] FOREIGN KEY([IdPersona])
REFERENCES [dbo].[Persona] ([Id])
GO
ALTER TABLE [dbo].[PersonaContacto] CHECK CONSTRAINT [FK_Contacto_Persona]
GO
ALTER TABLE [dbo].[PersonaRelacion]  WITH CHECK ADD  CONSTRAINT [FK_Relacion_Persona_Destinatario] FOREIGN KEY([IdPersona2])
REFERENCES [dbo].[Persona] ([Id])
GO
ALTER TABLE [dbo].[PersonaRelacion] CHECK CONSTRAINT [FK_Relacion_Persona_Destinatario]
GO
ALTER TABLE [dbo].[PersonaRelacion]  WITH CHECK ADD  CONSTRAINT [FK_Relacion_Persona_Originante] FOREIGN KEY([IdPersona1])
REFERENCES [dbo].[Persona] ([Id])
GO
ALTER TABLE [dbo].[PersonaRelacion] CHECK CONSTRAINT [FK_Relacion_Persona_Originante]
GO
ALTER TABLE [dbo].[PersonaRelacion]  WITH CHECK ADD  CONSTRAINT [FK_Relacion_TipoRelacion] FOREIGN KEY([IdTipoRelacion])
REFERENCES [dbo].[TipoRelacion] ([Id])
GO
ALTER TABLE [dbo].[PersonaRelacion] CHECK CONSTRAINT [FK_Relacion_TipoRelacion]
GO