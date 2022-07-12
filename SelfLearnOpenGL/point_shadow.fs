#version 330 core

in VS_OUT{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
}fs_in;

uniform sampler2D diffuseTexture;
uniform samplerCube depthMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

uniform float far_plane;
uniform bool shadows;

out vec4 FragColor;

float ShadowCalculation(vec3 fragPos)
{
	vec3 fragToLight=fragPos-lightPos;
	float closestDepth=texture(depthMap,fragToLight).r;
	float currentDepth=length(fragToLight);
	float shadow  = 0.0;
	float bias    = 0.05; 
	float samples = 4.0;
	float offset  = 0.1;
	for(float x = -offset; x < offset; x += offset / (samples * 0.5))
	{
	    for(float y = -offset; y < offset; y += offset / (samples * 0.5))
	    {
	        for(float z = -offset; z < offset; z += offset / (samples * 0.5))
	        {
	            float closestDepth = texture(depthMap, fragToLight + vec3(x, y, z)).r; 
	            closestDepth *= far_plane;   // undo mapping [0;1]
	            if(currentDepth - bias > closestDepth)
	                shadow += 1.0;
	        }
	    }
	}
	shadow /= (samples * samples * samples);
	return shadow;
}

void main()
{
	vec3 color=texture(diffuseTexture,fs_in.TexCoords).rgb;
	vec3 normal=normalize(fs_in.Normal);
	vec3 lightColor=vec3(0.6);
	vec3 ambient=0.3*lightColor;

	vec3 lightDir=normalize(lightPos-fs_in.FragPos);
	float diff=max(dot(lightDir,normal),0.0f);
	vec3 diffuse=diff*lightColor;

	vec3 viewDir=normalize(viewPos-fs_in.FragPos);
	vec3 halfwayDir=normalize(viewDir+lightDir);
	float spec=max(dot(halfwayDir,normal),0.0);
	spec=pow(spec,64.0);
	vec3 specular=spec*lightColor;

	float shadow=shadows?ShadowCalculation(fs_in.FragPos):0.0f;
	vec3 lighting =(ambient+(1.0-shadow)*(diffuse+specular))*color;
	FragColor=vec4(lighting,1.0f);
}

