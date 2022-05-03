#version 330 core
  
uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 viewPos;
uniform vec3 lightPos;

in vec3 Norm;
in vec3 FragPos;
out vec4 FragColor;

void main()
{

	// All in world-coordinates
	vec3 lightDir=normalize(lightPos-FragPos);
	vec3 viewDir=normalize(viewPos-FragPos);

	float diffuse=max(dot(lightDir,Norm),0);

	float ambientStrength=0.1f;
	float specularStrength=0.5f;

	vec3 reflectlight=normalize(reflect(-lightDir,Norm));
	float spec=pow(max(dot(reflectlight,viewDir),0.0f),128);
	
	vec3 aLight=ambientStrength*lightColor+specularStrength*spec*lightColor+diffuse*lightColor;
	FragColor=vec4(aLight*objectColor,1.0);

}