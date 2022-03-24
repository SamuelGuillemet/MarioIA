![Mario](/Docs/Images/Super_MarioIA_Bros.png)
***
# Ici vous retrouverez les principales caractéristiques de Mario
## La méthode de `groundCheck` :
Pour vérifier que Mario est en contact avec le sol et pour tenter de reproduire le comportement du jeu SMB1, il faut que Mario puisse se tenir sur le bord d'un block sans tomber au sol. On utilise 2 points à ses pieds qui detecteront le contact avec tout éléments faisant parti du layer `Defaut` du jeu.  
```csharp
_grounded = Physics2D.OverlapPoint(m_GroundCheck1.position, LayerMask.GetMask("Default")) || Physics2D.OverlapPoint(m_GroundCheck2.position, LayerMask.GetMask("Default"));
```
## Les colliders de Mario 
Mario possède 2 colliders différents:
* Le premier est un `CircleCollider` utile pour que Mario ne se bloque pas sur les bords des blocs quand il court sur le sol du niveau.
* Le second est un `BoxCollider` utile pour donner une forme rectangulaire à Mario et de rentrer en collision avec les murs. Il a les angles arrondios pour ne pas se bloquer sur le bord des plateforme en hauteur, où sur le bord des tuyaux.

Chaqun de ces 2 Colliders possèdent le ``Material2D`` slippy pour glisser le long des surface, pour ne pas se bloquer sur les murs verticaux.
***
<img src="../Images/Mario.png" width="300" >

***
## Les animations
| Animation | Nom |
| :-------: | :-: |
| <img src="../Images/MarioCourse.gif" width="" > | La marche |
| <img src="../Images/MarioCrouch.gif" width="" > | Le crouch |
| <img src="../Images/MarioJump.gif" width="" > | Le saut |

## Les fonctions
  
#### [MoveMario](/Assets/Scripts/Mario.cs#L120-L152)
```csharp
/// <summary>
/// The function that handle the deplacement of Mario
/// </summary>
```
***
#### [SetConstant](/Assets/Scripts/Mario.cs#L157-L175)
```csharp
/// <summary>
/// The function that handle the different constant
/// </summary>
```
***
#### [LimitSpeed](/Assets/Scripts/Mario.cs#L180-L188)
```csharp
/// <summary>
/// Function that handle the maximum of speed based on <data> _currentInput </data>
/// </summary>
```
***
#### [AdjustGravity](/Assets/Scripts/Mario.cs#L194-L208)
```csharp
/// <summary>
/// The function that handle the custom gravity
/// </summary>
/// <returns> Returns the value of gravity which has to be divided by 9.81f </returns>
```
***
#### [AdjustAcceleration](/Assets/Scripts/Mario.cs#L214-L264)
```csharp
/// <summary>
/// The function taht handle the custom acceleration
/// </summary>
/// <returns> Return the value of the acceleration</returns>
```
***
