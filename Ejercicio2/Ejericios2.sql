Select top 10
	*
From
	[Status]
	Inner join [option] on [option].--desconozco
	Inner join [Source] on [Source] --desconozco
Where
	[Status] = 'Shipped'
	and [option].Service_id in([891,841,871)
	and (status.date_h >= '20180101' and status.date_h <= '20180510' )
	and source.site = 'MLB'	

	--JQ se utiliza para analizar arhivos Json desde la linea de comandos, aplicando filtros 
	--en una secuencia de datos y que sea facil de leer, en un formato tabulado
	--Permite seleccionar un numero de resultados, incluso uno en puntual.
	--$ Jq .XXXXX[0] XXXXX.json--Ejemplo de optencion del 1er elemento de una matriz
	--$ Jq .XXXXX[0:2] XXXXX.json--Ejemplo de optencion de los 2 primeros elementos de una matriz
	--$ jq .XXXXX[0].xxxx | length --retorna el largo en caracteres del campo que busquemos



Ejercicio 2
A continuación se detalla una consulta efectuada a un Document Search que por detrás
apunta a una base de datos no relacional.
● Necesitamos que conviertas el search dado a una estructura de SQL que sería
equivalente si la hiciéramos en una base SLQ server.
SELECT [Atributos]
FROM [Tablas]
WHERE [Condiciones]
curl -H "Content-Type: application/json" -X POST -d '{
"query":{
"and":[
{
"eq":
{
"field":"source.site",
"value":"MLB"
}
},
{
"date_range":
{
"field":"status.date_h",
"gte":"2018-01-01T00:00:00.000-04:00",
"lte":"2018-05-10T00:00:00.000-04:00"
}
},
{
"eq":
{
"field":"status",
"value":"shipped"
}
},
{
"in":
{
"field":"option.service_id",
"value":[891,841,871]
}
}
]
},
"size":10
}' http://…./entities/shipment/search -s | jq .
● Investigar y proponer 2 posibles usos del comando "jq" para la explotación,
extracción y análisis de datos.

