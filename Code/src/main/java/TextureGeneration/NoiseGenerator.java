package TextureGeneration;

/**
 *
 * @author ruudandriessen
 */
public class NoiseGenerator {
    public static float[][] getRandomNoise(int w, int h) {
        float noise[][] = new float[w][h]; 
        for (int x = 0; x < w; x++) {
            for (int y = 0; y < h; y++) {
                noise[x][y] = (float) ((Math.random() % 32768) / 32768.0);
            }
        }
        return noise;
    }
}
