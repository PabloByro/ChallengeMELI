# ChallengeMELI
Observaciones Ejercicio 1
  Se realizo un proyecto en C# separado con 2 clases, no hice un proyecto separado en 3 o 4 capas, para no agregarle mucha complejidad a 
  algo que no lo merecia, y solo complicaria mas el proyecto.
  La aplicación de consola esta parametrizada mayormente desde el Appconfig. En caso de tener que hacer algun cambio, se hace de manera rapida y sencilla.
  Por medio de una funcion recursiva (ProcessRepositories) se recuparan los datos de la APi, cada bloque de datos recuperado, es transformado en el formato solicitado
  y enviado a la clase ClsLog, funcion WriteLog, para que grabe el log con el formato indicado.
  Luego se llama la clase ClsCSV, el cual grabara el archivo CSV
  
  Me encontre con uno problema que no logre solventar a tiempo.  
  Al momento de querer recuperar los items obtengo un error y creo que es por no tener un token, recien logre encontrar en la web que
  "A partir de abril de 2021 será obligatorio enviar el access token por header cada vez que realices llamadas a la API"

Observaciones Ejercicio 2
  Se realizo Query en SQL, para recuperar los mismos datos presentados por el ejercicio. No logre hacer el From y los inner join ya que me faltaron datos.

Observaciones Ejercicio 3
  Se realizo ejercicio de SQL, creando tablas de las estructura solicitada y haciendo los Querys solicitados por el ejercicio.
