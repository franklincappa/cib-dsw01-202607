CREATE TABLE pais (
idPais CHAR(3) PRIMARY KEY,
nombre VARCHAR(50) NOT null
)
GO

CREATE TABLE vendedor (
idVendedor INT PRIMARY KEY,
nombre VARCHAR(255) NOT NULL, 
direccion VARCHAR(255) NOT NULL, 
idPais CHAR(3) REFERENCES pais,
email VARCHAR(255) NOT NULL
)
GO


-- TABLA PAIS
INSERT INTO pais (idPais, nombre)
VALUES
('PER', 'Perú'),
('CHL', 'Chile'),
('COL', 'Colombia'),
('ARG', 'Argentina'),
('MEX', 'México'),
('ECU', 'Ecuador'),
('BOL', 'Bolivia'),
('USA', 'Estados Unidos');
GO


-- TABLA VENDEDOR
INSERT INTO vendedor
(
    idVendedor,
    nombre,
    direccion,
    idPais,
    email
)
VALUES
(1, 'Juan Pérez', 'Av. Javier Prado 123 - Lima', 'PER', 'juan.perez@empresa.com'),
(2, 'María Gómez', 'Calle Los Olivos 456 - Arequipa', 'PER', 'maria.gomez@empresa.com'),
(3, 'Carlos Rodríguez', 'Av. Providencia 890 - Santiago', 'CHL', 'carlos.rodriguez@empresa.com'),
(4, 'Ana Martínez', 'Carrera 15 #20-30 - Bogotá', 'COL', 'ana.martinez@empresa.com'),
(5, 'Luis Fernández', 'Av. Corrientes 1500 - Buenos Aires', 'ARG', 'luis.fernandez@empresa.com'),
(6, 'Sofía Ramírez', 'Av. Reforma 250 - Ciudad de México', 'MEX', 'sofia.ramirez@empresa.com'),
(7, 'Diego Torres', 'Av. Amazonas 500 - Quito', 'ECU', 'diego.torres@empresa.com'),
(8, 'Valeria Castro', 'Av. Arce 100 - La Paz', 'BOL', 'valeria.castro@empresa.com'),
(9, 'Michael Johnson', '5th Avenue 120 - New York', 'USA', 'michael.johnson@empresa.com'),
(10, 'Patricia Vargas', 'Av. La Marina 890 - Lima', 'PER', 'patricia.vargas@empresa.com'),
(11, 'José Mendoza', 'Av. Universitaria 101 - Lima', 'PER', 'jose.mendoza@empresa.com'),
(12, 'Lucía Herrera', 'Jr. Ayacucho 220 - Cusco', 'PER', 'lucia.herrera@empresa.com'),
(13, 'Miguel Salas', 'Av. Ejército 540 - Arequipa', 'PER', 'miguel.salas@empresa.com'),
(14, 'Andrea Rojas', 'Calle Real 890 - Huancayo', 'PER', 'andrea.rojas@empresa.com'),
(15, 'Fernando Díaz', 'Av. Grau 350 - Piura', 'PER', 'fernando.diaz@empresa.com'),
(16, 'Camila Soto', 'Av. Libertador 1500 - Santiago', 'CHL', 'camila.soto@empresa.com'),
(17, 'Ricardo Fuentes', 'Calle Moneda 250 - Santiago', 'CHL', 'ricardo.fuentes@empresa.com'),
(18, 'Paula Vargas', 'Av. Apoquindo 700 - Santiago', 'CHL', 'paula.vargas@empresa.com'),
(19, 'Jorge Molina', 'Carrera 10 #45-20 - Bogotá', 'COL', 'jorge.molina@empresa.com'),
(20, 'Daniela Ruiz', 'Calle 80 #15-10 - Medellín', 'COL', 'daniela.ruiz@empresa.com'),
(21, 'Esteban López', 'Carrera 30 #22-15 - Cali', 'COL', 'esteban.lopez@empresa.com'),
(22, 'Gabriela Torres', 'Av. Santa Fe 1800 - Buenos Aires', 'ARG', 'gabriela.torres@empresa.com'),
(23, 'Martín Cabrera', 'Av. Belgrano 550 - Córdoba', 'ARG', 'martin.cabrera@empresa.com'),
(24, 'Florencia Acosta', 'Av. Colón 410 - Rosario', 'ARG', 'florencia.acosta@empresa.com'),
(25, 'Alejandro Pérez', 'Paseo de la Reforma 100 - CDMX', 'MEX', 'alejandro.perez@empresa.com'),
(26, 'Valentina Cruz', 'Av. Insurgentes 450 - CDMX', 'MEX', 'valentina.cruz@empresa.com'),
(27, 'Héctor Ramírez', 'Av. Juárez 300 - Guadalajara', 'MEX', 'hector.ramirez@empresa.com'),
(28, 'Natalia Gómez', 'Av. Amazonas 420 - Quito', 'ECU', 'natalia.gomez@empresa.com'),
(29, 'Kevin Paredes', 'Av. Naciones Unidas 800 - Quito', 'ECU', 'kevin.paredes@empresa.com'),
(30, 'Patricia León', 'Malecón Simón Bolívar 250 - Guayaquil', 'ECU', 'patricia.leon@empresa.com'),
(31, 'Raúl Chávez', 'Av. Arce 300 - La Paz', 'BOL', 'raul.chavez@empresa.com'),
(32, 'Mónica Flores', 'Av. Busch 700 - Santa Cruz', 'BOL', 'monica.flores@empresa.com'),
(33, 'Cristian Vargas', 'Av. Blanco Galindo 950 - Cochabamba', 'BOL', 'cristian.vargas@empresa.com'),
(34, 'John Smith', 'Broadway 100 - New York', 'USA', 'john.smith@empresa.com'),
(35, 'Emily Johnson', 'Michigan Ave 500 - Chicago', 'USA', 'emily.johnson@empresa.com'),
(36, 'Robert Brown', 'Market Street 200 - San Francisco', 'USA', 'robert.brown@empresa.com'),
(37, 'Susan Miller', 'Ocean Drive 120 - Miami', 'USA', 'susan.miller@empresa.com'),
(38, 'David Wilson', 'Sunset Blvd 300 - Los Angeles', 'USA', 'david.wilson@empresa.com'),
(39, 'Karen Davis', 'Main Street 150 - Dallas', 'USA', 'karen.davis@empresa.com'),
(40, 'Frank Thomas', 'Liberty Ave 700 - Houston', 'USA', 'frank.thomas@empresa.com');
GO

CREATE OR ALTER PROCEDURE usp_paises
AS BEGIN
	SELECT idpais, nombre FROM pais
END
GO

CREATE OR ALTER PROCEDURE usp_vendedor
AS BEGIN 
 SELECT idVendedor, nombre, direccion, idPais, email FROM vendedor
END
GO

CREATE OR ALTER PROCEDURE usp_merge_vendedor
	@idvendedor INT,
	@nombre VARCHAR(255),
	@direccion VARCHAR(255), 
	@idpais CHAR(3),
	@email VARCHAR(255)
AS 
BEGIN
	SET NOCOUNT ON;
	MERGE vendedor AS target
	USING (SELECT @idvendedor AS id, @nombre AS nombre, @direccion AS direccion, @idpais AS idpais, @email AS email) AS src
	ON target.idVendedor=src.id
	WHEN MATCHED THEN 
		UPDATE SET target.nombre=src.nombre, target.direccion=src.direccion, target.idPais=src.idpais, target.email=src.email
	WHEN NOT MATCHED THEN 
		INSERT (idVendedor, nombre, direccion, idPais, email) VALUES(src.id, src.nombre, src.direccion, src.idpais, src.email);

END 
GO

