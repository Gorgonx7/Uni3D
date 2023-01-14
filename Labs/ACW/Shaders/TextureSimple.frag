//ACW FRAGMENT SHADER
#version 330

out vec4 FragColour;
in vec2 oTexture;

uniform sampler2D uTexture0;


void main()
{
	
	

	 vec3 texturecolor = texture(uTexture0, oTexture).rgb;
	 gl_FragColor = vec4(texturecolor, 1.0);
}