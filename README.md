# GAEV

> 👇 Usa un encabezado de nivel 1 (`#`) para el nombre del proyecto.
> También puedes usar una breve descripción o cita debajo.

Este proyecto permite generar de manera sencilla habitaciones proporcionando sus parámetros y luego guardarlas en un archivo .json, así como cargar la habitación generada

---
Autores: Jonás Rodríguez Unanyan y Maria Laura Hernández Hernández
## 📚 Índice

- [Instalación](#instalación)
- [Uso](#uso)
- [Estructura](#estructura)

---

## ⚙️ Instalación

> 👇 Usa listas numeradas con código incrustado usando triple acento grave (```) para comandos.

1. Clona el repositorio:
   ```bash
   git clone https://github.com/tu-usuario/tu-repo.git
   ```
2. Entra en la carpeta del proyecto:
   ```bash
   cd tu-repo
   ```
3. Instala las dependencias:
   ```bash
   pip install -r requirements.txt
   ```
##📦 Instalación

1. Clona el repositorio:
   ```bash
   git clone https://github.com/Jonas-RUnanyan/GAEV.git
   ```
2. Abre Unity Hub.

3. Selecciona "Add" y busca la carpeta del proyecto.

4. Asegúrate de abrirlo con la versión correcta de Unity (2022.3.30f1)

---

## 🧪 Uso

Para empezar, asegúrate de de que el objeto RoomGenerator esté activo y el objeto RoomLoader inactivo (click derecho > "Toggle Active State")
![Objeto RoomGenerator activado](roomGenerator_highlight.jpg)
Dale al play, los controles de la cámara son WASD para moverse, presionar la rueda del ratón y arrastar para mover la cámara. En el inspector del objeto RoomGenerator encontrarás el apartado "Room Par", haz doble click en RoomParameters para acceder al objeto que contiene los parámetros de la habitación.
![Parámetros de RoomGenerator](roomGenerator_parameters.jpg)
En este objeto podremos ver los parámetros de la habitación, que son altura, anchura y profundidad; si tiene o no tejado; las medidas de las puertas que tenga; las puertas en cada una de las paredes (colocadas de manera equidistante entre ellas) y por último el color de las paredes
![Parámetros de la habitación](roomParameters_parameters.jpg)
Para colocar distintos objetos en la habitación, deberemos irnos al objeto ItemPlacer
![ItemPlacer en el inspector de Unity](itemPlacer_highlight.jpg)
En él, veremos la lista de objetos que podemos colocar, y la caja de texto "Item Name", en la que deberemos escribir el nombre del objeto tal y como aparece en la lista. Para colocarlo simplemente deberemos colocar el ratón en el punto que deseemos y hacer click, y el objeto quedará colocado. Si deseáramos rotarlo, antes de colocarlo deberíamos mantener pulsado el click derecho y arrastar, hasta que veamos el objeto con la rotación que queramos
![Parámetros de ItemPlacer](itemPlacer_parameters.jpg)

Cuando tengamos una habitación a nuestro gusto, bastará con pulsar la tecla G para guardar en un archivo json nuestra habitación, cuya ubicación veremos en un mensaje que se imprimirá por la consola de Unity.

Si lo que queremos es caergar la habitación que hemos creado previamente, bastará con desactivar el objeto RoomGenerator y activar RoomLoader para que al dar al play la habitación se cargue automáticamente tal y como la habíamos guardado.
![Objeto RoomLoader activado](roomLoader_highlight.jpg)


## 📁 Estructura

```
GAEV/Assets
├── Plugins/             # Contiene el plugin newtonsoft, usado para la serialización en json de las habitaciones
├── PrefabAssets/        # Pruebas
├── Prefabs/             # Contiene la carpeta Resources, que contiene todo el mobiliario disponible
├── RoomAssets/          # Contiene los ScriptableObjects usados para guardar los datos de  las habitaciones
├── Scripts/             # Contiene todo el código del programa
└── resto de carpetas
```
