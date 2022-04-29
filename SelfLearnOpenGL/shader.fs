#version 330 core

uniform sampler2D myTexture0;
uniform sampler2D myTexture1;

in vec2 Textcoor;
in vec3 myColor;
out vec4 FragColor;

void main(){
	//FragColor=texture(myTexture1,Textcoor);
	FragColor = mix(texture(myTexture0,Textcoor),texture(myTexture1,Textcoor),0.2);
	FragColor = FragColor * vec4(myColor,1.0);
}