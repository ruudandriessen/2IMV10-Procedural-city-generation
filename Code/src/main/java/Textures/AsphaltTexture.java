/*
 * Copyright 2016 ruudandriessen.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package Textures;

import TextureGeneration.NoiseGenerator;
import TextureGeneration.TextureGenerator;
import java.nio.ByteBuffer;
import static org.lwjgl.opengl.GL13.GL_TEXTURE0;

/**
 *
 * @author ruudandriessen
 */
public class AsphaltTexture {
    public static int generate(int w, int h) {
        // Allocate memory for final 
        byte[] texture = new byte[w * h * 4];
        
        // Start with noise
        float[][] base = NoiseGenerator.getRandomNoise(w, h);
        
        for (int x = 0; x < w; x++) {
            for (int y = 0; y < h; y++) {                
//                base[x][y] = (float)(1 + Math.sin(((x + base[x][y]) / 2) * 50)) / 2;
//                base[x][y] = (float)(1 + Math.sin(x * 50)) / 2;
                base[x][y] = NoiseGenerator.noise(x*5, y*5);
                System.out.print(base[x][y] + " ");
            }
            System.out.print("\n");
        }
        
        for (int i = 0; i < w * h * 4; i += 4) {
            int x = (i/4) % w;
            int y = (i/4) / w;
            texture[i + 0] = (byte) (base[x][y] * 255);
            texture[i + 1] = (byte) (base[x][y] * 255);
            texture[i + 2] = (byte) (base[x][y] * 255);
            texture[i + 3] = (byte) 255;
        }
        
        
        // Create buffer from texture
        ByteBuffer buf = ByteBuffer.allocateDirect(w * h * 4);
        buf.put(texture);
        buf.flip();
        
        // Return generated id from texture data
        return TextureGenerator.getTexture(w, h, GL_TEXTURE0, buf);
    }
    
    private static void add(int w, int h, float[][] a, float[][] b) {
        for (int i = 0; i < w; i++) {
            for (int j = 0; j < h; j++) { 
                a[i][j] += b[i][j];  
                a[i][j] /= 2;
            }
        }
    } 
}
