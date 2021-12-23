<h1> Enrutamiento de Vehículos con IVNS </h1>

Le damos solución al Problema de Enrutamiento de Vehículos (VRP) empleando Búsqueda de Vecindad Infinitamente Variable (IVNS).

<h2> Contenidos </h2>

- [Ejecución](#ejecución)
- [Lo que trae esta versión](#lo-que-trae-esta-versión)
- [Lo que vendrá pronto](#lo-que-vendrá-pronto)
  - [Empleo de varias estructuras de datos](#empleo-de-varias-estructuras-de-datos)
  - [Interfaz gráfica](#interfaz-gráfica)

## Ejecución
El programa se puede ejecutar en Windows o en cualquier distribuci&oacute;n de Linux, siempre y cuando la arquitectura sea de 64 bits.

Luego de descomprimir los compilados, para ejecutar el programa basta con abrir un *shell* en la carpeta donde se encuentran los archivos y ejecutar
`./ivns`. Observará entonces la ayuda del programa y podrá entender cómo usarlo.

## Lo que trae esta versión
Contamos con una interfaz de consola que permite introducir los parámetros
* máxima cantidad de segundos que debe consumir el algoritmo,

* máxima cantidad de segundos que debe consumir la optimización de una solución en una vecindad dada,

* única vecindad a tener en cuenta. En el caso de que este parámetro no se inserte, el algoritmo explorará todas los posibles vecindades; y

* número de veces a ejecutar el algoritmo. Si este valor es distinto de 1, entonces el programa imprime los mejores resultados encontrados en cada una de las corridas, as&iacute; como el menor, el mayor y el costo promedio, a modo de resumen. Tambi&eacute;n se imprime la distribuci&oacute;n de clientes que debe seguir cada veh&iacute;culo para obtener la soluci&oacute;n de menor costo.  

  Debido a que el programa puede tardar una cantidad considerable de tiempo en terminar, el algoritmo se detiene al presionar la combinaci&oacute;n de teclas `Ctrl+C`, no sin antes imprimir la mejor soluci&oacute;n encontrada hasta el momento.

Para obtener una información m&aacute;s detallada, consulte la ayuda de la aplicación.

## Lo que vendrá pronto
### Empleo de varias estructuras de datos
Actualmente utilizamos la lista de C# para almacenar los clientes de una ruta. Para próximas versiones tendremos en cuenta implementaciones de AVL y Skip Lists con el objetivo de que el algoritmo sea más eficiente.

### Interfaz gráfica
Con la introducción de una interfaz gráfica haremos que el cliente se sienta más cómodo interactuando con la aplicación.
