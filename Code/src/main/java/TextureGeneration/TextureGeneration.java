package TextureGeneration;

import Util.ShaderLoader;
import Util.TexturedVertex;
import java.nio.ByteBuffer;
import java.nio.FloatBuffer;
import org.lwjgl.BufferUtils;
import org.lwjgl.LWJGLException;
import org.lwjgl.opengl.ContextAttribs;
import org.lwjgl.opengl.Display;
import org.lwjgl.opengl.DisplayMode;
import org.lwjgl.opengl.GL11;
import static org.lwjgl.opengl.GL11.GL_COLOR_BUFFER_BIT;
import static org.lwjgl.opengl.GL11.GL_FLOAT;
import static org.lwjgl.opengl.GL11.GL_NO_ERROR;
import static org.lwjgl.opengl.GL11.GL_TRIANGLES;
import static org.lwjgl.opengl.GL11.GL_UNSIGNED_BYTE;
import static org.lwjgl.opengl.GL11.glClear;
import static org.lwjgl.opengl.GL11.glClearColor;
import static org.lwjgl.opengl.GL11.glDeleteTextures;
import static org.lwjgl.opengl.GL11.glDrawElements;
import static org.lwjgl.opengl.GL11.glGetError;
import static org.lwjgl.opengl.GL11.glViewport;
import static org.lwjgl.opengl.GL13.GL_TEXTURE0;
import static org.lwjgl.opengl.GL15.GL_ARRAY_BUFFER;
import static org.lwjgl.opengl.GL15.GL_ELEMENT_ARRAY_BUFFER;
import static org.lwjgl.opengl.GL15.GL_STATIC_DRAW;
import static org.lwjgl.opengl.GL15.glBindBuffer;
import static org.lwjgl.opengl.GL15.glBufferData;
import static org.lwjgl.opengl.GL15.glDeleteBuffers;
import static org.lwjgl.opengl.GL15.glGenBuffers;
import org.lwjgl.opengl.GL20;
import static org.lwjgl.opengl.GL20.glAttachShader;
import static org.lwjgl.opengl.GL20.glBindAttribLocation;
import static org.lwjgl.opengl.GL20.glCreateProgram;
import static org.lwjgl.opengl.GL20.glDeleteProgram;
import static org.lwjgl.opengl.GL20.glDeleteShader;
import static org.lwjgl.opengl.GL20.glDetachShader;
import static org.lwjgl.opengl.GL20.glDisableVertexAttribArray;
import static org.lwjgl.opengl.GL20.glEnableVertexAttribArray;
import static org.lwjgl.opengl.GL20.glLinkProgram;
import static org.lwjgl.opengl.GL20.glUseProgram;
import static org.lwjgl.opengl.GL20.glValidateProgram;
import static org.lwjgl.opengl.GL20.glVertexAttribPointer;
import static org.lwjgl.opengl.GL30.glBindVertexArray;
import static org.lwjgl.opengl.GL30.glDeleteVertexArrays;
import static org.lwjgl.opengl.GL30.glGenVertexArrays;
import org.lwjgl.opengl.PixelFormat;
import org.lwjgl.util.glu.GLU;


// Notes: normal map, specular map, occlusion map and displacement map should be generated next to the texture aswell.
public class TextureGeneration { 
    // Screen variables
    private final String WINDOW_TITLE = "Procedural texture generation";
    private final int WIDTH = 320;
    private final int HEIGHT = 240;
    
    // Identifiers
    private int pId = 0;        // Program ID
    private int vaoId = 0;      // VAO id
    private int vboId = 0;      // VBO id
    private int vboiId = 0;     // VBO indices id
    private int vbocId = 0;     // VBO color id
    private int texId = 0;      // Texture id
    private int vsId = 0;       // Vertex shader id
    private int fsId = 0;       // Fragment shader id
    
    // Vertex and indices count
    private int vertexCount = 0, indicesCount = 0;
     
