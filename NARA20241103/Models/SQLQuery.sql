-- Creacion BD:
Create DataBase NARA20241103BD;
Go

-- Usar BD:
Use NARA20241103BD;
Go


-- Tabla #1:
Create Table Fiadores
(
IdFiador int Identity Primary Key,
Nombre Varchar(150) not null,
Fecha date not null,
Correlativo int not null,
DineroFiado decimal(10,2) not null,
);
Go


-- Tabla #2:
Create Table DetalleFamiliares
(
IdDetalleFamilia int Identity Primary Key,
Nombre Varchar(150) not null,
Parentesco Varchar(150) not null,
Telefono Varchar(150) not null,
Dui Varchar(150) not null,
FiadorDetalle int Foreign Key(FiadorDetalle) References Fiadores(IdFiador) On Delete Cascade
);
Go