#version 330 core

struct Material{
	sampler2D diffuse;
	sampler2D specular;
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
uniform float time;

in vec3 Norm;
in vec3 FragPos;
in vec2 TextCoors;
out vec4 FragColor;

void main()
{

	// All in world-coordinates
	vec3 lightDir=normalize(light.position-FragPos);
	vec3 viewDir=normalize(viewPos-FragPos);

	float diff=max(dot(lightDir,Norm),0);


	vec3 reflectlight=normalize(reflect(-lightDir,Norm));
	float spec=pow(max(dot(reflectlight,viewDir),0.0f),128);

	
	vec3 ambient=light.ambient*vec3(texture(material.diffuse,TextCoors));
	vec3 diffuse=diff*light.diffuse*vec3(texture(material.diffuse,TextCoors));
	vec3 specular=spec*light.specular*vec3(texture(material.specular,TextCoors));

	FragColor=vec4(ambient+diffuse+specular,1.0f);

}