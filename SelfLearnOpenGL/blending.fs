#version 330 core

in vec2 TextCoords;
uniform sampler2D grassT;

out vec4 FragColor;

void main()
{
	vec4 texColor=texture(grassT,TextCoords);
	if(texColor.a<0.1){
		discard;
	}
	FragColor=texColor;
}