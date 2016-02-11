#version 150 core
 
in vec4 in_Position;
in vec4 in_Color;
in vec2 in_textCoord2D;
 
out vec4 pass_Color;
out vec2 out_textCoord2D;
 
void main(void) {
    gl_Position = in_Position;

    pass_Color = in_Color;
    out_textCoord2D = in_textCoord2D;
}
