package TextureGeneration;

import Util.SimplexNoise;


/**
 *
 * @author ruudandriessen
 */
public class NoiseGenerator {
    public static float[][] getRandomNoise(int w, int h) {
        float noise[][] = new float[w][h]; 
        
        for (int i = 0; i < w; i++) {
            for (int j = 0; j < h; j++) {
                noise[i][j] = (float) Math.random();
            }
        }
        return noise;
    }
    
    public static float[][] getSimplexNoise(int w, int h) {        
        SimplexNoise sNoise = new SimplexNoise(20);
        float noise[][] = new float[w][h]; 
        
        for (int i = 0; i < w; i++) {
            for (int j = 0; j < h; j++) {
                noise[i][j] = (float) sNoise.noise(i, j);
            }
        }
        return noise;
    }
    
    public static float noise(int x, int y) {
        return (float) Math.random();
    }
    
    
//    float smoothNoise(float x, float y, float[][] noise, int noiseWidth, int noiseHeight)
//    {  
//       //get fractional part of x and y
//       float fractX = x - int(x);
//       float fractY = y - int(y);
//
//       //wrap around
//       int x1 = (int(x) + noiseWidth) % noiseWidth;
//       int y1 = (int(y) + noiseHeight) % noiseHeight;
//
//       //neighbor values
//       int x2 = (x1 + noiseWidth - 1) % noiseWidth;
//       int y2 = (y1 + noiseHeight - 1) % noiseHeight;
//
//       //smooth the noise with bilinear interpolation
//       double value = 0.0;
//       value += fractX       * fractY       * noise[x1][y1];
//       value += fractX       * (1 - fractY) * noise[x1][y2];
//       value += (1 - fractX) * fractY       * noise[x2][y1];
//       value += (1 - fractX) * (1 - fractY) * noise[x2][y2];
//
//       return value;
//    }
    
    public static float simplexNoise(float x, float y) {      
        SimplexNoise sNoise = new SimplexNoise(20); // :TODO: random seed?
        return (float) sNoise.noise(x, y);
    }
    
    public static float noiseMult(float x, float y, float mult) {
        return noiseMult(x, y, mult, mult);
    }
    
    public static float noiseMult(float x, float y, float multX, float multY) {
        return x * multX + y * multY;
    }
}
