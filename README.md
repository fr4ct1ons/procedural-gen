# Procedural Gen
Este projeto foi feito para praticar geração procedural com a Unity. Nele, há um gerador de mesh e um de terreno, ambos fazendo uso de um gerador de perlin noise. Há também um conjunto de spawnadores, para spawnar objetos em uma determinada área.

Feito por Gabriel Paes Landim de Lucena. 

Twitter: https://twitter.com/fr4ct1ons

GitHub: https://github.com/lucena-fr4ct1ons

## Geração de terrenos e meshes

Ambos scripts para gerar terrenos e meshes apresentam as seguintes variáveis, usadas para controlar o noise gerado:
 - Scale;
 - Octaves;
 - Persistence;
 - Lacunarity;
 - Seed;
 - Offset.

Há também o xSize, ySize e zSize, que determinam o tamanho do noise gerado. É importante notar que devido a limitações do terreno da Unity, essas variáveis devem ser uma potência de 2 e menores que 4096 para o script do gerador de terreno. Caso contrário, o terreno pode ficar irregular ou uma exceção pode ser gerada.
Por fim, há também uma AnimationCurve para "planificar" partes do terreno para que planícies sejam mantidas.

## Spawnador

Há um conjunto de spawnadores diferenciados - De área, polígono 2D, bezier, círculo, volume e linha. Eles já foram feitos anteriormente e podem ser encontrados em https://github.com/lucena-fr4ct1ons/cool-unity-stuff/tree/master/Cool%20Unity%20Stuff/Assets/SpawningSystem. É importante notar que o spawnador de área 2D foi usado neste projeto para spawnar as árvores na mesh gerada e com isso, também foi alterado para só spawnar um objeto se ele atirar um raycast em direção ao chão e bater em um colisor.

