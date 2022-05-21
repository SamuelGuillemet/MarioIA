![Mario](/Docs/Images/Super_MarioIA_Bros.png)
***
# Under MLAgent Stuff:
* `Checkpoint` : Script qui gère l'élément checkpoint pour l'IA et le système de reward.
* `Environment` : Le script parent de chaque niveau qui gère le système de checkpoints, de reset et l'assignation des variables pour chaque script enfant.
* `MLAgent` : Le script qui fournit le comportement de l'Agent.

***
# Other stuff
* `Bill` : Gestion du bloc qui tire les billbullets.
* `BillBullet` : Les munitions du lanceur Bill.
* `Blocks` : Script qui donne le comportement des blocs, prend aussi en charge la mort des ennemies sur le dessus des blocs quand mario tape par le dessous.
* `Coin` : If mario collects the coin, it will be destroyed.
* `DestroyAfterAnimation` : Détruit me gameObject après la fin de son animation.
* `Enemy` : Script global aux ennemis qui donne le comportement pours les ennemis `Goomba`, `Koopa`, `Hammerbros`, `BuzzyBeetle`.
* `Hammerbros` : Script qui fournit le comportement de l'ennemi HammerBros.
* `MainCamera` : Script qui permet à la caméra de suivre Mario et de reproduire le comportement de SMB, gère aussi la taille de la fenêtre.
* `Mario` : Script qui permet de déplacer Mario, fonctionne avec la physique similaire à celle de SMB, accélération et gravité custom, et gestion du trampoline.
* `Pipe` : Script qui s'occupe de faire spawn la plante et de téléporter Mario lorsqu'il s'accroupit sur le pipe.
* `Plante` : Tue mario en cas de collision.



