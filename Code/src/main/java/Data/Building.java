/*
 * Copyright 2016 rikschreurs.
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

import com.vividsolutions.jts.geom.Geometry;
import static org.lwjgl.opengl.GL11.GL_COLOR_BUFFER_BIT;

/**
 *
 * @author rikschreurs
 */
public class Building extends Entity {
    private Geometry geometry;
    
    public Building(Geometry geometry) {
        this.geometry = geometry;
        setup();
    }
    
    
    @Override
    public void setup() {
        
        //throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void render() {

    }

    @Override
    public void destroy() {
        //throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }
    
}
