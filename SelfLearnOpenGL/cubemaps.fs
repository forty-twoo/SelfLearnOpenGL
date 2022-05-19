#version 330 core
out vec4 FragColor;

in vec3 fragPos;
in vec3 Norm;

uniform vec3 camPos;
uniform samplerCube skybox;

void main()
{    
	vec3 camDir=normalize(fragPos-camPos);
	vec3 outDir=reflect(camDir,normalize(Norm));

	FragColor=vec4(texture(skybox,outDir).rgb,1.0f);
}