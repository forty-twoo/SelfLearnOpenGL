#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

float near=0.1;
float far=100.0;

uniform sampler2D texture1;

float LinearizeDepth(float depth){
	float ndcz=depth*2.0-1.0;
	return (2.0*near*far)/(far+near-ndcz*(far-near));
}

void main()
{    
    //FragColor = texture(texture1, TexCoords);

	float depth=LinearizeDepth(gl_FragCoord.z)/far;
	FragColor=vec4(vec3(depth),1.0f);
}