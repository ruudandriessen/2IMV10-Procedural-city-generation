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

import java.util.ArrayList;

/**
 *
 * @author ruudandriessen
 */
public class EntityManager {
    private ArrayList<Entity> entities;
    
    public void addEntity(Entity e) {
        entities.add(e);
    }
    
    public void removeEntity(Entity e) {
        entities.remove(e);
    }
    
    public void render() {
        entities.stream().forEach((e) -> {
            e.render();
        });
    }
    
    public void destroy() {
        entities.stream().forEach((e) -> {
            e.destroy();
        });
    }
}
