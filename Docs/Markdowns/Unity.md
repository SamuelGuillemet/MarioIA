![Mario](/Docs/Images/Super_MarioIA_Bros.png)
***
# Dans cette partie vous trouverez les explications relatives aux différents éléments d'Unity
## Layer et Tag :
Unity utilise le système de Layer et de Tag, qui sera beaucoup utilisé dans ce projet. On a :
| Tag | Description |
| :-: | :---------- |
| `Enemy` | Lorsque Mario rentre en contact avec un élément ayant ce tag, il meurt |
| `Platform` | Tag pour les éléments qui constituent le sol du niveau |
| `Coin` | Tag associé aux pièces, permet à Mario de savoir qu'il peut récupérer l'élément avec lequel il rentre en contact |
| `Destination` | Permet aux ennemis de savoir qu'ils peuvent rebondir sur la surface avec laquelle il rentre en contact, est mis sur les blocks du jeu |
| `Border` | Permet aux ennemies de saoir quand le bord d'une plateforme apparaît pour changer de direction |
| `Checkpoint` | Tout les checkpoints ont ce tag, permet à l'`Agent` de savoir quand il rentre en collision avec un checkpoint |
| `Flag` | Tag donné au drapeau de fin de niveau |
| `PlatefromUp` | Tag sur les blocks particuliers qui sont support de `HammerBros`, permet le passage à travers les blocks |
| `PlateformDown` | Tag sur les blocks particuliers qui sont support de `HammerBros`, permet le passage à travers les blocks |
***
| Layer | Description |
| :---: | :---------- |
| `Default` | Ce layer regroupe presque l'ensemble des éléments du jeu |
| `VisionCamera` | Ce layer contient des images en couleurs pour le `CameraSensor` |
| `Checkpoints` | Ce layer contient tout les éléments visuels qui ne doivent pas entrer en contact la détection du sol pour le saut de Mario |
***
Les `Sorting Layers` sont utile pour afficher la décoration du niveau en arrière plan, ou les tuyaux au premier plan du jeu.
* `Background`
* `Default`
* `Front` 
## Les composants principaux d'Unity:
### **[RigidBody2D](https://docs.unity3d.com/Manual/class-Rigidbody2D.html)**
<img src="https://docs.unity3d.com/uploads/Main/Rigidbody2D.png" width="500">
  
Permet à un objet de subir la gravité et d'être sous la simulation physique d'Unity. Contient les informations de vitesse de l'objet.
***
### **[CircleCollider2D](https://docs.unity3d.com/Manual/class-CircleCollider2D.html)** et **[BoxCollider2D](https://docs.unity3d.com/Manual/class-BoxCollider2D.html)**
<img src="https://docs.unity3d.com/uploads/Main/BoxCollider2DInspector.png" width="500"> <img src="https://docs.unity3d.com/uploads/Main/CircleCollider2DInspector.png" width="500">
  
Permet à un objet de subir des collision avec d'autres `Collider2D`. Prend la forme d'un cercle ou d'un rectangle.  
A chaque contact les fonctions suivantes sont appelées:
````C#
OnCollisionEnter2D(Collision2D other) {} 
OnCollisionExit2D(Collision2D other) {} 
OnCollisionStay2D(Collision2D other) {} 
````
`IsTrigger` permet à l'objet de rentrer en collision avec d'autres objet sans influence physique; a chaque contact appelle les fonctions suivantes:
````C#
OnTriggerEnter2D(Collider2D other) {}  
OnTriggerExit2D(Collider2D other) {}
OnTriggerStay2D(Collider2D other) {}
````
***
### **[Animator](https://docs.unity3d.com/Manual/class-Animator.html)**
<img src="https://docs.unity3d.com/uploads/Main/MecanimAnimatorComponent.png" width="500">
  
Permet d'animer un objet sous Unity.
***
### **[SpriteRenderer](https://docs.unity3d.com/Manual/class-SpriteRenderer.html)**
<img src="https://docs.unity3d.com/uploads/Main/2D_SpriteRenderer_1.png" width="500">
  
Permet d'associer un sprite à un objet sous Unity.