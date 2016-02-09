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
package ProceduralCityGeneration;

import Data.Building;
import Data.DataLoader;
import Data.EntityManager;
import Util.Util;
import org.lwjgl.LWJGLException;
import org.lwjgl.opengl.ContextAttribs;
import org.lwjgl.opengl.Display;
import org.lwjgl.opengl.DisplayMode;
import static org.lwjgl.opengl.GL11.GL_COLOR_BUFFER_BIT;
import static org.lwjgl.opengl.GL11.glClear;
import static org.lwjgl.opengl.GL11.glClearColor;
import static org.lwjgl.opengl.GL11.glViewport;
import org.lwjgl.opengl.PixelFormat;

/**
 *
 * @author ruudandriessen
 */
public class ProceduralCityGeneration {    
    private final String WINDOW_TITLE = "Procedural texture generation";
    private final int WIDTH = 320;
    private final int HEIGHT = 240;
    EntityManager entityManager;
    
    public void initialize() {
        setupGL();
        entityManager = new EntityManager();
        DataLoader dataLoader = new DataLoader();
        dataLoader.getGeometries().stream().forEach((g) -> {
            entityManager.addEntity(new Building(g));
        });
//        dataLoader.getGeometries().stream().forEach((g) -> {
//            entityManager.addEntity(new S(g));
//        });
        // Load textures
        // Load whatever
        // Done
    }
    
    private void destroy() {
        entityManager.destroy();
        destroyGL();
    }
    
    private void setupGL() {
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
         
        Util.exitOnGLError("Error in setupOpenGL");
    }
    
    private void destroyGL() {
        
        Display.destroy();
    }
    
    private void start() {
        while (!Display.isCloseRequested()) {
            cycle();
        }
        destroy();
    }
    
    private void cycle() {        
        // Do a single loop
        logic();
        render();

        // Force a maximum FPS of about 60
        Display.sync(60);
        
        // Let the CPU synchronize with the GPU if GPU is tagging behind
        Display.update();
    }
    
    private void logic() {
        
    }
    
    private void render() {
        glClear(GL_COLOR_BUFFER_BIT);
        
        entityManager.render();
    
        Util.exitOnGLError("Error in render");
    }
    
    public static void main(String[] args) {
        ProceduralCityGeneration pcg = new ProceduralCityGeneration();
        pcg.initialize();
        pcg.start();
    }
}
