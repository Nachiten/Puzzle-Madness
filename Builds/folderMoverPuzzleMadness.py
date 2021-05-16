import shutil
import os

# #### VARIABLES DE ENTRADA #### #
carpetaOrigen1 = "MonoBleedingEdge"
carpetaOrigen2 = "Puzzle Madness Launcher_Data"

carpetaOrigen1Copia = carpetaOrigen1 + "Copia"
carpetaOrigen2Copia = carpetaOrigen2 + "Copia"

# Creo directorios
os.mkdir(carpetaOrigen1Copia)
os.mkdir(carpetaOrigen2Copia)

# Muevo la carpeta de un lado a otro
shutil.move(carpetaOrigen1, carpetaOrigen1Copia)

# Muevo la carpeta de un lado a otro
shutil.move(carpetaOrigen2, carpetaOrigen2Copia)

os.rename(carpetaOrigen1Copia, carpetaOrigen1)
os.rename(carpetaOrigen2Copia, carpetaOrigen2)
