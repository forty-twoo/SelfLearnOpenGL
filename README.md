# SelfLearnOpenGL
Follow the tutorial 《LearnOpenGL》 and do some exercises.

### My notes and thoughts about the book

- Your NDC coordinates will then be transformed to screen-space coordinates via the viewport transform using the data you provided with glViewport. **The resulting screen-space coordinates are then transformed to fragments as inputs to your fragment shader.**

- **gl_FragCoord VS FragPos**: they are quite different concepts, don't confuse with it.`gl_FragCoord` is a built-in variable in shader while `FragPos` is just a user-defined variable. 

- When we output a clip-space vertex position to gl_Position in the vertex shader, OpenGL automatically does a perspective divide **e.g. transform clip-space coordinates in the range [-w,w] to [-1,1] by dividing the x,y,z component by the vector's w component.**

- **Depth Testing (I can't guaranteed the correctness, just my thoughts)**

    - First you should be familiar with the process of perspective matrix transformation. If not, read book 《Fundamentals of Computer Graphics》Page161 to Page168 again. 

    - The z coordinate of a point in View/Camera space is linear which means the z value is proportional to the point's actual distance to the camera. But when it's applied by perspective matrix transformation, the new z value will be not linear to it's actual distance anymore. The projection matrix transforms coordinates within the specialized range (viewing frustum) to normalized device coordinates [-1,1]. 

    -  Then the built-in variable `gl_FragCoord` : x and y components of gl_FragCoord represent the fragment's screen-space coordinates (with (0,0) being the bottom-left corner). The gl_FragCoord variable also contains a z-component which contains the depth value of the fragment. This z value is the value that is compared to the depth buffer's content and it's within range [0,1] (ndc.z/2+1).

    -  If we visualize depth buffer using the gl_FragCoord in fragment shader:

        ` FragColor = vec4(vec3(gl_FragCoord.z), 1.0);` 

        you may notice that every object in scene is white, because the gl_FragCoord.z is very closer to 1.0. The non-linear depth value after perspective transformation causes the problem.  Since the non-linear function is proportional to 1/z( according to the matrix calculation), z-values between `1.0` and `2.0` would result in depth values between `1.0` and `0.5` which is half of the [0,1] range, giving us enormous precision at small z-values. Z-values between `50.0` and `100.0` would account for only 2% of the [0,1] range.

    - **We can transform the non-linear depth values of the fragment back to  its linear values. This means we have to first re-transform the depth values from the range `[0,1]` to normalized device coordinates in the range `[-1,1]`. Then we want to reverse the non-linear equation as done in the projection matrix and apply this inversed equation to the resulting depth value. The result is then a linear depth value.**

             ```c
             #version 330 core
             out vec4 FragColor;
             
             float near = 0.1; 
             float far  = 100.0; 
               
             float LinearizeDepth(float depth) 
             {
                 float z = depth * 2.0 - 1.0; // back to NDC 
                 return (2.0 * near * far) / (far + near - z * (far - near));	
             }
             
             void main()
             {             
                 float depth = LinearizeDepth(gl_FragCoord.z) / far; // divide by far for demonstration
                 FragColor = vec4(vec3(depth), 1.0);
             }
             ```

        

- when you draw more than one objects, in order to make the normal vectors keep pointing at the right direction after the transformations, **you need to modify the normal matrix in the drawing loop every time you draw an object for the model matrix may change**, another solution is to calculate the normal matrix in vertex shader, but it will be slower.
- **Cubemap**: OpenGL treats it like an unit box, if you don't use any fancy instructions you will see that the rendering result is a small box. You have  to disable depth test when draw cubemap as skybox, because cubemap is so tiny and much closer to the camera compared to other objects. And remember cubemap is a 3D texture which needs three dimension coordinates to identify. 
- <img src="https://raw.githubusercontent.com/forty-twoo/SelfLearnOpenGL/master/images/CubeMap.png" width="500">
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
- (?)A weired bug, I bind the shadowMap instead of shadowMapFBO to the binding point by accident, then I bind back the default framebuffer(0) before rendering, After the render loop, (the code in the loop is just to present a red color or anything you like, but not using the wrong framebuffer), the screen is all black which is weird because I bind the correct default framebuffer back.
- In chapter Shadow_Mapping, the **Over Sampling** part, there are two reasons to produce the redundant shadows in the floor:
    -  1.Suppose a fragment of the floor has x/y coordinates in LightSpace beyond [0,1], the depth value it will sample depends on the ShadowMap wrapping method. So we could solve the problem with setting the wrapping method to GL_CLAMP_BORDER, and then make the border depth value 1.0; 
    -  2.Apart from x/y coordinates, the z coordinate in LightSpace might as well beyond [0,1] which means the fragment is outside the far-plane of the light's orthographic frustum. It's z value is always larger than 1.0 so our code will treat this fragment as it's in shadow and the wrapping method doesn't work for this situation. We can force the shadow value to 0.0 whenever the projected vector's z coordinate is larger than 1.0.