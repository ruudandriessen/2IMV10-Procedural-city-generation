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

import Util.TexturedVertex;
import com.vividsolutions.jts.geom.Coordinate;
import com.vividsolutions.jts.geom.CoordinateSequence;
import com.vividsolutions.jts.geom.LineString;
import java.nio.FloatBuffer;
import org.lwjgl.BufferUtils;

/**
 *
 * @author ruudandriessen
 */
public class Road extends Entity {

    LineString geometry;

    public Road(LineString geometry) {
        super();
        System.out.println("----- New Road -----");
        System.out.println(geometry);
        this.geometry = geometry;
        setup();
    }

    @Override
    public void setup() {
        CoordinateSequence cs = geometry.getCoordinateSequence();

        // Setup vertices using coordinate sequence 
        FloatBuffer verticesBuffer = BufferUtils.createFloatBuffer(cs.getDimension()
                * TexturedVertex.elementCount);
        for (int i = 0; i < cs.getDimension(); i++) {
            TexturedVertex v1 = new TexturedVertex();
            Coordinate c = cs.getCoordinate(i);

            v1.setXYZ((float) c.x, (float) c.y, (float) c.z);
//            v.setRGB(0, 1, 0);
           
            verticesBuffer.put(v1.getElements());
        }
        verticesBuffer.flip();
        
                
    }

    @Override
    public void render() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void destroy() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

}
