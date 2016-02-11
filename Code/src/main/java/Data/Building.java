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

import Util.Util;
import com.vividsolutions.jts.geom.Coordinate;
import com.vividsolutions.jts.geom.MultiPolygon;
import java.nio.FloatBuffer;
import java.util.ArrayList;
import java.util.List;
import org.dyn4j.geometry.Triangle;
import org.dyn4j.geometry.Vector2;
import org.dyn4j.geometry.decompose.SweepLine;
import org.lwjgl.BufferUtils;
import org.lwjgl.opengl.GL11;
import org.lwjgl.opengl.GL15;
import org.lwjgl.opengl.GL20;
import org.lwjgl.opengl.GL30;

/**
 *
 * @author rikschreurs
 */
public class Building extends Entity {

    private MultiPolygon geometry;
    float[] vertices;
    private int vaoId = 0;
    private int vboId = 0;
    private int vertexCount = 0;
    private int pId = 0;

    public Building(MultiPolygon geometry) {
        this.geometry = geometry;
        setup();
    }

    @Override
    public void setup() {
        // OpenGL expects vertices to be defined counter clockwise by default

        triangulize(geometry);

        
        // Sending data to OpenGL requires the usage of (flipped) byte buffers
        FloatBuffer verticesBuffer = BufferUtils.createFloatBuffer(vertices.length);
        verticesBuffer.put(vertices);
        verticesBuffer.flip();

        vertexCount = vertices.length;

        // Create a new Vertex Array Object in memory and select it (bind)
        // A VAO can have up to 16 attributes (VBO's) assigned to it by default
        vaoId = GL30.glGenVertexArrays();
        GL30.glBindVertexArray(vaoId);

        // Create a new Vertex Buffer Object in memory and select it (bind)
        // A VBO is a collection of Vectors which in this case resemble the location of each vertex.
        vboId = GL15.glGenBuffers();
        GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vboId);
        GL15.glBufferData(GL15.GL_ARRAY_BUFFER, verticesBuffer, GL15.GL_STATIC_DRAW);
        // Put the VBO in the attributes list at index 0
        GL20.glVertexAttribPointer(0, 3, GL11.GL_FLOAT, false, 0, 0);
        // Deselect (bind to 0) the VBO
        GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, 0);

        // Deselect (bind to 0) the VAO
        GL30.glBindVertexArray(0);

        Util.exitOnGLError("Error in setupQuad");
    }

    @Override
    public void render() {

        GL11.glClear(GL11.GL_COLOR_BUFFER_BIT);

        // Bind to the VAO that has all the information about the quad vertices
        GL30.glBindVertexArray(vaoId);
        GL20.glEnableVertexAttribArray(0);

        // Draw the vertices
        GL11.glDrawArrays(GL11.GL_TRIANGLES, 0, vertexCount);

        // Put everything back to default (deselect)
        GL20.glDisableVertexAttribArray(0);
        GL30.glBindVertexArray(0);

        Util.exitOnGLError("Error in Building render");

    }

    @Override
    public void destroy() {
        // Disable the VBO index from the VAO attributes list
        GL20.glDisableVertexAttribArray(0);

        // Delete the VBO
        GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, 0);
        GL15.glDeleteBuffers(vboId);

        // Delete the VAO
        GL30.glBindVertexArray(0);
        GL30.glDeleteVertexArrays(vaoId);
    }

    private void triangulize(MultiPolygon geometry) {
        List<Triangle> triangles = new ArrayList<>();
        Coordinate[] coordinates = geometry.convexHull().getCoordinates();
        Vector2[] points = new Vector2[coordinates.length - 1];
        for (int i = 0; i < coordinates.length - 1; i++) {
            points[i] = new Vector2(coordinates[i].y, coordinates[i].x);
        }
        if (points.length == 3) {
            triangles.add(new Triangle(points[0], points[1], points[2]));
        } else if (points.length < 3) {

        } else {
            SweepLine earClipper = new SweepLine();
            triangles = earClipper.triangulate(points);
        }
        vertices = new float[triangles.size()*3];
        for(int i = 0; i < triangles.size(); i++) {
            for(int j = 0; j < 3; j++) {
                vertices[i*3] = (float) triangles.get(i).getVertices()[j].x;
                vertices[i*3+1] = (float) triangles.get(i).getVertices()[j].y;
                vertices[i*3+2] = (float) 0.0;
            }
           
        }
    }

}
