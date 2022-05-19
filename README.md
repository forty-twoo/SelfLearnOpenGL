# SelfLearnOpenGL
Follow the tutorial 《LearnOpenGL》 and do some exercises.

### Hints for me

- when you draw more than one objects, in order to make the normal vectors keep pointing at the right direction after the transformations, **you need to modify the normal matrix in the drawing loop every time you draw an object for the model matrix may change**, another solution is to calculate the normal matrix in vertex shader, but it will be slower.
- Cubemap: OpenGL treats it like an unit box, if you don't use any fancy instructions you will see that the rendering result is a small box. You have  to disable depth test when draw cubemap as skybox, because cubemap is so tiny and much closer to the camera compared to other objects. And remember cubemap is a 3D texture which needs three dimension coordinates to identify. 

