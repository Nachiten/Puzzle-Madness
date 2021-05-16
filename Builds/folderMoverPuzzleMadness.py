import shutil
import os

# #### VARIABLES DE ENTRADA #### #
carpetaOrigen1 = "MonoBleedingEdge"
carpetaOrigen2 = "Puzzle Madness Launcher_Data"

if not os.path.exists(carpetaOrigen1) or not os.path.exists(carpetaOrigen2):
    print("[FolderMover] Las carpetas originales no existen")
    input()
    exit(-1)

# Creo nombres de copias
carpetaOrigen1Copia = carpetaOrigen1 + "Copia"
carpetaOrigen2Copia = carpetaOrigen2 + "Copia"

# Creo directorios copia
os.mkdir(carpetaOrigen1Copia)
os.mkdir(carpetaOrigen2Copia)

# Muevo las carpetas de un lado a otro
shutil.move(carpetaOrigen1, carpetaOrigen1Copia)
shutil.move(carpetaOrigen2, carpetaOrigen2Copia)

# Renombro las copias para tener igual nombre que las originales
os.rename(carpetaOrigen1Copia, carpetaOrigen1)
os.rename(carpetaOrigen2Copia, carpetaOrigen2)

print("[FolderMover] Las dos carpetas fueron creadas y movidas correctamente")
input()
