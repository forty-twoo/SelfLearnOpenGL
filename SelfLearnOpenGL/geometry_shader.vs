#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out VS_OUT{
	vec3 Normal;
}vs_out;


void main()
{
	mat3 normalMatrix=mat3(transpose(inverse(view*model)));
	vs_out.Normal=normalMatrix*aNormal;
	gl_Position=view*model*vec4(aPos,1.0f);
}
