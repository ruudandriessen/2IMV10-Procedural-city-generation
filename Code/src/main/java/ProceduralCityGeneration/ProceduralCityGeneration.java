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
import java.nio.ByteBuffer;
import java.nio.IntBuffer;
import java.nio.charset.Charset;
import org.lwjgl.BufferUtils;
import org.lwjgl.LWJGLException;
import org.lwjgl.opengl.ContextAttribs;
import org.lwjgl.opengl.Display;
import org.lwjgl.opengl.DisplayMode;
import static org.lwjgl.opengl.GL11.GL_COLOR_BUFFER_BIT;
import static org.lwjgl.opengl.GL11.GL_FALSE;
import static org.lwjgl.opengl.GL11.glClear;
import static org.lwjgl.opengl.GL11.glClearColor;
import static org.lwjgl.opengl.GL11.glViewport;
import static org.lwjgl.opengl.GL20.GL_COMPILE_STATUS;
import static org.lwjgl.opengl.GL20.GL_FRAGMENT_SHADER;
import static org.lwjgl.opengl.GL20.GL_VERTEX_SHADER;
import static org.lwjgl.opengl.GL20.glAttachShader;
import static org.lwjgl.opengl.GL20.glCompileShader;
import static org.lwjgl.opengl.GL20.glCreateProgram;
import static org.lwjgl.opengl.GL20.glCreateShader;
import static org.lwjgl.opengl.GL20.glGetShader;
import static org.lwjgl.opengl.GL20.glGetShaderInfoLog;
import static org.lwjgl.opengl.GL20.glLinkProgram;
import static org.lwjgl.opengl.GL20.glShaderSource;
import static org.lwjgl.opengl.GL20.glUseProgram;
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
        dataLoader.getMultiPolygons().stream().forEach((g) -> {
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

        int program = glCreateProgram();

        // create a vertex shader
        int vertexShaderId = glCreateShader(GL_VERTEX_SHADER);
        glShaderSource(vertexShaderId, toByteBuffer(
                "#version 150 core\n"
                + "in vec3 vVertex;\n"
                + "void main() {\n"
                + "  gl_Position = vec4(vVertex.x, vVertex.y, vVertex.z, 1.0);\n"
                + "}"));
        glCompileShader(vertexShaderId);
        if (glGetShader(vertexShaderId, GL_COMPILE_STATUS) == GL_FALSE) {
            printLogInfo(vertexShaderId);
            System.exit(-1);
        }

        // create a fragment shader
        int fragmentShaderId = glCreateShader(GL_FRAGMENT_SHADER);
        glShaderSource(fragmentShaderId, toByteBuffer(
                "#version 150 core\n"
                + "out vec4 color;\n"
                + "void main() {\n"
                + "  color = vec4(1.0);\n"
                + "}"));
        glCompileShader(fragmentShaderId);
        if (glGetShader(fragmentShaderId, GL_COMPILE_STATUS) == GL_FALSE) {
            printLogInfo(fragmentShaderId);
            System.exit(-1);
        }

        // attach to program
        glAttachShader(program, vertexShaderId);
        glAttachShader(program, fragmentShaderId);
        glLinkProgram(program);
        glUseProgram(program);

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

    private ByteBuffer toByteBuffer(final String data) {
        byte[] vertexShaderData = data.getBytes(Charset.forName("ISO-8859-1"));
        ByteBuffer vertexShader = BufferUtils.createByteBuffer(vertexShaderData.length);
        vertexShader.put(vertexShaderData);
        vertexShader.flip();
        return vertexShader;
    }

    private void printLogInfo(final int obj) {
        ByteBuffer infoLog = BufferUtils.createByteBuffer(2048);
        IntBuffer lengthBuffer = BufferUtils.createIntBuffer(1);
        glGetShaderInfoLog(obj, lengthBuffer, infoLog);

        byte[] infoBytes = new byte[lengthBuffer.get()];
        infoLog.get(infoBytes);
        if (infoBytes.length == 0) {
            return;
        }
        System.err.println(new String(infoBytes, Charset.forName("ISO-8859-1")));
    }
}
