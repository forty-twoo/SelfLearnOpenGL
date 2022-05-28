# SelfLearnOpenGL
Follow the tutorial 《LearnOpenGL》 and do some exercises.

### Hints for me

- when you draw more than one objects, in order to make the normal vectors keep pointing at the right direction after the transformations, **you need to modify the normal matrix in the drawing loop every time you draw an object for the model matrix may change**, another solution is to calculate the normal matrix in vertex shader, but it will be slower.
- **Cubemap**: OpenGL treats it like an unit box, if you don't use any fancy instructions you will see that the rendering result is a small box. You have  to disable depth test when draw cubemap as skybox, because cubemap is so tiny and much closer to the camera compared to other objects. And remember cubemap is a 3D texture which needs three dimension coordinates to identify. 
- **Uniform Buffer Object**: If you have some uniform variables which won't change between several shaders, you can define them as uniform buffer object and only need to set them **once** in the OpenGL code. The way to use Uniform buffer object is a little different because you have to allocate the binding point which is a number  to every ubo. The binding points connect the shader and the ubo in GPU. One uniform buffer object can contain several variables so the function `glBufferSubData` will be used to copy the actual data to the buffer for it can define the exact offsets.
- In GLSL, `struct`  is quite different from `block`. The **block name** should be the same in the next shader, but the **instance name** in the current shader can be anything we like. As long as both interface block name are equal, their corresponding input and output is matched together.`gl_in[]` is the GLSL **built-in** variables. Don't  confuse it with another user-defined name.  
- **Geometry shader input varying variable must be declared as an array.**
- (?) I have one question in [Exploding objects](https://learnopengl.com/Advanced-OpenGL/Geometry-Shader), Joey used the cross-product to calculate the face normal, but the order of the two vectors he used will produce a direction vector pointing to the inside. But the final effect proved to be correct. I just can't understand that.
- *I spent two days trying to understand Gamma-Correction, but failed. Maybe I should learn it after I know more about image processing.*
