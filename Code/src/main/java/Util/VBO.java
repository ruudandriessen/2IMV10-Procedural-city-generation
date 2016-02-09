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

import static org.lwjgl.opengl.GL15.GL_ARRAY_BUFFER;
import static org.lwjgl.opengl.GL15.glBindBuffer;
import static org.lwjgl.opengl.GL15.glGenBuffers;

/**
 *
 * @author ruudandriessen
 */
public class VBO {
    private final int vboId;
    
    public VBO() {
        this.vboId = glGenBuffers();
    }
    
    public void select() {
        glBindBuffer(GL_ARRAY_BUFFER, this.vboId);
    }
    
    public static void deselect() {
        glBindBuffer(GL_ARRAY_BUFFER, 0);
    }
    
    public int getId() {
        return this.vboId;
    }
}
