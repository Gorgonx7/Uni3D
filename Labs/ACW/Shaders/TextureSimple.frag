//ACW FRAGMENT SHADER
#version 330


in vec4 oSurfacePosition;
in vec4 oNormal;

uniform vec4 uEyePosition;
uniform mat4 uView;

out vec4 FragColour;



uniform vec4 SceneAmbiance;



in vec2 oTexture;

uniform sampler2D uTexture0;


void main()
{
	
	//gl_FragColor.r = (diffusepow * light_diffuse_color.r * texcolor.r) + (specularpow * light_specular_color.r);
	//gl_FragColor.g = (diffusepow * light_diffuse_color.g * texcolor.g) + (specularpow * light_specular_color.g);
	//gl_FragColor.b = (diffusepow * light_diffuse_color.b * texcolor.b) + (specularpow * light_specular_color.b);
	//gl_FragColor.a = 1.0;

	 vec3 texturecolor = texture(uTexture0, oTexture).rgb;
	 gl_FragColor = vec4(texturecolor * totalLight, 1.0);
	 //gl_FragColor = vec4(texturecolor, 1.0);
	//gl_FragColor = vec4(texture(uTexture0, ofragTextCoord).rgb * totalLight, 1.0);
}