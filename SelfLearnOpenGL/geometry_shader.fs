#version 330 core

uniform sampler2D texture_diffuse1;
in vec2 TextCoords;
out vec4 FragColor;

void main()
{
	FragColor=texture(texture_diffuse1,TextCoords);
}
