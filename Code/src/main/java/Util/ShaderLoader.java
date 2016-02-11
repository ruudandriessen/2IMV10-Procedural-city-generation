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
package Util;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import static org.lwjgl.opengl.GL20.GL_COMPILE_STATUS;
import static org.lwjgl.opengl.GL20.glCompileShader;
import static org.lwjgl.opengl.GL20.glCreateShader;
import static org.lwjgl.opengl.GL20.glGetShaderInfoLog;
import static org.lwjgl.opengl.GL20.glGetShaderi;
import static org.lwjgl.opengl.GL20.glShaderSource;

/**
 *
 * @author ruudandriessen
 */
public class ShaderLoader {
    /**
     * Loads a shader from a given filename and type, and compiles shader source
     * @param filename Shader source file
     * @param type Type of shader
     * @return Shader ID
     */
    static public int load(String filename, int type) {
        StringBuilder shaderSource = new StringBuilder();
        int shaderID = 0;
        InputStreamReader inputStream = new InputStreamReader(ShaderLoader.class.getClassLoader().getResourceAsStream("shaders/" + filename));
        try {
            BufferedReader reader = new BufferedReader(inputStream);
            String line;
            while ((line = reader.readLine()) != null) {
                shaderSource.append(line).append("\n");
            }
            reader.close();
        } catch (IOException e) {
            System.err.println("Could not read file.");
            e.printStackTrace();
            System.exit(-1);
        }

        shaderID = glCreateShader(type);
        glShaderSource(shaderID, shaderSource);
        glCompileShader(shaderID);
        
        if(glGetShaderi(shaderID, GL_COMPILE_STATUS) == 0){
            System.err.println("Shader compilation failed");
            System.err.println(glGetShaderInfoLog(shaderID, 1024));
            System.exit(1);
        }

        return shaderID;
    }
}
