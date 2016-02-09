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
import org.lwjgl.opengl.GL20;

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

        shaderID = GL20.glCreateShader(type);
        GL20.glShaderSource(shaderID, shaderSource);
        GL20.glCompileShader(shaderID);

        return shaderID;
    }
}
