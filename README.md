# SelfLearnOpenGL
Follow the tutorial 《LearnOpenGL》 and do some exercises.

### Hints for me

- The output of the vertex shader requires the coordinates to be in clip-space.
- Your NDC coordinates will then be transformed to screen-space coordinates via the viewport transform using the data you provided with glViewport. **The resulting screen-space coordinates are then transformed to fragments as inputs to your fragment shader.**

- when you draw more than one objects, in order to make the normal vectors keep pointing at the right direction after the transformations, **you need to modify the normal matrix in the drawing loop every time you draw an object for the model matrix may change**, another solution is to calculate the normal matrix in vertex shader, but it will be slower.
- **Cubemap**: OpenGL treats it like an unit box, if you don't use any fancy instructions you will see that the rendering result is a small box. You have  to disable depth test when draw cubemap as skybox, because cubemap is so tiny and much closer to the camera compared to other objects. And remember cubemap is a 3D texture which needs three dimension coordinates to identify. 
- **Uniform Buffer Object**: If you have some uniform variables which won't change between several shaders, you can define them as uniform buffer object and only need to set them **once** in the OpenGL code. The way to use Uniform buffer object is a little different because you have to allocate the binding point which is a number  to every ubo. The binding points connect the shader and the ubo in GPU. One uniform buffer object can contain several variables so the function `glBufferSubData` will be used to copy the actual data to the buffer for it can define the exact offsets.
- In GLSL, `struct`  is quite different from `block`. The **block name** should be the same in the next shader, but the **instance name** in the current shader can be anything we like. As long as both interface block name are equal, their corresponding input and output is matched together.`gl_in[]` is the GLSL **built-in** variables. Don't  confuse it with another user-defined name.  
- **Geometry shader input varying variable must be declared as an array.**
- (?) I have one question in [Exploding objects](https://learnopengl.com/Advanced-OpenGL/Geometry-Shader), Joey used the cross-product to calculate the face normal, but the order of the two vectors he used will produce a direction vector pointing to the inside. But the final effect proved to be correct. I just can't understand that.
- A mesh contains several faces.**A Face represents a render primitive of the object(triangles,squares,points).** A face contains the indices of the vertices that form a primitive.
- The tangent space system will most likely vary for any two faces.
- Still don't understand Gamma-correction.
- When using shadow map, the acne caused by **Self-Occlusion** is due to the light direction which is not parallel to the surface normal and the limited resolution of the shadow map you use.
- **A framebuffer object is not complete without a color buffer**, so if we don't use it we need to explicitly tell OpenGL we're not going to render any color data. We do this by setting both the read and draw buffer to GL_NONE with glDrawBuffer and glReadbuffer.
- In framebuffer, the texture attachment is roughly the same as usual situations, the main differences is that we set the dimensions equal to the screen size(but this is not required) and **we pass NULL as the texture's data parameter which means we only allocating the memory and not actually filling data to it.**