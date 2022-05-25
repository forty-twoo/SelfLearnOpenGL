#version 330 core
layout (triangles) in;
layout (triangle_strip, max_vertices=3) out;

uniform float time;

in VS_OUT{
	vec2 textCoords;
}gs_in[];

out vec2 TextCoords;

vec3 GetNormal()
{
	vec3 a=vec3(gl_in[2].gl_Position)-vec3(gl_in[1].gl_Position);
	vec3 b=vec3(gl_in[0].gl_Position)-vec3(gl_in[1].gl_Position);
	return normalize(cross(b,a));
}

vec4 Explode(vec4 position,vec3 normal)
{
	vec3 direction=normal*((sin(time)+1.0)/2.0f)*2.0f;
	return position+vec4(direction,0.0f);
}

void main()
{
	vec3 Normal=GetNormal();
	gl_Position=Explode(gl_in[0].gl_Position,Normal);
	TextCoords=gs_in[0].textCoords;
	EmitVertex();

	gl_Position=Explode(gl_in[1].gl_Position,Normal);
	TextCoords=gs_in[1].textCoords;
	EmitVertex();

	gl_Position=Explode(gl_in[2].gl_Position,Normal);
	TextCoords=gs_in[2].textCoords;
	EmitVertex();

	EndPrimitive();
}