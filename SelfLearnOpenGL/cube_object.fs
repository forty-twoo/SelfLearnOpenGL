#version 330 core

struct Material{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
};

struct  Light{
	vec3 position;
	vec3 direction;

	vec3 diffuse;
	vec3 specular;
	vec3 ambient;

	float cutOff;
	float outerCutOff;

	float constant;
	float linear;
	float quadratic;

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

	//remember to normalize 
	float theta=dot(viewDir,normalize(-light.direction));
	float epsilon=light.cutOff-light.outerCutOff;
	float intensity=clamp((theta-light.outerCutOff)/epsilon,0.0,1.0);
	diffuse*=intensity;
	specular*=intensity;

	float distance=length(light.position-FragPos);
	float attenuation=1.0f/(light.constant+light.linear*distance+light.quadratic*(distance*distance));
	ambient*=attenuation;
	diffuse*=attenuation;
	specular*=attenuation;

	FragColor=vec4(ambient+diffuse+specular,1.0f);
}