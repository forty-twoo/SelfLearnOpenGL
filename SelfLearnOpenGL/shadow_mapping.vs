#version 330 core
layout (location=0) in vec3 aPos;
layout (location=1) in vec3 aNormal;
layout (location=2) in vec2 aTexCoords;

out vec2 TexCoords;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;
uniform mat4 lightSpaceMatrix;

out VS_OUT{
	vec3 fragPos;
	vec4 fragPosInLightSpace;
	vec3 normal;
	vec2 texCoords;
}vs_out;

void main()
{
	vs_out.fragPos=vec3(model*vec4(aPos,1.0f));
	vs_out.normal=transpose(inverse(mat3(model)))*aNormal;
	vs_out.texCoords=aTexCoords;
	vs_out.fragPosInLightSpace=lightSpaceMatrix*model*vec4(aPos,1.0f);

	gl_Position=projection*view*model*vec4(aPos,1.0f);
}