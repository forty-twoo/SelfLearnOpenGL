#version 330 core

struct Material{
	vec3 diffuse;
	vec3 specular;
	vec3 ambient;
	float shininess;
};

struct  Light{
	vec3 position;
	vec3 diffuse;
	vec3 specular;
	vec3 ambient;
};

uniform Material material;
uniform Light light;
  
uniform vec3 viewPos;

in vec3 Norm;
in vec3 FragPos;
out vec4 FragColor;

void main()
{

	// All in world-coordinates
	vec3 lightDir=normalize(light.position-FragPos);
	vec3 viewDir=normalize(viewPos-FragPos);

	float diff=max(dot(lightDir,Norm),0);


	vec3 reflectlight=normalize(reflect(-lightDir,Norm));
	float spec=pow(max(dot(reflectlight,viewDir),0.0f),128);

	vec3 ambient=material.ambient*light.ambient;
	vec3 diffuse=diff*material.diffuse*light.diffuse;
	vec3 specular=spec*material.specular*light.specular;

	FragColor=vec4(ambient+diffuse+specular,1.0);

}