![Mario](/Docs/Images/Super_MarioIA_Bros.png)
***
# Les touches 
`Left Arrow` : Aller à gauche  
`Right Arrow` : Aller à droite  
`Up Arrow` : Sauter, un appui long fait sauter plus haut  
`Down Arrow` : Permet à mario de s'accroupir pour passer dans les tuyaux. **/!\ N'est pas disponible comme action de l'Agent /!\\**  
`Space` : Permet à mario de sprinter
***
# Le fonctionnement des déplacements
Le travail de recherche sur la physique se base en grande partie sur ce [site](https://drive.google.com/file/d/1IAZIW48kDXfpbrIQjIvmYxfSTMVZgTui/view?usp=sharing).  
Unity est un moteur de jeu capable de simuler la physique, nous allons donc grandement nous en servir pour alimenter notre jeu.
## L'accélération horizontale :
### Au sol :
| Type | Vitesse Max | Accélération |
| :--: | :---------: | :----------: |
| `Marche `| 5.85 m/s | 8.35 m/s² |
| `Course` | 9.6 m/s | 12.52 m/s² |
| `Release Direction` | - | -11.42 m/s² (Décélération) |
| `Opposite Direction` | - | -22.85 m/s² (Décélération) |
### En l'air:
Appuyer sur la touche de **Sprint** n'a pas d'effet sur les accélérations en l'air.
| Type |  Accélération |
| :--: | :----------: |
| `Marche `| 8.35 m/s² |
| `Course` | 12.52 m/s² |
| `Release Direction` | 0 m/s² (Release direction has no effect on speed) |
| `Opposite Direction` - Vitesse actuelle < **Marche** | -10 m/s² (Décélération) |
| `Opposite Direction` - Vitesse actuelle > **Marche**| -12.52 m/s² (Décélération) |
  
## La gravité et le saut:
Lorsque l'on tient le bouton saut enfoncé, on modifie la gravité qui s'applique à mario, c'est cela qui donne l'effet à mario de "flotter".
### Les vitesses de départ du saut:
Les vitesses initiales du saut dépendent de la vitesse horizontale du joueur au début de son saut.
| Type | Vitesse initiale |
| :--: | :--------------: |
| `Vitesse < 3.5 m/s` | 15 m/s |
| `Vitesse < 5.85 m/s` | 15 m/s |
| `Vitesse > 5.85 m/s` | 18.75 m/s |
### La gravité quand le bouton saut est enfoncé :
La gravité dépend de la vitesse horizontale du joueur au début de son saut.
| Type | Gravité |
| :--: | :--------------: |
| `Vitesse < 3.5 m/s` | 28.125 m/s² |
| `Vitesse < 5.85 m/s` | 26.36 m/s² |
| `Vitesse > 5.85 m/s` | 35.1 m/s² | 
### La gravité quand le bouton saut est relâché ou lorsque le joueur retombe :
La gravité dépend de la vitesse horizontale du joueur au début de son saut.
| Type | Gravité |
| :--: | :--------------: |
| `Vitesse < 3.5 m/s` | 98 m/s² |
| `Vitesse < 5.85 m/s` | 84 m/s² |
| `Vitesse > 5.85 m/s` | 126.5 m/s² |  

**/!\ Pour prévenir d'une vitesse de chute trop élevée, la vitesse de chute est limitée à 16.2 m/s /!\\**
  
## Autre :
| Type | Vitesse initiale |
| :--: | :--------------: |
| `Trampoline` | 26.25 m/s |
| `Bounce sur un ennemi` | 15 m/s |