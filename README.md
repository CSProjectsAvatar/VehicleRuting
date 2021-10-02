<h1> Enrutamiento de Vehículos con IVNS </h1>

Le damos solución al Problema de Enrutamiento de Vehículos (VRP) empleando Búsqueda de Vecindad Infinitamente Variable (IVNS).

<h2> Contenidos </h2>

- [Ejecución](#ejecución)
- [Lo que trae esta versión](#lo-que-trae-esta-versión)
- [Lo que vendrá pronto](#lo-que-vendrá-pronto)
  - [Nuevo comando](#nuevo-comando)
  - [Empleo de varias estructuras de datos](#empleo-de-varias-estructuras-de-datos)
  - [Interfaz gráfica](#interfaz-gráfica)
  - [Artículos formales](#artículos-formales)

## Ejecución
Se requiere una distribución de Linux de 64 bits.

Luego de descomprimir los compilados, para ejecutar el programa basta con abrir un *shell* en la carpeta donde se encuentran los archivos y ejecutar
`./ivns`. Observará entonces la ayuda del programa y podrá entender cómo usarlo.

## Lo que trae esta versión
Contamos con una interfaz de consola que permite introducir los parámetros
* máxima cantidad de segundos que debe consumir el algoritmo,
* máxima cantidad de segundos que debe consumir la optimización de una solución en una vecindad dada, y
* única vecindad a tener en cuenta. En el caso de que este parámetro no se inserte, el algoritmo explorará todas los posibles vecindades.

Para más información, consulte la ayuda de la aplicación.

## Lo que vendrá pronto
### Nuevo comando
Nos hemos propuesto implementar un nuevo comando de vecindad: **intercambiar subruta**. Este seleccionará dos rutas y luego una subcadena de cada una, para después intercambiarlas.

### Empleo de varias estructuras de datos
Actualmente utilizamos la lista de C# para almacenar los clientes de una ruta. Para próximas versiones tendremos en cuenta implementaciones de AVL y Skip Lists con el objetivo de que el algoritmo sea más eficiente.

### Interfaz gráfica
Con la introducción de una interfaz gráfica haremos que el cliente se sienta más cómodo interactuando con la aplicación.

### Artículos formales
Redactaremos artículos que expliquen formalmente nuestro acercamiento al problema y la solución algorítmica que brindamos.