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

import java.util.ArrayList;
import static org.lwjgl.opengl.GL30.glBindVertexArray;
import static org.lwjgl.opengl.GL30.glGenVertexArrays;

/**
 *
 * @author ruudandriessen
 */
public class VAO {
    private final static int VAO_SIZE = 16;
    private final int vaoId;
    private ArrayList<VBO> VBOs;
    
    public VAO() {
        this.vaoId = glGenVertexArrays();
        this.VBOs = new ArrayList<>();
    }
    
    public void select() {
        glBindVertexArray(this.vaoId);
    }
    
    public static void deselect() {
        glBindVertexArray(0);
    }
    
    public void addVBO(VBO vbo) {
        VBOs.add(vbo);
    }
    
    public void removeVBO(VBO vbo) {
        VBOs.remove(vbo);
    }
        
    public int getId() {
        return this.vaoId;
    }
}