    public TextureGeneration() {
        // Initialize OpenGL (Display)
        setupOpenGL();
         
        setupQuad();
        setupShaders();
        setupTextures();
         
        while (!Display.isCloseRequested()) {
            // Do a single loop (logic/render)
            loopCycle();
             
            // Force a maximum FPS of about 60
            Display.sync(60);
            // Let the CPU synchronize with the GPU if GPU is tagging behind
            Display.update();
        }
         
        // Destroy OpenGL (Display)
        destroyOpenGL();
    }
     
    private void setupOpenGL() {
        // Setup an OpenGL context with API version 3.2
        try {
            PixelFormat pixelFormat = new PixelFormat();
            ContextAttribs contextAtrributes = new ContextAttribs(3, 2)
                .withForwardCompatible(true)
                .withProfileCore(true);
             
            Display.setDisplayMode(new DisplayMode(WIDTH, HEIGHT));
            Display.setTitle(WINDOW_TITLE);
            Display.create(pixelFormat, contextAtrributes);
             
            glViewport(0, 0, WIDTH, HEIGHT);
        } catch (LWJGLException e) {
            e.printStackTrace();
            System.exit(-1);
        }
         
        // Setup an XNA like background color
        glClearColor(0.4f, 0.6f, 0.9f, 0f);
         
        // Map the internal OpenGL coordinate system to the entire screen
        glViewport(0, 0, WIDTH, HEIGHT);
         
        this.exitOnGLError("Error in setupOpenGL");
    }
    
    private void setupShaders() {        
         // Load our vertex shader
        vsId = ShaderLoader.load("vertexShader.glsl", GL20.GL_VERTEX_SHADER);

        // Load our fragment shader
        fsId = ShaderLoader.load("fragmentShader.glsl", GL20.GL_FRAGMENT_SHADER);

         // We'll need a program to render anything with OpenGL 3.2(+)
        pId = glCreateProgram();
        
        // attach to program
        glAttachShader(pId, vsId);
        glAttachShader(pId, fsId);
        
        // Bind shader variables
        // Position information will be attribute 0
        glBindAttribLocation(pId, 0, "in_Position");
        // Color information will be attribute 1
        glBindAttribLocation(pId, 1, "in_Color");        
        // Texture information will be attribute 2
        glBindAttribLocation(pId, 2, "in_TextureCoord");

        glLinkProgram(pId);
        glValidateProgram(pId);
         
        int errorCheckValue = GL11.glGetError();
        if (errorCheckValue != GL11.GL_NO_ERROR) {
            System.out.println("ERROR - Could not create the shaders:" + GLU.gluErrorString(errorCheckValue));
            System.exit(-1);
        }
    }
    private void setupTextures() {
        texId = TextureGenerator.getTexture(16, 16, GL_TEXTURE0);
         
        exitOnGLError("setupTexture");
    }
    
