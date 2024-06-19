# Bullet-Hell-Game

| <u>**Table of Contents**</u> |
| ----------------- |
| <u>***[About](#about)***</u> |
| <u>***[Installation](#installation)***</u> |
| <u>***[Design](#design)***</u> <ul><li>[Collisions](#collisions)</li><li>[Managing Entities](#managing-entities)</li><li>[Projectiles](#projectiles)</li></ul> |

## About

This game is a steampunk themed arcade shoot 'em up. The game design document explaining the concept and basic functionality of the game can be found [here](https://docs.google.com/document/d/1aVQhy6VemEaqebC-rwQucTiyn8EiKEAMLSkEYDBvMpk/edit?usp=sharing).
Additionally, any information on current goals be found the document.
However, the game is currently W.I.P. and many features may not yet be implemented.

## Installation

You can install the game by downloading the [latest release](https://github.com/WhyNot180/Bullet-Hell-Game/releases), extracting it, and then running the file titled "Bullet-Hell"

## Design

### Collisions

Collisions are the back-bone of any shoot 'em up, so it's important to get them right.
Thus, the game implements [Separating Axis Theorem (SAT)](https://en.wikipedia.org/wiki/Hyperplane_separation_theorem) collision detection.

SAT states that a collision will only occur if a line cannot be drawn between two shapes, thus if we can figure out whether this can be done, then we can detect collisions.
In order to do this we must first find axes on which the shapes can be projected (showing overlap if colliding).

Projecting a shape is exactly as it sounds, it simply gets a "shadow" of a shape on a line as so:
![](https://upload.wikimedia.org/wikipedia/commons/9/9b/Separating_axis_theorem2008.png)

Now an obvious question is how to find the separating axes to project on, but there is a simple answer: each axis is a perpendicular line from each face of a shape.
Thus, by finding the perpendiculars to each side of both shapes, we can find the axes to project on and find any overlap.

Unfortunately, this does not work for concave shapes as seen below, but luckily the game does not yet need to make use of any concave shapes:
![](https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Separating_axis_theorem2.svg/1024px-Separating_axis_theorem2.svg.png)

Of course this is great, but it can be computationally expensive if there are many collision checks every cycle, so the game implements [quadtrees](https://en.wikipedia.org/wiki/Quadtree) as well.

A quadtree is simply a tree data structure with 4 child nodes, but this simple structure can be used to recursively split an area into quadrants, allowing the game drastically reduce computational intensity by only checking for collisions between objects in the same quadrant.

A visualization can be seen below (note: the gif has drastically reduced resolution and frame-rate from the original):
![](https://i.sstatic.net/6cQeQ.gif)

### Managing Entities

### Projectiles