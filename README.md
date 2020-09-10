# LaBouleQuiRoule
Notre jeu est constitué d'un plateau et d'une boule. Le but du jeu est d'amener le boule à la fin du plateau. Il possible de tourner le plateau pour guider à la boule à l'aide des touches du clavier. Il est également possible de faire sauter la boule pour esquiver les obstacles. 5 niveaux à difficultés croissantes sont mis à disposition à l'utilisateur. Le niveau actuel de l'utilisateur est conservé d'une partie à une autre. Sur le plateau de jeu, se trouve différents élements pouvant aider l'utilisateur comme par exemples des plaques accélératrice ou des plaques qui font grossir la taille de la boule. A contrario des éléments négatifs comme des plaques décleratrice et des plaques faisant rétraicir la boule sont aussi présent.

## Objectifs supplémentaires
* Selection du niveau 
* Plaque grossisante et rétraicicante
* Musique de fond et lecture d'un son lors de plusieurs actions du jeu(saut, démarrage, accelerateur)

## Guide Utilisateur
* Pour faire tomber la boule sur le plateau et démarrer le jeu : touche ``e``
* Pour faire pivoter le plateau : 
	* rotation à gauche : fléche de gauche ou la touche ``a``
	* rotation à droite : fléche de droite ou la touche ``d``
* Pour faire sauter la boule : touche ``espace``

## Gestion des niveaux 
Pour sauvegarder le niveau de l'utilisateur d'une partie à l'autre nous avons utiliser le package : https://assetstore.unity.com/packages/tools/input-management/save-game-free-gold-update-81519. 
Initialement le joueur est au niveau 0. À chaque fois que le joueur arrive au bout d'une partie le niveau est incrémenté de 1 jusqu'à 5. 
A chaque niveau les étapes suivantes sont effectués dans le but d'augmenter la difficultés : 
* diminutions de la taille des murs 
* Allongement de la taille du plateau 
* Augmentation de la fréquence des éléments négatifs sur le plateau(plaque décleratrice, plaque rétraicicante, plaque vide)
* Diminution de la fréquence des éléments positifs sur le plateau (plaque accélératrice, plaque grossisante)

## Objectif du cours
### Chapitre1 : prefab
Les éléments du plateau de jeu sont composé de prefabs, notamment un cube et une plaque.

### Chapitre2 : texture, colider, rigid body
Les différents composants du jeu ainsi que la sphére ont tous un composant colider leur permettant d'effectuer des intéractions physique les un les autres.

La sphére à un composant rigid body qui lui permet d'avoir des forces physiques qui lui sont appliqués.

### Chapitre3 : Light temps-réelles, skybox, ombre, reflexions, ambiant occlusion
La skybox à été créé à partir de différentes images.

### Chapitre4 : Coroutines
Les coroutines sont utilisés pour faires disparaitres les éléments sous lequels la sphére est passée.

### Chapitre5 : Canvas + interactions GUI, GameManager

### Chapitre6 : Shaders
Un surface shader à été utilisé pour faire varier la couleur des murs en fonctions de la position de balle.

### Chapitre7 : Blen tree, triggers

## Sources
* images de la skybox : https://assetstore.unity.com/packages/2d/textures-materials/sky/starfield-skybox-92717
* musique de fond : https://www.youtube.com/watch?v=8bOVEJk28pU&feature=youtu.be
* Les autres sont du jeu sont libre de droit et proviennent de : https://freesound.org/

