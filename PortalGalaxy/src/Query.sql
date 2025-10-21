CREATE PROCEDURE uspListarInscripciones
(
	@InstructorId INT = NULL,
	@Taller NVARCHAR(100) = NULL,
	@Situacion INT = NULL,
	@FechaInicio DATE = NULL,
	@FechaFin DATE = NULL,
	@Pagina INT = 1,
	@Filas INT = 10
)
AS
BEGIN
	
	SELECT 
	T.Id,
	T.Nombre Taller,
	c.Nombre Categoria,
	Profe.Nombres Instructor,
	T.FechaInicio Fecha,
	CASE
		I.Situacion WHEN 0 THEN 'Asistirá'
		WHEN 1 THEN 'Cancelado'
	END AS Situacion ,
	COUNT(I.ID) AS Cantidad
FROM
	Taller t
INNER JOIN Instructor PROFE ON
	T.InstructorId = PROFE.ID
INNER JOIN Categoria c ON
	PROFE.CategoriaId = c.Id
INNER JOIN Inscripcion i ON
	I.TallerId = T.Id
WHERE
	T.Estado = 1
	AND (@InstructorId IS NULL OR (T.InstructorId = @InstructorId))
	AND (@Taller IS NULL
		OR (T.Nombre like '%' + @Taller + '%' ))
	AND (@Situacion IS NULL
		OR (I.Situacion = @Situacion ))
	AND (@FechaInicio IS NULL
		OR (I.FechaCreacion BETWEEN @FechaInicio AND @FECHAFIN ))
GROUP BY
	T.ID,
	T.Nombre ,
	C.Nombre ,
	PROFE.Nombres ,
	T.FechaInicio ,
	I.Situacion
ORDER BY
	T.FechaInicio desc
OFFSET @Pagina ROWS
FETCH NEXT @FILAS ROWS ONLY;

SELECT COUNT(*) AS TOTAL
FROM TALLER T
INNER JOIN Instructor PROFE ON
	T.InstructorId = PROFE.ID
INNER JOIN Categoria c ON
	PROFE.CategoriaId = c.Id
INNER JOIN Inscripcion i ON
	I.TallerId = T.Id
WHERE
	T.Estado = 1
	AND (@InstructorId IS NULL
		OR (T.InstructorId = @InstructorId))
	AND (@Taller IS NULL
		OR (T.Nombre like '%' + @Taller + '%' ))
	AND (@Situacion IS NULL
		OR (I.Situacion = @Situacion ))
	AND (@FechaInicio IS NULL
		OR (I.FechaCreacion BETWEEN @FechaInicio AND @FECHAFIN ))

END
