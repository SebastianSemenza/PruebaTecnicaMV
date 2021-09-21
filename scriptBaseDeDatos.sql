
create database PruebaTecnica
go
use PruebaTecnica
go
create table Ciudades(
	id int not null primary key identity,
	nombre varchar(50) not null,
	pais varchar(50) not null,
	parametro varchar(50) not null,
)
go
create table RegistrosClima(
	id int not null primary key identity,
	idCiudad int not null foreign key references Ciudades,
	temperatura varchar(50) not null,
	termica varchar(50) not null,
)
go
create procedure spAgregarRegClima
@idCiudad int,
@temperatura varchar(10),
@termica varchar(10)
as
insert into RegistrosClima (idCiudad,temperatura,termica) values (@idCiudad,@temperatura,@termica)

go

create procedure spHistorialPorCiudad
@idCiudad int
as
select RG.id,C.id,C.nombre,C.pais,RG.temperatura,RG.termica from RegistrosClima RG
inner join Ciudades C on C.id=RG.idCiudad
where RG.idCiudad=@idCiudad

go

create procedure spDevolverDatosCiudad
@idCiudad int
as
select C.parametro,C.nombre from Ciudades C where C.id=@idCiudad

go

create procedure spListarCiudades
as
select C.id,C.nombre from Ciudades C

go

--Datos de Ciudades
insert into Ciudades (nombre,pais,parametro) values ('Buenos Aires','Argentina','Buenos%20Aires,ar')
insert into Ciudades (nombre,pais,parametro) values ('Cordoba','Argentina','Cordoba,ar')
insert into Ciudades (nombre,pais,parametro) values ('Rosario','Argentina','Rosario,ar')
insert into Ciudades (nombre,pais,parametro) values ('La Plata','Argentina','La%20Plata,ar')
insert into Ciudades (nombre,pais,parametro) values ('Londres','Inglaterra','London')
insert into Ciudades (nombre,pais,parametro) values ('Paris','Francia','Paris')


--Datos Prueba Temperaturas
insert into RegistrosClima (idCiudad,temperatura,termica) values (1,'22','24')