    private void setupQuad() {       
       // We'll define our quad using 4 vertices of the custom 'TexturedVertex' class
        TexturedVertex v0 = new TexturedVertex(); 
        v0.setXYZ(-0.5f, 0.5f, 0); v0.setRGB(1, 0, 0); v0.setST(0, 0);
        TexturedVertex v1 = new TexturedVertex(); 
        v1.setXYZ(-0.5f, -0.5f, 0); v1.setRGB(0, 1, 0); v1.setST(0, 1);
        TexturedVertex v2 = new TexturedVertex(); 
        v2.setXYZ(0.5f, -0.5f, 0); v2.setRGB(0, 0, 1); v2.setST(1, 1);
        TexturedVertex v3 = new TexturedVertex(); 
        v3.setXYZ(0.5f, 0.5f, 0); v3.setRGB(1, 1, 1); v3.setST(1, 0);
         
        TexturedVertex[] vertices = new TexturedVertex[] {v0, v1, v2, v3};
        
        // Put each 'Vertex' in one FloatBuffer
        FloatBuffer verticesBuffer = BufferUtils.createFloatBuffer(vertices.length *
                TexturedVertex.elementCount);
        for (TexturedVertex vertice : vertices) {
            // Add position, color and texture floats to the buffer
            verticesBuffer.put(vertice.getElements());
        }
        verticesBuffer.flip();  
         
        // OpenGL expects to draw vertices in counter clockwise order by default
        byte[] indices = {
                0, 1, 2,
                2, 3, 0
        };
        indicesCount = indices.length;
        ByteBuffer indicesBuffer = BufferUtils.createByteBuffer(indicesCount);
        indicesBuffer.put(indices);
        indicesBuffer.flip();
         
        // Create a new Vertex Array Object in memory and select it (bind)
        vaoId = glGenVertexArrays();
        glBindVertexArray(vaoId);
         
        // Create a new Vertex Buffer Object in memory and select it (bind)
        vboId = glGenBuffers();
        glBindBuffer(GL_ARRAY_BUFFER, vboId);
        glBufferData(GL_ARRAY_BUFFER, verticesBuffer, GL_STATIC_DRAW);
        
        // Put the position coordinates in attribute list 0
        glVertexAttribPointer(0, TexturedVertex.positionElementCount, GL_FLOAT, 
                false, TexturedVertex.stride, TexturedVertex.positionByteOffset);
        // Put the color components in attribute list 1
        glVertexAttribPointer(1, TexturedVertex.colorElementCount, GL_FLOAT, 
                false, TexturedVertex.stride, TexturedVertex.colorByteOffset);
        // Put the texture coordinates in attribute list 2
        glVertexAttribPointer(2, TexturedVertex.textureElementCount, GL_FLOAT, 
                false, TexturedVertex.stride, TexturedVertex.textureByteOffset);
        
        glBindBuffer(GL_ARRAY_BUFFER, 0);
         
        // Deselect (bind to 0) the VAO
        glBindVertexArray(0);
         
        // Create a new VBO for the indices and select it (bind) - INDICES
        vboiId = glGenBuffers();
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, vboiId);
        glBufferData(GL_ELEMENT_ARRAY_BUFFER, indicesBuffer, GL_STATIC_DRAW);
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
         
        this.exitOnGLError("Error in setupQuad");
    }
     
    public void loopCycle() {
        glClear(GL_COLOR_BUFFER_BIT);
        
        glUseProgram(pId);
         
        // Bind to the VAO that has all the information about the vertices
        glBindVertexArray(vaoId);
        glEnableVertexAttribArray(0);
        glEnableVertexAttribArray(1);
        glEnableVertexAttribArray(2);
        
        // Bind to the index VBO that has all the information about the order of the vertices
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, vboiId);

        // Draw the vertices
        glDrawElements(GL_TRIANGLES, indicesCount, GL_UNSIGNED_BYTE, 0);

        // Put everything back to default (deselect)
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
        glDisableVertexAttribArray(0);
        glDisableVertexAttribArray(1);
        glDisableVertexAttribArray(2);
        glBindVertexArray(0);
        
        glUseProgram(0);
         
        this.exitOnGLError("Error in loopCycle");
    }
     
    private void destroyOpenGL() {    
        // Delete the texture
        glDeleteTextures(texId);
        
        // Delete the shaders
        glUseProgram(0);
        glDetachShader(pId, vsId);
        glDetachShader(pId, fsId);
         
        glDeleteShader(vsId);
        glDeleteShader(fsId);
        glDeleteProgram(pId);
         
        // Select the VAO
        glBindVertexArray(vaoId);
         
        // Disable the VBO index from the VAO attributes list
        glDisableVertexAttribArray(0);
        glDisableVertexAttribArray(1);
         
        // Delete the vertex VBO
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glDeleteBuffers(vboId);
         
        // Delete the color VBO
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glDeleteBuffers(vbocId);
         
        // Delete the index VBO
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
        glDeleteBuffers(vboiId);
         
        // Delete the VAO
        glBindVertexArray(0);
        glDeleteVertexArrays(vaoId);
         
        Display.destroy();
    }
     
    public void exitOnGLError(String errorMessage) {
        int errorValue = glGetError();
         
        if (errorValue != GL_NO_ERROR) {
            String errorString = GLU.gluErrorString(errorValue);
            System.err.println("ERROR - " + errorMessage + ": " + errorString);
             
            if (Display.isCreated()) Display.destroy();
            System.exit(-1);
        }
    }
    
    public static void main(String[] args) {
        TextureGeneration tg = new TextureGeneration();
    }
}