
 

 /*
Ejercicio 3 SQL
Las siguientes tablas describen una base de datos simplificada en la que se
registran compras de productos:
● Clientes (CodigoCliente int, Nombre char(60), Apellido char(60), Edad int)
● Productos (CodigoProducto int, Valor float, Descripcion char(60))
● Compras (CodigoProducto int, NumeroCompra int, Importe int,
CodigoCliente int)
a) Escriba la sentencia SQL necesaria para listar los apellidos de todos los
clientes cuyo apellido comience con la letra “f”.
b) Listar los CodigoCliente y Apellido de todos los clientes mayores de edad
(18 años) que tienen al menos una compra realizada con importe mayor a
1000 pesos y ordenarlos de forma descendente.
*/

--a)
Select 
	Cl.Apellido
From
	[Clientes] as Cl
Where
	Cl.Apellido like 'F%'

--b)
Select Distinct
	cl.CodigoCliente,
	Cl.Apellido
From
	Clientes as Cl 
	Inner join Compras as Co on Co.CodigoCliente = Cl.CodigoCliente
Where
	Cl.Edad > 18
	and Co.Importe >  1000
Order by 
	Co.CodigoCliente Desc


/*Creaccion de las 3 tablas*/
--Clientes
CREATE TABLE [dbo].[Clientes](
	[CodigoCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [char](50) NOT NULL,
	[Apellido] [char](60) NOT NULL,
	[Edad] [int] NOT NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[CodigoCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

--Compras
CREATE TABLE [dbo].[Compras](
	[CodigoProducto] [int] NOT NULL,
	[NumeroCompra] [char](50) NOT NULL,
	[Importe] [int] NOT NULL,
	[CodigoCliente] [int] NOT NULL,
 CONSTRAINT [PK_Compras] PRIMARY KEY CLUSTERED 
(
	[CodigoProducto] ASC,
	[NumeroCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


--Productos
CREATE TABLE [dbo].[Productos](
	[CodigoProducto] [int] IDENTITY(1,1) NOT NULL,
	[Valor] [float] NOT NULL,
	[Descripcion] [char](60) NOT NULL,
 CONSTRAINT [PK_Productos_1] PRIMARY KEY CLUSTERED 
(
	[CodigoProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]