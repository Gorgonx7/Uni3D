#version 330 core
uniform sampler2D t;
in vec2 textureCoord;
void main() {
   vec4 color1 = texture2D(t,textureCoord);
   gl_FragColor = color1;
}