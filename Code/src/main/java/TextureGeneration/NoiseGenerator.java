package TextureGeneration;


/**
 *
 * @author ruudandriessen
 */
public class NoiseGenerator {
    public static float[] getRandomNoise(int w, int h) {
        float noise[] = new float[w * h]; 
        
        for (int i = 0; i < w * h; i++) {
            noise[i] = (float) Math.random();
        }
        return noise;
    }
}
