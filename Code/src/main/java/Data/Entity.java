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
package Data;


/**
 *
 * @author ruudandriessen
 */
public abstract class Entity { 
    
    public Entity() {
        setup();
    }
    
    public abstract void setup();
        // Setup vertices
        // Insert vertices in FloatBuffer
        // Insert indices in bytebuffers
        // Create VAO and select
        // Create VBO for vertices and select
        // Bind vertices buffer
        // Create vertex att pointers
        // Deselect VAO
        // create VBO for indices and select
        // Bind incides buffer
        // Deselect VBO
    public abstract void render();
        // Select program?
        // Bind VAO
        // Select VBO vertex att array
        // Bind VBO with indices information
        // Draw vertices
        // Deselect vbo
        // Deselect vertex attrib array, vertex array
        // Deselect program (shaders, etc)?
    public abstract void destroy();
}
