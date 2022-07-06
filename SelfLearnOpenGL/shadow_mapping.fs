#version 330 core

out vec4 FragColor;

in VS_OUT{
	vec3 fragPos;
	vec4 fragPosInLightSpace;
	vec3 normal;
	vec2 texCoords;
}fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

float ShadowCalculation(vec4 fragPosInLightSpace,vec3 lightDir,vec3 normal){

	vec3 projCoords=fragPosInLightSpace.xyz/fragPosInLightSpace.w;
	projCoords=projCoords*0.5+0.5f;
	float closetDepth=texture(shadowMap,projCoords.xy).r;
	float currentDepth=projCoords.z;
	float bias=max(0.005,0.05*(1.0f-dot(lightDir,normal)));
	float shadow=0.0;

	vec2 texelSize=1.0/textureSize(shadowMap,0);
	for(int x=-1;x<=1;++x){
		for(int y=-1;y<=1;++y){
			float pcfDepth=texture(shadowMap,projCoords.xy+vec2(x,y)*texelSize).r;
			shadow+=currentDepth>(pcfDepth+bias)?1.0:0.0;
		}
	}
	shadow/=9.0f;
	
	if(projCoords.z>1.0f)
		shadow=0.0;
	return shadow;
}

void main()
{
	vec3 color=texture(diffuseTexture,fs_in.texCoords).rgb;
	vec3 normal=normalize(fs_in.normal);
	vec3 lightColor=vec3(0.3f);
	vec3 lightDir=normalize(lightPos-fs_in.fragPos);
	vec3 ambient=0.3*lightColor;

	float diff=max(dot(lightDir,normal),0.0f);
	vec3 diffuse=diff*lightColor;

	vec3 reflectDir=reflect(-lightDir,normal);
	vec3 halfDir=normalize(reflectDir+lightDir);
	float spec=0.0f;
	spec=pow(max(dot(halfDir,normal),0.0f),64.0);
	vec3 specular=spec*lightColor;

	float shadow=ShadowCalculation(fs_in.fragPosInLightSpace,lightDir,normal);
	vec3 lighting=ambient+(1.0f-shadow)*(diffuse+specular);
	FragColor=vec4(lighting*color,1.0f);
}


