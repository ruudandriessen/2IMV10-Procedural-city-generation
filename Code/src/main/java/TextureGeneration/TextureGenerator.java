package TextureGeneration;

import java.nio.ByteBuffer;
import org.lwjgl.opengl.Display;
import static org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR;
import static org.lwjgl.opengl.GL11.GL_NEAREST;
import static org.lwjgl.opengl.GL11.GL_NO_ERROR;
import static org.lwjgl.opengl.GL11.GL_REPEAT;
import static org.lwjgl.opengl.GL11.GL_RGBA;
import static org.lwjgl.opengl.GL11.GL_TEXTURE_2D;
import static org.lwjgl.opengl.GL11.GL_TEXTURE_MAG_FILTER;
import static org.lwjgl.opengl.GL11.GL_TEXTURE_MIN_FILTER;
import static org.lwjgl.opengl.GL11.GL_TEXTURE_WRAP_S;
import static org.lwjgl.opengl.GL11.GL_TEXTURE_WRAP_T;
import static org.lwjgl.opengl.GL11.GL_UNSIGNED_BYTE;
import static org.lwjgl.opengl.GL11.glBindTexture;
import static org.lwjgl.opengl.GL11.glGenTextures;
import static org.lwjgl.opengl.GL11.glGetError;
import static org.lwjgl.opengl.GL11.glTexImage2D;
import static org.lwjgl.opengl.GL11.glTexParameteri;
import static org.lwjgl.opengl.GL13.glActiveTexture;
import static org.lwjgl.opengl.GL30.glGenerateMipmap;
import org.lwjgl.util.glu.GLU;

/**
 *
 * @author ruudandriessen
 */
public class TextureGenerator {
    public static int getTexture(int tWidth, int tHeight, int textureUnit) {
        ByteBuffer buf = createTexture(tWidth, tHeight);
        
        // Create a new texture object in memory and bind it
        int texId = glGenTextures();
        glActiveTexture(textureUnit);
        glBindTexture(GL_TEXTURE_2D, texId);
         
        // Upload the texture data and generate mip maps (for scaling)
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, tWidth, tHeight, 0, 
                GL_RGBA, GL_UNSIGNED_BYTE, buf);
        glGenerateMipmap(GL_TEXTURE_2D);
         
        // Setup the ST coordinate system
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
        
        // Setup what to do when the texture has to be scaled
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, 
                GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, 
                GL_LINEAR_MIPMAP_LINEAR);
        
        exitOnGLError("loadtexture");
        
        return texId;
    }
    
    private static ByteBuffer createTexture(int tWidth, int tHeight) {
        byte[] texture = new byte[tWidth * tHeight * 4];
        float[] noise = NoiseGenerator.getRandomNoise(tWidth, tWidth);
        for (int i = 0; i < tHeight * tWidth * 4; i += 4) {
            texture[i + 0] = (byte) (noise[i / 4] * 255);
            texture[i + 1] = (byte) (noise[i / 4] * 255);
            texture[i + 2] = (byte) (noise[i / 4] * 255);
            texture[i + 3] = (byte) 255;
        }
        
        ByteBuffer buf = ByteBuffer.allocateDirect(tWidth * tHeight * 4);
        buf.put(texture);
        buf.flip();
        return buf;
    }
    
    public static void exitOnGLError(String errorMessage) {
        int errorValue = glGetError();
         
        if (errorValue != GL_NO_ERROR) {
            String errorString = GLU.gluErrorString(errorValue);
            System.err.println("ERROR - " + errorMessage + ": " + errorString);
             
            if (Display.isCreated()) Display.destroy();
            System.exit(-1);
        }
    }
}
