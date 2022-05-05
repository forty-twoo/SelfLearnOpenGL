# SelfLearnOpenGL
Follow the tutorial 《LearnOpenGL》 and do some exercises.

### Hints for me

- when you draw more than one objects, in order to make the normal vectors keep pointing at the right direction after the transformations, **you need to modify the normal matrix in the drawing loop every time you draw an object for the model matrix may change**, another solution is to calculate the normal matrix in vertex shader, but it will be slower.