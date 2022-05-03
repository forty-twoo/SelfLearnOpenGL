#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNorm;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat3 normatrix;

out vec3 Norm;
out vec3 FragPos;

void main()
{
	
	//Norm=aNorm;
	Norm=normalize(normatrix*aNorm);
	
	FragPos=vec3(model*vec4(aPos,1.0f));
	gl_Position = projection * view * model * vec4(aPos, 1.0);
}