//ACW FRAGMENT SHADER
#version 330


in vec4 oSurfacePosition;
in vec4 oNormal;

uniform vec4 uEyePosition;
uniform mat4 uView;

out vec4 FragColour;

struct LightProperties { 
	 vec4 position;
	 vec4 ambinat;
     vec4 diffuse;
     vec4 specular; 
     float constantAttenuation, linearAttenuation, quadraticAttenuation;
     float spotCutoff, spotExponent; 
     vec3 spotDirection;
};
uniform LightProperties uLight[3];
uniform vec4 SceneAmbiance;

struct MaterialProperties 
	{ 
		vec4 Emissive;
		vec4 AmbientReflectivity; 
		vec4 DiffuseReflectivity; 
		vec4 SpecularReflectivity; 
		float Shininess; 
		};

uniform MaterialProperties uMaterial;

in vec2 oTexture;

uniform sampler2D uTexture0;


void main()
{
	vec4 viewDirection = normalize(uEyePosition - oSurfacePosition);
	
	vec4 lightDir;
	float attenuation;
	vec3 totalLight = vec3(SceneAmbiance * uMaterial.AmbientReflectivity);
	vec3 lightDirection;
	for(int i = 0; i < 3; ++i)
	{
		if(uLight[i].position.w == 0.0)
		{
			// directional light
			attenuation = 1.0;
			lightDirection = normalize(vec3(uLight[i].position));
		} 
		else // point or spotlight
		{
			vec3 positionToLightSource = vec3(uLight[i].position - oSurfacePosition);
			float distance = length(positionToLightSource);
			lightDirection = normalize(positionToLightSource);
			attenuation = 1.0 / (uLight[i].constantAttenuation
			       + uLight[i].linearAttenuation * distance
			       + uLight[i].quadraticAttenuation * distance * distance);
			if (uLight[i].spotCutoff <= 90.0) // spotlight?
			{
				float clampedCosine = max(0.0, dot(-lightDirection, normalize(uLight[i].spotDirection)));
					if (clampedCosine < cos(radians(uLight[i].spotCutoff))) // outside of spotlight cone?
					{
						attenuation = 0.0;
					}
					else
					{
						 attenuation = attenuation * pow(clampedCosine, uLight[i].spotExponent);   
					}
			}
		}
		vec3 diffuseReflection = attenuation * vec3(uLight[i].diffuse) * vec3(uMaterial.DiffuseReflectivity) * max(0.0, dot(vec3(oNormal), lightDirection));
		vec3 specularReflection;
		vec3 holder = normalize(vec3(oNormal));
		if (dot(holder, lightDirection) < 0.0) // light source on the wrong side?
		{
			specularReflection = vec3(0.0, 0.0, 0.0); // no specular reflection
		}
		else // light source on the right side
		{
			specularReflection = attenuation * vec3(uLight[i].specular) * vec3(uMaterial.SpecularReflectivity) * pow(max(0.0, dot(reflect(-lightDirection, holder), vec3(viewDirection))), uMaterial.Shininess);
		}
		vec3 AmbinatReflection = vec3(uLight[i].ambinat * uMaterial.AmbientReflectivity);
		totalLight = totalLight + AmbinatReflection + diffuseReflection + specularReflection;	
	}
	

	 vec3 texturecolor = texture(uTexture0, oTexture).rgb;
	 gl_FragColor = vec4(texturecolor * totalLight, 1.0);
	
}